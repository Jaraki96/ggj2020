using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour {
    private const int EVENT_TIMING = 5;
    private const float HOLD_TIME = 10f;
    private const float TIMEOUT = 3f;
    public enum RandomKeyType {
        ROW,
        COLUMN,
        QUADRANT,
        RANDOM,
    }
    public float timeSinceLastEvent = 0;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(timeSinceLastEvent >= EVENT_TIMING) {
            // spawn key event
            RandomKeyType randomKeyType = (RandomKeyType)Random.Range(0, 4);
            KeyCode keyCode = KeyManager.instance.getRandomKey();
            switch (randomKeyType){
                case RandomKeyType.ROW:
                    Debug.Log("Getting Random Key By Row");
                    keyCode = KeyManager.instance.getRandomKeyByRow(0);
                    GameManager.instance.player1.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    keyCode = KeyManager.instance.getRandomKeyByRow(0);
                    GameManager.instance.player2.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    break;
                case RandomKeyType.COLUMN:
                    Debug.Log("Getting Random Key By Column");
                    keyCode = KeyManager.instance.getRandomKeyByColumn(0);
                    GameManager.instance.player1.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    keyCode = KeyManager.instance.getRandomKeyByColumn(0);
                    GameManager.instance.player2.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    break;
                case RandomKeyType.QUADRANT:
                    Debug.Log("Getting Random Key By Quadrant");
                    keyCode = KeyManager.instance.getRandomKeyByQuadrant(KeyManager.Quadrant.TOP_LEFT);
                    GameManager.instance.player1.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    keyCode = KeyManager.instance.getRandomKeyByQuadrant(KeyManager.Quadrant.TOP_LEFT);
                    GameManager.instance.player2.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    break;
                case RandomKeyType.RANDOM:
                default:
                    Debug.Log("Getting Random Key");
                    keyCode = KeyManager.instance.getRandomKey();
                    GameManager.instance.player1.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    keyCode = KeyManager.instance.getRandomKey();
                    GameManager.instance.player2.keyState.AddPressedKeyEvent(keyCode, HOLD_TIME, TIMEOUT);
                    break;
            }
            Debug.Log(keyCode);
            timeSinceLastEvent = 0;
        }
        timeSinceLastEvent += Time.deltaTime;
    }
}
