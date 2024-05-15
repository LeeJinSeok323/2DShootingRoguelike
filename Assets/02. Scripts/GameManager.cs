using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum UPGRADE{ DAMAGEUP, SPEEDUP, EXPRATEUP, RANGEUP,
                  FIRERATEUP, BULLETCOUNTUP, FOLLOWERS }

public class GameManager : MonoBehaviour
{
    #region 리소스 관리 변수
        public Transform playerTr;
        AudioSource audioSource;
        private PlayerChild playerChild;
        public AudioClip audioLevelUp;
    #endregion

    #region UI 관리
        public static bool levelUpFlag = false;
        public GameObject levelUpPanel;
        public Text upgrade1;
        public Text upgrade2;
        public Text upgrade3;
        public Image upgrade1Img;
        public Image upgrade2Img;
        public Image upgrade3Img;

        // 점수 UI
        public Text ScoreText;
        private int ScorePoint = 0;

        //체력, 경험치 바
        public Image hpBar;
        public Image expBar;

        //보스전 시 경고 효과
        BlinkingEffect effect;
        public GameObject warningPanel;
    #endregion

    #region 플레이어 스텟 관련 변수  
    
        public static float bulTime = 0.0f;
        public static int damage = 1;              // 공격력
        public static float moveSpeed = 3.0f;         // 이동속도
        public static float bulletCallTime = 0.7f;    // 연사력
        [SerializeField]
        public static int bulletCount = 1;         // 발사되는 총알 수
        public static float expRate = 1.0f;           // 경험치 획득률

        public float p_maxHp = 100.0f; // 최대체력
        public static float p_curHp;          // 현재체력
        public float EXP = 0;
        public float levelUpExp = 7;
        private int level = 1;
    #endregion

    #region 스테이지 및 몬스터 스폰 관련 변수
        public Transform[] points;
        public GameObject enemy1;
        public GameObject enemy2;
        public GameObject enemy3;
        public GameObject boss;
        public GameObject item;

        private List<float> enemy1SpawnTimes = new List<float> { 1.5f, 1f, 1f, 100000f };
        private List<float> enemy2SpawnTimes = new List<float> { 0f, 10f, 5f, 100000f};
        private List<float> enemy3SpawnTimes = new List<float> { 0f, 0f, 10f, 100000f };

        private int currentStage = 1;
        private float stageChangeInterval = 30f; // 2 minutes
        private float stageChangeTimer = 0f;
        public static bool gameOver = false;
    #endregion 

    public static GameManager instance = null;

    void Awake(){
        instance = this;
        playerChild = GameObject.FindWithTag("PlayerChild").GetComponent<PlayerChild>();
        audioSource = GetComponent<AudioSource>();
        effect = GetComponent<BlinkingEffect>();
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
    } 

    void Start(){
        //StartCoroutine("StartBossStage");
        p_curHp = p_maxHp;
        StageChange(currentStage); // 첫 스테이지 시작
        expBar.fillAmount = EXP/levelUpExp;
    

    }

    void Update()
    {   
        #region 스테이지에 따른 몬스터 스폰단계
        stageChangeTimer += Time.deltaTime;

        if (stageChangeTimer >= stageChangeInterval && currentStage != 4)
        {   
            Debug.Log(currentStage);
            currentStage++;
            stageChangeTimer = 0f;
            StageChange(currentStage);
        }
        #endregion

    
    }

    public void ResetPlayerStats(){
        level = 1;
        levelUpExp = 7;
        EXP = 7;
        p_curHp = p_maxHp;
        expRate = 1.0f;
        bulletCount = 1;
        bulletCallTime = 0.7f;
        moveSpeed = 3.0f;
        damage = 1;
        bulTime =0.0f;
        Bullet.shotSpeed = 5.0f;
        Bullet.destroyTime = 1f;
        gameOver = false;
    }
    
    void StageChange(int currentStage){
        StopAllCoroutines();
        StartCoroutine(SpawnEnemy(item, 10.0f));
        switch (currentStage)
        {
            case 1:
                StartCoroutine(SpawnEnemy(enemy1, enemy1SpawnTimes[currentStage - 1]));
                break;
            case 2:
                StartCoroutine(SpawnEnemy(enemy1, enemy1SpawnTimes[currentStage - 1]));
                StartCoroutine(SpawnEnemy(enemy2, enemy2SpawnTimes[currentStage - 1]));
                break;
            case 3:
                StartCoroutine(SpawnEnemy(enemy1, enemy1SpawnTimes[currentStage - 1]));
                StartCoroutine(SpawnEnemy(enemy2, enemy2SpawnTimes[currentStage - 1]));
                StartCoroutine(SpawnEnemy(enemy3, enemy3SpawnTimes[currentStage - 1]));
                break;
            case 4:
            // Stage 4 보스 스테이지
                StartCoroutine("StartBossStage");
                break;
            default:
                break;
        }
    }
    IEnumerator StartBossStage(){
        Debug.Log("보스스테이지");

        // 경고
        warningPanel.SetActive(true);
        effect.StartBlinking();
        yield return new WaitForSeconds(3.0f);
        effect.StopBlinking();
        warningPanel.SetActive(false);
        // 보스 등장 연출
        Debug.Log("보스 소환");
        Instantiate(boss, points[0].position, Quaternion.identity);
            
        
    }

   

