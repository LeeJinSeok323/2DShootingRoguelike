using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static float shotSpeed = 5.0f;
    public static float destroyTime = 1.0f;

    void Start()
    {
        shotSpeed = 5.0f;
        Destroy(this.gameObject, destroyTime);    
    }

    void Update()
    {
        transform.Translate(Vector3.right * shotSpeed * Time.deltaTime);        
    }
}
