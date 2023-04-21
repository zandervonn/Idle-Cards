//7
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    public int fieldScore { get; private set; }
    private float _mana = 100;
    private int maxMana = 100;
    public ManaBarActions manaBarActions;
    public float mana {
        get { return _mana; }
        set {
            _mana = value;
        }
    }
    public int HighScore { get; private set; }
    public int LastScore { get; private set; }
    public int BankValue { get; private set; }
    public int BuyCost { get; set; }
    public int RemoveCost { get; set; }
    public int RemoveCostMultiplier { get; private set; }
    public int TotalMoneyEarned { get; private set; }
    public CardsList cardsList{ get; private set; }
    public DrawCard drawCard;
    public CardManager cardManager;
    public DrawCardButton drawCardButton;


    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        manaBarActions = FindObjectOfType<ManaBarActions>();

        HighScore = 1; // to make sue money is always made
        LastScore = 0;
        BankValue = 100; // testing
        BuyCost = 1;
        RemoveCost = 1;


        RemoveCostMultiplier = 2;
        UpdateRemoveCost();

        // Initialize the cardsList variable
        cardsList = FindObjectOfType<CardsList>();
        cardsList.Initialize();

        cardManager = new CardManager(cardsList.cards);

    }

    public void UpdateHighScore(int newScore)
    {
        if (newScore > HighScore)
        {
            HighScore = newScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
    }

    public void UpdateLastScore(int newScore)
    {
        LastScore = newScore;
    }

    public void UpdateBankValue(bool high, bool last)
    {
        int moneyEarned = 0;

        if (high)
        {
            moneyEarned += HighScore;
        }
        if (last)
        {
            moneyEarned += LastScore;
        }

        BankValue += moneyEarned;
        TotalMoneyEarned += moneyEarned;
        CalculateLevel();
    }

    public float CalculateLevel()
    {
        return Mathf.Pow(2, Mathf.Log(TotalMoneyEarned, 5));
    }

    public void IncreaseScore(int amount) {
        fieldScore += amount;
        if(fieldScore < 0){
            fieldScore = 0;
        }
    }

    public void DecreaseScore(int amount) {
        fieldScore -= amount;
    }

    public void IncreaseMana(float amount) {
        if( mana + amount < maxMana)
        {
            mana += amount;
        }
        else
        {
            mana = maxMana;
        }
        
    }

    public void DecreaseMana(float amount) {
        if (mana - amount > 0)
        {
            mana -= amount;
        }
        else
        {
            mana = 0;
        }
    }

    public bool SpendBank(int cost) {
        if(BankValue >= cost){
            BankValue -= cost;
            return true;
        } else {
            Debug.Log("Cant afford this");
            return false;
        }
    }

    public bool SpendRound(int cost) {
        if(fieldScore >= cost){
            fieldScore -= cost;
            return true;
        } else {
            Debug.Log("Cant afford this");
            return false;
        }
    }


    public void OnManaDepleted() {
        updateScoreRecords();
        UpdateBankValue(true,true);
        ResetField();
    }

    public void OnResetRound() {
        updateScoreRecords();
        UpdateBankValue(false,true);
        ResetField();
    }

    public void ResetField() {
        DrawCard drawCardComponent = FindObjectOfType<DrawCard>();
        drawCardComponent.ClearCards();
        cardManager.ResetAvailableCards();
        drawCardComponent.DrawCards(4);
        manaBarActions.ResetElapsedTimeSinceRoundStart(); 
        ResetScore();
        ResetMana();
        ResetDrawCost();
    }

    public void ResetScore()
    {
        fieldScore = 0;
    }

    public void ResetDrawCost()
    {
        BuyCost = 1;
        DrawCardButton drawCardButton = FindObjectOfType<DrawCardButton>();
        drawCardButton.UpdateDrawPriceText();
    }

    public void updateScoreRecords()
    {
        UpdateHighScore(fieldScore);
        UpdateLastScore(fieldScore);
    }

    public void ResetMana()
    {
        mana = maxMana;
    }

    public void UpdateRemoveCost()
    {
        RemoveCost *= RemoveCostMultiplier;
    }
}