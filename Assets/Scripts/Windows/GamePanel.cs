using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : SingletonWindow<GamePanel>
{
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text opponentName;
    [SerializeField] private TMP_Text levelNumber;
    [SerializeField] private Image playerUnderline;
    [SerializeField] private Image opponentUnderline;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private Animator coinAnim;
    [SerializeField] private Button pauseButton;

    private float elapsedTime;
    private bool isStop = true;

    protected override void Awake()
    {
        base.Awake();

        pauseButton.onClick.AddListener(PausePanel.Instance.Show);
    }

    void Start()
    {
        playerUnderline.gameObject.SetActive(false);
        opponentUnderline.gameObject.SetActive(false);

        playerName.text = GameController.Instance.PlayrName;
        opponentName.text = GameController.Instance.OpponentName;
        levelNumber.text += GameData.Instance.levelNumber.ToString();
        timer.text = "00:00";

        Opponent.Instance.IsMakeMove = true;
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            coinAnim.SetTrigger("Blue");
            StartCoroutine(CheckAnimationComplete("BlueCoinAnim", () => Opponent.Instance.IsMakeMove = false));
        }
        else
        {
            coinAnim.SetTrigger("Red");
            StartCoroutine(CheckAnimationComplete("RedCoinAnim", () => Opponent.Instance.MakeMove()));
        }
    }

    void Update()
    {
        if (!isStop)
        {
            UpdateTimer();
            UpdatePlayersUnderline();
        }
    }

    private IEnumerator CheckAnimationComplete(string animName, System.Action onComlete)
    {
        while (true)
        {
            if (coinAnim.GetCurrentAnimatorStateInfo(0).IsName(animName) && !coinAnim.IsInTransition(0))
            {
                if (coinAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
                {
                    onComlete();
                    Destroy(coinAnim.gameObject);
                    break;
                }
                else
                    yield return null;
            }
            else
                yield return null;
        }

        isStop = false;
    }

    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        timer.text = timeString;
    }

    private void UpdatePlayersUnderline()
    {
        if (Opponent.Instance.IsMakeMove)
        {
            opponentUnderline.gameObject.SetActive(true);
            playerUnderline.gameObject.SetActive(false);
        }
        else
        {
            opponentUnderline.gameObject.SetActive(false);
            playerUnderline.gameObject.SetActive(true);
        }
    }

    public void StopTimer()
    {
        isStop = true;
    }

    public string GetTimerValue()
    {
        return timer.text;
    }
}
