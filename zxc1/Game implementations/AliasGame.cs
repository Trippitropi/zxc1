using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Base_classes;
using zxc1.Interfaces;
using zxc1.players;

namespace zxc1.Game_implementations
{
    public class AliasGame : IGame
    {
        private readonly List<IPlayer> _players;
        private readonly List<AliasTeam> _teams;
        private readonly List<string> _words;
        private int _currentTeamIndex;
        private readonly Random _random;
        private const int WordsPerRound = 5;
        private bool _rulesRead = false;

        public event EventHandler<GameRulesEventArgs> RulesAnnounced;
        public AliasGame()
        {
            _players = new List<IPlayer>();
            _teams = new List<AliasTeam>();
            _words = new List<string>
        {
            "яблуко", "собака", "комп'ютер", "телефон", "телевізор",
            "кіт", "вікно", "двері", "стіл", "стілець", "ручка",
            "папір", "кава", "чай", "вода", "морозиво", "сонце",
            "місяць", "зірка", "небо", "земля", "море", "океан",
            "річка", "гори", "ліс", "дерево", "квітка", "трава"
        };
            _currentTeamIndex = 0;
            _random = new Random();
        }


        private void AnnounceRules()
        {
            string rules = @"
=== ПРАВИЛА ГРИ ЕЛІАС ===

Мета гри:
  Пояснювати слова своїй команді, не використовуючи однокореневі слова, та
вгадувати якомога більше за відведений час.

Основні правила:
Гравці діляться на команди (мінімум 2 команди).
  Кожен раунд один гравець із команди пояснює слова, не називаючи їх прямо.
Інші члени команди повинні вгадати слово.
За кожне правильно вгадане слово команда отримує бал.
  Якщо слово не вдається пояснити, можна пропустити, але це може зняти бал.
Гра триває до досягнення встановленої кількості балів або до завершення 
певної кількості раундів.

Бажаємо приємної гри!
";

            OnRulesAnnounced(new GameRulesEventArgs("Мафія", rules));
        }

        public void AddPlayer()
        {
            Console.WriteLine("Введіть ім'я гравця: ");
            string name = Console.ReadLine();
            _players.Add(new Player(name));
            Console.WriteLine($"Гравець {name} доданий!");
        }

        public void CreateTeams()
        {
            if (_players.Count < 2)
            {
                Console.WriteLine("Недостатньо гравців! Додайте хоча б 2 гравця.");
                return;
            }

            Console.Write("Скільки команд ви хочете створити? ");
            if (!int.TryParse(Console.ReadLine(), out int numTeams) || numTeams < 1)
            {
                Console.WriteLine("Введіть коректне число команд.");
                return;
            }

            List<IPlayer> playersCopy = new List<IPlayer>(_players);
            ShuffleList(playersCopy);


            int playersPerTeam = _players.Count / numTeams;
            int extraPlayers = _players.Count % numTeams;

            for (int i = 0; i < numTeams; i++)
            {
                Console.Write($"Введіть назву для команди {i + 1}: ");
                string teamName = Console.ReadLine();

                int teamSize = playersPerTeam + (i < extraPlayers ? 1 : 0);

                List<IPlayer> teamPlayers = new List<IPlayer>();
                for (int j = 0; j < teamSize && playersCopy.Count > 0; j++)
                {
                    teamPlayers.Add(playersCopy[0]);
                    playersCopy.RemoveAt(0);
                }

                _teams.Add(new AliasTeam(teamName, teamPlayers));

                Console.WriteLine($"Команда '{teamName}' створена з гравцями: {string.Join(", ", teamPlayers.Select(p => p.Name))}");
            }
        }

        private void OnRulesAnnounced(GameRulesEventArgs e)
        {
            RulesAnnounced?.Invoke(this, e);
            _rulesRead = true;
            Console.WriteLine("\nПравила прочитано! Тепер ви можете почати гру.");
        }
        public void PlayRound()
        {
            if (!_rulesRead)
            {
                Console.WriteLine("\nУВАГА! Перед початком гри необхідно обов'язково прочитати правила.");
                Console.WriteLine("Виберіть опцію 'Показати правила' в головному меню.");
                return;
            }

            if (_teams.Count == 0)
            {
                Console.WriteLine("Спочатку створіть команди!");
                return;
            }

            AliasTeam team = _teams[_currentTeamIndex];
            Console.WriteLine($"\nЗараз хід команди '{team.Name}'");
            Console.WriteLine($"Гравці команди: {string.Join(", ", team.Players.Select(p => p.Name))}");
            Console.WriteLine($"Вам потрібно вгадати {WordsPerRound} слів. Для кожного слова відповідайте '+' якщо вгадали або '-' якщо пропускаєте.");

            Console.WriteLine("Натисніть Enter, щоб почати раунд...");
            Console.ReadLine();

            int roundPoints = 0;

           
            List<string> roundWords = GetRandomWordsForRound();

            for (int i = 0; i < WordsPerRound; i++)
            {
                string word = roundWords[i];
                Console.WriteLine($"Слово {i + 1}/{WordsPerRound} для пояснення: {word}");

                Console.Write("Команда вгадала слово? (+/-): ");
                string result = Console.ReadLine();
                if (result == "+")
                {
                    roundPoints++;
                    Console.WriteLine("Правильно! +1 бал");
                }
                else
                {
                    Console.WriteLine("Пропущено");
                }
            }

            if (roundPoints > 0)
            {
                team.AddPoints(roundPoints);
                Console.WriteLine($"Раунд завершено! Команда '{team.Name}' отримує {roundPoints} балів!");
                Console.WriteLine($"Загальний рахунок команди: {team.Points} балів");
            }
            else
            {
                Console.WriteLine($"Раунд завершено! Команда '{team.Name}' не отримує балів.");
            }

            _currentTeamIndex = (_currentTeamIndex + 1) % _teams.Count;
        }

        private List<string> GetRandomWordsForRound()
        {
            List<string> allWords = new List<string>(_words);
            List<string> roundWords = new List<string>();

            for (int i = 0; i < WordsPerRound && allWords.Count > 0; i++)
            {
                int index = _random.Next(allWords.Count);
                roundWords.Add(allWords[index]);
                allWords.RemoveAt(index);
            }

            return roundWords;
        }

        public void ShowScores()
        {
            Console.WriteLine("\nПоточний рахунок:");
            foreach (var team in _teams.OrderByDescending(t => t.Points))
            {
                Console.WriteLine($"Команда '{team.Name}': {team.Points} балів");
            }
        }

        public void PlayGame()
        {
            RulesAnnounced += (sender, args) =>
            {
                Console.WriteLine(args.Rules);
                Console.WriteLine("Натисніть Enter, щоб продовжити...");
                Console.ReadLine();
            };

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== ЕЛІАС ===");
                Console.WriteLine("1. Додати гравця");
                Console.WriteLine("2. Створити команди");
                Console.WriteLine("3. Почати раунд");
                Console.WriteLine("4. Показати рахунок");
                Console.WriteLine("5. Повернутися в головне меню");
                Console.WriteLine("6. Показати правила");

                Console.Write("Виберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPlayer();
                        break;
                    case "2":
                        CreateTeams();
                        break;
                    case "3":
                        PlayRound();
                        break;
                    case "4":
                        ShowScores();
                        break;
                    case "5":
                        exit = true;
                        break;
                    case "6":
                        AnnounceRules();
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        private void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

