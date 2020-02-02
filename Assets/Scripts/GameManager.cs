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
    public float timeLimit = 30;
    public float timer;
    
    // Start is called before the first frame update
    void Start() {
        instance = this;
        gameOverBackground.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Semicolon)) {
            Debug.Log("Success");
        }
        AudioManager.instance.PlaySound("Waves");
        AudioManager.instance.PlaySound("Background");
        timer += Time.deltaTime;
        State state = GetState();
        if(state != State.IN_PROGRESS && state != State.PAUSED) {
            gameOverBackground.enabled = true;
            // game is over
            Time.timeScale = 0;
            if(state == State.WIN) {
                // win
                gameOverText.text = "Land Ho!";
                AudioManager.instance.PlaySound("Win");
            } else {
                // loss
                gameOverText.text = "Get Wrecked!";
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
