using System;
using System.Reflection;
using static Racer.Car;

namespace Racer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Car racer game! Press Enter to start");
            Console.ReadKey();
            Console.WriteLine("The race has started!\n\n Press Enter to get updates on the cars position and speed!");

            Car Car1 = new Car
            {
                name = "Volvo",
                speed = 120,
                distanceSoFar = 0
            };
            Car Car2 = new Car
            {
                name = "Mercedes",
                speed = 120,
                distanceSoFar = 0
            };
            Car Car3 = new Car
            {
                name = "Audi",
                speed = 120,
                distanceSoFar = 0
            };

            var carRace1 = RaceStart(Car1);
            var carRace2 = RaceStart(Car2);
            var carRace3 = RaceStart(Car3);
            var carStatus = RaceStatus(new List<Car> { Car1, Car2, Car3 });

            var CarRaces = new List<Task> { carRace1, carRace2, carRace3, carStatus };

            int position = 0;

            while (CarRaces.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(CarRaces);
                if (finishedTask == carRace1)
                {
                    position += 1;
                    PrintCar(Car1, position);

                }
                else if (finishedTask == carRace2)
                {
                    position += 1;
                    PrintCar(Car2, position);
                }
                else if (finishedTask == carRace3)
                {
                    position += 1;
                    PrintCar(Car3, position);
                }
                else if (position == 3)
                {
                    Console.WriteLine("\nAll have finished, simulation over.");
                }

                await finishedTask;
                CarRaces.Remove(finishedTask);
            }
        }


        public async static Task RaceStatus(List<Car> cars)
        {
            while (true)
            {

                DateTime start = DateTime.Now;

                bool gotKey = false;

                while ((DateTime.Now - start).TotalSeconds < 2)
                {
                    if (Console.KeyAvailable)
                    {
                        gotKey = true;
                        break;
                    }
                }

                if (gotKey)
                {
                    Console.WriteLine("");
                    Console.ReadKey();
                    cars.ForEach(c =>
                    {
                        string distanceSF = c.distanceSoFar.ToString("0");

                        Console.WriteLine($"{c.name} has driven {distanceSF} meters and is going {c.speed}km/h.");
                    });
                    gotKey = false;
                }

                // Måste delaya här för att övriga tasks ska kunna gå klart.
                await Task.Delay(10);

                // När alla egg's remaining time är noll, avsluta simuleringen

                var totalRemaining = cars.Select(c => c.distanceSoFar).Sum();

                if (totalRemaining >= 30000m)
                {
                    return;
                }
            }
        }

        public async static Task<Car> RaceStart(Car car)
        {
            int timeWarp = 30;
            while (true)
            {
                await Wait();
                car.distanceSoFar += (((car.speed * 1000) * timeWarp) / 3600m);
                Event(car);

                if (car.delay > 0)
                {
                    await Wait(car.delay);
                    car.delay = 0;
                }
                if (car.distanceSoFar >= 10000m)
                {
                    return car;
                }
            }
        }
        public static void Event(Car car)
        {
            Random random = new Random();
            int n = random.Next(50);
            //Console.WriteLine(n);
            if (n == 36)
            {
                Console.WriteLine($"{car.name} has ran out of gas! It will take 30 seconds to refuel!");
                car.delay += 30;
            }
            else if (n == 1 || n == 2)
            {
                Console.WriteLine($"{car.name} has lost a tire! The change will take 20 seconds!");
                car.delay += 20;
            }
            else if (n >= 43 && n <= 47)
            {
                Console.WriteLine($"A bird crashed into {car.name}'s windshield! The removal will take 10 seconds!");
                car.delay += 10;
            }
            else if (n >= 10 && n <= 14)
            {
                car.speed = car.speed - 1;
                Console.WriteLine($"{car.name}'s engine made a wierd sound! The speed has gone down to {car.speed}km/h");
            }
            else
            {
                car.delay = 0;
            }
        }

        public async static Task Wait(int delay = 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay));
        }

        public static void PrintCar(Car car, int position)
        {
            Console.WriteLine($"\n\ncar {car.name} has finished the race in position {position}\n\n");
        }

    }

}