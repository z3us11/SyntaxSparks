using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class CardSaveData
{
    public int cardNumber;
    public string cardImageName;
    public bool isMatched;
}

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image cardFrontImg;
    private RectTransform rect;

    public int cardNumber;
    public CardSaveData saveData;

    public static Card selectedCard;
    private bool isFlipped = false;
    public bool isMatched = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.instance.gameStarted || isFlipped || isMatched)
            return;

        // flip immediately (logic)
        isFlipped = true;
        FlipCard();

        if (selectedCard == null)
        {
            // first selection
            selectedCard = this;
        }
        else
        {
            if (selectedCard == this)
            {
                Debug.Log("Same Card Clicked");
                // flip back immediately
                isFlipped = false;
                FlipCard();
                selectedCard = null;
            }
            else
            {
                UIManager.instance.UpdateMoves(1);

                if (selectedCard.cardNumber == cardNumber)
                {
                    Debug.Log("Match Found");
                    StartCoroutine(MatchFound(selectedCard, this));
                    UIManager.instance.UpdateCombo(1);
                    UIManager.instance.UpdateMatches(1);
                }
                else
                {
                    Debug.Log("No Match");
                    StartCoroutine(NoMatchFound(selectedCard, this));
                    UIManager.instance.UpdateCombo(-1);
                }
            }
        }
    }

    public void FlipCard()
    {
        float newScaleX = Mathf.Approximately(rect.localScale.x, 1f) ? -1f : 1f;
        rect.DOScaleX(newScaleX, 0.25f);
    }

    public void SetCard(int number, Sprite sprite)
    {
        cardNumber = number;
        cardFrontImg.sprite = sprite;
        isFlipped = false;
        isMatched = false;
        gameObject.SetActive(true);
    }

    public void SetSaveData()
    {
        saveData.isMatched = isMatched;
        saveData.cardNumber = cardNumber;
        saveData.cardImageName = cardFrontImg.sprite.name;
    }

    private IEnumerator MatchFound(Card first, Card second)
    {
        selectedCard = null;
        yield return new WaitForSeconds(0.25f); // short delay only for animation
        first.isMatched = second.isMatched = true;

        first.transform.DOScale(0, 0.25f).OnComplete(() => first.gameObject.SetActive(false));
        second.transform.DOScale(0, 0.25f).OnComplete(() => second.gameObject.SetActive(false));
    }

    private IEnumerator NoMatchFound(Card first, Card second)
    {
        selectedCard = null;
        yield return new WaitForSeconds(0.5f); // allow player to see mismatch
        first.isFlipped = false;
        second.isFlipped = false;
        first.FlipCard();
        second.FlipCard();
    }
}
