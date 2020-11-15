using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState {
  Attacking,
  Moving,
  Knockback,
}

public enum EnemyBehaviour {
  Agressive,
  Defensive,
  Evasive,
  Recover
}

public class EnemyController : Controller {

  public EnemyState state;
  public EnemyBehaviour behaviour;
  public float gauge;

  public bool guarding = false;
  public float moveSpeed = 3f;
  public float jumpForce = 20f;
  public bool canMove = false;
  private Rigidbody2D rigidbody;
  public bool isGrounded = false;
  public Transform initialPosition;

  public Vector2 direction = Vector2.zero;

  public Action action;

  private bool prepared = false;

  public float actionTimeReset = 2f;

  public float actionTime = 0f;

  private DamageController damageController;

  void Start() {
    rigidbody = GetComponent<Rigidbody2D>();
    damageController = GetComponent<DamageController>();
  }

  void Awake() {
    GameController.updateState += Prepare;
  }

  private void OnDestroy() {
    GameController.updateState -= Prepare;
  }

  void Update() {
    if (GameController.Instance.state == GameState.Preparation) {
      ToInitial();
    }

    if (GameController.Instance.state != GameState.Battle) {
      return;
    }

    if (!GameController.Instance.ready) {
      return;
    }

    if (state == EnemyState.Knockback && !damageController.onKnockback) {
      ChangeState(EnemyState.Moving);
    }


    if (state == EnemyState.Attacking) {
      if (frames < waitFrames) {
        frames++;
        return;
      }
      ChangeState(EnemyState.Moving);
      Destroy(action);
      frames = 0;
    }



    if (state == EnemyState.Knockback) {
      MoveKnockback();
    } else {
      var player = GameObject.Find("Player");

      var distance = Vector2.Distance(player.transform.position, rigidbody.position);


      GetDirection(player, actionTime >= actionTimeReset);
      if (distance <= 1.5) {
        direction = new Vector2(rigidbody.position.x - player.transform.position.x, direction.y).normalized;

        if (behaviour == EnemyBehaviour.Recover) {
          direction.x *= -1;
          Jump();
        }
      }

      Move();

      CheckAction(distance);
    }

    actionTime += Time.deltaTime;
  }

  void CheckAction(float distance) {
    if (actionTime >= actionTimeReset) {
      actionTime = 0;
      if (action) {
        if (distance < action.range) {
          var shouldAttack = System.Convert.ToBoolean(Random.Range(0, 3));


          if (shouldAttack) {
            if (EnemyState.Attacking == state) {
              return;
            }

            ChangeState(EnemyState.Attacking);

            WaitFrames(action.Use());
          }
        }
      }

    }
  }

  void GetDirection(GameObject player, bool shouldUpdate) {
    if (state == EnemyState.Attacking) {
      rigidbody.velocity = Vector2.zero;
    }

    if (shouldUpdate) {
      behaviour = behaviour != EnemyBehaviour.Recover ? GetBehaviour() : behaviour;
      if (behaviour == EnemyBehaviour.Agressive) {
        direction = new Vector2(player.transform.position.x - rigidbody.position.x, rigidbody.velocity.y).normalized;
      } else if (behaviour == EnemyBehaviour.Defensive || behaviour == EnemyBehaviour.Evasive) {
        var towards = Random.Range(0, 2);
        towards = towards == 0 ? -1 : towards;
        direction = new Vector2((player.transform.position.x - rigidbody.position.x) * towards, rigidbody.velocity.y).normalized;
      } else {
        direction = new Vector2(-1, 0).normalized;
      }
    }
  }

  void Move() {
    if (state == EnemyState.Attacking) {
      return;
    }
    this.rigidbody.velocity = new Vector2(direction.normalized.x * moveSpeed, rigidbody.velocity.y);
    ChangeState(EnemyState.Moving);
    if (this.rigidbody.velocity.x < 0 && this.rigidbody.transform.position.x <= -1) {
      this.rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
    }
  }

  void MoveKnockback() {

    if (damageController.onKnockback) {
      return;
    }

    this.rigidbody.velocity = new Vector2(-moveSpeed / 4, this.rigidbody.velocity.y);

    if (this.rigidbody.velocity.x <= -moveSpeed / 2) {
      this.rigidbody.velocity = new Vector2(-moveSpeed / 2, rigidbody.velocity.y);
    }

  }

  public void ChangeState(EnemyState newState) {
    if (behaviour == EnemyBehaviour.Recover) {
      return;
    }
    state = newState;
  }


  void ToInitial() {
    this.transform.position = Vector2.MoveTowards(this.transform.position, initialPosition.position, 50f * Time.deltaTime);

    this.rigidbody.gravityScale = 0;
  }

  void Prepare() {
    if (GameController.Instance.state != GameState.Preparation) {
      this.rigidbody.gravityScale = 1;
      return;
    }
    if (action) {
      Destroy(action);
    }

    var rand = Random.Range(0, 4);
    if (gauge >= 100f) {

      if (rand <= 1) {
        rand += Random.Range(0, ((int)gauge / 100) % 3);
      }
    }

    ActionCards card = (ActionCards)rand;

    switch (card) {

      case ActionCards.Neutral:
        action = this.gameObject.AddComponent<NeutralAction>();
        break;
      case ActionCards.Guard:
        action = this.gameObject.AddComponent<GuardAction>();
        break;

      case ActionCards.Focus:
        action = this.gameObject.AddComponent<FocusAction>();
        break;

      case ActionCards.Special:
        action = this.gameObject.AddComponent<SpecialAction>();
        break;
      default:
        return;
    }
  }

  public void WaitFrames(int frames) {
    if (GameController.Instance.state == GameState.Battle) {
      this.waitFrames = frames;
      this.frames = 0;
      rigidbody.velocity = Vector2.zero;
      direction = Vector2.zero;
    }
  }
  public void Guard() {
    guarding = true;
  }

  EnemyBehaviour GetBehaviour() {
    if (!action) {
      return EnemyBehaviour.Evasive;
    }
    if (action is FocusAction) {
      return EnemyBehaviour.Agressive;
    }
    if (action is NeutralAction) {
      return EnemyBehaviour.Evasive;
    }
    if (action is SpecialAction) {
      return EnemyBehaviour.Agressive;
    }
    if (action is GuardAction) {
      return EnemyBehaviour.Defensive;
    }
    return EnemyBehaviour.Evasive;
  }

  void Jump() {
    if (!isGrounded) {
      return;
    }
    Vector2 jumpVector = new Vector2(0, jumpForce * 3);
    rigidbody.AddForce(jumpVector);

  }

  private void OnTriggerExit2D(Collider2D other) {

    if (other.gameObject.name == "Stage") {
      isGrounded = false;
    }

    if (other.gameObject.name == "Bounds") {
      behaviour = EnemyBehaviour.Recover;
      ChangeState(EnemyState.Moving);
      GetDirection(GameObject.Find("Player"), true);
    }
  }



  private void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.name == "Stage") {
      isGrounded = true;
    }

    if (other.gameObject.name == "Bounds") {
      behaviour = GetBehaviour();
      ChangeState(EnemyState.Moving);
      GetDirection(GameObject.Find("Player"), true);
      Jump();
    }
  }

}
