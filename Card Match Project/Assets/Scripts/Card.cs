using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image cardFrontImg;
    RectTransform rect;
    int cardNumber;

    public static Card selectedCard;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FlipCard();
    }

    void FlipCard()
    {
        rect.DOScaleX(rect.localScale.x * -1, 0.25f);
    }
}
