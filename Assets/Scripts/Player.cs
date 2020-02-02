using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField]
    public KeyState keyState;
    public Text[] pressedKeysTexts;
    public Text[] repeatKeysTexts;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        int count = 0;
        // wipe all the texts
        foreach(Text text in pressedKeysTexts) {
            text.text = "";
        }
        foreach (Text text in repeatKeysTexts) {
            text.text = "";
        }
        foreach (char c in keyState.pressedKeyString) {
            if(count < keyState.pressedKeyString.Length) {
                pressedKeysTexts[count].text = c.ToString();
                count++;
            }
        }
        count = 0;
        foreach (char c in keyState.repeatKeyString) {
            if(count < keyState.repeatKeyString.Length) {
                repeatKeysTexts[count].text = c.ToString();
                count++;
            }
        }
    }
}
