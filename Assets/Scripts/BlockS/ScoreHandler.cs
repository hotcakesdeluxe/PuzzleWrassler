using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private BlockBoard _blockBoard;
    [SerializeField]private BlockPairSpawner _opponentBlockSpawner;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _score = 0;
    private int _combo = 0;
    void Start()
    {
        _blockBoard.scoreUpdateEvent.AddListener(UpdateScoreText);
    }

    void Update()
    {
        //Debug.Log(_combo);
    }
    private void UpdateScoreText(int blocksDestroyed)
    {
        _combo++;
        StartCoroutine(CheckForComboRoutine(blocksDestroyed));
    }

    private IEnumerator CheckForComboRoutine(int blocksDestroyed)
    {
        _score += (blocksDestroyed * 100) * _combo;
        yield return new WaitUntil(() => !_blockBoard.AnyFallingBlocks());
        if (!_blockBoard.WhatToDelete())
        {
            yield return new WaitForSeconds(.2f);
            _opponentBlockSpawner.SpawnGarbage(_combo);
            _scoreText.text = _score.ToString();
            _combo = 0;
        }
        
    }
}
