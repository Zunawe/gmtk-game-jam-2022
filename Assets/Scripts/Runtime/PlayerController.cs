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
  [SerializeField] private float _invincibilityTime;
  private float _invincibilityTimer;
  [SerializeField] private float _invincibilityFlashCycleTime;
  private float _invincibilityFlashCycleTimer;

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
  private Animator _animator;
  [SerializeField] private SpriteRenderer _spriteRenderer;
  private bool _isFacingUp;

  private int[] _dice = new int[6];

  void Awake () {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    _rigidbody = GetComponent<Rigidbody2D>();
    _animator = GetComponent<Animator>();
    Heal(_maxHealth);
  }

  void Update () {
    if (_invincibilityTimer > 0) {
      _invincibilityTimer -= Time.deltaTime;
      _invincibilityFlashCycleTimer -= Time.deltaTime;

      if (_invincibilityFlashCycleTimer <= 0) {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        _invincibilityFlashCycleTimer = _invincibilityFlashCycleTime;
      }
    } else {
      _spriteRenderer.enabled = true;
    }

    if (_rigidbody.velocity.magnitude > 0.01f) {
      _footstepTimer -= Time.deltaTime;

      if (_footstepTimer < 0) {
        AudioManager.Instance.PlaySfx("footstep");
        _footstepTimer = _footstepTime;
      }
    }

    if (_isFacingUp && _rigidbody.velocity.y < 0) {
      _isFacingUp = false;
    } else if (!_isFacingUp && _rigidbody.velocity.y > 0) {
      _isFacingUp = true;
    }

    _animator.SetFloat("VelocityX", _rigidbody.velocity.x);
    _animator.SetBool("IsFacingUp", _isFacingUp);
    _animator.SetFloat("SpeedX", Mathf.Abs(_rigidbody.velocity.x));
    _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
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

  public void OnMove (InputAction.CallbackContext context) {
    _movement = context.ReadValue<Vector2>();
    _movement.Normalize();
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

  public void OnDash (InputAction.CallbackContext context) {
    if (context.performed) {
      if (_dice[0] >= 1 && _dashTimer <= 0) {
        AudioManager.Instance.PlaySfx("dash");
        _dashStartPosition = transform.position;
        _dashDestination = _dashStartPosition + ((new Vector3(_movement.x, _movement.y, 0)) * _dashDistance);
        _dashTimer = _dashTime;
        RemoveDie(1);
      }
    }
  }

  public void OnRefreshShop (InputAction.CallbackContext context) {
    if (context.performed) {
      if (_dice[3] >= 4) {
        AudioManager.Instance.PlaySfx("shop reroll");
        ShopController.Instance.RefreshShop();
        RemoveDie(4, 4);
      }
    }
  }

  public void OnHeal (InputAction.CallbackContext context) {
    if (context.performed) {
      if (_dice[5] >= 6) {
        AudioManager.Instance.PlaySfx("heal");
        Heal(1);
        RemoveDie(6, 6);
      }
    }
  }

  public void AddDie (int value, int quantity = 1) {
    Debug.Log("Picked up die (" + value + ")");
    if (_dice[value - 1] < Mathf.Ceil((float)value / 2.0f) * 2) {
      _dice[value - 1] += quantity;
      InventoryUiController.Instance.UpdateInventory(_dice);
    }
  }

  public void RemoveDie (int value, int quantity = 1) {
    Debug.Log("Removed die (" + value + ")");
    _dice[value - 1] -= quantity;
    InventoryUiController.Instance.UpdateInventory(_dice);
  }

  public int[] GetDice () {
    int[] diceCopy = new int[_dice.Length];
    Array.Copy(_dice, diceCopy, _dice.Length);
    return diceCopy;
  }

  public void Damage (int damage) {
    if (_invincibilityTimer <= 0) {
      _currentHealth -= damage;
      HealthUiController.Instance.UpdateHealth(_currentHealth);

      _invincibilityTimer = _invincibilityTime;

      if (_currentHealth <= 0) {
        _currentHealth = 0;
        gameObject.SetActive(false);
        AudioManager.Instance.PlaySfx("losing sound");
      } else {
        AudioManager.Instance.PlaySfx("hit");
      }
    }
  }

  public void Heal (int amount) {
    _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    HealthUiController.Instance.UpdateHealth(_currentHealth);
  }
}
