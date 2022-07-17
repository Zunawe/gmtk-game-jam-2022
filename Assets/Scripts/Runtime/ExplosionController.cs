using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {
  public float Lifetime;
  public float Size;
  public int Damage;

  void Start () {
    transform.localScale = new Vector3(Size, Size, Size);
    AudioManager.Instance.PlaySfx("explosion");
  }

  void Update () {
    Lifetime -= Time.deltaTime;

    if (Lifetime <= 0) {
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Enemy") {
      other.GetComponentInParent<EnemyController>().Damage(Damage);
    }
  }
}
