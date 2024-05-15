using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float YscrollSpeed = 1.0f;
    public float XscrollSpeed = 2.0f;
    Material back;
    void Start()
    {
        back = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        float newOffsetY = back.mainTextureOffset.y + YscrollSpeed * Time.deltaTime;
        float newOffsetX = back.mainTextureOffset.x + XscrollSpeed * Time.deltaTime;
        Vector2 newOffset = new Vector2(newOffsetX, newOffsetY);
        back.mainTextureOffset = newOffset;
    }
}
