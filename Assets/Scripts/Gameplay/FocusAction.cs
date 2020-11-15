using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAction : Action {

  public int frames = 80;

  private int hitFrame = 30;

  private int endFrame = 70;

  private Controller controller;


  void Start() {
    range = 4.30f;
    damage = 120f;
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

    var size = obj.GetComponent<BoxCollider2D>().bounds.size;

    var gameObject = Instantiate(GameController.Instance.hitbox, obj.transform);
    Vector2 offset = this.gameObject.name == "Enemy" ? -new Vector2((range / 2f) + 0.5f, 0)
   : new Vector2((range / 2f) + 0.5f, 0);
    gameObject.transform.localScale = new Vector3(range, 1, 1);
    gameObject.transform.localPosition = offset;
    return gameObject;
  }


  public override int Use() {

    used = true;
    return frames;
  }
}
