using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
  public static PlayerController Instance { get; private set; }

  [SerializeField] float _moveSpeed;
  [SerializeField] GameObject _reticle;
  [SerializeField] ProjectileController _projectilePrefab;

  private Rigidbody2D _rigidbody;
  private Vector2 _movement;

  void Awake () {
    if (Instance == null) {
      Instance = this;
      Initialize();
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    
  }

  void Update () {
    
  }

  void FixedUpdate () {
    _rigidbody.velocity = _movement * _moveSpeed;
  }

  void OnDestroy () {
    if (Instance == this) {
      Instance = null;
    }
  }

  private void Initialize () {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  public void OnMove (InputAction.CallbackContext context) {
    _movement = context.ReadValue<Vector2>();
    _movement.Normalize();
  }

  public void OnDash (InputAction.CallbackContext context) {
    Debug.Log("Dash");
  }

  public void OnShoot (InputAction.CallbackContext context) {
    ProjectileController projectile = Instantiate(_projectilePrefab, _reticle.transform.position, Quaternion.identity);
    projectile.Direction = _reticle.transform.localPosition;
  }

  public void OnAim (InputAction.CallbackContext context) {
    Vector2 input = context.ReadValue<Vector2>();
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(input);
    mousePosition.z = 0;

    Vector3 direction = mousePosition - transform.position;
    direction.Normalize();

    _reticle.transform.localPosition = direction;
  }
}
