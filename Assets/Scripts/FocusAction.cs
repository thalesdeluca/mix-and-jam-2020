using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAction : Action {
  public float damage = 40;

  public void Update() {
    var action = Input.GetButtonDown("Action");

    if (action) {
      used = true;

    }
  }

  public override void Use() {
    if (used) {
      return;
    }

    Destroy(this);
  }
}
