using System;

namespace WBAssistantF.Module.AutoPlay
{
    [Serializable]
    public class PlayListItem
    {
        public string FilePath;

        public bool ShufflePlay = false;

        //public MTime[] PlayTIme = new MTime { };
        public bool StartFromLastPlayTime = true;
    }

    public enum MTime
    {
        One = 820,
        Two = 910,
        Three = 1000,
        Four = 1100,
        Six = 1500,
        Seven = 1655,
        Eight = 1745,
        Ten = 2020
    }
}