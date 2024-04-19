//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;

public class CS_Creater : MonoBehaviour 
{ 
   
    [Header("風の設定")]
    [SerializeField]private GameObject m_objWind;                   // 生成物
    [SerializeField] private float m_fCreateDelayOfWind = 0.5f;     // 生成ディレイ
    [SerializeField] private float m_fWindMoveSpeed = 0.125f;       // 動く速さの倍率
    [SerializeField] private float m_fWindPower = 1;                // 強さの倍率
    private float m_fCreatedWindTime = 0;                           // 生成経過時間

    [Header("雷の設定")]
    [SerializeField] private GameObject m_objThunder;                   // 生成物
    [SerializeField] private float m_fCreateDelayOfThunder = 0.125f;    // 生成ディレイ
    [SerializeField] private float m_fThunderMoveSpeed = 10;        // 動く速さの倍率
    [SerializeField] private float m_fThunderPower = 1;                 // 強さの倍率
    private float m_fCreatedThunderTime = 0;                            // 生成経過時間

    [Header("雨の設定")]
    [SerializeField] private GameObject m_objRain;              // 生成物
    [SerializeField] private int m_fRainCreatePerSecond= 10;    // 雨の毎秒生成数
    [SerializeField] private float m_fRainCreateRange = 25;      // 生成する半径
    private float m_fRequiredCreateNum = 0.0f;                  // 必要な生成数

    // Start is called before the first frame update
    private void Start()
    {
        // イベント設定
        CS_HandSigns.OnCreateWinds += CreateWind;
        CS_HandSigns.OnCreateThunders += CreateThunder;
        CS_HandSigns.OnCreateRains += CreateRain;
    }
    
    // Update is called once per frame
    private void Update()
    {
        m_fCreatedWindTime += Time.deltaTime;
        m_fCreatedThunderTime += Time.deltaTime;
    }

    // 風の生成する関数（購読 subscribe）
    // 引数：人差し指の付け根の位置
    // 引数：進行方向
    // 戻り値：なし
    //
    // ※引数、戻り値はイベントの発行側で決まる。
    //
    private void CreateWind(Vector3 position,Vector3 direction) 
    {
        // 時間が過ぎたら
        if (m_fCreatedWindTime >= m_fCreateDelayOfWind)
        {
            // 方向の設定
            float angle = Mathf.Atan2(direction.y, direction.x);
            Quaternion rotation = Quaternion.EulerAngles(0, 0, angle);
            // 風の生成
            GameObject obj = GameObject.Instantiate(m_objWind, position, rotation);
            CS_Wind cs_wind = obj.GetComponent<CS_Wind>();  //風のスクリプト取得

            cs_wind.Movement = direction.magnitude * m_fWindMoveSpeed;
            cs_wind.WindPower = direction.magnitude * m_fWindPower;
            
            // 時間のリセット
            m_fCreatedWindTime = 0;
        }
    }

    // 雷を生成する関数
    // 引数：生成位置
    // 引数：動いた距離
    // 戻り値：なし
    private void CreateThunder(Vector3 position,Vector3 direction) 
    {
        if (m_fCreatedThunderTime >= m_fCreateDelayOfThunder) 
        {
            // 方向の設定
            Quaternion rotation = Quaternion.EulerAngles(0, 0, -180*Mathf.Deg2Rad);
            // 雷の生成
            GameObject obj = GameObject.Instantiate(m_objThunder, position, rotation);
            CS_Thunder cs_thunder = obj.GetComponent<CS_Thunder>();  //雷のスクリプト取得

            cs_thunder.Movement = direction.magnitude * m_fThunderMoveSpeed;
            //cs_wind.WindPower = direction.magnitude * m_fWindPower;

            // 時間のリセット
            m_fCreateDelayOfThunder = 0;
        }
    }
    // 雨を生成する関数
    // 引数：生成位置
    // 引数：特に意味なし
    // 戻り値：なし
    private void CreateRain(Vector3 position, Vector3 direction)
    {
        // 生成する数を追加する
        m_fRequiredCreateNum += m_fRainCreatePerSecond * Time.deltaTime;
        for(int i = 0; i <= m_fRequiredCreateNum; i++) 
        {
            float x = Random.Range(0, 7);
            float y = Random.Range(0, 7);
            float radius = Random.Range(0, m_fRainCreateRange);
            
            Vector3 rad = new Vector3(Mathf.Cos(x), Mathf.Sin(y));
            rad.Normalize();

            Vector3 Pos = position + rad*radius;
            // 方向の設定
            Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
            // 雨の生成
            GameObject obj = GameObject.Instantiate(m_objRain, Pos, rotation);
            m_fRequiredCreateNum -= 1;
        }
    }
    
}