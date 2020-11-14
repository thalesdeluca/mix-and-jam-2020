using UnityEngine;

public abstract class Action : MonoBehaviour {
  public float damage = 20;
  public abstract void Use();
}