using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float maxDistance = 20f; // Ensure this is > 0 in the Inspector!

    void Start()
    {
        // Calculate lifetime and destroy the arrow after that time.
        // This is a safe way to handle self-destruction.
        float lifetime = 2f; // Default lifetime of 2 seconds
        if (speed > 0)
        {
            lifetime = maxDistance / speed;
        }
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Only move if speed is greater than 0
        if (speed > 0)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions with the player
        if (other.CompareTag("Player"))
        {
            return;
        }

        // Stop the arrow's movement
        speed = 0;

        // Stick the arrow to the object it hit
        transform.SetParent(other.transform);

        // Disable components to prevent further interactions
        if(GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
        if(GetComponent<Rigidbody>() != null) Destroy(GetComponent<Rigidbody>());
        Destroy(this); // Destroy this script component
    }
}