using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
  public static PlayerController Instance { get; private set; }

  [SerializeField] float _moveSpeed;

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
    Debug.Log("here");
    _movement = context.ReadValue<Vector2>();
    _movement.Normalize();
  }

  public void OnDash (InputAction.CallbackContext context) {
    Debug.Log("Dash");
  }

  public void OnShoot (InputAction.CallbackContext context) {
    Debug.Log("Shoot");
  }
}
