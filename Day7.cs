namespace Temporalis;

internal class Day7
{
    public static void Part1()
    {
        var res = File.ReadLines("input_day7.txt")
            .Select(line => new Set(new Hand(line.Split(' ')[0]), int.Parse(line.Split(' ')[1])))
            .OrderBy(s => s.Hand)
            .Select((s, i) => s.Bid * (i + 1))
            .Sum();
        Console.WriteLine(res);
    }

    public static void Part2()
    {
        var res = File.ReadLines("input_day7.txt")
            .Select(line => new Set(new Hand(line.Split(' ')[0], true), int.Parse(line.Split(' ')[1])))
            .OrderBy(s => s.Hand)
            .Select((s, i) => s.Bid * (i + 1))
            .Sum();
        Console.WriteLine(res);
    }

    private sealed record Set(Hand Hand, int Bid);

    private sealed class Hand : IComparable<Hand>
    {
        public Hand(string input, bool useJokers = false)
        {
            Cards = input.Select(c => CharToCard(c, useJokers)).ToArray();
            HandType = useJokers ? CardsToTypeWithJokers(Cards) : CardsToType(Cards);
        }

        private Card[] Cards { get; }
        private HandType HandType { get; }
        
        private static Card CharToCard(char c, bool useJokers = false)
        {
            return c switch
            {
                'A' => new Card(14),
                'K' => new Card(13),
                'Q' => new Card(12),
                'J' => new Card(useJokers ? 1 : 11),
                'T' => new Card(10),
                _ => new Card(int.Parse(c.ToString()))
            };
        }

        private static HandType CardsToType(IEnumerable<Card> cards)
        {
            var grouped = cards.GroupBy(card => card.Value).ToList();
            return grouped.Count switch
            {
                1 => HandType.FiveOfAKind,
                2 when grouped.Exists(group => group.Count() == 4) => HandType.FourOfAKind,
                2 when grouped.Exists(group => group.Count() == 3) &&
                       grouped.Exists(group => group.Count() == 2) => HandType.FullHouse,
                3 when grouped.Exists(group => group.Count() == 3) => HandType.ThreeKind,
                4 => HandType.Pair,
                5 => HandType.High,
                _ => HandType.TwoPair
            };
        }

        private static HandType CardsToTypeWithJokers(IList<Card> cards)
        {
            var jokers = cards.Count(card => card.Value == 1);
            var grouped = cards.Where(c => c.Value != 1).GroupBy(card => card.Value).ToList();
            if (cards.Distinct().Count() == 1) return HandType.FiveOfAKind;

            var type = grouped.Count switch
            {
                1 or 2 when grouped.Exists(group => group.Count() == 4) => HandType.FourOfAKind,
                2 when grouped.Exists(group => group.Count() == 3) &&
                                            grouped.Exists(group => group.Count() == 2) => HandType.FullHouse,
                <= 3 when grouped.Exists(group => group.Count() == 3) => HandType.ThreeKind,
                <= 3 when grouped.Count(group => group.Count() == 2) == 2 => HandType.TwoPair,
                <= 4 when grouped.Exists(group => group.Count() == 2) => HandType.Pair,
                _ => HandType.High
            };

            return UpgradeTypeWithJokers(type, jokers);
        }

        public int CompareTo(Hand? other)
        {
            if (other == null) return 1;
            if (HandType != other.HandType) return HandType.CompareTo(other.HandType);

            for (var i = 0; i < 5; i++)
            {
                var value = Cards[i].Value.CompareTo(other.Cards[i].Value);
                if (value == 0) continue;
                return value;
            }

            return 0;
        }

        public override string ToString() => $"{HandType}\t{string.Join(' ', Cards.ToList())}";

        private static HandType UpgradeTypeWithJokers(HandType type, int numJokers)
        {
            if (numJokers == 0) return type;
            return type switch
            {
                HandType.FiveOfAKind => HandType.FiveOfAKind,
                HandType.FourOfAKind => HandType.FiveOfAKind,
                HandType.ThreeKind when numJokers == 1 => HandType.FourOfAKind,
                HandType.ThreeKind when numJokers == 2 => HandType.FiveOfAKind,
                HandType.Pair when numJokers == 1 => HandType.ThreeKind,
                HandType.Pair when numJokers == 2 => HandType.FourOfAKind,
                HandType.Pair when numJokers == 3 => HandType.FiveOfAKind,
                HandType.High when numJokers == 1 => HandType.Pair,
                HandType.High when numJokers == 2 => HandType.ThreeKind,
                HandType.High when numJokers == 3 => HandType.FourOfAKind,
                HandType.High when numJokers == 4 => HandType.FiveOfAKind,
                HandType.High when numJokers == 5 => HandType.FiveOfAKind,
                HandType.TwoPair when numJokers == 1 => HandType.FullHouse,
                _ => type
            };
        }
    }

    private record Card(int Value)
    {
        public virtual bool Equals(Card? other) => Value == other?.Value;
        
        public override int GetHashCode() => Value;

        public override string ToString()
        {
            return Value switch
            {
                10 => "[T]",
                11 or 1 => "[J]",
                12 => "[Q]",
                13 => "[K]",
                14 => "[A]",
                _ => $"[{Value}]"
            };
        }
    }

    private enum HandType
    {
        High,
        Pair,
        TwoPair,
        ThreeKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }
}