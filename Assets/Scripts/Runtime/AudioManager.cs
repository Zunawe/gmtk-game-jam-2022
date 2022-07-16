using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
  public static AudioManager Instance { get; private set; }

  [SerializeField] private GameObject _musicContainer;
  [SerializeField] private GameObject _sfxContainer;

  private Dictionary<string, AudioSource> _music;
  private Dictionary<string, AudioSource> _sfx;

  void Awake () {
    if (Instance == null) {
      Instance = this;

      _music = new Dictionary<string, AudioSource>();
      _sfx = new Dictionary<string, AudioSource>();

      foreach (AudioSource track in _musicContainer.transform.GetComponentsInChildren<AudioSource>()) {
        _music.Add(track.name, track);
      }
      foreach (AudioSource sound in _sfxContainer.transform.GetComponentsInChildren<AudioSource>()) {
        _sfx.Add(sound.name, sound);
      }
    } else {
      Destroy(gameObject);
    }
  }

  void OnDestroy () {
    if (Instance == this) {
      Instance = null;
    }
  }

  public void PlayMusic (string name) {
    if (_music[name].isPlaying) {
      return;
    }

    foreach (AudioSource track in _music.Values) {
      track.Stop();
    }

    _music[name].Play();
  }

  public void PlayMusicForceRestart (string name) {
    foreach (AudioSource track in _music.Values) {
      track.Stop();
    }

    _music[name].Play();
  }

  public void PlaySfx (string name) {
    _sfx[name].Play();
  }

  public void PlaySfxJittered (string name) {
    _sfx[name].pitch = Random.Range(0.9f, 1.1f);
    _sfx[name].Play();
    _sfx[name].pitch = 1.0f;
  }
}
