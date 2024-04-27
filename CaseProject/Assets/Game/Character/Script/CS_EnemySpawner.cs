//-----------------------------------------------
//担当者：菅眞心
//敵生成クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_EnemySpawner : MonoBehaviour
{
    private float m_fTime = 0.0f;   //時間計測

    [SerializeField, Header("敵生成間隔")]
    private float m_fCreateTime = 2.0f;

    [SerializeField, Header("敵オブジェクト")]
    private GameObject EnemyObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_fTime += Time.deltaTime;  //時間計測
        int random = Random.Range(1, 100);

        //生成時間経過したらランダムな位置から生成
        if(m_fTime > m_fCreateTime)
        {
            GameObject enemy = EnemyObj;
            //とりあえず右と左から
            if(random < 50)
            {
                //左
                enemy.transform.position = new Vector3(-160, 0, 0);
                Instantiate(EnemyObj);
            }
            else
            {
                //右
                enemy.transform.position = new Vector3(160, 0, 0);
                Instantiate(EnemyObj);
            }

            m_fTime = 0.0f;
        }

    }
}
