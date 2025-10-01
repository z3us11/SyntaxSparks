using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    public int gridSize;
    public List<CardSaveData> spawnedCards = new List<CardSaveData>();

    public int moves;
    public int matches;
    public int combo;
    public int totalMatches;
}

public class GridGenerator : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] Transform gridSizesTransform;
    [SerializeField] Transform gameViewTransform;
    [SerializeField] Transform gridTransform;
    [SerializeField] Transform rowPrefab;
    [Header("Card")]
    [SerializeField] Card cardPrefab;
    [SerializeField] CardData[] cardsData;
    [Header("UI")]
    [SerializeField] Button resumeBtn;

    List<Transform> spawnedRows;
    List<Card> spawnedCards;

    int gridRows;
    int gridColumns;

    SaveData saveData;
    bool resumingGame;
    private const string SaveKey = "CardSaveData";

    private void Start()
    {
        saveData = new SaveData();

        if (HasSaveData())
        {
            resumeBtn.gameObject.SetActive(true);
            resumeBtn.onClick.RemoveAllListeners();
            resumeBtn.onClick.AddListener(() =>
            {
                LoadData();
            }); 
        }
        else
        {
            resumeBtn.gameObject.SetActive(false);
        }
    }

    bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SaveKey);
    }

    void LoadData()
    {
        if (!HasSaveData()) return;
        string json = PlayerPrefs.GetString(SaveKey);
        saveData = JsonUtility.FromJson<SaveData>(json);

        GameManager.instance.totalMatches = saveData.totalMatches;
        GameManager.instance.moves = saveData.moves;
        GameManager.instance.matches = saveData.matches;
        GameManager.instance.combo = saveData.combo;

        UIManager.instance.Init();
        
        resumingGame = true;
        SelectGridSize(saveData.gridSize);
    }

    public void SaveData()
    {
        foreach(var card in spawnedCards)
        {
            card.SetSaveData();
        }
        var savedCards = new List<Card>(spawnedCards);

        foreach(var card in savedCards)
        {
            saveData.spawnedCards.Add(new CardSaveData()
            {
                cardNumber = card.saveData.cardNumber,
                cardImageName = card.saveData.cardImageName,
                isMatched = card.saveData.isMatched
            });
        }
        saveData.totalMatches = GameManager.instance.totalMatches;  
        saveData.moves = GameManager.instance.moves;
        saveData.matches = GameManager.instance.matches;
        saveData.combo = GameManager.instance.combo;

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        if (resumeBtn != null)
            resumeBtn.gameObject.SetActive(false);
    }

    public void SelectGridSize(int index)
    {
        if (!resumingGame)
            ClearSave();

        InitGridSize(index);
        InitGrid(resumingGame);

        gridSizesTransform.gameObject.SetActive(false);
        gameViewTransform.gameObject.SetActive(true);
    }

    private void InitGridSize(int index)
    {
        switch (index)
        {
            case 0:
                gridRows = 2;
                gridColumns = 3;
                break;
            case 1:
                gridRows = 3;
                gridColumns = 4;
                break;
            case 2:
                gridRows = 4;
                gridColumns = 5;
                break;
            default:
                gridRows = 1;
                gridColumns = 1;
                break;
        }
        saveData.gridSize = index;
    }

    void InitGrid(bool resumingGame = false)
    {
        GameManager.instance.StartGame();
        SpawnRowsAndCards();

        if(resumingGame)
        {
            for(int i = 0; i < spawnedCards.Count; i++)
            {
                var cardSprites = Resources.LoadAll<Sprite>("Sprites/CardFaces");
                Sprite cardSprite = System.Array.Find(cardSprites, s => s.name == saveData.spawnedCards[i].cardImageName);
                spawnedCards[i].SetCard(saveData.spawnedCards[i].cardNumber, cardSprite);
                spawnedCards[i].isMatched = saveData.spawnedCards[i].isMatched;
            }
        }
        else
        {
            List<Card> currentCards = new List<Card>(spawnedCards);
            // Assign random cards to the spawned cards
            while (currentCards.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, cardsData.Length);
                currentCards[0].SetCard(cardsData[randomIndex].cardNumber, cardsData[randomIndex].cardFrontImage);
                int randomCardIndex = UnityEngine.Random.Range(1, currentCards.Count);
                currentCards[randomCardIndex].SetCard(cardsData[randomIndex].cardNumber, cardsData[randomIndex].cardFrontImage);

                currentCards.RemoveAt(randomCardIndex);
                currentCards.RemoveAt(0);

                GameManager.instance.totalMatches++;
            }
        }

        StartCoroutine(DisableAutoLayoutGroups());
    }

    private void SpawnRowsAndCards()
    {
        spawnedRows = new List<Transform>();
        spawnedCards = new List<Card>();

        for (int i = 0; i < gridRows; i++)
        {
            var row = Instantiate(rowPrefab, gridTransform);
            spawnedRows.Add(row);
            for (int j = 0; j < gridColumns; j++)
            {
                var card = Instantiate(cardPrefab, row);
                spawnedCards.Add(card);
            }
        }
    }

    IEnumerator DisableAutoLayoutGroups()
    {
        yield return new WaitForEndOfFrame();

        gridTransform.GetComponent<LayoutGroup>().enabled = false;
        foreach (var row in spawnedRows)
        {
            row.GetComponent<LayoutGroup>().enabled = false;
        }

        StartCoroutine(RevealCardsAtStart());
    }

    IEnumerator RevealCardsAtStart()
    {
        if(HasSaveData())
        {
            foreach (var card in spawnedCards)
            {
                if(card.isMatched)
                    card.gameObject.SetActive(false);
            }
        }

        Debug.Log("Revealing Cards");
        yield return new WaitForSeconds(0.5f);
        foreach (var card in spawnedCards)
        {
            card.FlipCard(false);
        }
        SoundManager.instance.PlayFlipSound();

        yield return new WaitForSeconds(1f);

        foreach (var card in spawnedCards)
        {
            card.FlipCard(false);
        }
        SoundManager.instance.PlayFlipSound();

        GameManager.instance.CanPlay();
    }
}
