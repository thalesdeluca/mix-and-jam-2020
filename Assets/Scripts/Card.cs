using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
  public bool used = false;
  public Action action;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {
    if (used) {
      return;
    }

    var action = Input.GetButtonDown("Action");
    Debug.Log(action);

    if (action) {
      used = true;
      Debug.Log("USED");
    }
  }
}
