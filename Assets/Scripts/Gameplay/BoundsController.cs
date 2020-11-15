using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsController : MonoBehaviour {
  private void OnTriggerEnter2D(Collider2D other) {
    if (!other.isTrigger) {
      GameController.Instance.HitBounds(other.gameObject);
    }

  }
}
