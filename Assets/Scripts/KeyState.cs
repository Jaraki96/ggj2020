using System.Collections.Generic;
using UnityEngine;

public class KeyState : MonoBehaviour {
    private const int TIMEOUT_DAMAGE = 5;
    private const float HOLD_TIME = 10f;
    private const float TIMEOUT = 3f;
    private const int NUM_PRESSES = 5;
    private const float PRESSED_PROBABILITY = 0.75f;
    private const float HEALING_SCALE = 3f;
    [System.Serializable]
    public class KeyEvent {
        public KeyManager.KeyLocation keyLocation;
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
            pressedKeyString += KeyManager.KeyCodeToString(keyEvent.keyLocation.keyCode) + ", ";
            if (Input.GetKey(keyEvent.keyLocation.keyCode)) {
                keyEvent.lifetime -= time;
                GameManager.instance.boat.health.TakeDamage(-time / HEALING_SCALE);
                AudioManager.instance.PlaySound("Healing");
            } else {
                keyEvent.timeout -= time;
                GameManager.instance.boat.health.TakeDamage(time);
                if(keyEvent.timeout <= 0) {
                    GameManager.instance.boat.health.TakeDamage(TIMEOUT_DAMAGE);
                    AudioManager.instance.PlaySound("Damage1");
                }
            }
            if (keyEvent.lifetime <= 0 || keyEvent.timeout <= 0) {
                // remove from KeyManager
                KeyManager.instance.RemoveKey(keyEvent.keyLocation.keyCode);
            }
        }
        for (int i = 0; i < repeatKeys.Count; ++i) {
            KeyEvent keyEvent = repeatKeys[i];
            repeatKeyString += KeyManager.KeyCodeToString(keyEvent.keyLocation.keyCode) + ", ";
            if (Input.GetKeyDown(keyEvent.keyLocation.keyCode)) {
                keyEvent.lifetime -= 1;
                GameManager.instance.boat.health.TakeDamage(1f / HEALING_SCALE);
                AudioManager.instance.PlaySound("Nailing");
            } else {
                keyEvent.timeout -= time;
                GameManager.instance.boat.health.TakeDamage(time);
                if (keyEvent.timeout <= 0) {
                    GameManager.instance.boat.health.TakeDamage(TIMEOUT_DAMAGE);
                    AudioManager.instance.PlaySound("Damage1");
                }
            }
            if(keyEvent.lifetime <= 0 || keyEvent.timeout <= 0) {
                // remove from KeyManager
                KeyManager.instance.RemoveKey(keyEvent.keyLocation.keyCode);
            }
        }
        for (int i = 0; i < keySequences.Count; ++i) {
            KeySequence keySequence = keySequences[i];
            KeyEvent keyEvent = keySequence.keyEvents[keySequence.currentIndex];
            if (Input.GetKeyDown(keyEvent.keyLocation.keyCode)) {
                keySequence.currentIndex++;
            } else {
                keyEvent.timeout -= time;
                GameManager.instance.boat.health.TakeDamage(time);
                if (keyEvent.timeout <= 0) {
                    GameManager.instance.boat.health.TakeDamage(TIMEOUT_DAMAGE);
                    AudioManager.instance.PlaySound("Damage1");
                }
            }
            if (keySequence.currentIndex == keySequence.keyEvents.Count || keyEvent.timeout <= 0) {
                // remove from KeyManager
                KeyManager.instance.RemoveKey(keyEvent.keyLocation.keyCode);
            }

        }
        // remove all dead key events
        pressedKeys.RemoveAll(keyEvent => keyEvent.lifetime <= 0 || keyEvent.timeout <= 0);
        repeatKeys.RemoveAll(keyEvent => keyEvent.lifetime <= 0 || keyEvent.timeout <= 0);
        keySequences.RemoveAll(keySequence => keySequence.currentIndex == keySequence.keyEvents.Count || keySequence.keyEvents[keySequence.currentIndex].timeout <= 0);
    }

    public void AddInterestingKeyEvent() {
        List<KeyManager.Location> locations = new List<KeyManager.Location>();
        foreach(KeyEvent keyEvent in pressedKeys) {
            locations.Add(keyEvent.keyLocation.location);
        }
        foreach (KeyEvent keyEvent in repeatKeys) {
            locations.Add(keyEvent.keyLocation.location);
        }
        if(locations.Count == 0) {
            AddPressedKeyEvent(KeyManager.instance.GetRandomKey(), HOLD_TIME, TIMEOUT);
        } else {
            int topLeftCount = 0;
            int topRightCount = 0;
            int bottomLeftCount = 0;
            int bottomRightCount = 0;
            foreach(KeyManager.Location location in locations) {
                switch (KeyManager.instance.GetQuadrant(location)) {
                    case KeyManager.Quadrant.TOP_LEFT:
                        topLeftCount++;
                        break;
                    case KeyManager.Quadrant.TOP_RIGHT:
                        topRightCount++;
                        break;
                    case KeyManager.Quadrant.BOTTOM_LEFT:
                        bottomLeftCount++;
                        break;
                    case KeyManager.Quadrant.BOTTOM_RIGHT:
                        bottomRightCount++;
                        break;
                }
            }
            float probability = Random.Range(0f, 1f);
            KeyManager.Quadrant quadrant = KeyManager.Quadrant.BOTTOM_RIGHT;
            if(topLeftCount <= topRightCount && topLeftCount <= bottomLeftCount && topLeftCount <= bottomRightCount) {
                // spawn in top left
                quadrant = KeyManager.Quadrant.TOP_LEFT;
            } else if (topRightCount <= topLeftCount && topRightCount <= bottomLeftCount && topRightCount <= bottomRightCount) {
                // spawn in top right
                quadrant = KeyManager.Quadrant.TOP_RIGHT;
            } else if (bottomLeftCount <= topRightCount && bottomLeftCount <= topLeftCount && bottomLeftCount <= bottomRightCount) {
                // spawn in bottom left
                quadrant = KeyManager.Quadrant.BOTTOM_LEFT;
            } else {
                // spawn in bottom right
            }
            if (probability <= PRESSED_PROBABILITY) {
                AddPressedKeyEvent(KeyManager.instance.GetRandomKeyByQuadrant(quadrant), HOLD_TIME, TIMEOUT);
            } else {
                AddRepeatKeyEvent(KeyManager.instance.GetRandomKeyByQuadrant(quadrant), NUM_PRESSES, TIMEOUT);
            }
        }
    }

    public void AddPressedKeyEvent(KeyManager.KeyLocation key, float duration, float timeoutDuration) {
        KeyEvent keyEvent = new KeyEvent {
            keyLocation = key,
            lifetime = duration,
            timeout = timeoutDuration
        };
        pressedKeys.Add(keyEvent);
    }

    public void AddRepeatKeyEvent(KeyManager.KeyLocation key, int numPresses, float timeoutDuration) {
        KeyEvent keyEvent = new KeyEvent {
            keyLocation = key,
            lifetime = numPresses,
            timeout = timeoutDuration
        };
        repeatKeys.Add(keyEvent);
    }

    public void AddKeySequence(List<KeyManager.KeyLocation> keys, float timeoutDuration) {
        KeySequence keySequence = new KeySequence {
            keyEvents = new List<KeyEvent>(),
            currentIndex = 0,
        };
        foreach(KeyManager.KeyLocation key in keys) {
            KeyEvent keyEvent = new KeyEvent {
                keyLocation = key,
                lifetime = timeoutDuration,
                timeout = timeoutDuration
            };
            keySequence.keyEvents.Add(keyEvent);
        }
        keySequences.Add(keySequence);
    }
}
