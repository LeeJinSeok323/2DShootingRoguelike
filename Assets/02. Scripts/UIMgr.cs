using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIMgr : MonoBehaviour
{       
    public GameObject levelUpPanel;
    public GameObject titlePanel;
    public GameObject creditPanel;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

    void Start(){
        Scene scene = SceneManager.GetActiveScene();
        
        // 둘다 액티브 상태에서
        if (scene.name == "EndScene"){
            if(GameManager.gameOver == true){
                gameClearPanel.SetActive(false);
            }
            else{
                gameOverPanel.SetActive(false);
            }
        }
    }
    
    #region 타이틀 화면, 크레딧 화면 버튼
    
    // 타이틀 화면
    public void OnClickStartBtn(Button button){
        GameManager.instance.ResetPlayerStats();
        SceneManager.LoadScene("PlayScene");
    }
    public void OnClickCreditBtn(Button button){
        titlePanel.SetActive(false);
        creditPanel.SetActive(true);
    }
    public void OnClickExitBtn(Button button){

    }

    // 크레딧에서 뒤로가기 버튼을 눌렀을 때
    public void OnClickBackBtn(Button button){
        titlePanel.SetActive(true);
        creditPanel.SetActive(false);
    }


    #endregion

    #region 게임 메인 업그레이드 버튼

    public void OnClickUpgrade1(Button button){
        Debug.Log("OnClickUpgrade1");
        string buttonText = button.GetComponentInChildren<Text>().text;
        GameManager.instance.Upgrade(buttonText);
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void OnClickUpgrade2(Button button){
        string buttonText = button.GetComponentInChildren<Text>().text;
        GameManager.instance.Upgrade(buttonText);
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void OnClickUpgrade3(Button button){
        string buttonText = button.GetComponentInChildren<Text>().text;
        GameManager.instance.Upgrade(buttonText);
        levelUpPanel.SetActive(false);
        Time.timeScale = 1;
    }
    #endregion

    #region 엔딩 씬
    public void OnClickRestartBtn(Button button){
        GameManager.instance.ResetPlayerStats();
        SceneManager.LoadScene("PlayScene");
    }

    public void OnClickEndcreditBtn(Button button){
        gameClearPanel.SetActive(false);
        creditPanel.SetActive(true);
    }
    
    public void OnclickTitleBtn(Button button){
        SceneManager.LoadScene("IntroScene");
    }
    #endregion

}
