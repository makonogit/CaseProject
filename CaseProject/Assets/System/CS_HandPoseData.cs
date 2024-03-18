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


    //===== 風生成用変数 ========
    
    private Vector3 previouspos;
    private float[] m_fSwingTime = new float[2] { 0.0f, 0.0f };  //横振りディレイ計算用

    [SerializeField,Header("横振りを検出する闘値")]
    private float m_fSwingThreshold = 0.1f; //指の横振りを検出する為の闘値
    [SerializeField,Header("横振りしてからの検出ディレイ(sc)")]
    private float m_fSwingDelay = 1.0f;       
    [SerializeField, Header("風オブジェクト")]
    private GameObject m_objWind;


    // Start is called before the first frame update
    void Start()
    {
     
        
    }

    // Update is called once per frame
    void Update()
    {
        //手の情報を取得
        if (!m_HandLandmark[0] || !m_HandLandmark[1])
        {
            if (transform.childCount > 0)
            {
                HandLandmarkListAnnotation hand = transform.Find("HandLandmarkList Annotation(Clone)").GetComponent<HandLandmarkListAnnotation>();

                if (hand)
                {
                    m_HandLandmark[(int)hand.GetHandedness()] = hand;
                }
            }
        }
        

        //--------------風の処理(簡易実装)-----------------
        if(m_HandLandmark[0] && m_HandLandmark[0].isActive)
        {
            CreateWind(HandLandmarkListAnnotation.Hand.Left);
        }

        if(m_HandLandmark[1] && m_HandLandmark[1].isActive)
        {
            CreateWind(HandLandmarkListAnnotation.Hand.Right);
        }


        //=====デバッグ　ポーズ認識確認==========
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fingerindex data;
            data = FingerData(HandLandmarkListAnnotation.Hand.Left);
            Debug.Log(FindKeyByValue(data));
            
        }
    }


    //風起こし関数
    //引数:手の左右
    private void CreateWind(HandLandmarkListAnnotation.Hand hand) 
    {

        //Debug.Log(hand);
        
        //左手人差し指の先の移動量を計算
        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();

        Vector3 currentpos = point[8].transform.position;

        //前のフレームからの移動量を計算
        Vector3 movement = currentpos - previouspos;

        //横方向の速度を計算
        float HorizontalSpeed = Mathf.Abs(Vector3.Dot(movement, point[8].transform.right)) / Time.deltaTime;

        m_fSwingTime[(int)hand] += Time.deltaTime; //横振りしてからの時間を計算

        if (HorizontalSpeed > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay)
        {
            //Debug.Log(HorizontalSpeed);

            m_fSwingTime[(int)hand] = 0.0f;
            GameObject windobj = m_objWind;
            if (hand == HandLandmarkListAnnotation.Hand.Left)
            {
               // windobj.transform.position = new Vector3(windobj.transform.position.x, point[0].transform.position.y * 0.1f, 0.0f);    //座標を設定
                windobj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);    //座標を設定
            }
            else
            {
                Debug.Log("右手");
                windobj.transform.position = new Vector3(windobj.transform.position.x * -1, point[0].transform.position.y * 0.1f, 0.0f);    //座標を設定
            }

            CS_Wind cs_wind = windobj.GetComponent<CS_Wind>();  //風のスクリプト取得

            //今の指の状態を取得
            Fingerindex data;
            data = FingerData(hand);
            m_sKey = FindKeyByValue(data);

            float angle = Mathf.Atan2(movement.y, movement.x);
            windobj.transform.eulerAngles = new Vector3(0.0f,0.0f,angle*Mathf.Rad2Deg);
            Debug.Log("風の角度" + angle);

            //Debug.Log(cs_wind.Movement);
            Debug.Log(m_sKey);

            //風の生成 指の状態によって風の強さを変更
            switch (m_sKey)
            {
                case "One":
                    windobj.GetComponent<SpriteRenderer>().color = Color.blue;
                    cs_wind.WindPower = 0.2f;
                    Instantiate(windobj);
                    break;
                case "Two":
                    windobj.GetComponent<SpriteRenderer>().color = Color.green;
                    cs_wind.WindPower = 0.4f;
                    Instantiate(windobj);
                    break;
                case "Three":
                    windobj.GetComponent<SpriteRenderer>().color = Color.yellow;
                    cs_wind.WindPower = 0.6f;
                    Instantiate(windobj);
                    break;
                case "For":
                    windobj.GetComponent<SpriteRenderer>().color = Color.red;
                    cs_wind.WindPower = 0.8f;
                    Instantiate(windobj);
                    break;
                default:
                    //Debug.Log("風を起こせない");
                    break;
            }
        }

        //現在の位置を保存
        previouspos = currentpos;
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
