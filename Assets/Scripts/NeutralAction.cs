using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : Action {
  public float damage = 20;

  public override void Use() {
    if (used) {
      return;
    }

    var action = Input.GetButtonDown("Action");

    if (action) {
      used = true;
      GameController.Instance.Damage(damage);
    }
    Destroy(this);
  }
}
