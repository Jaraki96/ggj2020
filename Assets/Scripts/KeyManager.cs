using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {
    private const int ROWS = 4;
    private const int COLUMNS = 10;
    public static KeyManager instance;
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
        KeyCode.Colon,
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
    private Dictionary<Location, KeyCode> keys = new Dictionary<Location, KeyCode>();
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

    public KeyCode getRandomKeyByRow(int selectedRow) {
        Location location = new Location {
            row = selectedRow,
            column = Random.Range(0, COLUMNS)
        };
        return keys[location];
    }

    public KeyCode getRandomKeyByColumn(int selectedColumn) {
        Location location = new Location {
            row = Random.Range(0, ROWS),
            column = selectedColumn
        };
        return keys[location];
    }

    public KeyCode getRandomKeyByQuadrant(Quadrant quadrant) {
        Location location = new Location {
            row = Random.Range(0, ROWS),
            column = Random.Range(0, COLUMNS)
        };
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
        return keys[location];
    }

    public KeyCode getRandomKey() {
        Location location = new Location {
            row = Random.Range(0, ROWS),
            column = Random.Range(0, COLUMNS)
        };
        return keys[location];
    }

    public KeyCode selectKey(Location location) {
        KeyCode keyCode = keys[location];
        selectedKeys.Add(keyCode);
        return keyCode;
    }

    public void removeKey(KeyCode keyCode) {
        selectedKeys.Remove(keyCode);
    }

    // Update is called once per frame
    void Update() {

    }
}
