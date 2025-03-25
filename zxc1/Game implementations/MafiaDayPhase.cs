using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zxc1.Interfaces;
using zxc1.Player_implementation;

namespace zxc1.Game_implementations
{
    public class MafiaDayPhaseService : IDayPhaseService
    {
        public string ExecuteDayPhase(Dictionary<Role, List<MafiaPlayer>> roles, List<MafiaPlayer> alivePlayers, int dayCount)
        {
            Console.WriteLine($"\n--- День {dayCount} ---");
            Console.WriteLine("Жителі обговорюють, хто може бути мафією.");

            Console.WriteLine($"Живі гравці: {string.Join(", ", alivePlayers.Select(p => p.Name))}");

            
            Console.WriteLine("Після обговорення жителі голосують. Натисніть Enter...");
            Console.ReadLine();

            MafiaPlayer votePlayer = null;
            while (true)
            {
                Console.Write("Кого вирішили повісити? (або 'нікого'): ");
                string voteName = Console.ReadLine();

                if (voteName.ToLower() == "нікого")
                {
                    Console.WriteLine("Жителі не змогли дійти згоди. Нікого не повісили.");
                    break;
                }

                votePlayer = alivePlayers.FirstOrDefault(p => p.Name == voteName);

                if (votePlayer == null)
                {
                    Console.WriteLine("Такого живого гравця немає. Спробуйте ще раз.");
                }
                else
                {
                    Console.WriteLine($"{voteName} був повішений жителями міста.");
                    votePlayer.Kill();
                    alivePlayers.Remove(votePlayer);
                    break;
                }
            }


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

            return null;
        }
    }

}
