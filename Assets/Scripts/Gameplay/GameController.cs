using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
  Initial,
  Preparation,
  Battle,
  Victory
}

public enum PlayerState {
  Attacking,
  Moving
}

public class GameController : Controller {
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
  public bool showPamphlet = false;
  public GameObject pamphlet;
  public GameObject victory;
  private List<GameObject> cards;

  public PlayerState playerState = PlayerState.Moving;

  public float battleTime = 10f;
  public float preparationTime = 10f;
  public float time = 0f;

  public float readyTime = 3f;
  public bool ready = false;

  private bool won = false;

  public Camera camera;

  public GameObject hitbox;

  // Start is called before the first frame update
  void Start() {
    ChangePhase(GameState.Preparation);
    showPamphlet = true;

    if (pamphlet) {
      //pamphlet.SetActive(true);
    }
  }

  // Update is called once per frame
  void Update() {
    switch (state) {
      case GameState.Initial:
        var action = Input.GetButtonDown("Action");
        if (action) {
          OnClickStart();
        }
        break;
      case GameState.Preparation:
        InstantiateCards();
        ready = false;
        if (time >= preparationTime) {
          time = 0f;
          ChangePhase(GameState.Battle);
          updateState();
        }
        time += Time.deltaTime;
        break;
      case GameState.Battle:
        wasCardsGenerated = false;

        if (!ready && time >= readyTime) {
          ready = true;
          time = 0;
        }

        if (playerState == PlayerState.Attacking) {
          if (frames >= waitFrames) {
            guarding = false;
            frames = 0;
            playerState = PlayerState.Moving;
            updateState();
          }
          frames++;
        } else {
          if (time >= battleTime) {
            time = 0f;
            ChangePhase(GameState.Preparation);
            updateState();
          }
        }

        time += Time.deltaTime;
        break;
      case GameState.Victory:
        var input = Input.anyKey;
        if (input) {
          SceneManager.LoadScene("StartScene");
        }
        break;
      default:
        break;
    }

  }

  void InstantiateCards() {
    if (wasCardsGenerated) {
      return;
    }
    if (cards != null) {
      clearCards();
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

    if (newState == GameState.Battle) {
      camera.GetComponent<CameraController>().MoveTo(new Vector3(0, 4.3f, -10));
    }

    if (newState == GameState.Preparation) {
      ready = false;
      camera.GetComponent<CameraController>().MoveTo(new Vector3(0, 0f, -10));
    }
    Debug.Log("State" + newState);
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

  public void WaitFrames(int frames) {
    if (GameController.Instance.state == GameState.Battle) {
      this.waitFrames = frames;
      this.frames = 0;
    }
  }

  public void Guard(int framesToWait) {
    if (state == GameState.Battle) {
      guarding = true;
      Attack(framesToWait);
    }
  }

  public void Attack(int framesToWait) {
    if (playerState == PlayerState.Attacking) {
      return;
    }
    playerState = PlayerState.Attacking;
    waitFrames = framesToWait;
    frames = 0;
  }


  public void HitBounds(GameObject obj) {
    if (state == GameState.Battle) {
      won = obj.tag != "Player";
      victory.SetActive(true);
      ChangePhase(GameState.Victory);
      updateState();
    }
  }

  public void OnClickStart() {
    ChangePhase(GameState.Preparation);
    pamphlet.SetActive(false);
  }


  private void Awake() {
    if (_instance != null && _instance != this) {
      Destroy(this.gameObject);
    } else {
      _instance = this;
    }
  }
}
