using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUiController : MonoBehaviour {
  public static GameOverUiController Instance { get; private set; }

  [SerializeField] private GameObject _gameOverScreen;

  void Awake () {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  void OnDestroy () {
    if (Instance == this) {
      Instance = null;
    }
  }

  public void GameOver () {
    _gameOverScreen.SetActive(true);
  }

  public void OnRetry () {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void OnQuit () {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    Application.Quit();
#endif
  }
}
