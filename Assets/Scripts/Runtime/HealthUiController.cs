using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUiController : MonoBehaviour {
  public static HealthUiController Instance { get; private set; }

  [SerializeField] private Sprite[] _dieSprites = new Sprite[7];
  private Image _image;

  void Awake () {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    _image = GetComponent<Image>();
  }

  public void UpdateHealth (int newValue) {
    _image.sprite = _dieSprites[newValue];
  }
}
