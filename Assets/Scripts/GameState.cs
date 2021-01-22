using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameState : MonoBehaviour
{
    [SerializeField] private BlockPairSpawner _playerOneSpanwer;
    [SerializeField] private BlockPairSpawner _playerTwoSpanwer;
    [SerializeField]private PlayerBlockInput _playerOneInput;
    [SerializeField]private PlayerBlockInput _playerTwoInput;

    private int countdownTime = 3;
    [SerializeField] private Text _countdownDisplayText;
    [SerializeField] private Text _pacWins;
    [SerializeField] private Text _ocWins;
    [SerializeField]private CanvasGroup _gameOverInstructions;

    private void Start()
    {
        StartCoroutine(StartCountdownRoutine());

        //game end event triggers when a player's well is filled so this appears a little reversed
        _playerOneSpanwer.gameEndEvent.AddListener(PlayerTwoWins);
        _playerTwoSpanwer.gameEndEvent.AddListener(PlayerOneWins);
    }

    private IEnumerator StartCountdownRoutine()
    {
        //do 3,2,1 ding ding ding instead
        while (countdownTime > 0)
        {
            _countdownDisplayText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1);
            countdownTime--;
        }
        _countdownDisplayText.text = "FIGHT!";
        yield return new WaitForSeconds(1);
        _playerOneInput.gameObject.SetActive(true);
        _playerTwoInput.gameObject.SetActive(true);
        _playerOneSpanwer.SpawnBlockPair();
        _playerTwoSpanwer.SpawnBlockPair();
        _countdownDisplayText.gameObject.SetActive(false);

    }

    private void PlayerOneWins()
    {
        StartCoroutine(WinnerFade(_pacWins));
    }

    private void PlayerTwoWins()
    {
        StartCoroutine(WinnerFade(_ocWins));
    }

    IEnumerator WinnerFade(Text winnerText)
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            winnerText.color = new Color(winnerText.color.r, winnerText.color.g, winnerText.color.b, i);
            _gameOverInstructions.alpha = i;
            yield return null;
        }
    }
}

