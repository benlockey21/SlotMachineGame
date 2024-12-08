using Microsoft.Extensions.Configuration;

namespace SlotMachine;

public class Program
{
    static void Main(string[] args)
    {
        var symbols = new List<Symbol>();
        try
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            configuration.GetSection("GameSettings:Symbols").Bind(symbols);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception loading game settings. {ex.Message}");
            Environment.Exit(0);
        }

        if (symbols.Count < 3)
        {
            Console.WriteLine($"Configuration error. Game requires at least 3 symbols to play.");
            Environment.Exit(0);
        }

        Game game = new Game(symbols);

        bool playAgain = true;
        while (playAgain)
        {
            Console.WriteLine("Would you like to play again? (Y/N)");
            string? input = Console.ReadLine();
            if (input != null && input.ToUpper() == "Y")
            {
                Game newGame = new Game(symbols);
            }
            else
            {
                playAgain = false;
            }
        }
    }
}