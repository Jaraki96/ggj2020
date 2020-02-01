using System.Collections.Generic;
using UnityEngine;

public class KeyState : MonoBehaviour {
    private const int TIMEOUT_DAMAGE = 5;
    [SerializeField]
    public struct KeyEvent {
        public KeyCode keyCode;
        public float lifetime;
        public float timeout;
    }
    [SerializeField]
    public struct KeySequence {
        public List<KeyEvent> keyEvents;
        public int currentIndex;
    }
    [SerializeField]
    public List<KeyEvent> pressedKeys = new List<KeyEvent>();
    [SerializeField]
    public List<KeyEvent> repeatKeys = new List<KeyEvent>();
    [SerializeField]
    public List<KeySequence> keySequences = new List<KeySequence>();
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        for(int i = 0; i < pressedKeys.Count; ++i) {
            KeyEvent keyEvent = pressedKeys[i];
            if (Input.GetKey(keyEvent.keyCode)) {
                keyEvent.lifetime -= Time.deltaTime;
                Debug.Log(keyEvent.lifetime);
            } else {
                keyEvent.timeout -= Time.deltaTime;
                GameManager.instance.boat.health -= Time.deltaTime;
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
            if (Input.GetKeyDown(keyEvent.keyCode)) {
                keyEvent.lifetime -= 1;
            } else {
                keyEvent.timeout -= Time.deltaTime;
                GameManager.instance.boat.health -= Time.deltaTime;
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
                keyEvent.timeout -= Time.deltaTime;
                GameManager.instance.boat.health -= Time.deltaTime;
                if (keyEvent.timeout <= 0) {
                    GameManager.instance.boat.health -= TIMEOUT_DAMAGE;
                }
            }
            if (keySequence.currentIndex == keySequence.keyEvents.Count || keyEvent.timeout <= 0) {
                // remove from KeyManager
                KeyManager.instance.RemoveKey(keyEvent.keyCode);
            }
        }
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
