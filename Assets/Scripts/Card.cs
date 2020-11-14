using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionCards {
  Guard,
  Neutral,
  Focus,
  Special
}

public class Card : MonoBehaviour {
  public float moveSpeed = 180f;
  private float initialMoveSpeed = 180f;
  private Vector2 direction;
  public ActionCards action;

  // Start is called before the first frame update
  void Start() {
    Generate();
  }

  void Update() {
    if (GameController.Instance.state == GameState.Preparation) {
      Move();
    }
  }

  void RandomizeDirection() {
    direction = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));

    if (direction == Vector2.zero) {
      RandomizeDirection();
    }

  }

  void Move() {
    var rigidbody = GetComponent<Rigidbody2D>();
    rigidbody.velocity = direction * moveSpeed * Time.deltaTime;
  }

  void Generate() {
    var rand = Random.Range(0, 4);
    RandomizeDirection();
    if (GameController.Instance.gauge >= 100f) {
      moveSpeed = initialMoveSpeed * (GameController.Instance.gauge / 100) * Time.deltaTime;
      if (rand <= 1) {
        rand += Random.Range(0, ((int)GameController.Instance.gauge / 100) % 3);
      }
    }
    action = (ActionCards)rand;
  }

  public void Hit() {
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    switch (action) {
      case ActionCards.Focus:
        GameController.Instance.gameObject.AddComponent<FocusAction>();
        break;
      case ActionCards.Guard:
        GameController.Instance.gameObject.AddComponent<GuardAction>();
        break;
      case ActionCards.Neutral:
        GameController.Instance.gameObject.AddComponent<NeutralAction>();
        break;
      case ActionCards.Special:
        GameController.Instance.gameObject.AddComponent<SpecialAction>();
        break;
      default:
        return;
    }
    GameController.Instance.clearCards(this.gameObject);
    GameController.Instance.ChangePhase(GameState.Battle);
  }

  private void OnCollisionStay2D(Collision2D other) {
    if (other.gameObject.tag == "Bounds") {
      RandomizeDirection();
    }
  }


}
