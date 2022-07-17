using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUiController : MonoBehaviour {
  public static PauseUiController Instance { get; private set; }

  [SerializeField] private GameObject _pauseScreen;

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

  public void OnResume () {
    Pause();
  }

  public void OnQuit () {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    Application.Quit();
#endif
  }

  public void Pause () {
    _pauseScreen.SetActive(!_pauseScreen.activeSelf);
    Time.timeScale = _pauseScreen.activeSelf ? 0 : 1.0f;

    if (_pauseScreen.activeSelf) {
      var sounds = FindObjectsOfType<AudioSource>();
      foreach (var sound in sounds) {
        if (sound.tag != "Music") {
          sound.Pause();
        }
      }
    } else {
      var sounds = FindObjectsOfType<AudioSource>();
      foreach (var sound in sounds) {
        if (sound.tag != "Music") {
          sound.UnPause();
        }
      }
    }
  }
}
