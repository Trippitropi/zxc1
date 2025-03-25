using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Base_classes;
using zxc1.Interfaces;

namespace zxc1.Monopoly
{
    public class MonopolyGame : IGame
    {
        private readonly List<IPlayer> _players;
        private readonly List<MonopolyPlayer> _monopolyPlayers;
        private readonly List<MonopolyProperty> _properties;
        private readonly MonopolyDice _dice;
        private readonly Random _random;
        private int _currentPlayerIndex;
        private bool _rulesRead = false;

        
        public event EventHandler<GameRulesEventArgs> RulesAnnounced;

        public MonopolyGame()
        {
            _players = new List<IPlayer>();
            _monopolyPlayers = new List<MonopolyPlayer>();
            _properties = InitializeProperties();
            _dice = new MonopolyDice();
            _random = new Random();
            _currentPlayerIndex = 0;
        }

        private List<MonopolyProperty> InitializeProperties()
        {
          
            var properties = new List<MonopolyProperty>
        {
            new MonopolyProperty("Старт", 0, 0, 0),
            new MonopolyProperty("Вул. Хрещатик", 1, 60, 2),
            new MonopolyProperty("Вул. Володимирська", 3, 60, 4),
            new MonopolyProperty("Вул. Богдана Хмельницького", 6, 100, 6),
            new MonopolyProperty("Вул. Шовковична", 8, 100, 6),
            new MonopolyProperty("Вул. Івана Франка", 9, 120, 8),
            new MonopolyProperty("Вул. Дениса Демиденка", 11, 140, 10),
            new MonopolyProperty("Вул. Ярославів Вал", 13, 140, 10),
            new MonopolyProperty("Вул. Велика Житомирська", 14, 160, 12),
            new MonopolyProperty("Вул. Антоновича", 16, 180, 14),
            new MonopolyProperty("Вул. Саксаганського", 18, 180, 14),
            new MonopolyProperty("Вул. Жилянська", 19, 200, 16),
        };
            return properties;
        }

        private void OnRulesAnnounced(GameRulesEventArgs e)
        {
            RulesAnnounced?.Invoke(this, e);
            _rulesRead = true;
            Console.WriteLine("\nПравила прочитано! Тепер ви можете почати гру.");
        }

        private void AnnounceRules()
        {
            string rules = @"
=== ПРАВИЛА ГРИ МОНОПОЛІЯ ===

Мета гри: стати найбагатшим гравцем, купуючи, здаючи в оренду та продаючи власність.

Основні правила:
1. Кожен хід гравець кидає кубики і переміщує свою фішку на відповідну кількість клітинок.
2. Якщо гравець потрапляє на вільну власність, він може купити її.
3. Якщо гравець потрапляє на власність, яка належить іншому гравцю, він повинен сплатити оренду.
4. Гравець, який не може сплатити борг, вважається банкрутом і виходить з гри.
5. Гра триває доки не залишиться один гравець (або за вибором часу).

Бажаємо приємної гри!
";

            OnRulesAnnounced(new GameRulesEventArgs("Монополія", rules));
        }

        public void AddPlayer()
        {
            Console.Write("Введіть ім'я гравця: ");
            string name = Console.ReadLine();

           
            string[] tokenOptions = { "Автомобіль", "Капелюх", "Черевик", "Корабель", "Собака", "Кіт" };

            Console.WriteLine("Виберіть фішку:");
            for (int i = 0; i < tokenOptions.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {tokenOptions[i]}");
            }

            Console.Write("Ваш вибір: ");
            if (int.TryParse(Console.ReadLine(), out int tokenChoice) && tokenChoice >= 1 && tokenChoice <= tokenOptions.Length)
            {
                string tokenName = tokenOptions[tokenChoice - 1];
                var player = new MonopolyPlayer(name, tokenName);
                _players.Add(player);
                _monopolyPlayers.Add(player);
                Console.WriteLine($"Гравець {name} з фішкою \"{tokenName}\" доданий!");
            }
            else
            {
                Console.WriteLine("Невірний вибір фішки. Використовуємо випадкову фішку.");
                string tokenName = tokenOptions[_random.Next(tokenOptions.Length)];
                var player = new MonopolyPlayer(name, tokenName);
                _players.Add(player);
                _monopolyPlayers.Add(player);
                Console.WriteLine($"Гравець {name} з фішкою \"{tokenName}\" доданий!");
            }
        }

