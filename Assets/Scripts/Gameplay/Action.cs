using UnityEngine;

public abstract class Action : MonoBehaviour {
  public bool used = false;
  public float range = 1f;

  public float damage = 0f;

  public GameObject hitbox;
  public abstract int Use();
}