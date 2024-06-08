//-----------------------------------------------
//担当者：菅眞心
//カメラ制御クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraControl : MonoBehaviour
{
    [SerializeField, Header("追従対象")]
    private GameObject m_TargetObj;

    [SerializeField,Header("カメラ移動制限")]
    private EdgeCollider2D m_LimitPos;

    [SerializeField, Header("ステージの上限")]
    private EdgeCollider2D m_MaxHeight;

    [SerializeField, Header("ステージデータ")]
    private CS_StageData m_csStagedata;

    //カメラの移動制限
    private Vector2 m_v2MaxLimit;
    private Vector2 m_v2MinLimit;

    private Transform m_tThisTrans;
    private Transform m_tTargetTrans;

    private Camera maincamera;

    //追従対象を変更
    public GameObject TARGET
    {
        set
        {
            m_TargetObj = value;
            m_tTargetTrans = value.transform;
        }
    }

    
    //ステージによって変わるのでSceneManagerで読み込み時に変更
    public float MAXHEIGHT
    {
        set
        {
            m_v2MaxLimit.y = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //管理クラスにデータ保存
        ObjectData.m_csCamCtrl = this;

        //カメラのサイズ取得
        maincamera = Camera.main;

        //移動制限の設定
        m_v2MinLimit.x = m_LimitPos.points[0].x + maincamera.orthographicSize * maincamera.aspect;
        m_v2MinLimit.y = m_LimitPos.points[1].y + maincamera.orthographicSize;
        m_v2MaxLimit.x = m_LimitPos.points[2].x - maincamera.orthographicSize * maincamera.aspect;
        //m_v2MaxLimit.y = m_LimitPos.points[3].y - maincamera.orthographicSize;

        //ステージの長さを取得してカメラの上限を設定
        int length = m_csStagedata.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_nStageLength;
        m_v2MaxLimit.y = ((length * 10) - 5) - maincamera.orthographicSize;

        //EdgeColliderを設定
        Vector2[] points = m_LimitPos.points;
        points[0].y = ((length * 10) - 5);
        points[3].y = ((length * 10) - 5);
        m_LimitPos.points = points;

        m_MaxHeight.offset = new Vector2(0.0f, (length * 10) - 5);


        //座標の設定
        m_tTargetTrans = m_TargetObj.transform;
        m_tThisTrans = this.transform;

        //Debug.Log(maincamera.ViewportToWorldPoint(Vector2.zero));
        //Debug.Log(maincamera.ViewportToWorldPoint(Vector2.one));

    }

    // Update is called once per frame
    void Update()
    {

        //前回座標
        Vector3 ClampPosition = new Vector3(m_tTargetTrans.position.x, m_tTargetTrans.position.y, m_tThisTrans.position.z);
        //移動制限
        ClampPosition.x = Mathf.Clamp(ClampPosition.x, m_v2MinLimit.x, m_v2MaxLimit.x);
        ClampPosition.y = Mathf.Clamp(ClampPosition.y + 2.5f, m_v2MinLimit.y, m_v2MaxLimit.y);

        m_tThisTrans.position = ClampPosition;
        
    }

    
}
