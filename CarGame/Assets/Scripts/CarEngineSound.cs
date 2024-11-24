using UnityEngine;

public class CarEngineSound : MonoBehaviour
{
    public AudioSource engineAudio; 
    public float maxSpeed = 200f;    
    public float idlePitch = 0.8f;   
    public float maxPitch = 2.5f;    
    public float currentSpeed;     
    public Controller car;
    void Update()
    {
        currentSpeed = car.speed;
        UpdateEngineSound();
    }

    void UpdateEngineSound()
    {
       
        float speedNormalized = Mathf.Clamp01(currentSpeed / maxSpeed);

        engineAudio.pitch = Mathf.Lerp(idlePitch, maxPitch, speedNormalized);

        engineAudio.volume = Mathf.Lerp(0.3f, 1f, speedNormalized);
    }
}
