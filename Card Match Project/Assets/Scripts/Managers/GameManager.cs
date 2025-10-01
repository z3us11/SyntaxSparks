using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GridGenerator gridGenerator;
    public bool gameStarted = false;

    public int moves;
    public int matches;
    public int combo;
    public int totalMatches;

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

    public void StartGame()
    {
        UIManager.instance.Init();
    }

    public void CanPlay()
    {
        gameStarted = true;
    }
    public void EndGame()
    {
        gameStarted = false;
    }

    public bool IsGameOver()
    {
        if (matches >= totalMatches)
        {
            Debug.Log("Game Over"); 
            return true;
        }
        else
        {
            Debug.Log("Not Over Yet");
            return false;
        }
    }
}
