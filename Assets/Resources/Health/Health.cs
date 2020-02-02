using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    public enum DisplayMode {
        NONE,
        FRACTION,
        PERCENTAGE,
        BOTH
    }
    public int maxHealth = 100;
    public bool destroyOnDeath;
    public bool render;
    public DisplayMode displayMode;
    public float width;
    public float height;
    public Vector2 offset;
    public Gradient healthGradient;
    public Color backgroundColor;
    public Font font;
    public int fontSize;
    public int precision;
    public Texture texture;
    float currentHealth;
    bool dead = false;
    Text healthBarText;
    RawImage healthBarImage;
    DisplayMode modeLastFrame;
    int precisionLastFrame;
    RectTransform healthBar;
    GameObject canvas;

    void Start() {
        currentHealth = maxHealth;
        if (render) {
            Canvas c = GetComponentInParent<Canvas>();
            if (!c) {
                canvas = UI.CreateCanvas(transform, offset, RenderMode.WorldSpace, width, height);
            } else {
                canvas = gameObject;
            }
            UI.CreatePanel("Health Bar Background", Vector2.zero, Vector2.one, canvas.transform, null, texture, backgroundColor);
            GameObject healthBarGO = UI.CreatePanel("Health Bar", Vector2.zero, Vector2.one, canvas.transform, null, texture, Color.white);
            healthBar = healthBarGO.GetComponent<RectTransform>();
            healthBarImage = healthBarGO.GetComponent<RawImage>();
            GameObject healthBarTextGO = UI.CreateText("Health Bar Text", "", Vector2.zero, Vector2.one, canvas.transform, font, Color.white, fontSize, TextAnchor.MiddleCenter, HorizontalWrapMode.Overflow, VerticalWrapMode.Overflow);
            RectTransform textRect = healthBarTextGO.GetComponent<RectTransform>();
            textRect.localScale = new Vector3(0.01f, 0.01f, 1);
            healthBarText = healthBarTextGO.GetComponent<Text>();
            UpdateText();
        }
    }
    public bool IsDead() {
        return dead;
    }
    public float GetCurrentHealth() {
        return currentHealth;
    }
    public void Reset() {
        currentHealth = maxHealth;
        dead = false;
        UpdateText();
    }
    public void Kill() {
        currentHealth = 0;
        dead = true;
        if (destroyOnDeath) {
            Destroy(gameObject);
        }
        UpdateText();
    }
    public void TestDamage() {
        TakeDamage(1);
    }
    public void TestHealing() {
        TakeDamage(-1);
    }
    public void TakeDamage(float amount) {
        // if amount is positive, it is damage
        // if amount is negative, it is healing
        if (dead) {
            return;
        }
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0) {
            currentHealth = 0;
            dead = true;
            if (destroyOnDeath) {
                Destroy(gameObject);
            }
        }
        UpdateText();
    }
    void FixedUpdate() {
        if(displayMode != modeLastFrame || precision != precisionLastFrame) {
            UpdateText();
        }
        modeLastFrame = displayMode;
        precisionLastFrame = precision;
        canvas.SetActive(render);
    }
    void UpdateText() {
        healthBar.anchorMax = new Vector2(currentHealth / maxHealth, healthBar.anchorMax.y);
        healthBarImage.color = healthGradient.Evaluate(currentHealth / maxHealth);
        string frac = currentHealth.ToString("F" + precision) + "/" + maxHealth;
        string perc = (currentHealth / maxHealth * 100).ToString("F" + precision) + "%";
        switch (displayMode) {
            case DisplayMode.NONE:
                healthBarText.text = "";
                break;
            case DisplayMode.FRACTION:
                healthBarText.text = frac;
                break;
            case DisplayMode.PERCENTAGE:
                healthBarText.text = perc;
                break;
            case DisplayMode.BOTH:
                healthBarText.text = frac + " " + perc;
                break;
        }
    }
}
