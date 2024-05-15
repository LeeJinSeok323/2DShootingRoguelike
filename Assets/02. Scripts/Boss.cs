using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Boss : MonoBehaviour
{
    // public enum State{
    //     DASH,
    //     SUMMON,
    //     TRACE,
    //     DIE
    // }

    public float EnemyHP = 1000.0f;
    public static float enemySpeed = 0.2f;
    private Vector3 playerDir; 
    Transform playerTr;
    Player player;
    bool isAttacked = false;
    Rigidbody2D rb; 
    CameraShake CameraShake;
    private float speed = 2.5f;
    public GameObject deadEffect;

    SpriteRenderer sprite;

    CircleCollider2D Collider;

    void Start()
    {   
        Collider = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        CameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
          
    }

    void Update()
    {   
        playerDir = playerTr.position - transform.position;
        if(!isAttacked)
            rb.velocity = new Vector2(playerDir.x, playerDir.y).normalized * speed;
        else
            rb.velocity = Vector2.zero;
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
            CameraShake.VibrateForTime(4f);
            StartCoroutine(LoadEndSceneAfterDelay());
            isAttacked = true;
        }

        if(col.gameObject.tag == "Player"){
            //Debug.Log("플레이어와 충돌");
            float damage;
            damage = 40.0f;
            GameManager.instance.PlayerHpMinus(damage);
            CameraShake.VibrateForTime(0.2f);
            isAttacked = true;
            StartCoroutine("AttackedPlayer");
        }
    }
    IEnumerator AttackedPlayer()
    {
        yield return new WaitForSeconds(5);
        isAttacked = false;
    }

    IEnumerator LoadEndSceneAfterDelay()
    {
        isAttacked = true;
        Collider.enabled = false;
        yield return new WaitForSeconds(4);
        
        for(int i = 0; i< 10; i++){
            sprite.sprite = null;
            yield return new WaitForSeconds(0.2f);
            sprite.sprite = Resources.Load<Sprite>("Boss");
            yield return new WaitForSeconds(0.2f);
        }
        
        sprite.sprite = null;
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
        // "EndScene" 씬 로드
        SceneManager.LoadScene("EndScene");
    }


}


/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Transform playerTr;
    public Transform[] points;
    
    public float dashSpeed = 200f;
    public float dashDuration = 2f;

    public float summonDelay = 0.5f;

    private bool isDashing = false;
    private bool isSummoning = false;
    private int currentPointIndex = 0;

    private float patternCooltime = 1f;
    private float GameTime;
    
    public GameObject dashEffect;
    Vector3 pos;
    
    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if(!isDashing && !isSummoning)
            GameTime += Time.deltaTime;
        // idle (회전)
        transform.Rotate(Vector3.forward * 100.0f * Time.deltaTime);
        Debug.Log(GameTime);
        if (GameTime >= patternCooltime)
        {
            RandPattern();
            patternCooltime = 0;
        }

    }

    void RandPattern()
    {
        Debug.Log("RandPattern");
        int pattern = Random.Range(0, 2);
        //if (pattern == 0)
        {
            DashPattern();
        }
        // else
        // {
        //     SummonPattern();
        // }
    }

    IEnumerator DashPattern()
{
    Debug.Log("RandPattern");

    Vector3 targetPos = playerTr.position;
    yield return new WaitForSeconds(2.0f);

    StartCoroutine(Dash(targetPos));
}

IEnumerator Dash(Vector3 targetPos)
{
    Instantiate(dashEffect, transform.position, transform.rotation);
    float startTime = Time.time;
    float endTime = startTime + dashDuration;

    while (Time.time < endTime)
    {
        float t = (Time.time - startTime) / dashDuration;
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
        yield return null;
    }

    transform.position = targetPos;
}

    void SummonPattern()
    {
        currentPointIndex = 0;
        MoveToNextPoint();
        isSummoning = true;
        
    }

    void MoveToNextPoint()
    {
        if (currentPointIndex < points.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, dashSpeed * Time.deltaTime);
            if (transform.position == points[currentPointIndex].position)
            {
                //Instantiate(minionPrefab, transform.position, Quaternion.identity);
                currentPointIndex++;
            }
        }
    }

}
*/