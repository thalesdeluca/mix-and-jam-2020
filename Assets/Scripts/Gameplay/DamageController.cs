using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageController : MonoBehaviour {
  public float gauge = 0f;

  private GameObject lastColliderId;

  private int id = 0;

  public float knockbackTime = 3f;

  public float time = 0;
  public bool canBeKnockedback = false;

  public bool onKnockback = false;

  private void Update() {
    if (lastColliderId) {
      var obj = lastColliderId.GetComponent<Action>();

      if (obj && !obj.hitbox) {
        onKnockback = false;
        canBeKnockedback = false;
      }
    }
  }

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
      onKnockback = true;
      Knockback(damage);
    }


  }

  void Knockback(float damage) {
    var rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    var direction = this.gameObject.name == "Player" ? -1 : 1;
    //rigidbody.velocity = new Vector2(rigidbody.velocity.x + (gauge * direction / 2f), rigidbody.velocity.y);
    rigidbody.AddForce(new Vector2((gauge * direction / 10), damage / 60), ForceMode2D.Impulse);

    if (this.gameObject.name == "Enemy") {
      GetComponent<EnemyController>().ChangeState(EnemyState.Knockback);
    }


    if (this.gameObject.name == "Player") {
      GameController.Instance.Knockback();
    }
    Debug.Log("knockback");
    canBeKnockedback = false;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    Debug.Log(other.gameObject.name);
    if (time >= knockbackTime) {
      onKnockback = false;
    }

    if (time >= knockbackTime + 2) {
      time = 0;
      canBeKnockedback = true;
    }


    if (other.gameObject.layer == 14 && id == other.gameObject.GetInstanceID()) {

      if (canBeKnockedback) {
        Debug.Log("canbe");
        Knockback(other.gameObject.transform.parent.GetComponent<Action>().damage);
      }
    }

    if (other.gameObject.layer == 14 && id != other.gameObject.GetInstanceID()) {
      canBeKnockedback = true;
      id = other.gameObject.GetInstanceID();
      lastColliderId = other.gameObject;
      TakeDamage(other.gameObject.transform.parent.GetComponent<Action>().damage);
    }
  }

  private void OnTriggerStay2D(Collider2D other) {
    time += Time.deltaTime;
    if (time >= knockbackTime) {
      onKnockback = false;
    }

    if (time >= knockbackTime + 1) {
      time = 0;
      canBeKnockedback = true;
    }


    if (other.gameObject.layer == 14 && id == other.gameObject.GetInstanceID()) {
      onKnockback = true;
      Debug.Log("Stay");
      if (canBeKnockedback) {
        Debug.Log("canbe");
        Knockback(other.gameObject.transform.parent.GetComponent<Action>().damage);
      }
    }
  }


  private void OnTriggerExit2D(Collider2D other) {
    if (other.gameObject.layer == 14 && id == other.gameObject.GetInstanceID()) {
      onKnockback = false;
    }
  }
}

