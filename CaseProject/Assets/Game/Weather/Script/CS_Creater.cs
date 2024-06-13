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
    [SerializeField] private GameObject m_objWind;         // 生成物
    [SerializeField] private float m_fWindPowerMagnification = 1;       // 強さの倍率

    [SerializeField, Header("Playerscript")]
    private CS_Player m_player;                            // プレイヤーのscript 追加：菅

    // Start is called before the first frame update
    private void Start()
    {
        // イベント設定
        CS_HandSigns.OnCreateWinds += CreateWinds;

        if (!m_player) { Debug.LogWarning("Playerのscriptが設定されていません"); }

    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void DeleteEvent()
    {
        //ゲームオーバー時に呼ぶ　追加：菅
        CS_HandSigns.OnCreateWinds -= CreateWinds; 
    }


    private void CreateWinds(Vector3 position, Vector3 direction) 
    {
        //風のSEを再生
        ObjectData.m_csSoundData.PlaySE("Wind");

        Vector3 pos = position;
        pos.z = 0;
        Vector3 dir = direction;
        dir.y = 0;
        dir.Normalize();
        bool IsLeftHand =dir.x > 0;
        // 方向の設定
        Quaternion rotation = Quaternion.identity;
        // 風の生成
        GameObject obj = GameObject.Instantiate(m_objWind, pos, rotation);
        if (IsLeftHand) obj.transform.localScale = InvertScaleX(obj);
        CS_Wind cswind = obj.GetComponent<CS_Wind>();  //風のスクリプト取得
        cswind.WindDirection = IsLeftHand ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT; //風の向き設定　追加：菅眞心
        cswind.WindPower = direction.magnitude * m_fWindPowerMagnification;
        cswind.SetCameraPos = this.transform.position;
        cswind.DeleteFlag = true;
        cswind.SetCS_Player(m_player);
    }
    // スケールXを反転する
    // 引き数：反転したいオブジェ
    // 戻り値：反転した値
    private Vector3 InvertScaleX(GameObject obj) 
    {
        Vector3 scale = obj.transform.localScale;
        scale.x *= -1;
        return scale;
    }

    private void OnDestroy()
    {
        // イベント設定
        CS_HandSigns.OnCreateWinds -= CreateWinds;
    }

    
}