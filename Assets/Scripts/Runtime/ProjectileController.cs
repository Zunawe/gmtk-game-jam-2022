using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
  [SerializeField] private string _targetTag;
  [SerializeField] private float _moveSpeed;
  [SerializeField] private int _damage;
  private Vector3 _direction = new Vector3(1.0f, 0, 0);
  public Vector3 Direction {
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
    _rigidbody.velocity = (Vector2)(Direction * _moveSpeed);
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (_targetTag == other.tag) {
      if (other.tag == "Enemy") {
        other.GetComponentInParent<EnemyController>().Damage(_damage);
      } else if (other.tag == "Player") {
        PlayerController.Instance.Damage(_damage);
      }
      Destroy(gameObject);
    } else if (other.tag != "Enemy" && other.tag != "Player") {
      Destroy(gameObject);
    }
  }
}
