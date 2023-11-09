using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public static float currentHealth;
    private float updateVelocity;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Gradient healthBarColor;
    [SerializeField] private Image healthBar;
    [SerializeField] private float updateSpeed;

    private Slider health;

    private void Awake()
    {
        health = GetComponent<Slider>();
        canvas.worldCamera = Camera.main;
    }

    private void LateUpdate()
    {
        UpdateHealth();
    }

    public void SetMaxHealth(float maxHealth)
    {
        // set max health
        health.maxValue = maxHealth;

        // give full hp
        health.value = maxHealth;
        currentHealth = health.value;

        // adjust color gradient
        healthBar.color = healthBarColor.Evaluate(1);
    }

    public void UpdateHealth()
    {
        if (health.value == currentHealth) return;
        health.value = Mathf.SmoothDamp(health.value, currentHealth, ref updateVelocity, 0.01f, updateSpeed);
        healthBar.color = healthBarColor.Evaluate(health.normalizedValue);
    }
}
