using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float EnemyHP = 20.0f;
    public static float enemySpeed = 0.2f;
    private Vector3 playerDir; 
    Transform playerTr;
    Player player;
    public GameObject deadEffect;
    public GameObject expBall;
    Rigidbody2D rb; 
    CameraShake CameraShake;
    
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        CameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
          
    }

    void Update()
    {   
        playerDir = playerTr.position - transform.position;
        rb.velocity = new Vector2(playerDir.x, playerDir.y).normalized;
        //transform.Translate(playerDir * enemySpeed * Time.deltaTime);

        //transform.position = Vector3.Lerp(transform.position, playerTr.position, Time.deltaTime * enemySpeed);
        transform.Rotate(Vector3.forward * enemySpeed * 200.0f * Time.deltaTime);


    }
    
    public void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Bullet"){
            //Debug.Log("EnemyHP: "+ EnemyHP);
            EnemyHP -= 1.0f * GameManager.damage;
        }

        if(EnemyHP <= 0.0f){
            //Debug.Log("Enemy 사망");
            Instantiate(deadEffect, transform.position, Quaternion.identity);
            Instantiate(expBall, transform.position, Quaternion.identity);
            CameraShake.VibrateForTime(0.05f);
            GameManager.instance.ScoreUP();
            Destroy(this.gameObject);
        }

        if(col.gameObject.tag == "Player"){
            //Debug.Log("플레이어와 충돌");
            float damage;
            if(this.gameObject.tag == "Enemy1")
                damage = 10.0f;
            else if(this.gameObject.tag == "Enemy2")
                damage = 20.0f;  
            else if(this.gameObject.tag == "Enemy3")
                damage = 40.0f;
            else{ damage = 0.0f;}
            GameManager.instance.PlayerHpMinus(damage);
            Instantiate(deadEffect, transform.position, Quaternion.identity);
            CameraShake.VibrateForTime(0.05f);
            Destroy(this.gameObject);
        }
    }
}
