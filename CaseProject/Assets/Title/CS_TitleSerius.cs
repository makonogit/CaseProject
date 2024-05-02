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
    private Transform m_target1;
    [SerializeField, Header("スクロール終了後目標地点2")]
    private Transform m_target2;
    
    [SerializeField, Header("移動速度")]
    private float m_speed;
    [SerializeField, Header("回転速度")]
    private float m_rotateSpeed = 50.0f;

    [SerializeField, Header("待機時間")]
    private float m_waitTime = 2.0f;

    private float m_nowWaitTime = 0.0f;

    // Start is called before the first frame update
    

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
                Vector3 targetToThis = m_target1.position - this.transform.position;
                //ベクトルyがマイナス？
                if(targetToThis.y < 0.0f)
                {
                    m_speed = 0.0f;//スピードを無くす
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.WAIT1;//待機状態1へ
                }
                break;
            case CS_TitleHandler.TITLE_STATE.WAIT1:
                m_nowWaitTime += Time.deltaTime;//現在の待機時間を加算

                //待機時間一定以上経った？
                if(m_nowWaitTime >= m_waitTime)
                {
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.SCROLL;//スクロール状態へ
                    m_nowWaitTime = 0.0f;
                    m_waitTime = 1.0f;
                }
                break;
            case CS_TitleHandler.TITLE_STATE.STOP:
                //現在の待機時間が待機時間より下？
                if (m_nowWaitTime <= m_waitTime)
                {
                    m_nowWaitTime += Time.deltaTime;//デルタタイムを加算
                    if (m_nowWaitTime >= m_waitTime)
                    { 
                        m_speed = 5.0f;
                    }
                    return;
                }
                //目標値のベクトルを取って移動
                Vector3 targetVec = (m_target2.position - transform.position);
                targetVec.z = 0.0f;
                Debug.Log("ターゲット" + targetVec.normalized);
                Vector3 newPos = transform.position + targetVec.normalized * m_speed * Time.deltaTime;
                transform.position = newPos;

                //目標とのベクトルの長さが一定未満？
                if(targetVec.magnitude < 0.5f)
                {
                    m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.WAIT2;//待機状態2へ
                }

                break;
        }
    }
}
