using UnityEngine;

public class SunRotator : MonoBehaviour
{
    [Tooltip("Time (in seconds) for a full day-night cycle.")]
    public float dayDuration = 45f;

    void Update()
    {
        // Rotate the sun smoothly over time
        float rotationSpeed = 360f / dayDuration; // degrees per second
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
