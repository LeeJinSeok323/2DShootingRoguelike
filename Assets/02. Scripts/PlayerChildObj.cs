using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildObj : MonoBehaviour
{   
    GameObject childSpPoint;
    public GameObject bulletPrefab;
    private float delayTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        childSpPoint = GameObject.FindWithTag("ChildSpPoint");
    }

    // Update is called once per frame
    void Update()
    {
        delayTime += Time.deltaTime;
        if(Input.GetMouseButton(0)){
            if(delayTime >1.0f){
                Instantiate(bulletPrefab, transform.position, childSpPoint.transform.rotation);
                delayTime = 0.0f;
            }
        }
    }
}
