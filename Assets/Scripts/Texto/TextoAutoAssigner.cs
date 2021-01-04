using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PHL.Texto;
using System.Text;
using ArabicSupport;

public class TextoAutoAssigner : MonoBehaviour
{
    [SerializeField] private TextoData _texto;

    [SerializeField, HideInInspector] private TextMeshProUGUI _textMeshPro;
    [SerializeField, HideInInspector] private Text _text;

    private bool _baseValuesCached;
    private TMP_FontAsset _baseTMPFont;
    private Material _baseTMPMaterial;
    private Font _baseFont;

    private void Reset()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _text = GetComponent<Text>();
    }

    private void Start()
    {
        if (!_baseValuesCached)
        {
            CacheBaseValues();
        }

        Texto.languageUpdatedEvent.AddListener(UpdateText);
    }
    
    private void OnEnable()
    {
        if(!_baseValuesCached)
        {
            CacheBaseValues();
        }

        UpdateText();
    }

    private void CacheBaseValues()
    {
        if (_textMeshPro != null)
        {
            _baseTMPFont = _textMeshPro.font;
            _baseTMPMaterial = _textMeshPro.fontSharedMaterial;
        }

        if(_text != null)
        {
            _baseFont = _text.font;
        }

        _baseValuesCached = true;
    }

    public void UpdateText()
    {
        if (_textMeshPro != null)
        {
            TextoFontDatabase.instance.AssignFont(_textMeshPro, _baseTMPFont, _baseTMPMaterial);
        }

        if(_text != null)
        {
            TextoFontDatabase.instance.AssignFont(_text, _baseFont);
        }

        if (_texto != null)
        {
            if (_textMeshPro != null)
            {
                _textMeshPro.text = _texto.GetLine();
            }
            else if (_text != null)
            {
                _text.text = _texto;
            }
        }
    }
}