        public void PlayRound()
        {
            if (!_rulesRead)
            {
                Console.WriteLine("\nУВАГА! Перед початком гри необхідно обов'язково прочитати правила.");
                Console.WriteLine("Виберіть опцію 'Показати правила' в головному меню.");
                return;
            }

            if (_monopolyPlayers.Count < 2)
            {
                Console.WriteLine("Потрібно мінімум 2 гравці для початку гри.");
                return;
            }

            MonopolyPlayer currentPlayer = _monopolyPlayers[_currentPlayerIndex];

            if (currentPlayer.IsBankrupt)
            {
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _monopolyPlayers.Count;
                return;
            }

            Console.WriteLine($"\nХід гравця {currentPlayer.Name} (Фішка: {currentPlayer.Token.Name})");
            Console.WriteLine($"Поточна позиція: {currentPlayer.Token.Position}, Гроші: {currentPlayer.Money}$");
            Console.WriteLine("Натисніть Enter, щоб кинути кубики...");
            Console.ReadLine();

            
            var (dice1, dice2) = _dice.RollTwo();
            int steps = dice1 + dice2;
            Console.WriteLine($"Випало {dice1} і {dice2}. Разом {steps}");

            
            int oldPosition = currentPlayer.Token.Position;
            currentPlayer.Token.Move(steps);
            int newPosition = currentPlayer.Token.Position;

           
            if (newPosition < oldPosition)
            {
                currentPlayer.AddMoney(200);
                Console.WriteLine("Гравець пройшов через Старт і отримав 200$");
            }

            // Знаходимо власність, на яку потрапив гравець
            var property = _properties.FirstOrDefault(p => p.Position == newPosition);
            if (property != null)
            {
                Console.WriteLine($"Гравець потрапив на клітинку: {property.Name}");

                if (property.Position == 0)
                {
                    Console.WriteLine("Це клітинка Старт. Нічого не відбувається.");
                }
                else if (property.Owner == null)
                {
                    Console.WriteLine($"Ця власність вільна. Ціна: {property.Price}$");
                    Console.Write("Бажаєте купити цю власність? (так/ні): ");
                    string choice = Console.ReadLine();
                    if (choice.ToLower() == "так")
                    {
                        currentPlayer.BuyProperty(property);
                    }
                }
                else if (property.Owner != currentPlayer)
                {
                    property.PayRent(currentPlayer);
                }
                else
                {
                    Console.WriteLine("Це ваша власність.");
                }
            }

           
            if (currentPlayer.IsBankrupt)
            {
                Console.WriteLine($"Гравець {currentPlayer.Name} збанкрутував і вибуває з гри.");

                
                _monopolyPlayers.Remove(currentPlayer);

                if (_monopolyPlayers.Count == 1)
                {
                    Console.WriteLine($"\nГра закінчена! Переможець: {_monopolyPlayers[0].Name}");
                    return;
                }
            }

            // Переходимо до наступного гравця
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _monopolyPlayers.Count;
        }

        public void ShowStatus()
        {
            Console.WriteLine("\n=== СТАТУС ГРИ ===");
            foreach (var player in _monopolyPlayers.OrderByDescending(p => p.Money))
            {
                Console.WriteLine($"Гравець: {player.Name}, Фішка: {player.Token.Name}");
                Console.WriteLine($"Позиція: {player.Token.Position}, Гроші: {player.Money}$");
                if (player.Properties.Count > 0)
                {
                    Console.WriteLine("Власність: " + string.Join(", ", player.Properties.Select(p => p.Name)));
                }
                else
                {
                    Console.WriteLine("Власність: немає");
                }
                Console.WriteLine();
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
                Console.WriteLine("\n=== МОНОПОЛІЯ ===");
                Console.WriteLine("1. Додати гравця");
                Console.WriteLine("2. Показати правила");
                Console.WriteLine("3. Почати раунд");
                Console.WriteLine("4. Показати статус гри");
                Console.WriteLine("5. Повернутися в головне меню");

                Console.Write("Виберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPlayer();
                        break;
                    case "2":
                        AnnounceRules();
                        break;
                    case "3":
                        PlayRound();
                        break;
                    case "4":
                        ShowStatus();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }
    }
}
