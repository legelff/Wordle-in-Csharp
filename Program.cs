using System;
using System.Collections.Generic;
using System.IO;

namespace wordle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome to lightweight wordle implemented in C#!\n");

                Console.Write("Start? [y/n]: ");

                string ansString = Console.ReadLine().ToString().Trim().ToLower();

                bool answer = ansString == "y" ? true : false;

                Console.Write("Toggle extra details (unused letters, correct letters, incorrect position letters, incorrect letters)? [y/n]: ");

                string detString = Console.ReadLine().ToString().Trim().ToLower();

                bool details = detString == "y" ? true : false;
                Console.WriteLine();

                if (answer && (ansString == "y" || ansString == "n") && (detString == "y" || detString == "n"))
                {
                    string[] words = File.ReadAllLines(@"words.txt");
                    Random rand = new Random();
                    int randomIndex = rand.Next(0, words.Length);
                    string word = words[randomIndex];

                    List<char> allLetters = new List<char>
                {
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z'
                };

                    List<char> unusedLetters = new List<char>
                {
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                    'U', 'V', 'W', 'X', 'Y', 'Z'
                };

                    List<char> usedLetters = new List<char>();

                    List<char> incorrectLetters = new List<char>();
                    List<char> incorrectPosition = new List<char>();
                    List<char> correctLetters = new List<char>();

                    bool wordNotFound = true;
                    int tries = 5;
                    string[] letterPos = new string[5];

                    Console.WriteLine("Good luck! type 'exit' to leave");

                    while (wordNotFound || tries <= 0)
                    {
                        if (tries >= 0)
                        {
                            Console.Write("Your guess: ");
                            string guess = Console.ReadLine().Trim().ToUpper();

                            if (guess != "EXIT")
                            {
                                while (guess.Length != 5)
                                {
                                    Console.Write("Word length too little/big, guess again: ");
                                    guess = Console.ReadLine().Trim().ToUpper();
                                }
                            }


                            if (guess != "EXIT")
                            {
                                if (guess == word)
                                {
                                    Console.WriteLine("You got it!");
                                    Console.WriteLine("Some interesting stats:");
                                    Console.WriteLine($"Attempts: {tries}");
                                    Console.WriteLine($"Total letters used: {usedLetters.Count}");
                                    Console.WriteLine($"Total incorrect letters guessed: {incorrectLetters.Count}");
                                    Console.WriteLine($"Total partially correct letters guessed: {incorrectPosition.Count}");
                                    wordNotFound = false;
                                    break;
                                }

                                for (int i = 0; i < 5; i++)
                                {
                                    if (word.Contains(guess[i].ToString()))
                                    {
                                        if (guess[i] == word[i])
                                        {
                                            if (!usedLetters.Contains(guess[i]))
                                            {
                                                unusedLetters.Remove(guess[i]);
                                                correctLetters.Add(guess[i]);
                                                usedLetters.Add(guess[i]);
                                            }

                                            else if (incorrectPosition.Contains(guess[i]))
                                            {
                                                correctLetters.Add(guess[i]);
                                                incorrectPosition.Remove(guess[i]);
                                            }

                                            letterPos[i] = guess[i].ToString();

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write(guess[i].ToString() + " ");
                                            Console.ResetColor();
                                        }

                                        else
                                        {
                                            if (!usedLetters.Contains(guess[i]))
                                            {
                                                unusedLetters.Remove(guess[i]);
                                                incorrectPosition.Add(guess[i]);
                                                usedLetters.Add(guess[i]);
                                            }

                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.Write(guess[i].ToString() + " ");
                                            Console.ResetColor();
                                        }
                                    }

                                    else
                                    {
                                        if (!usedLetters.Contains(guess[i]))
                                        {
                                            unusedLetters.Remove(guess[i]);
                                            incorrectLetters.Add(guess[i]);
                                            usedLetters.Add(guess[i]);
                                        }

                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        Console.Write(guess[i].ToString() + " ");
                                        Console.ResetColor();
                                    }
                                }

                                Console.WriteLine();

                                for (int i = 0; i < allLetters.Count; i++)
                                {
                                    if (correctLetters.Contains(allLetters[i]))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(allLetters[i].ToString() + " ");
                                        Console.ResetColor();
                                    }

                                    else
                                    {
                                        if (incorrectPosition.Contains(allLetters[i]))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.Write(allLetters[i].ToString() + " ");
                                            Console.ResetColor();
                                        }

                                        else
                                        {
                                            if (usedLetters.Contains(allLetters[i]))
                                            {
                                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                                Console.Write(allLetters[i].ToString() + " ");
                                                Console.ResetColor();
                                            }

                                            else
                                            {
                                                Console.Write(allLetters[i].ToString() + " ");
                                            }
                                        }
                                    }
                                }

                                Console.WriteLine();

                                if (details)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine($"Unused letters: {string.Join(", ", unusedLetters)}");

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"Correct letters: {string.Join(", ", correctLetters)}");

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"Incorrect position letters: {string.Join(", ", incorrectPosition)}");

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Incorrect letters: {string.Join(", ", incorrectLetters)}");

                                    Console.ResetColor();
                                }

                                Console.WriteLine($"Remaining attempts: {tries}");
                                tries--;
                                Console.WriteLine();
                            }

                            else
                            {
                                Console.WriteLine($"The word was: {word}");
                                Console.WriteLine("Nice try!");
                                wordNotFound = false;
                                break;
                            }
                        }

                        else
                        {
                            Console.WriteLine($"The word was: {word}");
                            Console.WriteLine("Nice try!");
                            wordNotFound = false;
                            break;
                        }
                    }
                }

                else
                {
                    if ((ansString != "y" || ansString != "n") || (detString != "y" || detString != "n"))
                    {
                        Console.WriteLine("Answer with y or n only!");
                    }

                    else
                    {
                        Console.WriteLine("Why did you open the game in the first place then?");
                    }
                }

                Console.ReadKey();
            }
            

            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadKey();
            }
        }
    }
}
