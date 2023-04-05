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
    public CardsList cardsList{ get; private set; }
    //public List<CardInstance> availableCards;
    //public List<CardInstance> ownedCards;
    public DrawCard drawCard;

    // public CardManager cardManager; 
    public CardManager cardManager;


    private void Awake() {

        //cardManager = new CardManager(cardsList.cards.Select(card => new CardInstance(card, 1)).ToList());

        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("GameManager Instance set");

        manaBarActions = FindObjectOfType<ManaBarActions>();

        HighScore = 0;
        LastScore = 0;
        BankValue = 100; /// testing
        BuyCost = 0;

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
            if (high){
                BankValue += HighScore;
            }
            if (last){
                BankValue += LastScore;
            }
        }

    public void IncreaseScore(int amount) {
        Debug.Log("updating score");
        fieldScore += amount;
        if(fieldScore < 0){
            fieldScore = 0;
        }
        Debug.Log("NewScore = " + fieldScore);
    }

    public void DecreaseScore(int amount) {
        Debug.Log("updating score");
        fieldScore -= amount;
        Debug.Log("NewScore = " + fieldScore);
    }

    public void IncreaseMana(float amount) {
        mana += amount;
    }

    public void DecreaseMana(float amount) {
        mana -= amount;
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
//        availableCards = new List<Card>(ownedCards);
        cardManager.ResetAvailableCards();
        drawCardComponent.DrawCards(4);
        manaBarActions.ResetElapsedTimeSinceRoundStart();
        BuyCost = 0;
        ResetScore();
        ResetMana();
    }

    public void ResetScore()
    {
        fieldScore = 0;
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
}