using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PHL.Common.Utility;
using UnityEngine.UI;

namespace PHL.Texto
{
    [CreateAssetMenu(fileName = "TextosFontDatabase", menuName = "PHL/Singletons/TextoFontDatabase", order = 0)]
    public class TextoFontDatabase : ScriptableObjectSingleton<TextoFontDatabase>
    {
        [SerializeField] private LanguageFontGroup _baseFontGroup;
        [SerializeField] private LanguageFontGroup[] _fontGroups;

        public void AssignFont(Text text, Font baseFont)
        {
            int fontIndex = -1;

            for (int i = 0; i < _baseFontGroup.fontMaterialGroups.Length; i++)
            {
                if (_baseFontGroup.fontMaterialGroups[i].font == baseFont)
                {
                    fontIndex = i;
                    break;
                }
            }

            if(fontIndex >= 0)
            {
                for (int i = 0; i < _fontGroups.Length; i++)
                {
                    if (_fontGroups[i].language == Texto.currentLanguage)
                    {
                        text.font = _fontGroups[i].fontMaterialGroups[fontIndex].font;
                        text.lineSpacing = _fontGroups[i].lineHeight;
                        break;
                    }
                }
            }
        }

        public void AssignFont(TextMeshProUGUI tmp, TMP_FontAsset baseFont, Material baseMaterial)
        {
            int fontIndex = -1;
            int materialIndex = -1;

            for (int i = 0; i < _baseFontGroup.fontMaterialGroups.Length; i++)
            {
                if (_baseFontGroup.fontMaterialGroups[i].textMeshProFont == baseFont)
                {
                    fontIndex = i;

                    for (int j = 0; j < _baseFontGroup.fontMaterialGroups[i].textMeshProMaterials.Length; j++)
                    {
                        if (_baseFontGroup.fontMaterialGroups[i].textMeshProMaterials[j] == baseMaterial)
                        {
                            materialIndex = j;
                            break;
                        }
                    }

                    break;
                }
            }

            if (fontIndex >= 0 && materialIndex >= 0)
            {
                for (int i = 0; i < _fontGroups.Length; i++)
                {
                    if (_fontGroups[i].language == Texto.currentLanguage)
                    {
                        tmp.font = _fontGroups[i].fontMaterialGroups[fontIndex].textMeshProFont;
                        tmp.fontSharedMaterial = _fontGroups[i].fontMaterialGroups[fontIndex].textMeshProMaterials[materialIndex];
                        tmp.lineSpacing = _fontGroups[i].lineHeight;
                        tmp.fontStyle = new FontStyles();
                        break;
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class LanguageFontGroup
    {
        public TextoLanguage language;
        public float lineHeight;
        public FontMaterialGroup[] fontMaterialGroups;
    }

    [System.Serializable]
    public class FontMaterialGroup
    {
        public Font font;
        public TMP_FontAsset textMeshProFont;
        public Material[] textMeshProMaterials;
    }
}