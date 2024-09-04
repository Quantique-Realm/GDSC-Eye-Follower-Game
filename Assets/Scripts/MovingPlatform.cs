using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float platformSpeed = 5f;  // Speed of the platform moving towards the player
    private float platformLength;     // Length of the platform for recycling
    private Transform playerTransform;

    void Start()
    {
        // Get the length of the platform based on its z-scale or collider size
        platformLength = GetComponent<BoxCollider>().size.z * transform.localScale.z;

        // Find the player transform
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Move the platform towards the player (relative to the player running forward)
        transform.Translate(Vector3.back * platformSpeed * Time.deltaTime);

        // Check if the platform has moved past the player by more than its length
        if (transform.position.z < playerTransform.position.z - platformLength)
        {
            RepositionPlatform();
        }
    }

    private void RepositionPlatform()
    {
        // Reposition the platform tile to the front, based on its length
        Vector3 newPosition = transform.position;

        // Move the platform forward by a length equal to twice the platform length (assuming 2 platforms)
        newPosition.z += platformLength * 2;

        // Apply the new position
        transform.position = newPosition;
    }
}
