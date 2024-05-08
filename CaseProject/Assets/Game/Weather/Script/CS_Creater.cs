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
    [SerializeField] private float m_fWindPower = 1;       // 強さの倍率

    [SerializeField, Header("Playerscript")]
    private CS_Player m_player;                            // プレイヤーのscript 追加：菅

    // Start is called before the first frame update
    private void Start()
    {
        // イベント設定
        CS_HandSigns.OnCreateWinds += CreateWind;

        if (!m_player) { Debug.LogWarning("Playerのscriptが設定されていません"); }

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

        //Vector3 scale = obj.transform.localScale;
        //scale.x = direction.magnitude * m_fWindPower * dir.x;
        //obj.transform.localScale = scale;

        //風の力から風オブジェクトの数を決定
        int nWindObjNum = (int)((direction.magnitude * m_fWindPower * dir.x) / 10) - 1;
        nWindObjNum = Mathf.Abs(nWindObjNum);
        float fWindX = pos.x;

        //風オブジェクトを生成する
        while (nWindObjNum > 0)
        {
            //風のサイズ分ずらして配置
            fWindX += 4.0f * dir.x;
            Vector3 Pos = new Vector3(fWindX, pos.y, 0.0f);

            GameObject windobj = Instantiate(m_objWind, Pos, rotation);
            CS_Wind cswind = windobj.GetComponent<CS_Wind>();  //風のスクリプト取得
            cswind.WindDirection = dir.x > 0 ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT; //風の向き設定　追加：菅眞心
            cswind.WindPower = direction.magnitude * m_fWindPower * dir.x;
            cswind.SetCameraPos = this.transform.position;
            cswind.DeleteFlag = true;

            if (cswind.WindDirection == CS_Wind.E_WINDDIRECTION.LEFT)
            {
                windobj.transform.localScale = new Vector3(windobj.transform.localScale.x * -1, windobj.transform.localScale.y, 1.0f);
            }

            if (nWindObjNum == 1) 
            {
                cswind.IsWindEnd = true;
            }

            nWindObjNum--;

        }

        CS_Wind cs_wind = obj.GetComponent<CS_Wind>();  //風のスクリプト取得
        cs_wind.WindDirection = dir.x > 0 ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT;　//風の向き設定　追加：菅眞心
        cs_wind.WindPower = direction.magnitude * m_fWindPower * dir.x;
        cs_wind.SetCameraPos = this.transform.position;
        cs_wind.DeleteFlag = true;

        //プレイヤーの移動関数を直接呼び出し
        float windpower = direction.magnitude * m_fWindPower * dir.x;
        windpower = Mathf.Abs(windpower);
        m_player.WindMove(cs_wind.WindDirection, windpower);
    }
    
    // 風の生成位置を求める関数
    // 引数：風の方向
    // 戻り値：風の生成位置
    private Vector3 GetWindPosition(Vector3 direction) 
    {
        Vector3 pos = this.transform.position;
        pos.z = 0;// 奥行は必要ない
        const float offsetX = 8;
        if (direction.x < 0) pos.x += offsetX;
        else pos.x -= offsetX;

        pos.y -= 2.0f;

        return pos;
    }
}