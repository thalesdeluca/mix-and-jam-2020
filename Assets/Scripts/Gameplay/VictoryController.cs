using System;
using UnityEngine;

public class VictoryController : MonoBehaviour {
  private bool changed;

  private void Start() {
    changed = false;
  }

  private void Update() {
    if (GameController.Instance.state == GameState.Victory && !changed) {
      changed = true;
      TMPro.TextMeshProUGUI text = GameObject.Find("VictoryText").GetComponent<TMPro.TextMeshProUGUI>();
      text.text = GameController.Instance.won ? "You Win" : "You Lose";
    }
  }
}