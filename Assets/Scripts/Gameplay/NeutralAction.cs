using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralAction : Action {
  public int frames = 50;
  private int hitFrame = 30;

  private int endFrame = 50;

  private Controller controller;

  private GameObject hitbox;

  void Start() {
    range = 2f;
    damage = 20f;
    controller = GetComponent<Controller>();
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

    if (this.gameObject.name == "GameController") {
      var action = Input.GetButtonDown("Action");
      if (action) {
        GameController.Instance.WaitFrames(Use());
      }
    }
  }

  GameObject InstantiateHitbox() {
    GameObject obj = this.gameObject.name == "Enemy" ? this.gameObject : GameObject.Find("Player");

    var size = obj.GetComponent<BoxCollider2D>().bounds.size;
    Vector2 offset = (Vector2)obj.transform.position + new Vector2(size.x, 0);
    return Instantiate(GameController.Instance.hitbox, offset, Quaternion.identity, obj.transform);
  }

  public override int Use() {
    if (used) {
      return 0;
    }
    used = true;
    return frames;
  }
}
