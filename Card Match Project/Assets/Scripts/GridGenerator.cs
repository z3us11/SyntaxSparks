using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] Transform gridSizesTransform;
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
        gridTransform.gameObject.SetActive(true);
    }

    void InitGrid()
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

        // Assign random cards to the spawned cards
        while (spawnedCards.Count > 0)
        {
            int randomIndex = Random.Range(0, cardsData.Length);
            spawnedCards[0].SetCard(cardsData[randomIndex].cardNumber, cardsData[randomIndex].cardFrontImage);
            int randomCardIndex = Random.Range(1, spawnedCards.Count);
            spawnedCards[randomCardIndex].SetCard(cardsData[randomIndex].cardNumber, cardsData[randomIndex].cardFrontImage);

            spawnedCards.RemoveAt(randomCardIndex);
            spawnedCards.RemoveAt(0);
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
    }
}
