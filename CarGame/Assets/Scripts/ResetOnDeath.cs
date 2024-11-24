using UnityEngine;

public class ResetOnDeath : MonoBehaviour
{
    // Reference to the player's start position
    
    private Vector3 initialPlayerPosition;
    public Transform currentWaypoint;

    private void Start()
    {
        // Save the player's initial position
        initialPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the DeathZone is the player
        if (other.CompareTag("Player"))
        {
            //Debug.Log("fghjfgjfjgj");
            initialPlayerPosition = other.GetComponent<InputManager>().previousWaypoint.transform.position;
            ResetPlayer(other.gameObject);
        }
    }

    private void ResetPlayer(GameObject player)
    {
        // Reset the player's position to the initial position
        player.transform.position = initialPlayerPosition;
        //player.transform.position = currentWaypoint.position;

        // Optionally, reset velocity if the player has a Rigidbody
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
