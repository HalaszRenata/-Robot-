using System;
using System.Collections.Generic;

//Se cere realizarea unei aplicații console în C#
//care să modeleze un robot uriaș ucigaș care poate elimina ținte de diferite tipuri (animale, oameni, supereroi) pe planeta Pământ.
//Aplicația va citi de la utilizator numărul total de ținte ce urmează să fie omorâte și va genera aleatoriu un număr de animale, oameni și supereroi care trebuie eliminați.
//Robotul va parcurge aceste ținte și le va elimina până când nu mai există nicio formă de viață pe Pământ.
//La final, aplicația va afișa un mesaj indicând dacă mai există viață pe Pământ.

namespace GiantKillerRobotApp
{
    // Enum pentru Intensitatea Laserului
    public enum Intensity
    {
        Stun,
        Kill
    }

    // Clasă abstractă pentru entitățile țintite
    public abstract class Target
    {
        public bool IsAlive { get; set; } = true;
    }

    // Ținte specifice
    public class Animal : Target { }
    public class Human : Target { }
    public class Superhero : Target { }

    // Clasa Planetă
    public class Planet
    {
        public List<Target> LifeForms { get; } = new List<Target>();

        public bool ContainsLife => LifeForms.Exists(lifeForm => lifeForm.IsAlive);
    }

    // Clasa Robot Ucigaș Uriaș
    public class GiantKillerRobot
    {
        public Intensity EyeLaserIntensity { get; set; }
        public List<Target> Targets { get; set; }
        private int currentTargetIndex = 0;

        public bool Active { get; set; } = true;

        public Target CurrentTarget => Targets[currentTargetIndex];

        public void Initialize()
        {
            Console.WriteLine("Robotul a fost initializat.");
        }

        public void FireLaserAt(Target target)
        {
            if (EyeLaserIntensity == Intensity.Kill)
            {
                target.IsAlive = false;
                Console.WriteLine($"S-a tras cu laserul în {target.GetType().Name}. Tinta este acum moartă.");
            }
            else
            {
                Console.WriteLine("Intensitatea laserului nu este setată pe kill.");
            }
        }

        public void AcquireNextTarget()
        {
            do
            {
                currentTargetIndex++;
                if (currentTargetIndex >= Targets.Count)
                {
                    currentTargetIndex = 0;
                }
            } while (!CurrentTarget.IsAlive);

            Console.WriteLine($"S-a achizitionat o noua tinta: {CurrentTarget.GetType().Name}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Inițializarea robotului
            GiantKillerRobot robot = new GiantKillerRobot();
            robot.Initialize();
            robot.EyeLaserIntensity = Intensity.Kill;

            // Citirea numărului total de ținte de la utilizator
            int totalTargetsToKill = GetNumberOfTargets("total de tinte care urmeaza a fi omorate");

            // Generarea aleatorie a numărului de ținte pentru fiecare tip
            Random random = new Random();
            int numAnimals = random.Next(totalTargetsToKill + 1);
            int numHumans = random.Next(totalTargetsToKill + 1 - numAnimals);
            int numSuperheroes = totalTargetsToKill - numAnimals - numHumans;

            Console.WriteLine($"Număr de animale: {numAnimals}");
            Console.WriteLine($"Număr de oameni: {numHumans}");
            Console.WriteLine($"Număr de supereroi: {numSuperheroes}");

            // Adăugarea țintelor în lista robotului
            robot.Targets = new List<Target>();
            for (int i = 0; i < numAnimals; i++)
            {
                robot.Targets.Add(new Animal());
            }
            for (int i = 0; i < numHumans; i++)
            {
                robot.Targets.Add(new Human());
            }
            for (int i = 0; i < numSuperheroes; i++)
            {
                robot.Targets.Add(new Superhero());
            }

            // Inițializarea Pământului
            Planet earth = new Planet();
            earth.LifeForms.AddRange(robot.Targets);

            // Bucla de operațiune a robotului
            while (robot.Active && earth.ContainsLife)
            {
                if (robot.CurrentTarget.IsAlive)
                {
                    robot.FireLaserAt(robot.CurrentTarget);
                }
                else
                {
                    robot.AcquireNextTarget();
                }
            }

            if (earth.ContainsLife)
            {
                Console.WriteLine("Operatiunea robotului s-a incheiat. Pamantul contine inca viata.");
            }
            else
            {
                Console.WriteLine("Operatiunea robotului s-a incheiat. Pamantul nu mai contine nicio forma de viata.");
                Console.ReadLine();
            }
        }

        static int GetNumberOfTargets(string targetType)
        {
            Console.Write($"Introduceti numarul de {targetType}: ");
            return int.Parse(Console.ReadLine());
        }
    }
}

