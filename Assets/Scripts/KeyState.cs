using System.Collections.Generic;
using UnityEngine;

public class KeyState : MonoBehaviour {
    private const int TIMEOUT_DAMAGE = 5;
    [System.Serializable]
    public class KeyEvent {
        public KeyCode keyCode;
        public float lifetime;
        public float timeout;
    }
    [System.Serializable]
    public class KeySequence {
        public List<KeyEvent> keyEvents;
        public int currentIndex;
    }
    private List<KeyEvent> pressedKeys;
    private List<KeyEvent> repeatKeys;
    private List<KeySequence> keySequences;
    public string repeatKeyString;
    public string pressedKeyString;
    // Start is called before the first frame update
    void Start() {
        pressedKeys = new List<KeyEvent>();
        repeatKeys = new List<KeyEvent>();
        keySequences = new List<KeySequence>();
    }

    // Update is called once per frame
    void Update() {
        float time = Time.deltaTime;
        repeatKeyString = "";
        pressedKeyString = "";
        for(int i = 0; i < pressedKeys.Count; ++i) {
            KeyEvent keyEvent = pressedKeys[i];
            pressedKeyString = keyEvent.keyCode + ", ";
            if (Input.GetKey(keyEvent.keyCode)) {
                keyEvent.lifetime -= time;
            } else {
                keyEvent.timeout -= time;
                GameManager.instance.boat.health -= time;
                if(keyEvent.timeout <= 0) {
                    GameManager.instance.boat.health -= TIMEOUT_DAMAGE;
                }
            }
            if (keyEvent.lifetime <= 0 || keyEvent.timeout <= 0) {
                // remove from KeyManager
                KeyManager.instance.RemoveKey(keyEvent.keyCode);
            }
        }
        for (int i = 0; i < repeatKeys.Count; ++i) {
            KeyEvent keyEvent = repeatKeys[i];
            repeatKeyString += keyEvent.keyCode + ", ";
            if (Input.GetKeyDown(keyEvent.keyCode)) {
                keyEvent.lifetime -= 1;
                Debug.Log(keyEvent.lifetime);
            } else {
                keyEvent.timeout -= time;
                GameManager.instance.boat.health -= time;
                if (keyEvent.timeout <= 0) {
                    GameManager.instance.boat.health -= TIMEOUT_DAMAGE;
                }
            }
            if(keyEvent.lifetime <= 0 || keyEvent.timeout <= 0) {
                // remove from KeyManager
                KeyManager.instance.RemoveKey(keyEvent.keyCode);
            }
        }
        for (int i = 0; i < keySequences.Count; ++i) {
            KeySequence keySequence = keySequences[i];
            KeyEvent keyEvent = keySequence.keyEvents[keySequence.currentIndex];
            if (Input.GetKeyDown(keyEvent.keyCode)) {
                keySequence.currentIndex++;
            } else {
                keyEvent.timeout -= time;
                GameManager.instance.boat.health -= time;
                if (keyEvent.timeout <= 0) {
                    GameManager.instance.boat.health -= TIMEOUT_DAMAGE;
                }
            }
            if (keySequence.currentIndex == keySequence.keyEvents.Count || keyEvent.timeout <= 0) {
                // remove from KeyManager
                KeyManager.instance.RemoveKey(keyEvent.keyCode);
            }
        }
    }

    private void LateUpdate() {
        // remove all dead key events
        pressedKeys.RemoveAll(keyEvent => keyEvent.lifetime <= 0 || keyEvent.timeout <= 0);
        repeatKeys.RemoveAll(keyEvent => keyEvent.lifetime <= 0 || keyEvent.timeout <= 0);
        keySequences.RemoveAll(keySequence => keySequence.currentIndex == keySequence.keyEvents.Count || keySequence.keyEvents[keySequence.currentIndex].timeout <= 0);
    }

    public void AddPressedKeyEvent(KeyCode key, float duration, float timeoutDuration) {
        KeyEvent keyEvent = new KeyEvent {
            keyCode = key,
            lifetime = duration,
            timeout = timeoutDuration
        };
        pressedKeys.Add(keyEvent);
    }

    public void AddRepeatKeyEvent(KeyCode key, int numPresses, float timeoutDuration) {
        KeyEvent keyEvent = new KeyEvent {
            keyCode = key,
            lifetime = numPresses,
            timeout = timeoutDuration
        };
        repeatKeys.Add(keyEvent);
    }

    public void AddKeySequence(List<KeyCode> keys, float timeoutDuration) {
        KeySequence keySequence = new KeySequence {
            keyEvents = new List<KeyEvent>(),
            currentIndex = 0,
        };
        foreach(KeyCode keyCode in keys) {
            KeyEvent keyEvent = new KeyEvent {
                keyCode = keyCode,
                lifetime = timeoutDuration,
                timeout = timeoutDuration
            };
            keySequence.keyEvents.Add(keyEvent);
        }
        keySequences.Add(keySequence);
    }
}
