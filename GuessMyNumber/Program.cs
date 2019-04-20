using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessMyNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayGame();
        }

        static void PlayGame()
        {
            Console.CursorVisible = false;

            ConsoleKeyInfo PromptForMenu()
            {
                string prompt;
                prompt = "Welcome to Guess My Number Game:\n\t1 - Bisection Algorithm\n\t2 - Human Plays\n\t3 - Computer Plays\n\tEsc - quit\n\nPress option on keyboard... ";

                Console.Write(prompt);
                return Console.ReadKey();
            }

            ConsoleKeyInfo input = PromptForMenu();
            string err = "";

            while (!input.Key.Equals(ConsoleKey.Escape))
            {
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        DemoBisection(1, 10);
                        break;
                    case ConsoleKey.D2:
                        HumanPlays(1, 100);
                        break;
                    case ConsoleKey.D3:
                        ComputerPlays(1, 1000);
                        break;
                    default:
                        err = "Invalid choice. Try again!\n";
                        break;
                }
                Console.Clear();
                Console.Write(err);
                input = PromptForMenu();
                err = "";
            }
            Console.Clear();
            Console.WriteLine("Thank you for Playing!\n");
        }

        static void DemoBisection(int low, int high)
        {
            int value;
            int iterations = 0;

            void PrintSteps(int start, int end, int i)
            {
                Console.WriteLine($"Step {i}");
                Console.WriteLine($"  Looking for {value} in");
                Console.WriteLine($"  [{String.Join(",", Enumerable.Range(start, end - start + 1))}]\n");
            }

            int binarySearch(int start, int end)
            {
                iterations++;
                PrintSteps(start, end, iterations);
                int midPoint = (start + end) / 2;
                if (midPoint == value || start >= end) return midPoint;
                else if (value > midPoint)
                {
                    return binarySearch(midPoint + 1, end);
                }
                else
                {
                    return binarySearch(start, midPoint - 1);
                }
            }

            do
            {
                value = PromptForInput(low, high);
                Console.WriteLine($"Found number {binarySearch(low, high)} in {iterations} iterations.\n");
                iterations = 0;
            } while (PlayAgainPrompt());
        }

        static void HumanPlays(int low, int high)
        {
            Random r = new Random();

            int guess, value, iterations;
            var stats = new List<int>();

            do
            {
                value = r.Next(low, high + 1);
                iterations = 0;
                guess = -1;

                do
                {
                    iterations++;
                    guess = PromptForInput(low, high, value, guess);
                }
                while (value != guess);

                Console.WriteLine($"\nYou guessed {value} in {iterations} iterations.\n");
                stats.Add(iterations);

                Console.WriteLine($"You played {stats.Count} time(s), your avg is {stats.Average()} iterations.\n");

            } while (PlayAgainPrompt());
        }

        static void ComputerPlays(int low, int high)
        {
            int value, guess, iterations, newLow, newHigh;
            do
            {
                value = PromptForInput(low, high);
                guess = (low + high) >> 1;
                iterations = 0;
                newLow = low;
                newHigh = high;
                while (value != guess)
                {
                    Console.Clear();
                    Console.WriteLine($"Computer guessed: {guess}");
                    Console.WriteLine($"Looking for {value} in [{newLow}, {newHigh}]\n");

                   Console.Write($"Press H for High or L for Low");
                    var pressedKey = Console.ReadKey(true).Key;
                    if (pressedKey == ConsoleKey.H && guess > value)
                    {
                        newHigh = guess - 1;
                        guess = (newLow + newHigh) >> 1;
                        iterations++;
                    }
                    else if (pressedKey == ConsoleKey.L && guess < value)
                    {
                        newLow = guess + 1;
                        guess = (newLow + newHigh) >> 1;
                        iterations++;
                    }
                }
                iterations++;
               Console.WriteLine("\n");
               Console.WriteLine($"Computer guessed {value} in {iterations} iterations.\n");
            } while (PlayAgainPrompt());
        }

        static int PromptForInput(int low, int high, int target = -1, int guess = -1)
        {
            Console.CursorVisible = true;
            int value;
            do
            {
                Console.Clear();
                string prompt = $"Enter number between {low} and {high}: ";
                Console.Write(prompt);
                if (target != guess && guess >= low)
                {
                    string lowHigh = (guess < target) ? "low" : "high";
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                    Console.Write($"Your guess {guess} is too {lowHigh}");
                    Console.SetCursorPosition(prompt.Length, Console.CursorTop - 1);
                }
            }
            while (!int.TryParse(Console.ReadLine(), out value) || value < low || value > high);
            Console.CursorVisible = false;

            return value;
        }

        static bool PlayAgainPrompt()
        {
            Console.Write("Press any key to play again or Escape exit...");
            return Console.ReadKey(true).Key != ConsoleKey.Escape;
        }
    }
}
