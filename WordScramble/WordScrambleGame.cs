using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WSG
{
    public class WordScrambleGame : IWordScrambleGame
    {
        private static bool isGameOver = false;
        private static int currentPlayers = 0;
        private static string userHostingGame = null;
        private const int maxPlayers = 3;
        private static string hostName = null;
        private static List<String> activePlayers = new List<string>();
        private static Word gameWords;

        [OperationBehavior]
        public bool isGameBeingHosted()
        {
            if (userHostingGame != null)
                return true;
            else
            {
                currentPlayers++;
                return false;
            }
        }

        [OperationBehavior]
        public string hostGame(string playerName, string wordToScramble)
        {
            if (userHostingGame != null)
            {
                GameAlreadyHostedException fault = new GameAlreadyHostedException();
                fault.alreadyHostedStatus = true;
                fault.alreadyHostedReason = "Game is already hosted.";
                FaultReason fr = new FaultReason(fault.alreadyHostedReason);
                throw new FaultException<GameAlreadyHostedException>(fault, fr);
            }
            else
            {
                string scrambledWord = scrambleWord(wordToScramble);
                string normalWord = wordToScramble;
                gameWords = new Word();
                gameWords.unscrambledWord = normalWord;
                gameWords.scrambledWord = scrambledWord;

                hostName = playerName.ToLower();
                userHostingGame = "yes";

                return scrambledWord;
            }
        }

        [OperationBehavior]
        public Word join(string playerName)
        {
            if (maxPlayers == currentPlayers)
            {
                MaxPlayersException fault = new MaxPlayersException();
                fault.maxPlayersStatus = true;
                fault.maxPlayersReason = "Game is full. No more players allowed.";
                FaultReason fr = new FaultReason(fault.maxPlayersReason);
                throw new FaultException<MaxPlayersException>(fault, fr);
            }
            else if (hostName == playerName.ToLower())
            {
                HostPlayGameException fault = new HostPlayGameException();
                fault.hostPlayStatus = true;
                fault.hostPlayReason = "Host cannot play game.";
                FaultReason fr = new FaultReason(fault.hostPlayReason);
                throw new FaultException<HostPlayGameException>(fault, fr);
            }
            else if (userHostingGame == null)
            {
                NoHostException fault = new NoHostException();
                fault.noHostStatus = true;
                fault.noHostReason = "No host. No game.";
                FaultReason fr = new FaultReason(fault.noHostReason);
                throw new FaultException<NoHostException>(fault, fr);
            }
            else
            {
                currentPlayers++;
                activePlayers.Add(playerName);

                return gameWords;
            }
        }

        public bool guessWord(string playerName, string guessedWord, string answer)
        {
            ///
            /// if you want to test the PlayerUnknownException then do the following:
            /// 01. host
            /// 02. add another normal player to play game, call him jim
            /// 03. uncomment below's activePlayers.Remove("jim");
            /// 04. Guess word and exception will occur
            ///

            //activePlayers.Remove("jim");

            string status = "unknown";
            foreach (var item in activePlayers)
            {
                if (playerName == item)
                    status = "known";
            }

            if (status == "unknown")
            {
                UnknownPlayerException fault = new UnknownPlayerException();
                fault.unknownPlayerStatus = true;
                fault.unknownPlayerReason = "Unknown player playing. Get out!";
                FaultReason fr = new FaultReason(fault.unknownPlayerReason);
                throw new FaultException<UnknownPlayerException>(fault, fr);
            }
            else if (isGameOver)
            {
                GameOverException fault = new GameOverException();
                fault.gameOverReason = "Game is over. Can't answer.";
                fault.gameOverStatus = true;
                FaultReason fr = new FaultReason(fault.gameOverReason);
                throw new FaultException<GameOverException>(fault, fr);
            }
            else
            {
                if (guessedWord == answer)
                {
                    userHostingGame = null;
                    isGameOver = true;
                    currentPlayers = 0;

                    return true;
                }
                else
                    return false;
            }
        }

        private string scrambleWord(string word)
        {
            char[] chars = word.ToArray();
            Random r = new Random(2011);
            for (int i = 0; i < chars.Length; i++)
            {
                int randomindex = r.Next(0, chars.Length);
                char temp = chars[randomindex];
                chars[randomindex] = chars[i];
                chars[i] = temp;
            }
            return new string(chars);
        }
    }
}
