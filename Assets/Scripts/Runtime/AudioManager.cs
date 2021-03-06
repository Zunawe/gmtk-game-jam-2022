using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.InputSystem;
#endif

public class AudioManager : MonoBehaviour {
  private Regex SFX_NAME_REGEX = new Regex(@"^([a-zA-Z ]+)\d*$");
  public static AudioManager Instance { get; private set; }

  [SerializeField] private GameObject _musicContainer;
  [SerializeField] private GameObject _sfxContainer;

#if UNITY_EDITOR
  public string TestSfxName;
  public string TestMusicName;
#endif

  private Dictionary<string, AudioSource> _music;
  private Dictionary<string, List<AudioSource>> _sfx;
  private List<AudioSource> _playingSfx;

  void Awake () {
    if (Instance == null) {
      DontDestroyOnLoad(gameObject);
      Instance = this;

      _music = new Dictionary<string, AudioSource>();
      _sfx = new Dictionary<string, List<AudioSource>>();
      _playingSfx = new List<AudioSource>();

      foreach (AudioSource track in _musicContainer.transform.GetComponentsInChildren<AudioSource>()) {
        _music.Add(track.name, track);
      }

      foreach (AudioSource sound in _sfxContainer.transform.GetComponentsInChildren<AudioSource>()) {
        string folderName = SFX_NAME_REGEX.Match(sound.name).Groups[1].Value;

        if (!_sfx.ContainsKey(folderName)) {
          _sfx.Add(folderName, new List<AudioSource>());
        }
        _sfx[folderName].Add(sound);
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

  public AudioSource PlaySfx (string name) {
    AudioSource player = PlaySfxLooping(name);
    if (player == null) {
      return null;
    }

    player.loop = false;
    Destroy(player.gameObject, player.clip.length * 1.5f);

    return player;
  }

  public AudioSource PlaySfxLooping (string name) {
    if (string.IsNullOrEmpty(name)) {
      return null;
    }

    if (_sfx.ContainsKey(name)) {
      int i = Random.Range(0, _sfx[name].Count);

      AudioSource player = Instantiate(_sfx[name][i], transform.position, Quaternion.identity);
      player.loop = true;
      player.Play();

      return player;
    } else {
      Debug.Log("Couldn't find sound effect: " + name);
      return null;
    }
  }

#if UNITY_EDITOR
  public void OnPlaySound (InputAction.CallbackContext context) {
    if (context.performed) {
      PlaySfx(TestSfxName);
    }
  }

  public void OnPlayMusic (InputAction.CallbackContext context) {
    if (context.performed) {
      PlayMusicForceRestart(TestMusicName);
    }
  }
#endif
}
