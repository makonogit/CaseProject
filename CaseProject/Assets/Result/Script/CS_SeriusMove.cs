using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SeriusMove : MonoBehaviour
{
    [SerializeField, Header("リザルトコントローラー")]
    private CS_ResultController m_rController;

    [SerializeField, Header("星座のプレハブリスト")]
    private List<GameObject> m_constellationList = new List<GameObject>();
    private GameObject m_constellation;//このリザルトで使う星座オブジェクト
    private Transform m_targetObj;//シリウスが移動する場所
    private Vector3 m_targetPos;//シリウスが移動する場所
    private List<Transform> m_starList = new List<Transform>();//星座内にある星のリスト

    [SerializeField, Header("星の移動スピード")]
    private float m_fSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        //今回使う星座を入れる
        m_constellation = Instantiate(m_constellationList[(int)m_rController.StageType],Vector3.zero, Quaternion.identity);
        //m_constellationList.Clear();//リストはもう使わないのでクリア

        if(m_constellation == null) { Debug.LogWarning("星座がありません"); }
        Debug.Log(m_constellation.name);
        //星座オブジェクトからstarsという子オブジェクトを見つける
        Transform starsInCostellation = m_constellation.transform.Find("stars");

        bool isTarget = starsInCostellation != null;
        if (!isTarget) { Debug.LogWarning("starsが無い"); }

        m_targetObj = starsInCostellation.Find("targetStar");//目標位置のオブジェクト
        //ターゲットがあるか
        isTarget = m_targetObj != null;
        m_targetPos = m_constellation.transform.TransformPoint(starsInCostellation.localPosition + m_targetObj.position);
        //m_targetPos = m_constellation.transform.TransformPoint(m_targetPos);
        Debug.Log("ターゲット位置" + m_targetPos);
        if (!isTarget) { Debug.LogWarning("ターゲットが無い"); }

        //スターとシリウスを見つける
        Transform seriusStar = transform.Find("Star");//Starオブジェクトを見つける
        isTarget = seriusStar != null;
        if (!isTarget) { Debug.LogWarning("Starが無い"); }
        Transform sirius = transform.Find("Sirius");//Seriusオブジェクトを見つける
        isTarget = sirius != null;
        if (!isTarget) { Debug.LogWarning("Siriusが無い"); }
        seriusStar.localScale = m_targetObj.lossyScale;

        //シリウスの新たなポジション設定
        Vector3 seriusNewPos = seriusStar.position + Vector3.up * seriusStar.localScale.y;
        seriusNewPos.y += sirius.localScale.y / 2;
        sirius.position = seriusNewPos;
    }

    // Update is called once per frame
    void Update()
    {
        // 目標値に近づいたら到着と判断
        if (Vector3.Distance(transform.position, m_targetObj.position) < 0.1f)
        {
            NextStateReady();//次の状態へ行く準備
            Destroy(this);
            return;
        }

        //移動
        Move();
       
    }

    //次の処理に行くための準備
    private void NextStateReady()
    {
        //星座オブジェクトからlinesオブジェクトを見つけ出し
        GameObject lines = m_constellation.transform.Find("lines").gameObject;
        //ターゲットがあるか
        bool isTarget = lines != null;
        if (!isTarget) { Debug.LogWarning("linesが無い"); }
        lines.AddComponent<CS_LineController>();
        lines.GetComponent<CS_LineController>().SetResControlloer(this.m_rController);
        
        //はめる効果音をならしてリザルトの状態をライン表示状態にして消去
        m_rController.ResultState = CS_ResultController.RESULT_STATE.BORN_LINE;
    }

    //移動処理
    private void Move()
    {
       
        Vector3 direction = (m_targetObj.position- transform.position).normalized;

        // 目標値に向かって移動
        transform.position += direction * m_fSpeed * Time.deltaTime;
    }
}

