using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePickupController : MonoBehaviour {
  public int Value;

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Player") {
      Debug.Log(PlayerController.Instance == null);
      PlayerController.Instance.AddDie(Value);

      Destroy(gameObject);
    }
  }
}
