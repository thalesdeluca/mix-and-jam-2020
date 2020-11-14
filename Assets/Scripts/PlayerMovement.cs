using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  public float moveSpeed = 20f;
  public bool canMove = false;
  private Rigidbody2D rigidbody;
  // Start is called before the first frame update  void Start()
  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
    GameController.updateState += EnableMove;
  }

  // Update is called once per frame
  void Update() {
    var horizontal = Input.GetAxisRaw("Horizontal");
    var vertical = Input.GetAxisRaw("Vertical");

    if (canMove) {
      Debug.Log(horizontal + " " + vertical);
      Vector2 direction = new Vector2(horizontal, vertical);
      rigidbody.AddForce(direction * moveSpeed);
    }
  }

  void EnableMove() {
    canMove = true;
  }
}
