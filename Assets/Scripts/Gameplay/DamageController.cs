using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageController : MonoBehaviour {
  public float gauge = 0f;


  public void TakeDamage(float damage) {
    if (this.gameObject.name == "Player") {
      if (GameController.Instance.guarding) {
        GameController.Instance.guarding = false;
        return;
      }
    }

    if (this.gameObject.name == "Enemy") {
      var enemy = GetComponent<EnemyController>();
      if (enemy.guarding) {
        enemy.guarding = false;
        return;
      }
    }

    if (GameController.Instance.state == GameState.Battle) {
      gauge += damage;
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.layer == 14) {
      TakeDamage(other.gameObject.transform.parent.GetComponent<Action>().damage);
    }
  }
}

