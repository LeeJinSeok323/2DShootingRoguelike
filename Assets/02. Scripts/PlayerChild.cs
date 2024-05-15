using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChild : MonoBehaviour
{
    [SerializeField]
    private float speed = 60.0f; //회전속도
    [SerializeField]
    private int childCnt = 1; // 생성된 자식 수 
    [SerializeField]
    private GameObject child = null; //자식 프리펩
    [SerializeField]
    private float distance = 1.2f;

    int increaseCount = 0;

    Player player;
       


    void Start(){
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    void Update(){
        transform.Rotate(Vector3.forward, speed*Time.deltaTime);
    }
    public void CreateChild(){
        increaseCount++;
        if(increaseCount >3){
            Debug.Log("플레이어 HP 증가");
            increaseCount = 3;
            return;
        }
        if (increaseCount <= 3){
            GameObject[] playerChilds = GameObject.FindGameObjectsWithTag("PlayerChildObj");
            childCnt += playerChilds.Length;
            for(int i=0; i<playerChilds.Length; i++){
                Destroy(playerChilds[i]);
            }
        }
        for(int i=0;i <childCnt; ++i){
            GameObject go = Instantiate(child);

            float angle = 360.0f/ childCnt;
            float newY = Mathf.Sin(i* angle * Mathf.Deg2Rad);
            float newX = Mathf.Cos(i* angle * Mathf.Deg2Rad);
            
            newY = (newY * distance) + this.transform.position.y;
            newX = (newX * distance) + this.transform.position.x;

            go.transform.position = new Vector3(newX, newY, 0.0f);

            go.transform.parent = this.transform;
            
        }
    }
}