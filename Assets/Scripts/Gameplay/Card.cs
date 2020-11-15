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
  public float moveSpeed = 3.8f;
  private float initialMoveSpeed = 3.8f;
  private Vector2 direction;
  public ActionCards action;

  // Start is called before the first frame update
  void Start() {
    Generate();
    GameController.updateState += ResetVelocity;
  }

  void OnDestroy() {
    GameController.updateState -= ResetVelocity;
  }

  void Update() {
    if (GameController.Instance.state == GameState.Preparation) {
      Move();
    }
  }

  void ResetVelocity() {
    if (GameController.Instance.state != GameState.Preparation) {
      if (this) {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
      }

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
    rigidbody.velocity = direction.normalized * moveSpeed;
  }

  void Generate() {
    var rand = 3; //Random.Range(0, 4);
    RandomizeDirection();
    if (GameController.Instance.gauge >= 100f) {
      moveSpeed = initialMoveSpeed * (GameController.Instance.gauge / 100);
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
        GameObject.Find("Player").AddComponent<FocusAction>();
        break;
      case ActionCards.Guard:
        GameObject.Find("Player").AddComponent<GuardAction>();
        break;
      case ActionCards.Neutral:
        GameObject.Find("Player").AddComponent<NeutralAction>();
        break;
      case ActionCards.Special:
        GameObject.Find("Player").AddComponent<SpecialAction>();
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
