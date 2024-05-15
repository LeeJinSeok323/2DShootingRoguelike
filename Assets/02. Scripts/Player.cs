using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   
    public GameObject bulletPrefab;
    public Transform[] spPoints;

    void Start(){
        //gameObject.Find("").GetComponentsInChildren<Transform>();
        GameManager.instance.ExpUp(7.0f);
    }

    void Update()
    {   
        #region 플레이어이동
        if(Input.GetKey(KeyCode.A)){ //왼
            if(Get_Pos().x > -10.4f){
                Move_Pos(Vector3.left * GameManager.moveSpeed * Time.deltaTime);
            }
        }   
        if(Input.GetKey(KeyCode.D)){ //오
            if(Get_Pos().x < 10.4f){
                Move_Pos(Vector3.right * GameManager.moveSpeed * Time.deltaTime);
            }
        }   
        if(Input.GetKey(KeyCode.W)){ // 위
            if(Get_Pos().y < 5.7f){
                Move_Pos(Vector3.up * GameManager.moveSpeed * Time.deltaTime);
            }
        }   
        if(Input.GetKey(KeyCode.S)){ // 아래
            if(Get_Pos().y > -5.7f){
                Move_Pos(Vector3.down * GameManager.moveSpeed * Time.deltaTime);
            }
        }   
        #endregion
        
        #region 커서를 향해서 회전
        Vector3 mPosition = Input.mousePosition; // 마우스위치
        Vector3 oPosition = transform.position; // 플레이어 위치
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition); //마우스 3차원 변환.
        
        float dy = target.y - oPosition.y;
        float dx = target.x - oPosition.x;
        float rotateDegree = Mathf.Atan2(dy,dx) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f,0f, rotateDegree);
        #endregion

        #region 총알 발사
        GameManager.bulTime += Time.deltaTime;
        if (GameManager.bulTime > GameManager.bulletCallTime)
        {
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < GameManager.bulletCount; i++)
                {
                    if (i < spPoints.Length)
                    {
                        Instantiate(bulletPrefab, spPoints[i].position, spPoints[i].rotation);
                    }
                }
                GameManager.bulTime = 0.0f;
            }
        }
        #endregion
    }

    private void Move_Pos(Vector3 move) {this.transform.position += move;}
    private Vector3 Get_Pos(){return this.transform.position; }

    
    
    
}
