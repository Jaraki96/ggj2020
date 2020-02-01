using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {
    private const int rows = 4;
    private const int columns = 10;
    public static KeyManager instance;
    private KeyCode[] keyCodes = {
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
    public struct Key {
        public KeyCode keyCode;
        public int row;
        public int column;
    }

    public HashSet<Key> keys = new HashSet<Key>();
    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < rows * columns; ++i) {
            Key key = new Key();
            key.row = i / columns;
            key.column = i % columns;
            key.keyCode = keyCodes[i];
            keys.Add(key);
        }
        instance = this;
    }

    // Update is called once per frame
    void Update() {

    }
}
