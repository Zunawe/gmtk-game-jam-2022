using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwingController : MonoBehaviour {
  public float Lifetime;
  public int Damage;

  void Update () {
    Lifetime -= Time.deltaTime;

    if (Lifetime <= 0) {
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Enemy") {
      AudioManager.Instance.PlaySfx("stick");
      Debug.Log("Hit");
      other.GetComponentInParent<EnemyController>().Damage(Damage);
    }
  }
}
