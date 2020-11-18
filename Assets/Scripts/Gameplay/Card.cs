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

  public Sprite cardS;
  public Sprite cardN;
  public Sprite cardG;
  public Sprite cardF;

  public Transform hitPosition;

  public bool hit = false;


  // Start is called before the first frame update
  void Start() {
    Generate();
    GameController.updateState += ResetVelocity;
    hitPosition = GameObject.Find("CardHit").transform;
  }

  void OnDestroy() {
    GameController.updateState -= ResetVelocity;
  }

  void Update() {
    if (GameController.Instance.state == GameState.Preparation) {
      Move();
    }

    if (hit) {
      this.transform.position = Vector2.MoveTowards(this.transform.position, hitPosition.position, 50f * Time.deltaTime);
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
    var rand = Random.Range(0, 4);
    RandomizeDirection();
    if (GameController.Instance.gauge >= 100f) {
      moveSpeed = initialMoveSpeed * (GameController.Instance.gauge / 100);
      if (rand <= 1) {
        rand += Random.Range(0, ((int)GameController.Instance.gauge / 100) % 3);
      }
    }
    action = (ActionCards)rand;

    switch (action) {
      case ActionCards.Neutral:
        GetComponent<SpriteRenderer>().sprite = cardN;
        break;
      case ActionCards.Focus:
        GetComponent<SpriteRenderer>().sprite = cardF;
        break;
      case ActionCards.Special:
        GetComponent<SpriteRenderer>().sprite = cardS;
        break;
      case ActionCards.Guard:
        GetComponent<SpriteRenderer>().sprite = cardG;
        break;
      default:
        break;
    }
  }

  public void Hit() {
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    hit = true;
    this.transform.parent = GameObject.Find("CardHit").transform;
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
