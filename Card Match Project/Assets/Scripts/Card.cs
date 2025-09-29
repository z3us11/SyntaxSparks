using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Image cardFrontImg;
    
    public void FlipCard()
    {
        transform.Do
    }

    public void SetCardFront(Sprite sprite)
    {
        cardFrontImg.sprite = sprite;
    }
}
