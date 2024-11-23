using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Controller car;
    public GameObject needle;
    private float startPosition = 227f, endPosition = -52.5f, desiredPosition;
    public float vehicleSpeed;
    
    

    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        vehicleSpeed = car.speed;
        updateNeedle();
    }

    public void updateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = vehicleSpeed/100;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width/5, Screen.height/6),"MONEY: "  +PlayerPrefs.GetFloat("Money"));
    }
}
