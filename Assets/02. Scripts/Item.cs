using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private float speed = 0.13f;
    public GameObject itemEffect;
    private Transform playerTr;

    void Start(){
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update(){
        transform.position = Vector3.Lerp(transform.position, playerTr.position, Time.deltaTime * speed);
        transform.Rotate(Vector3.forward *speed * 100.0f* Time.deltaTime);
    
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Player")){
            Instantiate(itemEffect, transform.position, Quaternion.identity);
            GetItem();
            Destroy(this.gameObject);
        }
    }
    
    void GetItem(){
        if (GameManager.p_curHp < 97.0f)
            GameManager.p_curHp += 3.0f;
        else if (GameManager.p_curHp < 100.0f)
            GameManager.p_curHp = 100.0f;

        GameManager.instance.hpBar.fillAmount = GameManager.p_curHp/GameManager.instance.p_maxHp;
    }

}
