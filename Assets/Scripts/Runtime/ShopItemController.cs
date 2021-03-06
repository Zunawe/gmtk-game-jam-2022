using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemController : MonoBehaviour {
  public int[] _cost = new int[6];

  [SerializeField] private SpriteRenderer _spriteRenderer;
  
  PlayerController.WeaponType[] possibleTypes = new[] {
    PlayerController.WeaponType.STICK,
    PlayerController.WeaponType.CARD
  };
  
  public Sprite[] Sprites = new Sprite[2];
  public PlayerController.WeaponType _weaponType = PlayerController.WeaponType.STICK;

  [SerializeField] private DieCostController _dieCostController;

  void Start () {
    Generate();
  }

  private void OnTriggerEnter2D (Collider2D other) {
    if (other.tag == "Player") {
      List<int> spentDice = GetSpentDice();
      if (spentDice == null) {
        // Debug.Log("Can't Afford");
      } else {
        foreach (int die in spentDice) {
          PlayerController.Instance.RemoveDie(die);
        }
        PlayerController.Instance._currentWeapon = _weaponType;

        AudioManager.Instance.PlaySfx("purchase");
        Destroy(gameObject);
      }
    }
  }

  private List<int> GetSpentDice () {
    int numDice = 0;
    for (int i = 0; i < _cost.Length; ++i) {
      numDice += _cost[i];
    }

    int[] duplicatedDice = PlayerController.Instance.GetDice();
    List<int> foundDice = new List<int>();

    for (int i = 0; i < _cost.Length; ++i) {
      for (int j = 0; j < _cost[i]; ++j) {
        for (int k = i; k < duplicatedDice.Length; ++k) {
          if (duplicatedDice[k] > 0) {
            --duplicatedDice[k];
            foundDice.Add(k + 1);
            break;
          }
        }
      }
    }

    if (foundDice.Count < numDice) {
      return null;
    }
    return foundDice;
  }

  public void Generate () {
    _weaponType = possibleTypes[Random.Range(0, possibleTypes.Length)];
    _spriteRenderer.sprite = Sprites[(int)_weaponType];

    for (int i = 0; i < _cost.Length; ++i) {
      _cost[i] = 0;
    }

    ++_cost[Random.Range(0, 6)];
    ++_cost[Random.Range(0, 6)];
    ++_cost[Random.Range(0, 6)];

    _dieCostController.UpdateCost(_cost);
  }
}
