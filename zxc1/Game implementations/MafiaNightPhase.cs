using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;
using zxc1.Player_implementation;

namespace zxc1.Game_implementations
{
    public class MafiaNightPhase : INightPhaseService
    {
        public string ExecuteNightPhase(Dictionary<Role, List<MafiaPlayer>> roles, List<MafiaPlayer> alivePlayers, int dayCount)
        {
            Console.WriteLine($"\n--- Ніч {dayCount} ---");

            List<MafiaPlayer> mafiaAlive = roles[Role.Mafia].Where(p => alivePlayers.Contains(p)).ToList();
            if (mafiaAlive.Count == 0)
            {
                return "мирні";
            }

            List<MafiaPlayer> civiliansAlive = alivePlayers.Where(p => !roles[Role.Mafia].Contains(p)).ToList();
            if (civiliansAlive.Count == 0)
            {
                return "мафія";
            }

           
            Console.WriteLine("Мафія прокидається. Натисніть Enter...");
            Console.ReadLine();
            Console.Clear(); 

            Console.WriteLine($"Живі гравці: {string.Join(", ", alivePlayers.Select(p => p.Name))}");

            MafiaPlayer target = null;
            while (target == null)
            {
                Console.Write("Кого вбиває мафія? ");
                string targetName = Console.ReadLine();
                target = alivePlayers.FirstOrDefault(p => p.Name == targetName);

                if (target == null)
                {
                    Console.WriteLine("Такого живого гравця немає. Спробуйте ще раз.");
                }
            }

            Console.Clear();


            MafiaPlayer? commissionerAlive = roles[Role.Commissioner].FirstOrDefault(p => alivePlayers.Contains(p));
            if (commissionerAlive != null)
            {
                Console.WriteLine("Комісар прокидається. Натисніть Enter...");
                Console.ReadLine();
                Console.Clear(); 

                Console.WriteLine($"Живі гравці: {string.Join(", ", alivePlayers.Select(p => p.Name))}");
                MafiaPlayer checkPlayer = null;
                while (checkPlayer == null)
                {
                    Console.Write("Кого перевіряє комісар? ");
                    string checkName = Console.ReadLine();
                    checkPlayer = alivePlayers.FirstOrDefault(p => p.Name == checkName);

                    if (checkPlayer == null)
                    {
                        Console.WriteLine("Такого живого гравця немає. Спробуйте ще раз.");
                    }
                    else
                    {
                        string role = roles[Role.Mafia].Contains(checkPlayer) ? "мафія" : "не мафія";
                        Console.WriteLine($"Гравець {checkName} - {role}");
                    }
                }

                Console.WriteLine("Натисніть Enter, щоб продовжити...");
                Console.ReadLine();
                Console.Clear(); 
            }


            MafiaPlayer? doctorAlive = roles[Role.Doctor].FirstOrDefault(p => alivePlayers.Contains(p));
            if (doctorAlive != null)
            {
                Console.WriteLine("Лікар прокидається. Натисніть Enter...");
                Console.ReadLine();
                Console.Clear(); 

                Console.WriteLine($"Живі гравці: {string.Join(", ", alivePlayers.Select(p => p.Name))}");
                MafiaPlayer healPlayer = null;
                while (healPlayer == null)
                {
                    Console.Write("Кого лікує лікар? ");
                    string healName = Console.ReadLine();
                    healPlayer = alivePlayers.FirstOrDefault(p => p.Name == healName);

                    if (healPlayer == null)
                    {
                        Console.WriteLine("Такого живого гравця немає. Спробуйте ще раз.");
                    }
                }

                if (healPlayer == target)
                {
                    target = null; 
                }

                Console.WriteLine("Натисніть Enter, щоб продовжити...");
                Console.ReadLine();
                Console.Clear(); 
            }

            
            Console.WriteLine("\n--- Результат ночі ---");
            if (target != null)
            {
                Console.WriteLine($"\nВранці жителі знаходять тіло {target.Name}.");
                target.Kill();
                alivePlayers.Remove(target);
            }
            else
            {
                Console.WriteLine("\nВранці всі жителі прокидаються живими.");
            }

            return null;
        }
    }
}