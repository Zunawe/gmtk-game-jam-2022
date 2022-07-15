using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
  [SerializeField] private float _moveSpeed;
  [SerializeField] private int _damage;
  private Vector2 _direction = new Vector2(1.0f, 0);
  public Vector2 Direction {
    get { return _direction; }
    set {
      _direction = value;
      _direction.Normalize();
    }
  }

  private Rigidbody2D _rigidbody;

  void Start () {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  void FixedUpdate () {
    _rigidbody.velocity = Direction * _moveSpeed;
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Enemy") {
      other.GetComponentInParent<EnemyController>().Damage(_damage);
    }
    Destroy(gameObject);
  }
}
