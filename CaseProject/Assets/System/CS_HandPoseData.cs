//-----------------------------------------------
//担当者：菅眞心
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;


public class CS_HandPoseData : MonoBehaviour
{

    //両手のデータ
    private HandLandmarkListAnnotation[] m_HandLandmark = new HandLandmarkListAnnotation[2];

    public PointListAnnotation LeftHandData
    {
        get
        {
            return m_HandLandmark[0].GetLandmarkList();
        }
    }

    public PointListAnnotation RightHandData
    {
        get
        {
            return m_HandLandmark[1].GetLandmarkList();
        }
    }

    //指のデータ
    public struct Fingerindex
    {
        public bool thumb;     //親指
        public bool index;     //人差し指
        public bool middle;    //中指
        public bool ring;      //薬指
        public bool little;    //小指

        public Fingerindex(bool t,bool i,bool m,bool r ,bool l)
        {
            thumb = t;
            index = i;
            middle = m;
            ring = r;
            little = l;
        }
    }

    //ポーズデータ
    public readonly Dictionary<string, Fingerindex> PoseData = new Dictionary<string, Fingerindex>
    {
        {"Zero",new Fingerindex(false,false,false,false,false) }, 
        {"One",new Fingerindex(false,true,false,false,false) },
        {"Two",new Fingerindex(false,true,true,false,false) }, 
        {"Three",new Fingerindex(false,true,true,true,false) },
        {"For",new Fingerindex(false,true,true,true,true) },
        {"Five",new Fingerindex(true,true,true,true,true) },
        {"Six",new Fingerindex(true,false,false,false,false) },
        {"Seven",new Fingerindex(true,true,false,false,false) },
        {"Eight",new Fingerindex(true,true,true,false,false) },
        {"Nine",new Fingerindex(true,true,true,true,false) },
    };

    private string m_sKey;  //キーの名前


