using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAction : Action {
  public int frames = 180;
  private int hitFrame = 10;

  private int endFrame = 100;

  private Controller controller;



  void Start() {
    range = 0f;
    var obj = this.gameObject.name == "Enemy" ? this.gameObject : GameController.Instance.gameObject;
    controller = obj.GetComponent<Controller>();

  }

  void Update() {
    if (!GameController.Instance.ready) {
      return;
    }

    if (used) {
      if (controller.frames >= frames) {
        Destroy(this);
      }
    }


    if (this.gameObject.name == "Player") {
      var action = Input.GetButtonDown("Action");
      if (action) {
        if (used) {
          return;
        }
        GameController.Instance.Guard(Use());
      }
    }

  }


  public override int Use() {
    if (used) {
      return 0;
    }
    used = true;
    return frames;
  }
}
