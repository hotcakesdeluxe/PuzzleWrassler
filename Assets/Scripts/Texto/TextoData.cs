using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PHL.Texto
{
    [System.Serializable]
    public class TextoLine
    {
        public TextoLanguage language = TextoLanguage.English;
        [TextArea(3, 10)] public string text;
        [HideInInspector] [SerializeField] private bool _initialized;

        public TextoLine()
        {
            language = TextoLanguage.English;
        }
    }
    
    [CreateAssetMenu(fileName = "Texto", menuName = "PHL/Texto", order = 0)]
    public class TextoData : ScriptableObject
    {
        public string uniqueID;
        public List<TextoLine> lines = new List<TextoLine>();
           
        public string GetLine()
        {
            return GetLine(Texto.currentLanguage);
        }

        public string GetLine(TextoLanguage language)
        {
            TextoLine line = lines.Find(x => x.language == language);

            if (line != null)
            {
                string finalText = line.text;

                finalText = finalText.Replace("<important>", string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGBA(TextoSettingsData.instance.highlightColor)));
                finalText = finalText.Replace("</important>", "</color>");

                return finalText;
            }

            return "[No text found]";
        }

        public static implicit operator string(TextoData texto)
        {
            if(texto == null)
            {
                return "";
            }

            return texto.GetLine();
        }

        public override string ToString()
        {
            return GetLine();
        }
    }
}