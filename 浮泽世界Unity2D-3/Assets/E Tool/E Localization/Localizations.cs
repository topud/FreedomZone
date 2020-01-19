using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E.Tool
{
    public class Localizations : ScriptableObject
    {
        public Localization[] strings;
    }

    [Serializable]
    public class Localization
    {
        public string id;
        public string ch, en, jp;
    }
}
