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
        if(!GameManager.instance.gameStarted)
            return;

        if (selectedCard == null)
        {
            selectedCard = this;
            FlipCard();
        }
        else
        {
            if (selectedCard == this)
            {
                Debug.Log("Same Card Clicked");
                selectedCard = null;
                FlipCard();
            }
            else
            {
                UIManager.instance.UpdateMoves(1);

                if (selectedCard.cardNumber == cardNumber)
                {
                    // Match
                    Debug.Log("Match Found");
                    FlipCard();
                    StartCoroutine(MatchFound());
                    UIManager.instance.UpdateCombo(1);
                    UIManager.instance.UpdateMatches(1);
                }
                else
                {
                    // No Match
                    Debug.Log("No Match");
                    FlipCard();
                    StartCoroutine(NoMatchFound());
                    UIManager.instance.UpdateCombo(-1);
                }
            }
        }
    }

    public void FlipCard()
    {
        rect.DOScaleX(rect.localScale.x * -1, 0.25f);
    }

    public void SetCard(int number, Sprite sprite)
    {
        cardNumber = number;
        cardFrontImg.sprite = sprite;
    }
    IEnumerator MatchFound()
    {
        yield return new WaitForSeconds(0.5f);
        selectedCard.transform.DOScale(0, 0.25f).OnComplete(()=>selectedCard.gameObject.SetActive(false));
        transform.DOScale(0, 0.25f).OnComplete(() => gameObject.SetActive(false));
        selectedCard = null;
    }

    IEnumerator NoMatchFound()
    {
        yield return new WaitForSeconds(0.5f);
        selectedCard.FlipCard();
        FlipCard();
        selectedCard = null;
    }
}
