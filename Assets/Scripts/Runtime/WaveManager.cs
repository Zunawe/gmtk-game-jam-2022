using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
  public static WaveManager Instance { get; private set; }

  [SerializeField] private EnemyController _enemyPrefab;

  [SerializeField] private int _currentLevel;

  [SerializeField] private float _downtime;
  private float _downtimeTimer;

  private List<EnemyController> _enemies = new List<EnemyController>();

  void Awake () {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    _downtimeTimer = 5.0f; // Time to first wave
  }

  void Update () {
    if (_downtimeTimer > 0) {
      _downtimeTimer -= Time.deltaTime;

      if (_downtimeTimer <= 0) {
        ++_currentLevel;

        for (int i = 0; i < _currentLevel; ++i) {
          _enemies.Add(Instantiate(_enemyPrefab, transform.position, Quaternion.identity));
        }
      }
    } else {
      bool waveComplete = true;
      foreach (EnemyController enemy in _enemies) {
        if (enemy != null) {
          waveComplete = false;
          break;
        }
      }

      if (waveComplete) {
        _enemies.Clear();
        _downtimeTimer = _downtime;
      }
    }
  }

  void OnDestroy () {
    if (Instance == this) {
      Instance = null;
    }
  }
}
