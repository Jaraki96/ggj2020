using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void Process(KeyCode keyCode) {
        if (Input.GetKeyDown(keyCode)) {
            // do something
        } else {
            // do something else
        }
    }
}
