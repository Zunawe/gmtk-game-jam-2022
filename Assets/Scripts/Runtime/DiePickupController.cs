using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePickupController : MonoBehaviour {
  public int Value;
  public float DespawnTime;
  private float _despawnTimer;
  public float SecondsBeforeFlash;
  public float FlashCycleTime;
  private float _flashCycleTimer;
  [SerializeField] private Sprite[] _sprites = new Sprite[6];
  private SpriteRenderer _spriteRenderer;

  void Start () {
    _spriteRenderer = GetComponent<SpriteRenderer>();
    _spriteRenderer.sprite = _sprites[Value - 1];
    _despawnTimer = DespawnTime;
  }

  void Update () {
    _despawnTimer -= Time.deltaTime;

    if (_despawnTimer < SecondsBeforeFlash) {
      _flashCycleTimer -= Time.deltaTime;

      if (_flashCycleTimer < 0) {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        _flashCycleTimer = FlashCycleTime;
      }
    }

    if (_despawnTimer <= 0) {
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Player") {
      PlayerController.Instance.AddDie(Value);

      AudioManager.Instance.PlaySfx("itempickup");
      Destroy(gameObject);
    }
  }
}
