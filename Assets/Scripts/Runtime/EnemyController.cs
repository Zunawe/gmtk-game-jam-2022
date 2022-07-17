using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
  [SerializeField] private DiePickupController _diePickupPrefab;
  [SerializeField] private int _health;
  [SerializeField] private int _contactDamage;
  private Rigidbody2D _rigidbody;
  private NavMeshAgent _agent;
  private Vector3 _playerPositionOffset;

  [SerializeField] private ProjectileController _projectilePrefab;
  [SerializeField] private float _fireProjectileCooldown;
  private float _fireProjectileTimer;

  void Start () {
    _rigidbody = GetComponent<Rigidbody2D>();
  
    _agent = GetComponent<NavMeshAgent>();
    _agent.updateRotation = false;
    _agent.updateUpAxis = false;

    _playerPositionOffset = (Vector3)(Random.insideUnitCircle);
    _playerPositionOffset *= 5.0f;

    _fireProjectileTimer = Random.Range(_fireProjectileCooldown, _fireProjectileCooldown * 2);
  }

  void Update () {
    Vector3 drift = (Vector3)(Random.insideUnitCircle) * 0.0001f;
    _agent.destination = PlayerController.Instance.transform.position + _playerPositionOffset + drift;

    _fireProjectileTimer -= Time.deltaTime;
    if (_fireProjectileTimer <= 0) {
      FireProjectile();
      _fireProjectileTimer =_fireProjectileCooldown;
    }
  }

  private void OnCollisionEnter2D (Collision2D other) {
    if (other.gameObject.tag == "Player") {
      PlayerController.Instance.Damage(_contactDamage);
    }
  }

  private void FireProjectile () {
    Vector3 direction = PlayerController.Instance.transform.position - (transform.position + (Vector3)Random.insideUnitCircle);
    direction.Normalize();
    ProjectileController projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
    projectile.transform.position = transform.position + (direction * 0.8f);
    projectile.transform.parent = null;
    projectile.Direction = direction;
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
    AudioManager.Instance.PlaySfx("dice drop");
    Destroy(gameObject, 0.0f);
  }
}
