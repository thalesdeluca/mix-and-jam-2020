using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  public float moveSpeed = 3f;
  public float jumpForce = 80f;
  public bool canMove = false;
  private Rigidbody2D rigidbody;
  public bool isGrounded = false;

  public Transform initialPosition;

  // Start is called before the first frame update  void Start()
  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
  }

  private void Awake() {
    GameController.updateState += EnableMove;
  }

  void OnDestroy() {
    GameController.updateState -= EnableMove;
  }

  // Update is called once per frame
  void Update() {
    var horizontal = Input.GetAxisRaw("Horizontal");
    var jump = Input.GetAxisRaw("Jump");

    if (GameController.Instance.state == GameState.Preparation) {
      ToInitial();
    }

    if (!GameController.Instance.ready) {
      return;
    }

    if (canMove) {
      if (horizontal != 0) {
        rigidbody.velocity = new Vector2(horizontal * moveSpeed, rigidbody.velocity.y);
        var enemy = GameObject.Find("Enemy");
        var playerX = this.rigidbody.position.x + this.GetComponent<BoxCollider2D>().bounds.size.x / 2;
        var enemyX = enemy.transform.position.x - enemy.GetComponent<BoxCollider2D>().bounds.size.x / 2;
        if (playerX >= enemyX) {
          rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }

        if (rigidbody.transform.position.x >= 1 && rigidbody.velocity.x > 0 && GameController.Instance.playerState != PlayerState.Knockback) {
          rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
        }
      }



      if (isGrounded) {
        if (jump > 0) {
          if (rigidbody.velocity.y < jumpForce) {
            Vector2 jumpVector = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + 1);
            rigidbody.velocity = jumpVector;
          }

        }
      }

      if (GameController.Instance.playerState == PlayerState.Attacking) {
        canMove = false;
        this.rigidbody.velocity = Vector2.zero;
      }
    }



  }
  void ToInitial() {
    this.transform.position = Vector2.MoveTowards(this.transform.position, initialPosition.position, 50f * Time.deltaTime);

    this.rigidbody.gravityScale = 0;
  }

  void EnableMove() {
    canMove = GameController.Instance.state == GameState.Battle;
    if (!canMove) {
      if (!rigidbody) {
        return;
      }
      this.rigidbody.velocity = Vector2.zero;
    } else {
      this.rigidbody.gravityScale = 1;
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Ground") {
      isGrounded = true;
    }
  }

  private void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.tag == "Ground") {
      isGrounded = false;
    }
  }
}
