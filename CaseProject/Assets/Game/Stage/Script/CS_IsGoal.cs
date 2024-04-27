//-----------------------------------------------
//担当者：菅眞心
//浮島(ゴール)クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_IsGoal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //追記：中島2024.04.03
            //ゲームオーバーフラグをfalseに設定
            CS_ResultController.GameOverFlag = false;
            SceneManager.LoadScene("Result");
        }
    }

}
