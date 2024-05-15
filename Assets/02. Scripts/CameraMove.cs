using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject playerTr;

    void LateUpdate() {
        Camera.main.transform.position = playerTr.transform.position;
    } 
}
