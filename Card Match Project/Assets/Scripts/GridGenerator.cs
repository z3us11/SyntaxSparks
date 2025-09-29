using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] Transform gridSizesTransform;
    [SerializeField] Transform gridTransform;
    [SerializeField] Transform rowPrefab;
    [SerializeField] Transform cardPrefab;

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
                gridRows = 5;
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
        for(int i = 0; i < gridRows; i++)
        {
            var row = Instantiate(rowPrefab, gridTransform);
            for(int j = 0; j < gridColumns; j++)
            {
                var card = Instantiate(cardPrefab, row);
            }
        }
    }
}
