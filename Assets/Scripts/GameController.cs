using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
  Initial,
  Preparation,
  Battle,
  Victory
}

public class GameController : MonoBehaviour {
  private static GameController _instance;
  public static GameController Instance { get { return _instance; } }
  public GameState state = GameState.Initial;

  public float gauge = 0f;

  public bool guarding = false;

  public delegate void UpdateState();
  public static event UpdateState updateState;

  public GameObject cardsContainer;

  public GameObject cardPrefab;

  public int amountCards = 3;

  public bool wasCardsGenerated = false;
  private List<GameObject> cards;

  // Start is called before the first frame update
  void Start() {
    ChangePhase(GameState.Preparation);
  }

  // Update is called once per frame
  void Update() {
    switch (state) {
      case GameState.Initial:
        break;
      case GameState.Preparation:
        InstantiateCards();
        break;
      case GameState.Battle:
        wasCardsGenerated = false;
        break;
      case GameState.Victory:
        break;
      default:
        break;
    }
  }

  void InstantiateCards() {
    if (wasCardsGenerated) {
      return;
    }
    cards = new List<GameObject>();
    for (int i = 0; i < amountCards; i++) {
      var card = Instantiate(cardPrefab);
      card.transform.parent = cardsContainer.transform;
      var size = cardsContainer.GetComponent<BoxCollider2D>().bounds.size;

      var randomX = Random.Range(cardsContainer.transform.position.x - (size.x / 2), cardsContainer.transform.position.x + (size.x / 2));
      var randomY = Random.Range(cardsContainer.transform.position.y - (size.y / 2), cardsContainer.transform.position.y + (size.y / 2));

      card.transform.position = new Vector2(randomX, randomY);
      cards.Add(card);
    }
    wasCardsGenerated = true;

  }

  public void ChangePhase(GameState newState) {
    if (state == GameState.Victory) {
      return;
    }
    state = newState;
    updateState();
  }

  public void clearCards() {
    foreach (var card in cards.ToArray()) {
      Destroy(card);
    }
    cards.Clear();
  }

  public void clearCards(GameObject cardRemain) {
    foreach (var card in cards.ToArray()) {
      if (card == cardRemain) {
        continue;
      }
      Destroy(card);
      cards.Remove(card);
    }
  }

  public void Damage(float damage) {
    if (guarding) {
      guarding = false;
      return;
    }

    if (state == GameState.Battle) {
      gauge += damage;
    }
  }

  public void Guard() {
    if (state == GameState.Battle) {
      guarding = true;
    }
  }


  private void Awake() {
    if (_instance != null && _instance != this) {
      Destroy(this.gameObject);
    } else {
      _instance = this;
    }
  }
}
