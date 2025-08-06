using UnityEngine;

/// <summary>
/// Attach this script to all enemy GameObjects.
/// It can be expanded later with health, AI, etc.
/// </summary>
public class Enemy : MonoBehaviour
{
    // You can add enemy-specific variables here later, like:
    // public float health = 100f;

    void Start()
    {
        // Ensure the GameObject has the "Enemy" tag for detection.
        if (gameObject.tag != "Enemy")
        {
            Debug.LogWarning("This enemy is missing the 'Enemy' tag.", this);
        }
    }
}