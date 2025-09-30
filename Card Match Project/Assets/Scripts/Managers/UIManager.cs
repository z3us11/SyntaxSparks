using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button homeBtn;
    [Space]
    [SerializeField] TMP_Text movesTxt;
    [SerializeField] TMP_Text matchesTxt;
    [SerializeField] TMP_Text comboTxt;
    [Space]
    [SerializeField] Transform gameOverView;
    [SerializeField] Button gameOverHomeBtn;
    [SerializeField] TMP_Text gameOverMovesTxt;
    [SerializeField] TMP_Text gameOverMatchesTxt;
    [SerializeField] TMP_Text gameOverComboTxt;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        homeBtn.onClick.RemoveAllListeners();
        gameOverHomeBtn.onClick.RemoveAllListeners();

        homeBtn.onClick.AddListener(() =>
        {
            GameManager.instance.EndGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
        gameOverHomeBtn.onClick.AddListener(() =>
        {
            GameManager.instance.EndGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        });
    }

    public void Init()
    {
        movesTxt.text = "Moves\n0";
        matchesTxt.text = "Matches\n0";
        comboTxt.text = "Combo\n0";
    }

    public void UpdateMoves(int value)
    {
        GameManager.instance.moves += value;
        movesTxt.text = "Moves\n" + GameManager.instance.moves.ToString();
    }

    public void UpdateMatches(int value)
    {
        GameManager.instance.matches += value;
        matchesTxt.text = "Matches\n" + GameManager.instance.matches.ToString();

        if (GameManager.instance.IsGameOver())
            StartCoroutine(ShowGameOver());
    }

    public void UpdateCombo(int value)
    {
        if (value == -1)
            GameManager.instance.combo = 0;
        else
            GameManager.instance.combo += value;
        comboTxt.text = "Combo\n" + GameManager.instance.combo.ToString();
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.EndGame();
        gameOverView.gameObject.SetActive(true);
        gameOverMovesTxt.text = "Moves\n" + GameManager.instance.moves.ToString();
        gameOverMatchesTxt.text = "Matches\n" + GameManager.instance.matches.ToString();
        gameOverComboTxt.text = "Max Combo\n" + GameManager.instance.combo.ToString();
    }
    

    
}
