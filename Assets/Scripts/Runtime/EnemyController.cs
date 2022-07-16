using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
  [SerializeField] private DiePickupController _diePickupPrefab;
  [SerializeField] private int _health;
  [SerializeField] private int _contactDamage;
  [SerializeField] private float _moveSpeed;
  private Rigidbody2D _rigidbody;

  void Start () {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate () {
    _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, PlayerController.Instance.transform.position, _moveSpeed * Time.fixedDeltaTime));
  }

  private void OnCollisionEnter2D (Collision2D other) {
    if (other.gameObject.tag == "Player") {
      PlayerController.Instance.Damage(_contactDamage);
    }
  }

  public void Damage (int damage) {
    if (_health > 0) {
      _health -= damage;

      if (_health <= 0) {
        _health = 0;
        Die();
      }
    }
  }

  private void Die () {
    DiePickupController pickup = Instantiate(_diePickupPrefab, transform.position, Quaternion.identity);
    pickup.Value = Random.Range(1, 7);
    Destroy(gameObject, 0.0f);
  }
}
