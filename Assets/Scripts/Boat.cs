using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour {
    private const float TICKDOWN_SCALE = 4f;
    public Health health;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        health.TakeDamage(Time.deltaTime / TICKDOWN_SCALE);
    }
}
