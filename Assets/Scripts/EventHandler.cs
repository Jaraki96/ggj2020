using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour {
    private const int EVENT_TIMING = 5;
    public float timeSinceLastEvent = 0;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(timeSinceLastEvent >= EVENT_TIMING) {
            // spawn key event
            GameManager.instance.player1.keyState.AddInterestingKeyEvent();
            GameManager.instance.player2.keyState.AddInterestingKeyEvent();
            timeSinceLastEvent = 0;
        }
        timeSinceLastEvent += Time.deltaTime;
    }
}
