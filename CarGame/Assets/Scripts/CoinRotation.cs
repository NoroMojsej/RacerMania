using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate the coin around its Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
