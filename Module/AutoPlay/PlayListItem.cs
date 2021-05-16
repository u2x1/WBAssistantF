using System;
using System.Collections.Generic;
using System.Text;

namespace WBAssistantF.Module.AutoPlay
{
    public class PlayListItem
    {
        string filePath;
        PlayTime[] playTime;
    }

    enum PlayTime
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
