//-----------------------------------------------
//担当者：中島愛音
//シリウスの処理
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleSerius : MonoBehaviour
{
    [SerializeField, Header("タイトル管理スクリプト")]
    private CS_TitleHandler m_titleHandler;
    [SerializeField, Header("目標地点")]
    private GameObject m_target;
    [SerializeField, Header("移動速度")]
    private float m_speed;
    [SerializeField, Header("回転速度")]
    private float m_rotateSpeed = 50.0f;

    [SerializeField, Header("待機時間")]
    private float m_waitTime = 2.0f;

    private float m_nowWaitTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //回転させる
        this.transform.Rotate(Vector3.forward * m_rotateSpeed * Time.deltaTime);
        switch (m_titleHandler.TitleState)
        {
            case CS_TitleHandler.TITLE_STATE.BORN_SERIUS:
                //y座標を更新
                Vector3 pos = this.transform.position;
                pos.y += m_speed * Time.deltaTime;
                this.transform.position = pos;
                //ターゲットまでのベクトル
                Vector3 targetToThis = m_target.transform.position - this.transform.position;
                //ベクトルyがマイナス？
                if(targetToThis.y < 0.0f)
                {
                    m_speed = 0.0f;//スピードを無くす
                    Destroy(m_target);//ターゲットを消す
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.WAIT1;//待機状態1へ
                }
                break;
            case CS_TitleHandler.TITLE_STATE.WAIT1:
                m_nowWaitTime += Time.deltaTime;//現在の待機時間を加算

                //待機時間一定以上経った？
                if(m_nowWaitTime >= m_waitTime)
                {
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.SCROLL;//スクロール状態へ
                }
                break;
        }
    }
}
