using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
  Initial,
  Preparation,
  Battle,
  Victory
}

public class GameController : MonoBehaviour {
  public GameState state = GameState.Initial;
  public Card cardSelected;

  public delegate void UpdateState();
  public static event UpdateState updateState;

  // Start is called before the first frame update
  void Start() {
  }

  // Update is called once per frame
  void Update() {

  }

  public void ChangePhase(GameState newState) {
    if (state == GameState.Victory) {
      return;
    }

    state = newState;
    updateState();
  }

}
