using UnityEngine;

public class EarthquakeShake : MonoBehaviour
{
    [Header("Shake Settings (VR Safe)")]
    [Range(0.001f, 0.2f)]
    public float amplitude = 0.05f; // force max du shake

    [Range(0.1f, 10f)]
    public float frequency = 4f; // fréquence du tremblement

    public float duration = 2f;

    [Header("Vertical Motion?")]
    [Tooltip("Active un léger mouvement vertical (attention VR)")]
    public bool allowVerticalMovement = false;

    private Vector3 originalPos;
    private float timer;
    private bool shaking;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (!shaking) return;

        timer += Time.deltaTime;
        float t = timer / duration;

        // Perlin noise pour une vibration organique
        float shakeX = (Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f) * amplitude;
        float shakeZ = (Mathf.PerlinNoise(0f, Time.time * frequency) - 0.5f) * amplitude;
        float shakeY = allowVerticalMovement ? (Mathf.PerlinNoise(Time.time * frequency, Time.time) - 0.5f) * amplitude * 0.5f : 0f;

        transform.localPosition = originalPos + new Vector3(shakeX, shakeY, shakeZ);

        // Stop
        if (timer >= duration)
            StopShake();
    }

    public void TriggerShake()
    {
        timer = 0f;
        shaking = true;
    }

    public void StopShake()
    {
        shaking = false;
        transform.localPosition = originalPos;
    }

    // Timeline functions
    public void StartShakeFromTimeline() => TriggerShake();
    public void StopShakeFromTimeline() => StopShake();
}
