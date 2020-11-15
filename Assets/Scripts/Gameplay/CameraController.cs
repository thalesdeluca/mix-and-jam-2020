using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
  private Vector3 point = new Vector3(0, 0, -10);
  public float speed = 10f;

  void Update() {
    this.transform.position = Vector3.Lerp(this.transform.position, point, speed * Time.deltaTime);
  }

  public void MoveTo(Vector3 point) {
    this.point = point;
  }
}
