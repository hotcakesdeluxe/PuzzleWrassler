using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _score = 0;
    void Start()
    {
        _blockBoard.scoreUpdateEvent.AddListener(UpdateScoreText);
    }
    void UpdateScoreText(int blocksDestroyed)
    {
        _score += blocksDestroyed * 100;
        _scoreText.text = _score.ToString();
    }
}
