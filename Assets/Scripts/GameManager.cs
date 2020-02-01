using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
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

    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        if(getWinner() != null) {
            // game is over
        }
    }

    private Player getWinner() {
        if(boat.health <= 0) {
            return player2;
        } else if (timer >= timeLimit){
            return player1;
        }
        return null;
    }
}
