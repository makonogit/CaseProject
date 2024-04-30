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
    [SerializeField] private float m_fWindPower = 1;                // 強さの倍率
    


    // Start is called before the first frame update
    private void Start()
    {
        // イベント設定
        CS_HandSigns.OnCreateWinds += CreateWind;
        
    }
    
    // Update is called once per frame
    private void Update()
    {
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
        Debug.Log("Create Wind");
        
        Vector3 dir = direction;
        dir.y = 0;
        dir.Normalize();

        Vector3 pos =GetWindPosition(dir);
        // 方向の設定
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        // 風の生成
        GameObject obj = GameObject.Instantiate(m_objWind, pos, rotation);
        // サイズ変更
        Vector3 scale = obj.transform.localScale;
        scale.x = direction.magnitude * m_fWindPower * dir.x;
        obj.transform.localScale = scale;

        CS_Wind cs_wind = obj.GetComponent<CS_Wind>();  //風のスクリプト取得
        cs_wind.WindDirection = dir.x > 0 ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT;　//風の向き設定　追加：菅眞心
        cs_wind.WindPower = direction.magnitude * m_fWindPower;
        cs_wind.SetCameraPos = this.transform.position;
    }
    
    // 風の生成位置を求める関数
    // 引数：風の方向
    // 戻り値：風の生成位置
    private Vector3 GetWindPosition(Vector3 direction) 
    {
        Vector3 pos = this.transform.position;
        pos.z = 0;// 奥行は必要ない
        const float offsetX = 10;
        if (direction.x < 0) pos.x += offsetX;
        else pos.x -= offsetX;

        return pos;
    }
}