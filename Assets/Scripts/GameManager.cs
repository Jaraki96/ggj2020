using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public enum State {
        IN_PROGRESS,
        PAUSED,
        WIN,
        LOSS
    }
    [SerializeField]
    public Player player1;
    [SerializeField]
    public Player player2;
    [SerializeField]
    public Boat boat;
    public Text gameOverText;
    public Image gameOverBackground;
    public GameObject background;
    public Button playAgainButton;
    public Button mainMenuButton;
    public GameObject player1UI;
    public GameObject player2UI;
    public float timeLimit = 30;
    public float timer;
    public bool soundPlayed = false;
    
    // Start is called before the first frame update
    void Start() {
        instance = this;
        Time.timeScale = 1;
        gameOverBackground.enabled = false;
        playAgainButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        background.transform.position = new Vector3(37.5f - 75f * (timeLimit - timer) / timeLimit, 2, 2);
        AudioManager.instance.PlaySound("Waves");
        AudioManager.instance.PlaySound("Background");
        timer += Time.deltaTime;
        State state = GetState();
        if(state != State.IN_PROGRESS && state != State.PAUSED) {
            gameOverBackground.enabled = true;
            playAgainButton.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            player1UI.SetActive(false);
            player2UI.SetActive(false);
            // game is over
            Time.timeScale = 0;
            if(state == State.WIN) {
                // win
                gameOverText.text = "Land Ho!";
                if (!soundPlayed) {
                    AudioManager.instance.PlaySound("Win");
                    soundPlayed = true;
                }
            } else {
                // loss
                float probability = Random.Range(0f, 1f);
                gameOverText.text = "Get Wrecked!";
                if(probability > 0.5f) {
                    if (!soundPlayed) {
                        AudioManager.instance.PlaySound("Loss");
                        soundPlayed = true;
                    }
                } else {
                    if (!soundPlayed) {
                        AudioManager.instance.PlaySound("Loss2");
                        soundPlayed = true;
                    }
                }
            }

        }
    }

    public State GetState() {
        if(boat.health.GetCurrentHealth() <= 0) {
            return State.LOSS;
        } else if (timer >= timeLimit){
            return State.WIN;
        }
        return State.IN_PROGRESS;
    }
}
