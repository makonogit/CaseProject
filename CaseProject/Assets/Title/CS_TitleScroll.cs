//-----------------------------------------------
//担当者：中島愛音
//カメラのスクロール
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleScroll : MonoBehaviour
{
    private Camera m_camera;
    [SerializeField, Header("ゴールオブジェクト")]
    private GameObject m_goalBackGround;
    private Vector3 m_goalPos;
    [SerializeField, Header("スクロール速度")]
    private float m_scrollSpeed = 1.0f; //スクロール速度
    [SerializeField, Header("加速度")]
    private float m_acceleration = 0.1f; // 加速度
    [SerializeField, Header("減速を開始する距離の割合(0~1.0)")]
    public float m_decelerationRatio = 0.7f; // 減速を開始する距離の割合
    [SerializeField, Header("減速開始してから止まるまでの時間")]
    float m_decelerationTime = 2.0f;

    [SerializeField, Header("タイトル管理スクリプト")]
    private CS_TitleHandler m_titleHandler;

    private float m_DistanceAll;//目標までの距離

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        m_goalPos = m_goalBackGround.transform.position;
        m_DistanceAll = Vector3.Distance(m_goalPos, this.transform.position);//目標までの距離設定
    }

    // Update is called once per frame
    void Update()
    {
        //タイトルの状態がスクロールでないなら終了
        if(m_titleHandler.TitleState != CS_TitleHandler.TITLE_STATE.SCROLL) { return; }

        // カメラの上方向への移動量を取得
        m_camera.transform.Translate(Vector3.up * m_scrollSpeed * Time.deltaTime);

        float nowDistance = Vector3.Distance(m_goalPos, m_camera.transform.position);//現在の距離
        float decelerationDistance = m_DistanceAll * (1.0f - m_decelerationRatio);//減速を開始する距離

        // ゴールに到達したかどうかをチェック
        Vector2 goalToCamera = m_goalPos - m_camera.transform.position;
        //目標値のyより上？
        if (goalToCamera.y < 0.0f) 
        {
            Vector3 pos = new Vector3(m_goalPos.x, m_goalPos.y, m_camera.transform.position.z);
            m_camera.transform.position = pos; // ゴール位置にカメラを移動
            m_scrollSpeed = 0.0f;
            m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.STOP;//ストップ状態へ
        }
        else if (nowDistance < decelerationDistance)
        { 
            //減速開始点までの距離
            float distanceToDecelerate = m_DistanceAll - nowDistance;

            //減速する時間
            float decelerationTime = m_decelerationTime * (distanceToDecelerate / decelerationDistance);

            //減速
            m_scrollSpeed = Mathf.Lerp(m_scrollSpeed, 0, Time.deltaTime / decelerationTime);
        }
        else
        {
            //加速
            m_scrollSpeed += m_acceleration * Time.deltaTime;
        }

        //速度を制限
        m_scrollSpeed = Mathf.Clamp(m_scrollSpeed, 0.0f, Mathf.Infinity);
    }
}
