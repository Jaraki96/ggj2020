using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyState : MonoBehaviour {
    public struct RepeatKey {
        public KeyCode keyCode;
        public int numPresses;
    }
    public HashSet<KeyCode> pressedKeys;
    public HashSet<RepeatKey> repeatKeys;
    public HashSet<List<KeyCode>> keySequences;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
