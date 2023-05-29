//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MaxManaChange : MonoBehaviour, IPointerDownHandler {

    private GameManager gameManager;
    private float manaChangeCost = 1;
    public bool increase = true;
    private float manaIncriment = 0.05f;
    public Text manaChangeCostText;

    public void OnPointerDown(PointerEventData eventData)
    {

        GameManager gameManager = GameManager.Instance;
        if (!increase && gameManager.maxMana * (1 - manaIncriment) < gameManager.minMana)
        {
            return;
        }
        else if (gameManager.SpendBank((int)manaChangeCost))
        {
            if (increase)
            {
                gameManager.ChangeMaxMana(gameManager.maxMana * manaIncriment);
            }
            else
            {
                gameManager.ChangeMaxMana(-gameManager.maxMana * manaIncriment);
            }
            UpdateManaChangeText();
        }
    }

    public void UpdateManaChangeText()
    {
        GameManager gameManager = GameManager.Instance;
        manaChangeCost += ((((gameManager.TotalMoneyEarned * 0.00355f) + 1f ) * (gameManager.maxMana/50))); //todo add to savestate
        manaChangeCostText.text = "$" +  (manaChangeCost).ToString("F0");
    }
}