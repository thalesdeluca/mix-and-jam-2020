using UnityEngine;

public abstract class Action : MonoBehaviour {
  public bool used = false;
  public abstract void Use();
}