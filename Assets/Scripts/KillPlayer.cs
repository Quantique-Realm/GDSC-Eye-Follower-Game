using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger game over
            GameManager.Instance.GameOver();
        }
    }
}
