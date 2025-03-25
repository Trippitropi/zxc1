using zxc1.Base_classes;
using zxc1.Interfaces;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        IGameFactory gameFactory = new GameFactory();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Грати в Еліас");
            Console.WriteLine("2. Грати в Мафію");
            Console.WriteLine("3. Грати в Монополію");
            Console.WriteLine("4. Вихід");

            Console.Write("Виберіть опцію: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    IGame aliasGame = gameFactory.CreateGame("alias");
                    aliasGame.PlayGame();
                    break;
                case "2":
                    IGame mafiaGame = gameFactory.CreateGame("mafia");
                    mafiaGame.PlayGame();
                    break;
                case "3":
                    IGame monopolyGame = gameFactory.CreateGame("monopoly");
                    monopolyGame.PlayGame();
                    break;
                case "4":
                    Console.WriteLine("Дякую за гру! До побачення!");
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }
}