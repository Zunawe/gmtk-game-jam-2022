using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
  [SerializeField] private int _health;

  void Start () {
    
  }

  void Update () {
    
  }

  public void Damage (int damage) {
    _health -= damage;

    if (_health <= 0) {
      _health = 0;
      Die();
    }
  }

  private void Die () {
    Destroy(gameObject, 0.0f);
  }
}
