using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSG.svc;

namespace WSG
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WordScrambleGameClient svc = new WordScrambleGameClient();

                Console.WriteLine("Hello. Please enter your name.");
                string playerName = Console.ReadLine();

                Console.WriteLine("Welcome, " + playerName + "!" + " Would you like to host? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    svc.isGameBeingHosted();
                    Console.WriteLine("Please enter word to scramble.");
                    string inputWord = Console.ReadLine();
                    string scrambledWord = svc.hostGame(playerName, inputWord);

                    Console.WriteLine("You are hosting. WORD: " + inputWord + ". SCRAMBLED: " + scrambledWord + ".");
                    Console.ReadKey();
                }

                Console.WriteLine("Would you like to play? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Word gameWords = svc.join(playerName);
                    Console.WriteLine("Scramble this word: " + gameWords.scrambledWord + ".");

                    string guessedWord;
                    bool gameOver = false;
                    while (!gameOver)
                    {
                        guessedWord = Console.ReadLine();
                        gameOver = svc.guessWord(playerName, guessedWord, gameWords.unscrambledWord);
                        if (!gameOver)
                            Console.WriteLine("Incorrect. Try again.");
                    }

                    Console.WriteLine("CORRECT! You win!");
                    Console.ReadLine();
                }
                
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
