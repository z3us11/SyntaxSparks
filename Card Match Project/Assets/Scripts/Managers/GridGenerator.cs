using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    List<Transform> spawnedRows;
    List<Card> spawnedCards;

    int gridRows;
    int gridColumns;

    public void SelectGridSize(int index)
    {
        switch(index)
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

        InitGrid();
        gridSizesTransform.gameObject.SetActive(false);
        gameViewTransform.gameObject.SetActive(true);
    }

    void InitGrid()
    {
        GameManager.instance.StartGame();

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

        List<Card> currentCards = new List<Card>(spawnedCards);
        // Assign random cards to the spawned cards
        while (currentCards.Count > 0)
        {
            int randomIndex = Random.Range(0, cardsData.Length);
            currentCards[0].SetCard(cardsData[randomIndex].cardNumber, cardsData[randomIndex].cardFrontImage);
            int randomCardIndex = Random.Range(1, currentCards.Count);
            currentCards[randomCardIndex].SetCard(cardsData[randomIndex].cardNumber, cardsData[randomIndex].cardFrontImage);

            currentCards.RemoveAt(randomCardIndex);
            currentCards.RemoveAt(0);

            GameManager.instance.totalMatches++;
        }

        StartCoroutine(DisableAutoLayoutGroups());
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
        Debug.Log("Revealing Cards");
        yield return new WaitForSeconds(0.5f);
        foreach (var card in spawnedCards)
        {
            card.FlipCard();
        }
        yield return new WaitForSeconds(1f);
        foreach (var card in spawnedCards)
        {
            card.FlipCard();
        }
        GameManager.instance.CanPlay();
    }
}