    //手の向き
    private enum HandDirection
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }


    //===== 風生成用変数 ========
    
    private Vector3 m_v3Currentpos;
    private float[] m_fSwingTime = new float[2] { 0.0f, 0.0f };  //横振りディレイ計算用

    [SerializeField,Header("横振りを検出する闘値")]
    private float m_fSwingThreshold = 100.0f; //指の横振りを検出する為の闘値
    [SerializeField,Header("横振りしてからの検出ディレイ(sc)")]
    private float m_fSwingDelay = 1.0f;       
    [SerializeField, Header("風オブジェクト")]
    private GameObject m_objWind;


    [SerializeField, Header("座標取得の待機フレーム")]
    private float m_fWaitFream = 0.2f;
    private float m_fWaitFreamTime = 0.0f;

    [SerializeField, Header("フレーム保管数")]
    private int m_recordNum = 50;           //保管数
    private Vector3[] m_recordPositions;    //保管用配列
    private int m_recordIndex = 0;
    private bool m_isRecording = false;    //レコーディングフラグ
    private bool m_isRecordFinish = false;    //レコーディング完了フラグ
    //風のステータス
    public struct WindStatus
    {
        public float angle;
        public float speed;
        public float distance;
    }
    private WindStatus m_windStatus;
    //===== 雨生成用変数 ========
    [SerializeField, Header("生成する指の番号")]
    private int[] m_rCreateFingerNums = { 4, 8, 12, 16, 20 };

    [SerializeField, Header("次に雨粒を生成するまでの時間")]
    private float m_rIntervalTime = 0.3f;//次に生成するまでの時間

    private float m_rNowTime = 0.0f;//現在の時間

    [SerializeField, Header("雨オブジェクト")]
    private GameObject m_objRain;

    [SerializeField, Header("雲オブジェクト")]
    private GameObject m_CloudObj;

    //===========押し引きアクション用================
    private float[] m_HandDepth = new float[40];
    private int m_nPushData = -1;           //押したか引いたか　0:引き　1:押し
    private int m_nPushFream = 0;           //現在の保存フレーム
    private const int m_nMaxPushFream = 40; //何フレーム保存するか
    [SerializeField, Header("手の押し引きの闘値")]
    private float m_fPushThreshold = 1.0f;
    private bool m_IsSavePush = false;       //保存し終わったか

    //押し引きデータのゲッター
    public int PUSHDATA
    {
        get
        {
            return m_nPushData;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<m_nMaxPushFream;i++)
        {
            m_HandDepth[i] = 0.0f;
        }

        m_recordPositions = new Vector3[m_recordNum];   
    }

    // Update is called once per frame
    void Update()
    {
        //手の情報を取得
        if (transform.childCount > 0 && (!m_HandLandmark[0] || !m_HandLandmark[1]))
        {
            HandLandmarkListAnnotation hand = transform.Find("HandLandmarkList Annotation(Clone)").GetComponent<HandLandmarkListAnnotation>();

            if (!m_HandLandmark[0] && (hand[20].transform.position.x < 0.0f))
            {
                m_HandLandmark[0] = hand;
            }

            if (!m_HandLandmark[1] && (hand[20].transform.position.x > 0.0f))
            {
                m_HandLandmark[1] = hand;
            }

        }

        ////--------------風の処理(簡易実装)-----------------
        //if (m_HandLandmark[0] && m_HandLandmark[0].isActive)
        //{
        //    RecordFingerPosition(HandLandmarkListAnnotation.Hand.Left);
        //    CreateWind(HandLandmarkListAnnotation.Hand.Left);
        //}

        //if(m_HandLandmark[1] && m_HandLandmark[1].isActive)
        //{
        //    RecordFingerPosition(HandLandmarkListAnnotation.Hand.Right);
        //    CreateWind(HandLandmarkListAnnotation.Hand.Right);

        //}

        //----------------雨の生成処理-----------------------
        if (m_HandLandmark[0] && m_HandLandmark[0].isActive)
        {
           CreateRain(HandLandmarkListAnnotation.Hand.Left);
            PushHand(HandLandmarkListAnnotation.Hand.Left);
        }

        if (m_HandLandmark[1] && m_HandLandmark[1].isActive)
        {
           CreateRain(HandLandmarkListAnnotation.Hand.Right);
        }

        //=====デバッグ　ポーズ認識確認==========
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fingerindex data;
            data = FingerData(HandLandmarkListAnnotation.Hand.Left);
            Debug.Log(FindKeyByValue(data));
        }
    }

    public Vector3 GetHandVector(HandLandmarkListAnnotation.Hand hand)
    {
        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();
        //Vector3 currentpos = point[12].transform.position;

        //中指の付け根を手の中心座標とする
        Vector3 HandPos =point[9].transform.position;

        //3つの点を取得し、それらを結ぶベクトルを計算
        Vector3 posA = point[5].transform.position;
        Vector3 posB = point[17].transform.position;
        Vector3 posC = point[0].transform.position;
        Vector2 vecAB = new Vector2(posB.x - posA.x, posB.y - posA.y);
        Vector2 vecAC = new Vector2(posC.x - posA.x, posC.y - posA.y);
        float cross = vecAB.x * vecAC.y - vecAB.y * vecAC.x;

        //2つのベクトルの外積を取得し、面の法線ベクトルを計算
        Vector3 normal = new Vector3(0,0,cross).normalized;

        //手の位置から平面へのベクトルを計算
        Vector3 toHand = new Vector2(HandPos.x - posA.x, HandPos.y - posA.y);

        //平面上への射影を求める
        Vector3 projectedHand = Vector2.Dot(toHand, new Vector2(normal.x,normal.y)) * new Vector2(normal.x,normal.y);

        //平面から手の方向ベクトルを求める
        Vector3 direction = (projectedHand - toHand).normalized;

        //Debug.Log(direction);
        
        //面の法線ベクトルを使って無期ベクトルを計算
     //   Vector3 vecDir = Vector3.Cross(normal, vecAB).normalized;
        return direction;
    }


    private HandDirection GetHandDirection(Vector3 Handvector)
    {
        if(Handvector.x > 0.5f && Handvector.y < -0.5f)
        {
            return HandDirection.LEFT;
        }

        if (Handvector.x > 0.2f && Handvector.y > 0.8f)
        {
            return HandDirection.RIGHT;
        }

        if (Handvector.x > 0.9f && Handvector.y > -0.2f)
        {
            return HandDirection.DOWN;
        }

        if(Handvector.x < -0.8f && Handvector.y > 0.2f)
        {
            return HandDirection.UP;
        }

        return HandDirection.NONE;

    }

    private Vector3 CurrentVector;

    //ポジション保管
    private void RecordFingerPosition(HandLandmarkListAnnotation.Hand hand)
    {

        if (m_isRecordFinish) { return; }
        //左手人差し指の先の移動量を計算
        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();

        Vector3 Oldpos = point[5].transform.position;
        Vector3 OldVector = GetHandVector(hand);
        
        //ベクトルから手の向きを取得
        HandDirection handrirection = GetHandDirection(OldVector);

        m_fWaitFreamTime += Time.deltaTime; //待機フレーム加算

        Vector3 MoveDirection = Vector3.zero;

        if (Oldpos != m_v3Currentpos)
        {
            MoveDirection = (Oldpos - m_v3Currentpos).normalized;
        }

        //Debug.Log(MoveDirection);

        //左向きで左に振った時
        if (handrirection == HandDirection.LEFT &&
            OldVector != CurrentVector && MoveDirection.x > 0.0f)
        {
            return;
        }
        ////右向きで右に振った時
        //if(handrirection == HandDirection.RIGHT &&
        //    OldVector != CurrentVector && HorizontalDistance < 0.0f)
        //{
        //    return;
        //}
        //下向きで上に振った時
        //if (handrirection == HandDirection.DOWN &&
        //    OldVector != CurrentVector && OldVector.y > CurrentVector.y)
        //{
        //    return;
        //}
        ////上向きで下に振った時
        //if (handrirection == HandDirection.UP &&
        //    OldVector != CurrentVector && OldVector.y < CurrentVector.y)
        //{
        //    return;
        //}

        //if(handrirection == HandDirection.NONE) { return; }

        float movement = Vector3.Distance(m_v3Currentpos, Oldpos);

        m_fSwingTime[(int)hand] += Time.deltaTime; //横振りしてからの時間を計算
        //-----------------テスト--------------------
        //今の指の状態を取得
        Fingerindex data;
        data = FingerData(hand);
        m_sKey = FindKeyByValue(data);
        
        //----------------------------------------
        //距離が一定以上かつレコーディング中じゃない？
        if (movement > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay &&!m_isRecording)
        {
            m_isRecording = true;//レコード開始
           // Debug.Log("レコード開始");
        }

        //レコーディング開始かつ完了していない？
        if (m_isRecording && !m_isRecordFinish )
        {
            m_recordPositions[m_recordIndex] = Oldpos;//現在の指の位置を保存
            m_recordIndex++;
            //保管配列のサイズ以上？
            if(m_recordIndex >= m_recordPositions.Length)
            {
                m_isRecordFinish = true;//レコード完了
                SetWindStatus();//風のステータスを設定
                m_fSwingTime[(int)hand] = 0.0f;
                //Debug.Log("レコード完了");
            }
        }
        if (m_fWaitFreamTime > m_fWaitFream)
        {
            //現在の位置を保存
            CurrentVector = GetHandVector(hand);
            m_v3Currentpos = point[5].transform.position;
            m_fWaitFreamTime = 0.0f;
        }

    }



    //風のステータスの設定関数
    private void SetWindStatus()
    {
        Vector3 firstPos = m_recordPositions[0];
        Vector3 finalPos = m_recordPositions[m_recordPositions.Length -1];
        m_windStatus.distance = Vector3.Distance(firstPos, finalPos);//距離

        float recordTotalTime = Time.deltaTime * m_recordPositions.Length;//レコード時間
        //最初に位置から最後の位置までの速度設定
        m_windStatus.speed = m_windStatus.distance / recordTotalTime;

        //最初のフレームと最後のフレームのベクトル
        Vector3 movevec = finalPos - firstPos;
       
        //角度設定
        m_windStatus.angle = Mathf.Atan2(movevec.y, movevec.x);
    }
    //風起こし関数
    //引数:手の左右
    private void CreateWind(HandLandmarkListAnnotation.Hand hand) 
    {
        Fingerindex data;
        data = FingerData(hand);
        m_sKey = FindKeyByValue(data);
        //手がパーか
        bool isFive = (string.Compare(m_sKey, ("Five")) == 0);
        if (!isFive) { return; }
        if (!m_isRecordFinish) { return; }

        GameObject windobj = m_objWind;
        windobj.transform.position = new Vector3(m_recordPositions[0].x, m_recordPositions[0].y, 0.0f);    //座標を保管したポジション配列の最初に設定
        Debug.Log(m_recordPositions[0]);
        CS_Wind cs_wind = windobj.GetComponent<CS_Wind>();  //風のスクリプト取得

        //--------------風の角度設定--------------------
        windobj.transform.eulerAngles = new Vector3(0.0f, 0.0f, m_windStatus.angle* Mathf.Rad2Deg);
        //Debug.Log("風の角度" + m_windStatus.angle);
        //風のスピード
        cs_wind.Movement = m_windStatus.speed * 0.01f;

        //Debug.Log(cs_wind.Movement);
        //Debug.Log(m_sKey);

        //windobj.GetComponent<SpriteRenderer>().color = Color.green;
        cs_wind.WindPower = 1.0f* m_windStatus.speed;
        Instantiate(windobj);//風を生成するます
       
        InitRecord();//レコード関連の変数を初期化
        return;
        //Debug.Log(hand);

        //左手人差し指の先の移動量を計算
        //PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();

        //Vector3 currentpos = point[8].transform.position;

        ////前のフレームからの移動量を計算
        //Vector3 movementvec = currentpos - previouspos; 

        //float movement = Vector3.Distance(currentpos, previouspos);

        ////横方向の速度を計算
        ////float HorizontalSpeed = Mathf.Abs(Vector3.Dot(movement, point[8].transform.right)) / Time.deltaTime;

        //m_fSwingTime[(int)hand] += Time.deltaTime; //横振りしてからの時間を計算
        ////if(HorizontalSpeed > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay)

        //if (movement > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay)
        //{
        //    //Debug.Log(HorizontalSpeed);

        //    m_fSwingTime[(int)hand] = 0.0f;
        //    GameObject windobj = m_objWind;
        //    if (hand == HandLandmarkListAnnotation.Hand.Left)
        //    {
        //       // windobj.transform.position = new Vector3(windobj.transform.position.x, point[0].transform.position.y * 0.1f, 0.0f);    //座標を設定
        //        windobj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);    //座標を設定(テスト)
        //    }
        //    else
        //    {
        //        //Debug.Log("右手");
        //        //windobj.transform.position = new Vector3(windobj.transform.position.x * -1, point[0].transform.position.y * 0.1f, 0.0f);    //座標を設定
        //    }

        //    CS_Wind cs_wind = windobj.GetComponent<CS_Wind>();  //風のスクリプト取得

        //    //今の指の状態を取得
        //    Fingerindex data;
        //    data = FingerData(hand);
        //    m_sKey = FindKeyByValue(data);

        //    //--------------風の角度設定--------------------
        //    float angle = Mathf.Atan2(movementvec.y, movementvec.x);
        //    windobj.transform.eulerAngles = new Vector3(0.0f,0.0f,angle*Mathf.Rad2Deg);
        //    Debug.Log("風の角度" + angle);

        //    //Debug.Log(cs_wind.Movement);
        //    Debug.Log(m_sKey);

        //    //風の生成 指の状態によって風の強さを変更
        //    switch (m_sKey)
        //    {
        //        case "One":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.blue;
        //            cs_wind.WindPower = 0.2f;
        //            Instantiate(windobj);
        //            break;
        //        case "Two":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.green;
        //            cs_wind.WindPower = 0.4f;
        //            Instantiate(windobj);
        //            break;
        //        case "Three":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.yellow;
        //            cs_wind.WindPower = 0.6f;
        //            Instantiate(windobj);
        //            break;
        //        case "For":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.red;
        //            cs_wind.WindPower = 0.8f;
        //            Instantiate(windobj);
        //            break;
        //        default:
        //            //Debug.Log("風を起こせない");
        //            break;
        //    }
        //}

        //m_fWaitFreamTime += Time.deltaTime; //待機フレーム加算
        //if (m_fWaitFreamTime > m_fWaitFream)
        //{
        //    //現在の位置を保存
        //    previouspos = currentpos;
        //    m_fWaitFreamTime = 0.0f;
        //}


    }


    //押し引き関数
    //引数：手の左右
    //戻り値：-1 判定不可　0:引いた　1:押した
    private int PushHand(HandLandmarkListAnnotation.Hand hand)
    {
        Fingerindex data;
        data = FingerData(hand);
        m_sKey = FindKeyByValue(data);

        //手を広げていなかったら計算しない
        if(m_sKey != "Five")
        {
            m_nPushFream = 0;
            return -1;
        }

        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();
        //中指の付け根を手の中心としてZ座標を保存
        float HandPos = point[9].transform.position.z;

        //手の位置の差分を計算して、配列に保存
        float HandPosDelta = 0.0f;
        for(int i = 0;i<m_nMaxPushFream;i++)
        {
            int PrevFream = (m_nPushFream - i - 1 + m_nMaxPushFream) % m_nMaxPushFream;
            HandPosDelta += HandPos - m_HandDepth[PrevFream];
        }
        m_HandDepth[m_nPushFream] = HandPos;
        m_nPushFream = (m_nPushFream + 1) % m_nMaxPushFream;

        //保存し終わったら手の動きを検出
        if (!m_IsSavePush && m_nPushFream == 0)
        {
            m_IsSavePush = true;

            //手の位置が闘値を超えたかを確認
            bool IsLeave = HandPosDelta / m_nMaxPushFream > m_fPushThreshold;
            bool IsPush = HandPosDelta / m_nMaxPushFream < -m_fPushThreshold;
            

            Debug.Log(m_fPushThreshold);
            if (IsPush)
            {
                Debug.Log("おした");
                return m_nPushData = 1;
            }
            if (IsLeave)
            {
                Debug.Log("引いた");
                return m_nPushData = 0;
            }

        }
        else
        {
            m_IsSavePush = false;
            for(int i = 0; i< m_nMaxPushFream;i++)
            {
                m_HandDepth[i] = 0;
            }
        }

        
        return m_nPushData = -1;

        {
            m_HandDepth[m_nPushFream] = point[9].transform.position.z;

            m_nPushFream++;

            //指定フレーム数記録して終端まで保存したら押したか引いたかを返す
            if (m_nPushFream == m_nMaxPushFream - 1)
            {
                for (int i = 0; i < m_nMaxPushFream - 1; i++)
                {
                    if (m_HandDepth[i] < m_HandDepth[i + 1])
                    {
                        if (m_nPushData != -1 && m_nPushData == 0)
                        {
                            m_nPushFream = 0;
                            m_nPushData = -1;
                            return m_nPushData;
                        }
                        m_nPushData = 1;    //押している
                    }
                    if (m_HandDepth[i] > m_HandDepth[i + 1])
                    {
                        if (m_nPushData != -1 && m_nPushData == 1)
                        {
                            m_nPushFream = 0;
                            m_nPushData = -1;
                            return m_nPushData;
                        }
                        m_nPushData = 0;    //引いている
                    }
                }

                //フレームのリセット
                m_nPushFream = 0;

                return m_nPushData;
            }

            return -1;
        }
    }

    private void InitRecord()
    {
        m_isRecordFinish = false;
        m_isRecording = false;
        m_recordIndex = 0;
        //Debug.Log("レコード初期化");
    }

    //雨降らし関数
    //引数:手の左右
    private void CreateRain(HandLandmarkListAnnotation.Hand hand)
    {
        //手が下を向いているか
        HandDirection handdirection = GetHandDirection(GetHandVector(hand));
        //Debug.Log(handdirection);
        if (handdirection != HandDirection.DOWN) { return; }

        m_rNowTime += Time.deltaTime;//現在時間加算
        Fingerindex data;
        data = FingerData(hand);
        //Debug.Log(FindKeyByValue(data));
        m_sKey = FindKeyByValue(data);
        //手のポーズが5?
        if (m_sKey == "Five" && m_rNowTime > m_rIntervalTime)
        {
            m_rNowTime = 0.0f;//現在時間初期化
           // Debug.Log("雨の生成開始");
            //手のリストを取得
            PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();


            //生成
            float XMinpos = m_CloudObj.transform.position.x - m_CloudObj.transform.localScale.x * 10;
            float XMaxpos = m_CloudObj.transform.position.x + m_CloudObj.transform.localScale.x * 10;
            float RandomXpos = Random.Range(XMinpos, XMaxpos);
            m_objRain.transform.position = new Vector3(RandomXpos, m_CloudObj.transform.position.y + m_CloudObj.transform.localScale.y, 0.0f);
            Instantiate(m_objRain);
        }
    }

    //指が上がっているかの判定関数
    //引数:右手or左手
    //戻り値:指のデータ
    Fingerindex FingerData(HandLandmarkListAnnotation.Hand hand)
    {
        //データを取得
        PointListAnnotation LandMarkData = m_HandLandmark[(int)hand].GetLandmarkList();

        Fingerindex fingerdata;

        //親指の付け根から30離れていたらあげている判定
        fingerdata.thumb = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[4].transform.position) > 20;
        fingerdata.index = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[8].transform.position) > 20;
        fingerdata.middle = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[12].transform.position) > 20;
        fingerdata.ring = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[16].transform.position) > 20;
        fingerdata.little = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[20].transform.position) > 20;

        return fingerdata;
    }


    //Dictionaryの値からキーを取得する関数
    //引数:Dictionaryの値
    //戻り値:Key
    string FindKeyByValue(Fingerindex targetvalue)
    {
        // Dictionaryをループして指定した値に対応するキーを検索
        foreach (KeyValuePair<string,Fingerindex> pair in PoseData)
        {
            if(pair.Value.Equals(targetvalue))
            {
                return pair.Key;
            }
        }

        //見つからなかったらnullを返す
        return null;

    }



}
