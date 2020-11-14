using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlingshotController : MonoBehaviour {
  public bool shot = false;
  public bool canAim = false;

  public GameObject aimPrefab;

  private GameObject aim;
  public Transform aimPosition;

  public float aimSpeed = 180f;

  private Vector2 destinationPosition;

  public GameObject projectilePrefab;
  public Transform projectileMuzzle;
  private GameObject projectile;

  private float projectileSpeed = 200f;

  // Start is called before the first frame update
  void Start() {
    GameController.updateState += EnableAim;
  }

  // Update is called once per frame
  void Update() {
    var fire = Input.GetButtonDown("Action");
    var horizontal = Input.GetAxis("Horizontal");
    var vertical = Input.GetAxis("Vertical");

    if (canAim) {

      MoveAim(horizontal, vertical);

      if (fire) {
        Fire();
      }
    }
  }

  void Fire() {
    if (!canAim) {
      return;
    }
    canAim = false;

    ThrowProjectile(aim.transform.position);
    Destroy(aim.gameObject);
  }

  void MoveAim(float horizontal, float vertical) {
    if (aim) {
      if (horizontal != 0 || vertical != 0) {
        FollowAim();
      }
      var rigidBody = aim.GetComponent<Rigidbody2D>();
      rigidBody.velocity = (new Vector2(horizontal, vertical).normalized) * aimSpeed * Time.deltaTime;
    }
  }

  void FollowAim() {
    if (!aim) {
      return;
    }

    Vector2 sides = this.transform.position - aim.transform.position;
    transform.up = -sides;
  }

  public void EnableAim() {
    if (GameController.Instance.state == GameState.Preparation) {
      aim = Instantiate(aimPrefab);
      aim.transform.position = aimPosition.position;
      canAim = true;
    }
  }

  public void EnableAim(Vector2 aimVector) {
    if (GameController.Instance.state == GameState.Preparation) {
      aim = Instantiate(aimPrefab);
      aim.transform.position = aimVector;
      canAim = true;
    }
  }

  void ThrowProjectile(Vector2 destination) {
    projectile = Instantiate(projectilePrefab);
    projectile.transform.position = projectileMuzzle.position;
    projectile.GetComponent<ProjectileController>().Init(destination, this);
  }
}
