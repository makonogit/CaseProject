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

    [Header("タイトルロゴ")]
    [SerializeField] private GameObject m_titleLogo1;
    [SerializeField] private GameObject m_titleLogo2;

    [SerializeField, Header("スターパーティクル")]
    private GameObject m_starParticle;

    [SerializeField, Header("拡大率")]
    private float m_scaleFactor = 0.1f;
    [SerializeField, Header("拡大スピード")]
    private float m_scaleSpeed = 2;
    [SerializeField, Header("最大拡大")]
    private float m_maxScale = 10;

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
                        m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.MAGNIFICATION_SERIUS;
                    }
                    return;
                }
                break;
            case CS_TitleHandler.TITLE_STATE.MAGNIFICATION_SERIUS:
                if(transform.localScale.x < m_maxScale)
                {
                    transform.localScale += new Vector3(m_scaleFactor, m_scaleFactor, 0) * m_scaleSpeed * Time.deltaTime;
                    if(transform.localScale.x > m_maxScale)
                    {
                        Destroy(m_titleLogo1);
                        Destroy(m_starParticle);
                        m_titleLogo2.SetActive(true);
                        m_scaleSpeed *= 1.5f;
                        m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.REDUCTION_SERIUS;
                    }
                   
                }     
                break;
            case CS_TitleHandler.TITLE_STATE.REDUCTION_SERIUS:
                if (transform.localScale.x > 0.0f)
                {
                    transform.localScale -= new Vector3(m_scaleFactor, m_scaleFactor, 0) * m_scaleSpeed * Time.deltaTime;
                    if (transform.localScale.x < 0.0f)
                    {
                        Destroy(this.gameObject);
                        m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.WAIT2;
                    }
                }
                break;
        }
    }
}
