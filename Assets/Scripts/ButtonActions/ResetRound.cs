//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResetRound : MonoBehaviour, IPointerDownHandler {

    private GameManager gameManager;
    private float resetCost = 1;
    public Text resetCostText;

    public void OnPointerDown(PointerEventData eventData) {
        GameManager gameManager = GameManager.Instance;
        if(gameManager.SpendBank((int)resetCost))
        gameManager.OnResetRound();
        UpdateDrawPriceText();
    }

    public void UpdateDrawPriceText()
    {
        GameManager gameManager = GameManager.Instance;
        resetCost = (gameManager.TotalMoneyEarned * 0.2f) + 1f;
        resetCostText.text = "$" +  (resetCost).ToString("F0");
    }
}