using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiController : MonoBehaviour {
  public static InventoryUiController Instance { get; private set; }

  [SerializeField] private Image _inventoryDieImagePrefab;
  [SerializeField] private Sprite[] _dieSprites = new Sprite[6];

  private RectTransform _rectTransform;
  private List<Image>[] _buckets = new List<Image>[6];

  void Awake () {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    _rectTransform = GetComponent<RectTransform>();
    for (int i = 0; i < _buckets.Length; ++i) {
      _buckets[i] = new List<Image>();
    }
  }

  public void UpdateInventory (int[] dice) {
    for (int i = 0; i < _buckets.Length; ++i) {
      foreach (Image image in _buckets[i]) {
        Destroy(image.gameObject);
      }
      _buckets[i].Clear();

      for (int j = 0; j < dice[i]; ++j) {
        Image newImage = Instantiate(_inventoryDieImagePrefab, transform);
        newImage.sprite = _dieSprites[i];
        newImage.transform.localPosition = (Vector3)(_rectTransform.anchoredPosition) + (new Vector3((6 * 48) - (i * 48), j * 48, 0));
        _buckets[i].Add(newImage);
      }
    }
  }
}
