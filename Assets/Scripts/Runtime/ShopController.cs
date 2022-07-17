using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour {
  private const int NUMBER_OF_ITEMS = 2;

  public static ShopController Instance { get; private set; }

  private ShopItemController[] _items = new ShopItemController[NUMBER_OF_ITEMS];
  [SerializeField] private ShopItemController _shopItemControllerPrefab;

  void Awake () {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  void Start () {
    RefreshShop();
  }

  void Update () {
    
  }

  public void RefreshShop () {
    for (int i = 0; i < _items.Length; ++i) {
      if (_items[i] != null) {
        Destroy(_items[i].gameObject);
      }

      _items[i] = Instantiate(_shopItemControllerPrefab, transform);
      _items[i].transform.localPosition = new Vector3((i * 2.0f) - ((_items.Length - 1) * 2.0f / 2.0f), -3f, 0);
    }
  }
}
