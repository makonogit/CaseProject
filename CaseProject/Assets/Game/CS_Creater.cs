//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;
using static CS_HandPoseData;

public class CS_Creater : MonoBehaviour 
{
    [Header("風のオブジェクト")]
    [SerializeField]private GameObject m_objWind;

    

    [ Header("生成ディレイ")]
    [SerializeField] private float m_fCreateDelayOfWind = 0.5f;
    [Header("風の移動速度倍率")]
    [SerializeField] private float m_fWindMoveSpeed = 0.125f;
    [Header("風の力倍率")]
    [SerializeField] private float m_fWindPower = 1;
    // 風生成経過時間
    private float m_fCreatedWindTime = 0;
    // Start is called before the first frame update
    private void Start()
    {
        // イベント設定
        CS_HandSigns.OnCreatWinds += CreateWid;
    }
    
    // Update is called once per frame
    private void Update()
    {
        m_fCreatedWindTime += Time.deltaTime;
    }

    // 風の生成する関数（購読 subscribe）
    // 引数：人差し指の付け根の位置
    // 引数：進行方向
    // 戻り値：なし
    //
    // ※引数、戻り値はイベントの発行側で決まる。
    //
    private void CreateWid(Vector3 position,Vector3 direction) 
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
}