namespace SlotMachine;

public class Game
{
    private readonly List<Symbol> _symbols;
    private decimal _balance;
    private Random _random;

    public Game(List<Symbol> symbols)
    {
        _symbols = InitializeSymbols(symbols);
        _balance = GetDepositAmount();
        _random = new Random();
        Play();
    }

    private void Play()
    {
        while (_balance > 0)
        {
            decimal stakeAmount = GetStakeAmount(_balance);
            Console.WriteLine();
            _balance -= stakeAmount;

            decimal amountWon = RunSpin(stakeAmount);
            Console.WriteLine();
            if (amountWon > 0)
            {
                _balance += amountWon;
                Console.WriteLine($"You have won: {amountWon}");
            }
            else
            {
                Console.WriteLine($"You have not won.");
            }

            Console.WriteLine($"Current balance: {_balance}");
            Console.WriteLine();
        }

        Console.WriteLine("Game over. You have no money left.");
        return;
    }

    private decimal GetDepositAmount()
    {
        decimal value = 0;
        bool entered = false;
        while (!entered)
        {
            Console.WriteLine("Please deposit money you would like to play with:");
            var input = Console.ReadLine();
            if (decimal.TryParse(input, out value) && value > 0)
            {
                entered = true;
            }
            else
            {
                Console.WriteLine("Invalid entry. Please try again.");
            }
        }
        return value;
    }

    private decimal GetStakeAmount(decimal balance)
    {
        decimal value = 0;
        bool entered = false;
        while (!entered)
        {
            Console.WriteLine("Enter stake amount:");
            var input = Console.ReadLine();
            if (decimal.TryParse(input, out value) && value > 0)
            {
                if (value > balance)
                {
                    Console.WriteLine("You have entered more than the deposit. Please try again.");
                }
                else
                {
                    entered = true;
                }
            }
            else
            {
                Console.WriteLine("Invalid entry. Please try again.");
            }
        }
        return value;
    }

    private decimal RunSpin(decimal stakeAmount)
    {
        decimal amountWon = 0;

        for (int i = 0; i < 4; i++)
        {
            List<Symbol> symbolsPicked = new List<Symbol>();
            for (int j = 0; j < 3; j++)
            {
                int number = _random.Next(0, 100);
                Symbol symbol = _symbols[number];
                symbolsPicked.Add(symbol);
                Console.Write(symbol.Type);
            }

            bool isWinningSpin = false;

            int countUnique = symbolsPicked.GroupBy(i => i.Type).Count();
            if (countUnique == 1)
            {
                if (!symbolsPicked.First().IsWildcard)
                {
                    isWinningSpin = true;
                }
            }
            else if (countUnique == 2)
            {
                int wildcards = symbolsPicked.Where(s => s.IsWildcard).Count();
                if (wildcards > 0)
                {
                    isWinningSpin = true;
                }
            }

            if (isWinningSpin)
            {
                decimal multiplier = symbolsPicked.Sum(s => s.Coefficient);
                amountWon += (multiplier * stakeAmount);
            }

            Console.WriteLine();
        }

        return amountWon;
    }

    private List<Symbol> InitializeSymbols(List<Symbol> symbols)
    {
        List<Symbol> list = new List<Symbol>();

        foreach (Symbol symbol in symbols)
        {
            for (int i = 1; i <= symbol.PercentageChance; i++)
            {
                list.Add(symbol);
            }
        }

        return list;
    }

}
