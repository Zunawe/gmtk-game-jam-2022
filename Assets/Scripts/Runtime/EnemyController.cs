using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
  [SerializeField] private DiePickupController _diePickupPrefab;
  [SerializeField] private int _health;
  [SerializeField] private int _contactDamage;

  void Start () {
    
  }

  void Update () {
    
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.gameObject.tag == "Player") {
      PlayerController.Instance.Damage(_contactDamage);
    }
  }

  public void Damage (int damage) {
    _health -= damage;

    if (_health <= 0) {
      _health = 0;
      Die();
    }
  }

  private void Die () {
    DiePickupController pickup = Instantiate(_diePickupPrefab, transform.position, Quaternion.identity);
    pickup.Value = Random.Range(1, 7);
    Destroy(gameObject, 0.0f);
  }
}
