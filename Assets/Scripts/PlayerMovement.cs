using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  public float moveSpeed = 300f;
  public float jumpForce = 80f;
  public bool canMove = false;
  private Rigidbody2D rigidbody;

  public bool isGrounded = false;


  // Start is called before the first frame update  void Start()
  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
    GameController.updateState += EnableMove;
  }

  // Update is called once per frame
  void Update() {
    var horizontal = Input.GetAxisRaw("Horizontal");
    var jump = Input.GetAxis("Jump");

    if (canMove) {
      rigidbody.velocity = new Vector2(horizontal * moveSpeed * Time.deltaTime, rigidbody.velocity.y);

      if (isGrounded) {
        Vector2 jumpVector = new Vector2(0, jump * jumpForce);
        rigidbody.AddForce(jumpVector);
      }
    }
  }

  void EnableMove() {
    if (GameController.Instance.state == GameState.Battle) {
      canMove = true;
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
