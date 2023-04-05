//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DrawCardButton : MonoBehaviour, IPointerDownHandler {

    private GameManager gameManager;

      public void OnPointerDown(PointerEventData eventData) {
          GameManager gameManager = GameManager.Instance;
          if(gameManager.cardManager.availableCards.Count > 0){
                if(gameManager.SpendRound(1 + gameManager.BuyCost) ){
                      DrawCard drawCardComponent = FindObjectOfType<DrawCard>();
                      drawCardComponent.DrawCards(1);
                      gameManager.BuyCost++;
                }
          } else {
                Debug.Log("No cards left");
          }
      }
}