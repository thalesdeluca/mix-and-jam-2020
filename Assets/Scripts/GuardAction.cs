using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAction : Action {
  public override void Use() {
    if (used) {
      return;
    }

    var action = Input.GetButtonDown("Action");

    if (action) {
      used = true;
      GameController.Instance.Guard();
    }
    Destroy(this);
  }
}
