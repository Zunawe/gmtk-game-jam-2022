using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCostController : MonoBehaviour {
  [SerializeField] private GameObject _dieCostDieIndicatorPrefab;
  [SerializeField] private Sprite[] _indicatorSprites;

  public void UpdateCost (int[] cost) {
    foreach(Transform child in transform) {
      Destroy(child.gameObject);
    }

    int numDice = 0;
    for (int i = 0; i < cost.Length; ++i) {
      numDice += cost[i];
    }
    
    float width = _indicatorSprites[0].bounds.size.x + 0.1f;

    int k = 0;
    for (int i = 0; i < cost.Length; ++i) {
      for (int j = 0; j < cost[i]; ++j) {
        Vector3 position = new Vector3((k * width) - ((numDice - 1) * width / 2), 0, 0);
        GameObject indicator = new GameObject();
        indicator.transform.parent = transform;
        indicator.transform.localPosition = position;
        SpriteRenderer spriteRenderer = indicator.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _indicatorSprites[i];
        ++k;
      }
    }
  }
}
