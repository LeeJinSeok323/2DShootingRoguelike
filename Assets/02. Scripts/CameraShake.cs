using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.5f;
    private float shakeTime;
    private Vector3 initalPosition;
    // Start is called before the first frame update
    void OnEnable()
    {
        initalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTime> 0)
        {
            if(shakeTime > 0){
                transform.position = Random.insideUnitSphere * shakeAmount + initalPosition;
                shakeTime -= Time.deltaTime;
            }
            else{
                shakeTime = 0.0f;
                transform.position = initalPosition;
            }
            
        }

    }

    public void VibrateForTime(float time){
        shakeTime = time;
    }
}