    void RemoveAllEnemies(string enemyTag)
    {   
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
    // 적을 스폰하는 함수
    IEnumerator SpawnEnemy(GameObject enemy, float spawnTime){
        while(true){
            //Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
            int idx = Random.Range(1, points.Length); 
            Instantiate(enemy, points[idx].position, Quaternion.identity);
            
            yield return new WaitForSeconds(spawnTime);
        }
    }

    // 적 처치 시 점수 증가
    public void ScoreUP(){
        if (ScorePoint == 20000){
            SceneManager.LoadScene("EndScene");
        }
        ScorePoint += 100;
        ScoreText.text = ScorePoint.ToString();
    }    

    // 플레이어 체력 감소 
    public void PlayerHpMinus(float Damaged){
        p_curHp -= Damaged;
        if(p_curHp < 0.0f)
        {
            gameOver = true;
            SceneManager.LoadScene("EndScene");
        }
        hpBar.fillAmount = p_curHp/p_maxHp;
    }

    #region 레벨업과 업그레이드 함수

    public void ExpUp(float incresedExp){

        EXP += incresedExp;
        while(true){
            if(EXP >= levelUpExp){
                LevelUp();
            }
            else{
                break;
            }
        }
        
        expBar.fillAmount = EXP/levelUpExp;

    }

    public void LevelUp(){
        levelUpFlag = true;
        Time.timeScale = 0f;
        levelUpPanel.SetActive(true);
        EXP -= levelUpExp;
        levelUpExp = levelUpExp * 1.1f;
        level++;
        audioSource.clip = audioLevelUp;
        audioSource.Play();


        // 랜덤하게 업그레이드 선택
        UPGRADE[] upgrades = new UPGRADE[3];

        for (int i = 0; i < 3; i++)
        {
            upgrades[i] = SelectRandomUpgrade();
        }

        // UI에 선택된 업그레이드 표시
        DisplayUpgrades(upgrades);
        levelUpFlag = false;
    }

        
    private UPGRADE SelectRandomUpgrade()
    {
        float randomValue = Random.value; // 0.0 ~ 1.0 사이의 랜덤한 값

        if (randomValue < rate[0] && bulletCount < 5){// BULLETCOUNTUP은 레어 등급 (20% 확률)
            float subRandomValue = Random.value;
            if (subRandomValue < 0.5f)
                return UPGRADE.BULLETCOUNTUP;       
            else
                return UPGRADE.FOLLOWERS;
        }
            
        else if (randomValue < rate[0] + rate[1]) // DAMAGEUP과 FIRERATEUP은 Uncommon (35% 확률)
        {
            float subRandomValue = Random.value;
            if (subRandomValue < 0.5f && bulletCallTime >= 0.3f)
                return UPGRADE.FIRERATEUP;
            else
                return UPGRADE.DAMAGEUP;
        }
        else // SHOTSPEEDUP, EXPRATEUP, SPEEDUP은 Normal (45% 확률)
        {
            float subRandomValue = Random.value;
            if (subRandomValue < 0.33f && moveSpeed < 15.0f)
                return UPGRADE.SPEEDUP;
            else if (subRandomValue < 0.67f && expRate < 3.0f)
                return UPGRADE.EXPRATEUP;
            else
                return UPGRADE.RANGEUP;
        }
        default{}
    }
    // 레벨업 업그레이드 UI
    private void DisplayUpgrades(UPGRADE[] upgrades)
    {   
        
        upgrade1.text = GetUpgradeText(upgrades[0]);
        upgrade2.text = GetUpgradeText(upgrades[1]);
        upgrade3.text = GetUpgradeText(upgrades[2]);
        
        upgrade1Img.sprite = Resources.Load<Sprite>(GetUpgradeImg(upgrades[0]));
        upgrade2Img.sprite = Resources.Load<Sprite>(GetUpgradeImg(upgrades[1]));
        upgrade3Img.sprite = Resources.Load<Sprite>(GetUpgradeImg(upgrades[2]));
    }

    //업그레이드 이미지 이름 불러오기
    string GetUpgradeImg(UPGRADE upgrade){
        switch (upgrade)
        {
            case UPGRADE.DAMAGEUP:
                return "DamageUp";
            case UPGRADE.SPEEDUP:
                return "SpeedUp";
            case UPGRADE.EXPRATEUP:
                return "ExpRateUp";
            case UPGRADE.RANGEUP:
                return "RangeUp";
            case UPGRADE.FIRERATEUP:
                return "FireRateUp";
            case UPGRADE.BULLETCOUNTUP:
                return "BulletCountUp";
            case UPGRADE.FOLLOWERS:
                return "AddChild";
            default:
                return "";
        }
    }

    // 업그레이드 문구 불러오기
    string GetUpgradeText(UPGRADE upgrade){
        switch (upgrade)
        {
            case UPGRADE.DAMAGEUP:
                return "데미지 증가";
            case UPGRADE.SPEEDUP:
                return "스피드 증가";
            case UPGRADE.EXPRATEUP:
                return "EXP획득량 증가";
            case UPGRADE.RANGEUP:
                return "사거리 증가";
            case UPGRADE.FIRERATEUP:
                return "연사력 증가";
            case UPGRADE.BULLETCOUNTUP:
                return "주무기+";
            case UPGRADE.FOLLOWERS:
                return "보조무기+";
            default:
                return "";
        }
    }

    // 실제 업그레이드 진행
    public void Upgrade(string upgrade){
        Debug.Log("upgrade 실행");
        switch (upgrade)
        {
            case "데미지 증가":
                damage += 1;
                break;
            case "스피드 증가":
                moveSpeed += 1;
                break;
            case "EXP획득량 증가":
                expRate += 0.2f;
                break;
            case "사거리 증가":
                Bullet.shotSpeed += 1f;
                Bullet.destroyTime += 0.3f;
                break;
            case "연사력 증가":
                if(bulletCallTime > 1.0f)
                    bulletCallTime -=0.2f;
                else
                    bulletCallTime -=0.1f;
                break;
                
            case "주무기+":
                bulletCount++;
                break;
            case "보조무기+":
                playerChild.CreateChild();
                break;
        }
    }
    #endregion


}
