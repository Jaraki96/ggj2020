using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {
    private const int ROWS = 4;
    private const int COLUMNS = 10;
    public static KeyManager instance;
    public struct KeyLocation {
        public KeyCode keyCode;
        public Location location;
    }
    public enum Quadrant {
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT
    }
    [SerializeField]
    public KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Alpha0,
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
        KeyCode.T,
        KeyCode.Y,
        KeyCode.U,
        KeyCode.I,
        KeyCode.O,
        KeyCode.P,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.Semicolon,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C,
        KeyCode.V,
        KeyCode.B,
        KeyCode.N,
        KeyCode.M,
        KeyCode.Comma,
        KeyCode.Period,
        KeyCode.Question,
    };
    [SerializeField]
    public struct Location {
        public int row;
        public int column;

        public override string ToString() {
            return row + ", " + column;
        }
    }
    public Dictionary<Location, KeyCode> keys = new Dictionary<Location, KeyCode>();
    private HashSet<KeyCode> selectedKeys = new HashSet<KeyCode>();
    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < ROWS * COLUMNS; ++i) {
            Location location = new Location {
                row = i / COLUMNS,
                column = i % COLUMNS,
            };
            keys.Add(location, keyCodes[i]);
        }
        instance = this;
    }

    public KeyLocation GetRandomKeyByRow(int selectedRow) {
        Location location = new Location {
            row = -1,
            column = -1
        };
        int count = 0;
        while (!keys.ContainsKey(location) || selectedKeys.Contains(keys[location]) && count <= COLUMNS) {
            location = new Location {
                row = selectedRow,
                column = Random.Range(0, COLUMNS)
            };
            count++;
        }

        return SelectKey(location);
    }

    public KeyLocation GetRandomKeyByColumn(int selectedColumn) {
        Location location = new Location {
            row = -1,
            column = -1
        };
        int count = 0;
        while (!keys.ContainsKey(location) || selectedKeys.Contains(keys[location]) && count <= ROWS) {
            location = new Location {
                row = Random.Range(0, ROWS),
                column = selectedColumn
            };
            count++;
        }
        return SelectKey(location);
    }

    public KeyLocation GetRandomKeyByQuadrant(Quadrant quadrant) {
        Location location = new Location {
            row = -1,
            column = -1
        };
        int count = 0;
        while (!keys.ContainsKey(location) || selectedKeys.Contains(keys[location]) && count <= ROWS / 2 * COLUMNS / 2) {
            switch (quadrant) {
                case Quadrant.TOP_LEFT:
                    location = new Location {
                        row = Random.Range(0, ROWS / 2),
                        column = Random.Range(0, COLUMNS / 2)
                    };
                    break;
                case Quadrant.TOP_RIGHT:
                    location = new Location {
                        row = Random.Range(0, ROWS / 2),
                        column = Random.Range(COLUMNS / 2, COLUMNS)
                    };
                    break;
                case Quadrant.BOTTOM_LEFT:
                    location = new Location {
                        row = Random.Range(ROWS / 2, ROWS),
                        column = Random.Range(0, COLUMNS / 2)
                    };
                    break;
                case Quadrant.BOTTOM_RIGHT:
                    location = new Location {
                        row = Random.Range(ROWS / 2, ROWS),
                        column = Random.Range(COLUMNS / 2, COLUMNS)
                    };
                    break;
            }
            count++;
        }
        return SelectKey(location);
    }

    public KeyLocation GetRandomKey() {
        Location location = new Location {
            row = -1,
            column = -1
        };
        int count = 0;
        while (!keys.ContainsKey(location) || selectedKeys.Contains(keys[location]) && count <= COLUMNS * ROWS) {
            location = new Location {
                row = Random.Range(0, ROWS),
                column = Random.Range(0, COLUMNS)
            };
            count++;
        }
        return SelectKey(location);
    }

    public Quadrant GetQuadrant(Location location) {
        if(location.row < ROWS / 2) {
            if(location.column < COLUMNS / 2) {
                return Quadrant.TOP_LEFT;
            } else {
                return Quadrant.TOP_RIGHT;
            }
        } else {
            if (location.column < COLUMNS / 2) {
                return Quadrant.BOTTOM_LEFT;
            } else {
                return Quadrant.BOTTOM_RIGHT;
            }
        }
    }

    public KeyLocation SelectKey(Location selectedLocation) {
        KeyCode key = keys[selectedLocation];
        selectedKeys.Add(key);
        return new KeyLocation {
            keyCode = key,
            location = selectedLocation
        };
    }

    public void RemoveKey(KeyCode keyCode) {
        selectedKeys.Remove(keyCode);
    }

    public static string KeyCodeToString(KeyCode keyCode) {
        switch (keyCode) {
            case KeyCode.Semicolon:
            case KeyCode.Colon:
                return ";";
            case KeyCode.Period:
            case KeyCode.Greater:
                return ">";
            case KeyCode.Slash:
            case KeyCode.Question:
                return "?";
            case KeyCode.Comma:
            case KeyCode.Less:
                return "<";
        }
        return keyCode.ToString().Replace("Alpha", "");
    }

    // Update is called once per frame
    void Update() {

    }
}
