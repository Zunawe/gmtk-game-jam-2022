using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePickupController : MonoBehaviour {
  public int Value;
  [SerializeField] private Sprite[] _sprites = new Sprite[6];

  void Start () {
    GetComponent<SpriteRenderer>().sprite = _sprites[Value - 1];
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Player") {
      PlayerController.Instance.AddDie(Value);

      AudioManager.Instance.PlaySfx("itempickup");
      Destroy(gameObject);
    }
  }
}
