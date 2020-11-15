using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAction : Action {
  public int frames = 180;
  private int hitFrame = 30;

  private int endFrame = 50;

  private Controller controller;

  private GameObject hitbox;


  void Start() {
    range = 0f;
    controller = GetComponent<Controller>();
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


    if (this.gameObject.name == "GameController") {
      var action = Input.GetButtonDown("Action");
      if (action) {
        GameController.Instance.WaitFrames(Use());
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
