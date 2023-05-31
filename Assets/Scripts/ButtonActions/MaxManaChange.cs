//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MaxManaChange : MonoBehaviour, IPointerDownHandler {

    private GameManager gameManager;
    public bool increase = true;
    private float manaIncriment = 0.05f;
    public Text manaChangeCostText;

    public void Update()
    {
        GameManager gameManager = GameManager.Instance;
        manaChangeCostText.text = "$" + (gameManager.MaxManaChangeCost).ToString("F0");
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        GameManager gameManager = GameManager.Instance;
        if (!increase && gameManager.maxMana * (1 - manaIncriment) < gameManager.minMana)
        {
            return;
        }
        else if (gameManager.SpendBank((int)gameManager.MaxManaChangeCost))
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
        gameManager.MaxManaChangeCost += ((((gameManager.TotalMoneyEarned * 0.00355f) + 1f ) * (gameManager.maxMana/50)));
    }
}