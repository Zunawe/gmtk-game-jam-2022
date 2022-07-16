using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
  public static PlayerController Instance { get; private set; }

  [Header("Health")]
  [SerializeField] private int _maxHealth;
  [SerializeField] private int _currentHealth;

  [Header("Movement")]
  [SerializeField] private float _moveSpeed;
  private Vector2 _movement;

  [Header("Dash")]
  [SerializeField] private float _dashDistance;
  [SerializeField] private float _dashTime;
  private float _dashTimer;
  private Vector3 _dashStartPosition;
  private Vector3 _dashDestination;
  private bool _isDashing;
  
  [Header("Footstep SFX")]
  [SerializeField] private float _footstepTime;
  private float _footstepTimer;

  [Header("Shooting")]
  [SerializeField] private GameObject _reticle;
  [SerializeField] private ProjectileController _projectilePrefab;

  private Rigidbody2D _rigidbody;

  private int[] _dice = new int[6];

  void Awake () {
    if (Instance == null) {
      Instance = this;
      Initialize();
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    _rigidbody = GetComponent<Rigidbody2D>();
    _currentHealth = _maxHealth;
  }

  void Update () {
    if (_rigidbody.velocity.magnitude > 0.01f) {
      _footstepTimer -= Time.deltaTime;

      if (_footstepTimer < 0) {
        AudioManager.Instance.PlaySfx("footstep");
        _footstepTimer = _footstepTime;
      }
    }
  }

  void FixedUpdate () {
    if (_dashTimer > 0) {
      _dashTimer -= Time.fixedDeltaTime;
      _rigidbody.MovePosition(Vector3.Lerp(_dashStartPosition, _dashDestination, 1.0f - (_dashTimer / _dashTime)));
    } else {
      _rigidbody.velocity = _movement * _moveSpeed;
    }
  }

  void OnDestroy () {
    if (Instance == this) {
      Instance = null;
    }
  }

  private void Initialize () {
  }

  public void OnMove (InputAction.CallbackContext context) {
    _movement = context.ReadValue<Vector2>();
    _movement.Normalize();
  }

  public void OnDash (InputAction.CallbackContext context) {
    if (context.performed) {
      if (_dashTimer <= 0) {
        AudioManager.Instance.PlaySfx("dash");
        _dashStartPosition = transform.position;
        _dashDestination = _dashStartPosition + ((new Vector3(_movement.x, _movement.y, 0)) * _dashDistance);
        _dashTimer = _dashTime;
      }
    }
  }

  public void OnShoot (InputAction.CallbackContext context) {
    if (context.performed) {
      ProjectileController projectile = Instantiate(_projectilePrefab, _reticle.transform.position, Quaternion.identity);
      projectile.Direction = _reticle.transform.localPosition;
    }
  }

  public void OnAim (InputAction.CallbackContext context) {
    Vector2 input = context.ReadValue<Vector2>();
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(input);
    mousePosition.z = 0;

    Vector3 direction = mousePosition - transform.position;
    direction.Normalize();

    _reticle.transform.localPosition = direction;
  }

  public void OnHeal (InputAction.CallbackContext context) {
    if (context.performed) {
      if (_dice[5] > 0) {
        AudioManager.Instance.PlaySfx("heal");
        _dice[5] -= 1;
        _currentHealth += 1;
      }
    }
  }

  public void AddDie (int value) {
    Debug.Log("Picked up die (" + value + ")");
    ++_dice[value - 1];
  }

  public void RemoveDie (int value) {
    Debug.Log("Removed die (" + value + ")");
    --_dice[value - 1];
  }

  public int[] GetDice () {
    int[] diceCopy = new int[_dice.Length];
    Array.Copy(_dice, diceCopy, _dice.Length);
    return diceCopy;
  }

  public void Damage (int damage) {
    _currentHealth -= damage;

    if (_currentHealth <= 0) {
      _currentHealth = 0;
      gameObject.SetActive(false);
      AudioManager.Instance.PlaySfx("losing sound");
    } else {
      AudioManager.Instance.PlaySfx("hit");
    }
  }

  public void Heal (int amount) {
    _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
  }
}
