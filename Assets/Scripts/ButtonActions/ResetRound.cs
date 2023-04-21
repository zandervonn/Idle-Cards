//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResetRound : MonoBehaviour, IPointerDownHandler {

    private GameManager gameManager;

      public void OnPointerDown(PointerEventData eventData) {
          GameManager gameManager = GameManager.Instance;
          gameManager.OnResetRound();
      }
}