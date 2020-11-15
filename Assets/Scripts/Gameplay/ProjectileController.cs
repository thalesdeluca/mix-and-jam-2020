using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
  private Vector2 destination;

  private bool reached = false;

  private float projectileSpeed = 12f;

  private Collider2D collider;

  private SlingshotController slingshot;

  // Update is called once per frame
  void Update() {
    if (destination != null) {

      if (reached) {
        slingshot.EnableAim();
        Destroy(this.gameObject);
        return;
      }

      var rigidbody = GetComponent<Rigidbody2D>();
      Vector2 newPosition = Vector2.MoveTowards(rigidbody.transform.position, destination, projectileSpeed * Time.deltaTime);
      this.transform.position = newPosition;

      var distanceRemaining = (Vector2.Distance(destination, newPosition) / 100f) * Time.deltaTime;

      if (distanceRemaining == 0) {
        reached = true;

        if (collider) {
          collider.gameObject.GetComponent<Card>().Hit();

        } else {
          slingshot.EnableAim(destination);
        }
        Destroy(this.gameObject);
      }
    }
  }

  public void Init(Vector2 destination, SlingshotController slingshot) {
    this.destination = destination;
    this.slingshot = slingshot;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.tag == "Card") {
      collider = other;
    }

  }

  private void OnTriggerExit2D(Collider2D other) {
    collider = null;
  }
}
