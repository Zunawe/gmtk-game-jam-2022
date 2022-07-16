using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
  public static WaveManager Instance { get; private set; }

  private const int MAX_ACTIVE_ENEMIES = 5;

  [SerializeField] private EnemyController _enemyPrefab;

  [SerializeField] private int _currentLevel;

  [SerializeField] private float _downtime;
  private float _downtimeTimer;

  [SerializeField] private GameObject _spawnPointContainer;
  private List<Vector3> _spawnPoints;

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

    _spawnPoints = new List<Vector3>();
    foreach (Transform child in _spawnPointContainer.transform) {
      _spawnPoints.Add(child.position);
    }
  }

  void Update () {
    if (_downtimeTimer > 0) {
      _downtimeTimer -= Time.deltaTime;

      if (_downtimeTimer <= 0) {
        ++_currentLevel;

        for (int i = 0; i < _currentLevel * 2; ++i) {
          EnemyController newEnemy = Instantiate(_enemyPrefab, _spawnPoints[Random.Range(0, _spawnPoints.Count)], Quaternion.identity);
          newEnemy.gameObject.SetActive(false);
          _enemies.Add(newEnemy);
        }
      }
    } else {
      List<EnemyController> notYetSpawned = new List<EnemyController>();
      List<EnemyController> activeEnemies = new List<EnemyController>();

      bool waveComplete = true;
      foreach (EnemyController enemy in _enemies) {
        if (enemy != null) {
          waveComplete = false;

          if (enemy.gameObject.activeSelf == true) {
            activeEnemies.Add(enemy);
          } else {
            notYetSpawned.Add(enemy);
          }
        }
      }

      if (waveComplete) {
        _enemies.Clear();
        _downtimeTimer = _downtime;
      } else if (activeEnemies.Count < MAX_ACTIVE_ENEMIES && notYetSpawned.Count > 0) {
        notYetSpawned[Random.Range(0, notYetSpawned.Count)].gameObject.SetActive(true);
      }
    }
  }

  void OnDestroy () {
    if (Instance == this) {
      Instance = null;
    }
  }
}
