using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WSG
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IWordScrambleGame
    {
        [OperationContract]
        [FaultContract(typeof(NoHostException))]
        bool isGameBeingHosted();

        [OperationContract]
        [FaultContract(typeof(GameAlreadyHostedException))]
        string hostGame(string playerName, string wordToScramble);

        [OperationContract]
        [FaultContract(typeof(NoHostException))]
        [FaultContract(typeof(MaxPlayersException))]
        [FaultContract(typeof(HostPlayGameException))]
        //[FaultContract(typeof(NoHostException))]
        Word join(string playerName);

        [OperationContract]
        [FaultContract(typeof(UnknownPlayerException))]
        bool guessWord(string playerName, string guessedWord, string unscrambledWord);
    }

    [DataContract]
    public class Word
    {
        [DataMember]
        public string scrambledWord;

        [DataMember]
        public string unscrambledWord;
    }

    [DataContract]
    public class GameAlreadyHostedException
    {
        [DataMember]
        public string alreadyHostedReason;

        [DataMember]
        public bool alreadyHostedStatus;
    }

    [DataContract]
    public class MaxPlayersException
    {
        [DataMember]
        public string maxPlayersReason;

        [DataMember]
        public bool maxPlayersStatus;
    }

    [DataContract]
    public class HostPlayGameException
    {
        [DataMember]
        public string hostPlayReason;

        [DataMember]
        public bool hostPlayStatus;
    }

    [DataContract]
    public class NoHostException
    {
        [DataMember]
        public string noHostReason;

        [DataMember]
        public bool noHostStatus;
    }

    [DataContract]
    public class UnknownPlayerException
    {
        [DataMember]
        public string unknownPlayerReason;

        [DataMember]
        public bool unknownPlayerStatus;
    }

    [DataContract]
    public class GameOverException
    {
        [DataMember]
        public string gameOverReason;

        [DataMember]
        public bool gameOverStatus;
    }
}
