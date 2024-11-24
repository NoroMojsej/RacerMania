using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int scoreValue = 10; // Points for collecting this coin
    private AudioSource coinSound; // Reference to the AudioSource component

    void Start()
    {
        coinSound = GetComponent<AudioSource>(); // Get the AudioSource on the coin
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player collecting the coin
        {
            // Play the coin sound
            if (coinSound != null)
            {
                coinSound.Play();
            }

            // Add score to the GameManager
            GameManager instance = FindObjectOfType<GameManager>();
            if (instance != null)
            {
                instance.AddScore(scoreValue);
            }

            // Destroy the coin after the sound plays
            Destroy(gameObject, coinSound.clip.length);
        }
    }
}
