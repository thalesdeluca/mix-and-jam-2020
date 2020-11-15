using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : Action {
  public int frames = 50;
  private int hitFrame = 30;

  private int endFrame = 50;

  private Controller controller;

  void Start() {
    range = 2f;
    damage = 60f;
    var obj = this.gameObject.name == "Enemy" ? this.gameObject : GameController.Instance.gameObject;
    controller = obj.GetComponent<Controller>();

  }

  void Update() {
    if (!GameController.Instance.ready) {
      return;
    }

    if (used) {
      if (controller.frames == hitFrame) {
        hitbox = InstantiateHitbox();
      }

      if (controller.frames == endFrame && hitbox) {
        Destroy(hitbox);
      }

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
        GameController.Instance.WaitFrames(Use());
      }
    }
  }

  GameObject InstantiateHitbox() {
    GameObject obj = this.gameObject.name == "Enemy" ? this.gameObject : GameObject.Find("Player");

    var gameObject = Instantiate(GameController.Instance.hitbox, obj.transform);
    Vector2 offset = this.gameObject.name == "Enemy" ? -new Vector2((range / 2f) + 0.5f, 0)
  : new Vector2((range / 2f) + 0.5f, 0);
    gameObject.transform.localScale = new Vector3(range, 1, 1);
    gameObject.transform.localPosition = offset;
    return gameObject;
  }

  public override int Use() {
    if (used) {
      return 0;
    }
    used = true;
    return frames;
  }
}
