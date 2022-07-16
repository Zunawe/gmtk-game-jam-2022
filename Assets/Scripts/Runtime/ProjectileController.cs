using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
  [SerializeField] private string _targetTag;
  [SerializeField] private float _moveSpeed;
  [SerializeField] private int _damage;

  [SerializeField] private string _throwSoundName;
  [SerializeField] private string _hitSoundName;
  [SerializeField] private string _travelSoundName;
  private AudioSource _travelSound;

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
    AudioManager.Instance.PlaySfx(_throwSoundName);
    _travelSound = AudioManager.Instance.PlaySfxLooping(_travelSoundName);

    if (_travelSound != null) {
      _travelSound.transform.parent = transform;
      _travelSound.transform.localPosition = Vector3.zero;
    }
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

      AudioManager.Instance.PlaySfx(_hitSoundName);
      Destroy(gameObject);
    } else if (other.tag != "Enemy" && other.tag != "Player") {
      AudioManager.Instance.PlaySfx(_hitSoundName);
      Destroy(gameObject);
    }
  }

  void OnBecameInvisible () {
    Destroy(gameObject);
  }
}
