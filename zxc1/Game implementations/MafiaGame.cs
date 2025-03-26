using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Base_classes;
using zxc1.Interfaces;
using zxc1.observerPattern;
using zxc1.Player_implementation;

namespace zxc1.Game_implementations
{
    public class MafiaGame : IGame
    {
        private readonly List<IPlayer> _players;
        private readonly IRoleDistributor _roleDistributor;
        private readonly INightPhaseService _nightPhaseService;
        private readonly IDayPhaseService _dayPhaseService;
        private bool _rulesRead = false;

        private readonly GameRulePublisher _rulePublisher;
        private List<IDisposable> _ruleSubscriptions = new List<IDisposable>();

      

        public MafiaGame(
            IRoleDistributor roleDistributor,
            INightPhaseService nightPhaseService,
            IDayPhaseService dayPhaseService)
        {
            _players = new List<IPlayer>();
            _roleDistributor = roleDistributor;
            _nightPhaseService = nightPhaseService;
            _dayPhaseService = dayPhaseService;
            _rulePublisher = new GameRulePublisher();

            _ruleSubscriptions.Add(_rulePublisher.Subscribe(new ConsoleRuleObserver()));

            _rulePublisher.RulesAnnounced += (sender, args) =>
            {
                _rulesRead = true;
                Console.WriteLine("\nПравила прочитано! Тепер ви можете почати гру.");
            };
        }

        private void AnnounceRules()
        {
            string rules = @"
Мета гри:
Виявити та усунути мафію або захопити місто, якщо ви на боці мафії.

Основні правила:
Гра ділиться на нічні та денні фази.
Вночі мафія та спеціальні ролі виконують свої дії, не розкриваючи себе.
Вдень усі гравці обговорюють та голосують за підозрюваного для виключення з гри.
Гравець вибуває, якщо отримує більшість голосів.
Гра триває, поки всі мафіозі не будуть виключені (перемога мирних) або кількість мафії зрівняється з мирними (перемога мафії).

Бажаємо приємної гри!
";

            _rulePublisher.PublishRules("Мафія", rules);
        }
        public void AddPlayer()
        {
            Console.Write("Введіть ім'я гравця: ");
            string name = Console.ReadLine();
            _players.Add(new Player(name));
            Console.WriteLine($"Гравець {name} доданий!");
        }

        public void Dispose()
        {
            foreach (var subscription in _ruleSubscriptions)
            {
                subscription.Dispose();
            }
        }
        public void StartGame()
        {
            if (!_rulesRead)
            {
                Console.WriteLine("\nУВАГА! Перед початком гри необхідно обов'язково прочитати правила.");
                Console.WriteLine("Виберіть опцію 'Показати правила' в головному меню.");
                return;
            }
            try
            {
                Dictionary<Role, List<MafiaPlayer>> roles = _roleDistributor.DistributeRoles(_players);
                List<MafiaPlayer> allMafiaPlayers = roles.Values.SelectMany(list => list).ToList();
                List<MafiaPlayer> alivePlayers = new List<MafiaPlayer>(allMafiaPlayers);

                ShowRoles(roles);

                int dayCount = 1;
                string winner = null;

                while (winner == null)
                {
                    
                    Console.Clear();

                    winner = _nightPhaseService.ExecuteNightPhase(roles, alivePlayers, dayCount);
                    if (winner != null)
                    {
                        break;
                    }

                    Console.WriteLine("Натисніть Enter, щоб продовжити до денної фази...");
                    Console.ReadLine();

                   
                    Console.Clear();

                    winner = _dayPhaseService.ExecuteDayPhase(roles, alivePlayers, dayCount);
                    if (winner != null)
                    {
                        break;
                    }

                    dayCount++;
                    Console.WriteLine("Натисніть Enter, щоб продовжити до наступного дня...");
                    Console.ReadLine();

                    
                    Console.Clear();
                }

                Console.WriteLine($"\nГра закінчена! Перемогли {winner}!");
                Console.WriteLine("\nРолі гравців:");
                foreach (var roleEntry in roles)
                {
                    string roleStr = RoleToString(roleEntry.Key);
                    Console.WriteLine($"{roleStr}: {string.Join(", ", roleEntry.Value.Select(p => p.Name))}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ShowRoles(Dictionary<Role, List<MafiaPlayer>> roles)
        {
            List<MafiaPlayer> allPlayers = roles.Values.SelectMany(list => list).ToList();

            foreach (MafiaPlayer player in allPlayers)
            {
                Console.WriteLine($"\nГравець {player.Name}, натисніть Enter, щоб побачити свою роль...");
                Console.ReadLine();

               
                Console.Clear();

                string roleStr = RoleToString(player.Role);
                Console.WriteLine($"{player.Name}, ваша роль: {roleStr}");

                if (player.Role == Role.Mafia)
                {
                    List<MafiaPlayer> otherMafia = roles[Role.Mafia].Where(p => p.Name != player.Name).ToList();
                    if (otherMafia.Count > 0)
                    {
                        Console.WriteLine($"Інші члени мафії: {string.Join(", ", otherMafia.Select(p => p.Name))}");
                    }
                }

                Console.WriteLine("Натисніть Enter, щоб очистити екран...");
                Console.ReadLine();

                Console.Clear();
            }
        }

        private string RoleToString(Role role)
        {
            switch (role)
            {
                case Role.Civilian: return "мирний житель";
                case Role.Mafia: return "мафія";
                case Role.Commissioner: return "комісар";
                case Role.Doctor: return "лікар";
                default: return "невідома роль";
            }
        }

        public void PlayGame()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== МАФІЯ ===");
                Console.WriteLine("1. Додати гравця");
                Console.WriteLine("2. Почати гру");
                Console.WriteLine("3. Правила гри");
                Console.WriteLine("4. Повернутися в головне меню");

                Console.Write("Виберіть опцію: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddPlayer();
                        break;
                    case "2":
                        StartGame();
                        break;
                    case "3":
                        AnnounceRules();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }

             
                if (choice != "2" && choice != "4") 
                {
                    Console.WriteLine("Натисніть Enter, щоб продовжити...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
    }
}