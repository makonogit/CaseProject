//------------------------------
// 担当者：中島　愛音
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;


public class CS_TitleHandler : MonoBehaviour
{
    [SerializeField, Header("ハンドサイン")]
    private CS_HandSigns m_handSigns;

    //private List<HandLandmarkListAnnotation> m_handLandmark = new List<HandLandmarkListAnnotation>();

    [SerializeField, Header("次のシーンの名前")]
    private string m_nextSceneName;

    private Vector3[] m_v3HandDir = new Vector3[2];    //手の向き
    private Vector3[] m_v3HandMove = new Vector3[2];   //手の動き

    [SerializeField] private List<Vector3> m_Directions = new List<Vector3>();
    private List<float> m_Time = new List<float>();
    //[SerializeField, Header("両手の初期位置")]
    //[Header("0:右手の初期位置")]
    //[Header("1:左手の初期位置")]
    //private GameObject[] m_startHandsObjPosition = new GameObject[2];
    //private Vector3[] m_startHandsScreenPosition = new Vector3[2];

    //[SerializeField, Header("シリウスが出てくる両手の位置")]
    //[Header("0:右手の初期位置")]
    //[Header("1:左手の初期位置")]
    //private GameObject[] m_bornSeriusObjPosition = new GameObject[2];


    //[SerializeField, Header("両手の配置の認識可能範囲")]
    //private float m_recognizableDistance = 2.0f;

    bool m_isUpdate = true;

    //状態が待機時2の待機時間
    private float m_nowWaitTime = 0.0f;
    private float m_waitTime = 1.0f;

    public enum TITLE_STATE
    {
        SET_HANDS,  //両手をを初期位置にセットできているか
        CALL_SERIUS,//シリウスを呼ぶ
        BORN_SERIUS,//シリウスの登場
        WAIT1,      //待機1
        SCROLL,     //画面スクロール中
        STOP,       //スクロール終了
        MAGNIFICATION_SERIUS,//拡大シリウス
        REDUCTION_SERIUS,//縮小シリウス
        WAIT2,      //待機2
        GAME_END
    }

    [SerializeField] private TITLE_STATE m_titleState = TITLE_STATE.SET_HANDS;

    public TITLE_STATE TitleState
    {
        set
        {
            m_titleState = value;
        }
        get
        {
            return m_titleState;
        }
    }

