using System.Collections;
using System.Collections.Generic;
using PHL.Common.Utility;
using UnityEngine;

namespace PHL.Texto
{
    public enum TextoLanguage
    {
        English,
        French,
        Italian,
        German,
        Spanish,
        LatinSpanish,
        BrazilianPortuguese,
        SimplifiedChinese,
        Russian,
        Japanese,
        Arabic,
        Polish
    }

    //Static Texto-related functions
    public static class Texto
    {
        public static TextoLanguage currentLanguage { get; private set; }
        public static SecureEvent languageUpdatedEvent { get; private set; } = new SecureEvent();
        
        public static void SetLanguage(TextoLanguage newLanguage)
        {
            currentLanguage = newLanguage;
            languageUpdatedEvent.Invoke();
        }

        public static TextoLanguage StringToLanguage(string languageString)
        {
            languageString = languageString.ToLower().Replace(" ", "");

            if (languageString == "english")
            {
                return TextoLanguage.English;
            }
            else if (languageString == "french")
            {
                return TextoLanguage.French;
            }
            else if (languageString == "italian")
            {
                return TextoLanguage.Italian;
            }
            else if (languageString == "german")
            {
                return TextoLanguage.German;
            }
            else if (languageString == "spanish")
            {
                return TextoLanguage.Spanish;
            }
            else if (languageString == "latinspanish")
            {
                return TextoLanguage.LatinSpanish;
            }
            else if (languageString == "brazilianportuguese")
            {
                return TextoLanguage.BrazilianPortuguese;
            }
            else if (languageString == "simplifiedchinese")
            {
                return TextoLanguage.SimplifiedChinese;
            }
            else if (languageString == "russian")
            {
                return TextoLanguage.Russian;
            }
            else if (languageString == "japanese")
            {
                return TextoLanguage.Japanese;
            }
            else if (languageString == "arabic")
            {
                return TextoLanguage.Arabic;
            }
            else if (languageString == "polish")
            {
                return TextoLanguage.Polish;
            }

            return TextoLanguage.English;
        }
    }
}
