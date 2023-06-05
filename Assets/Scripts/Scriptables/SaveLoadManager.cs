using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public List<CardInstanceData> ownedCardsData;
    public int highScore;
    public int lastScore;
    public float maxMana;
    public int TotalMoneyEarned;
    public int RemoveCost;
    public int BuyCost;
    public long BankValue;
    public long LastPauseTimeTicks;
    public float MaxManaChangeCost;
    public float CurrentMana;
    public float Multiplier;

    public GameState()
    {
        ownedCardsData = new List<CardInstanceData>();
    }
}

[System.Serializable]
public class CardInstanceData
{
    public string cardName;
    public int level;
    public int timesPlayed;
    public float nextUpgradeExtra;
    public int UpgradeCost;
    public int rarity;
}

public class SaveLoadManager : MonoBehaviour
{
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

        public void SaveGameState()
    {
        GameState gameState = new GameState();

        gameManager = FindObjectOfType<GameManager>();

        foreach (var cardInstance in gameManager.cardManager.GetOwnedCards())
        {
            var cardData = new CardInstanceData
            {
                cardName = cardInstance.card.cardName,
                level = cardInstance.level,
                timesPlayed = cardInstance.timesPlayed,
                nextUpgradeExtra = cardInstance.nextUpgradeExtra,
                UpgradeCost = cardInstance.UpgradeCost,
                rarity = cardInstance.rarity
            };

            gameState.ownedCardsData.Add(cardData);
        }

        gameState.highScore = gameManager.HighScore;
        gameState.lastScore = gameManager.LastScore;
        gameState.maxMana = gameManager.maxMana;
        gameState.TotalMoneyEarned = gameManager.TotalMoneyEarned;
        gameState.RemoveCost = gameManager.RemoveCost;
        gameState.BuyCost = gameManager.BuyCost;
        gameState.BankValue = gameManager.BankValue;
        gameState.LastPauseTimeTicks = gameManager.LastPauseTime.Ticks;
        gameState.MaxManaChangeCost = gameManager.MaxManaChangeCost;
        gameState.CurrentMana = gameManager.mana;
        gameState.Multiplier = gameManager.CurrentMultiplier;


        string json = JsonUtility.ToJson(gameState);

        Debug.Log("Saved JSON: " + json);

        PlayerPrefs.SetString("GameState", json);
        PlayerPrefs.Save();
    }


    public void LoadGameState()
    {
        if (PlayerPrefs.HasKey("GameState"))
        {
            string json = PlayerPrefs.GetString("GameState");

            Debug.Log("Loaded JSON: " + json);

            GameState gameState = JsonUtility.FromJson<GameState>(json);

            gameManager = FindObjectOfType<GameManager>();

            List<CardInstanceData> ownedCardsData = new List<CardInstanceData>();
            foreach (var cardData in gameState.ownedCardsData)
            {
                Card card = gameManager.cardManager.GetCardByName(cardData.cardName);
                CardInstance cardInstance = new CardInstance(card, gameManager.cardManager, cardData.level, cardData.rarity)
                {
                    timesPlayed = cardData.timesPlayed,
                    nextUpgradeExtra = cardData.nextUpgradeExtra,
                    UpgradeCost = cardData.UpgradeCost
                };

                //Add the CardInstanceData based on the CardInstance
                cardData.cardName = cardInstance.card.cardName;
                cardData.level = cardInstance.level;
                cardData.timesPlayed = cardInstance.timesPlayed;
                cardData.nextUpgradeExtra = cardInstance.nextUpgradeExtra;
                cardData.UpgradeCost = cardInstance.UpgradeCost;
                cardData.rarity = cardInstance.rarity;

                ownedCardsData.Add(cardData);
            }

            // This line is moved after the loop which updates ownedCardsData
            gameManager.cardManager = new CardManager(gameManager.cardsList.cards);

            // Update ownedCards of the existing CardManager
            gameManager.cardManager.SetOwnedCards(ownedCardsData);

            gameManager.HighScore = gameState.highScore;
            gameManager.LastScore = gameState.lastScore;
            gameManager.maxMana = gameState.maxMana;
            gameManager.TotalMoneyEarned = gameState.TotalMoneyEarned;
            gameManager.RemoveCost = gameState.RemoveCost;
            gameManager.BuyCost = gameState.BuyCost;
            gameManager.BankValue = gameState.BankValue;
            gameManager.LastPauseTime = new DateTime(gameState.LastPauseTimeTicks);
            gameManager.MaxManaChangeCost = gameState.MaxManaChangeCost;
            gameManager.mana = gameState.CurrentMana;
            gameManager.CurrentMultiplier = gameState.Multiplier;



            gameManager.CalculateIdleEarnings();
        }
        else
        {
            Debug.Log("No saved game state found");
        }
    }
}