    private bool m_isChangeSceneInpossible = false;
    public bool IsChangeSceneImpossible
    {
        get
        {
            return m_isChangeSceneInpossible;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //m_handLandmark = m_handSigns.HandMark;
        CS_HandSigns.OnCreateWinds += Swing;

        //for(int i =0; i < 2; i++)
        //{
        //    Vector3 worldPos = m_startHandsObjPosition[i].transform.position;
        //    m_startHandsScreenPosition[i] = worldPos;
        //}

        if (!m_handSigns) { Debug.LogWarning("CS_HandSigns.csがアタッチされていません"); }
    }

    

    // Update is called once per frame
    void Update()
    {
        CheckGoNextScene();//次のシーンへいくかどうかの処理
        

        // リストの更新
        TimeOverRemoveList();
        
        //更新しないなら終了
        if (!m_isUpdate) { return; }



        //両手の情報を取得
        for (int i = 0; i < 2; i++) 
        {
            //両手がパーじゃなかったら終了
            bool is_handpose = m_handSigns.GetHandPose(m_handSigns.HandInfo[i].HandLandmark) == (byte)CS_HandSigns.HandPose.PaperSign;
            //if (!is_handpose) { return; }

            m_v3HandDir[i] = m_handSigns.GetHandDirection(m_handSigns.HandInfo[i].HandLandmark.GetLandmarkList());
            m_v3HandMove[i] = m_handSigns.GetHandMovement(m_handSigns.HandInfo[i].vec3MoveDistanceList);

        }


        switch (m_titleState)
        {
            case TITLE_STATE.CALL_SERIUS:
                m_isUpdate = false;
                m_titleState = TITLE_STATE.BORN_SERIUS;
                break;
        }


        // シリウスを呼ぶ
        if (IsCallSerius()) m_titleState = TITLE_STATE.BORN_SERIUS;


        //Debug.Log("Dir0" + m_v3HandDir[0] + "Dir1" + m_v3HandDir[1]);
        //Debug.Log("Move0" + m_v3HandMove[0] + "Move1" + m_v3HandMove[1]);

        {
            ////ハンドマークを取得
            ////0を右手、１を左手とする
            //m_handLandmark = m_handSigns.HandMark;

            //if (m_handLandmark.Count < 2)
            //{
            //    return;
            //}


            //string[] hand = { "右手", "左手" };
            //PointListAnnotation point1 = m_handLandmark[0].GetLandmarkList();
            //PointListAnnotation point2 = m_handLandmark[1].GetLandmarkList();
            ////右手が左手側にあるならhandLandmarkの中身を入れ替える
            //if(point1[9].transform.position.x < point2[9].transform.position.x)
            //{
            //    HandLandmarkListAnnotation mark = m_handLandmark[0];
            //    m_handLandmark[0] = m_handLandmark[1];
            //    m_handLandmark[1] = mark;
            //}

            //for (int i = 0; i < 2; i++)
            //{
            //    PointListAnnotation point = m_handLandmark[i].GetLandmarkList();

            //    float dis = Vector2.Distance(point[9].transform.position, m_startHandsScreenPosition[i]);

            //    if (dis > m_recognizableDistance) { return; }
            //}

            //    switch (m_titleState)
            //    {
            //        case TITLE_STATE.SET_HANDS:
            //            for (int i = 0; i < 2; i++)
            //            {
            //                m_startHandsObjPosition[i].transform.position = m_bornSeriusObjPosition[i].transform.position;
            //                m_startHandsScreenPosition[i] = m_startHandsObjPosition[i].transform.position;
            //            }
            //            m_titleState = TITLE_STATE.CALL_SERIUS;
            //            break;
            //        case TITLE_STATE.CALL_SERIUS:
            //            for (int i = 0; i < 2; i++)
            //            {
            //                Destroy(m_startHandsObjPosition[i]);
            //                Destroy(m_bornSeriusObjPosition[i]);
            //            }
            //            m_isUpdate = false;
            //            m_titleState = TITLE_STATE.BORN_SERIUS;
            //            break;
            //    }
        }
    }

    private void OnDestroy(){
        CS_HandSigns.OnCreateWinds -= Swing;
    }
    
    // 手をスウィングした時の位置情報を保存する関数
    // 引き数：位置情報
    // 引き数：移動方向
    // 戻り値：なし
    void Swing(Vector3 position, Vector3 direction){
        // セットハンド以外なら抜ける
        if (m_titleState != TITLE_STATE.SET_HANDS) return;
        m_Directions.Add(direction);
        m_Time.Add(Time.time);
    }

    // シリウスを呼ぶ判定をする関数
    // 引き数；なし
    // 戻り値：シリウスを呼ぶTrue
    bool IsCallSerius() 
    {
        for (int i = 0; i < m_Directions.Count-1; i++) 
        {
            float dot = Vector3.Dot(m_Directions[i], m_Directions[i + 1]);
            // 風の向きが反対ならTrue
            if (dot < 0) return true;
        }
        
        return false;
    }
    // 規定時間を超えたらリストから排除する関数
    // 引数：なし
    // 戻り値：なし
    void TimeOverRemoveList() 
    {
        // リストがないなら抜ける
        if (m_Time.Count <= 0) return;
        // 時間を超えたか
        float diff = Time.time - m_Time[0];
        const float RegulationTime = 1.0f;
        bool isTimeOver =diff > RegulationTime;
        // 規定時間を超えたらリストから排除
        if (isTimeOver) 
        {
            m_Directions.RemoveAt(0);
            m_Time.RemoveAt(0);
        }
    }

    //シーンのロード
    void CheckGoNextScene()
    {
        //待機時間2?
        if (TitleState != TITLE_STATE.WAIT2) { return; }

        if (m_nowWaitTime <= m_waitTime)
        {
            m_nowWaitTime += Time.deltaTime;//デルタタイム加算
            return;
        }
        SceneManager.LoadScene("SelectScene");  
    }

    //ゲーム終了
    public void GameEnd()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
