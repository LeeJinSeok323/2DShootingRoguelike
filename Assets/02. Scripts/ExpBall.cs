using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBall : MonoBehaviour
{
    bool isTriggered = false; // 중복 실행 방지를 위한 변수
    float exp;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")){
            if (!isTriggered) // 중복 실행이 아닌 경우에만 처리
            {
                isTriggered = true; // 중복 실행 방지 변수 업데이트

                if (this.CompareTag("SmallExpBall"))
                {
                    exp = 2.0f * GameManager.expRate;
                }
                else if (this.CompareTag("LargeExpBall"))
                {
                    exp = 10.0f* GameManager.expRate; 
                }
                else if (this.CompareTag("XLargeExpBall")){
                    exp = 15.0f* GameManager.expRate;
                }
                GameManager.instance.ExpUp(exp);
                // 오브젝트를 파괴합니다.
                Destroy(gameObject);
            }
        }
        
    }
}