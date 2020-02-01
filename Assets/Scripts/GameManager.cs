using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float timeLimit = 40;
    public float timer;
    
    // Start is called before the first frame update
    void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        State state = getState();
        if(state != State.IN_PROGRESS || state != State.PAUSED) {
            // game is over
            if(state == State.WIN) {
                // win
            } else {
                // loss
            }

        }
    }

    private State getState() {
        if(boat.health <= 0) {
            return State.LOSS;
        } else if (timer >= timeLimit){
            return State.WIN;
        }
        return State.IN_PROGRESS;
    }
}
