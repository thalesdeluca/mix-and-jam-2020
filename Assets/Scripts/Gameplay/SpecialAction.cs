using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAction : Action {

  public int frames = 250;

  private int hitFrame = 70;

  private int endFrame = 230;

  private Controller controller;

  void Start() {
    range = 6f;
    damage = 200f;
    var obj = this.gameObject.name == "Enemy" ? this.gameObject : GameController.Instance.gameObject;
    controller = obj.GetComponent<Controller>();

  }

  void Update() {
    if (!GameController.Instance.ready && GameController.Instance.state != GameState.Battle) {
      return;
    }

    if (used) {
      if (controller.frames == hitFrame) {
        hitbox = InstantiateHitbox();
        if (hitbox == null) {
          return;
        }
      }

      if (controller.frames == endFrame && hitbox) {
        Destroy(hitbox);
      }

      if (controller.frames >= frames) {
        var card = GameObject.Find("CardHit");
        if (card.transform.childCount > 0) {
          var child = card.transform.GetChild(0);
          if (child) {
            Destroy(child.gameObject);
          }
        }


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
    if (GameController.Instance.state != GameState.Battle) {
      return null;
    }

    GameObject obj = this.gameObject.name == "Enemy" ? this.gameObject : GameObject.Find("Player");

    var size = obj.GetComponent<BoxCollider2D>().bounds.size;

    var gameObject = Instantiate(GameController.Instance.hitbox, obj.transform);
    Vector2 offset = this.gameObject.name == "Enemy" ? -new Vector2((range / 2f) + 0.5f, 0)
  : new Vector2((range / 2f) + 0.5f, 0);
    var joint = gameObject.transform.Find("joint");
    joint.transform.localScale = new Vector3(range, 1, 1);

    var punch = gameObject.transform.Find("arm");
    punch.transform.localPosition = new Vector2(joint.GetComponent<BoxCollider2D>().bounds.size.x + 0.8f, 0);
    gameObject.transform.localPosition = offset;
    if (this.gameObject.name == "Enemy") {
      gameObject.transform.eulerAngles = Vector2.up * 180;
    }
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
