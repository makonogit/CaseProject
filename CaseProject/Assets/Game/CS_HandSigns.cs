//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;


public class CS_HandSigns : MonoBehaviour
{
    //両手のデータ
    private HandLandmarkListAnnotation[] m_HandLandmark = new HandLandmarkListAnnotation[2];

    // 手の移動距離
    [Header("動きリスト")]
    [SerializeField] private List<Vector3> m_vec3RightMoveDistanceList = new List<Vector3>();
    [SerializeField] private List<Vector3> m_vec3LeftMoveDistanceList = new List<Vector3>();
    
    // リストの最大数
    [Header("リストの最大数")]
    [SerializeField] private int m_nListMaxNum = 8;

    [Header("手の速度の判定範囲")]
    [SerializeField]private float m_fMinSpeed = 5.0f;
    [SerializeField]private float m_fMaxSpeed = 100.0f;

    // 手のポーズ
    public enum HandPose 
    {
        Thumb   = 0x01,
        Index   = 0x02,
        Middle  = 0x04,
        Ring    = 0x08,
        Little  = 0x10,
        RockSign        = 0x00, // グー
        ScissorsSign    = 0x06, // チョキ
        PaperSign       = 0x1f, // パー
    }
    //---------イベント---------

    // イベントハンドラ
    public delegate void EHHandSigns(Vector3 position,Vector3 direction);

    // 風生成イベント
    public static event EHHandSigns OnCreatWinds;

    //--------------------------

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // 手の情報を取得
        if(isGetHand)TakeHandsStates();
        // モーションの判定
        HandsMotionIdentify();

    }
    // 手の情報を取得するか
    // 戻り値：取得する ture 取得しない false
    private bool isGetHand 
    {
        get
        {
            // 手の情報がどちらも入っているならfalse
            bool isHandsNull = !m_HandLandmark[0] || !m_HandLandmark[1];
            if (!isHandsNull) return false;

            // 子オブジェを持っていない時false
            bool isHaveChild = transform.childCount > 0;
            if (!isHaveChild)return false;

            return true;
        }
    }
   
    // 手の情報を取得する関数
    // 引数：なし
    // 戻り値：取得した True 取得できなかった False
    private bool TakeHandsStates() 
    {
        // 子オブジェから手の情報を探す
        HandLandmarkListAnnotation hand = transform.Find("HandLandmarkList Annotation(Clone)").GetComponent<HandLandmarkListAnnotation>();
        if (!hand) return false;// 取得できなかった

        int handNum = (int)hand.GetHandedness();
        
        // 手の情報を設定する
        m_HandLandmark[handNum] = hand;
        // リストに追加
        AddHandPointList( m_HandLandmark[handNum] .GetLandmarkList(),handNum);
        
        return true;
    }

    // 手のポイントリストに追加する
    // 引数：設定する手の位置情報
    // 引数：左手か右手か
    // 引数：手のリスト
    // 戻り値：なし
    private void AddHandPointList(PointListAnnotation point,int handNum) 
    {
        List<Vector3> moveVecList = GetMoveVecList(handNum);
        // 距離を求める
        Vector3 handPos = point[5].transform.position;

        // リストに追加
        moveVecList.Insert(0, handPos);

        // リストが多くなった分消す
        for (int i = moveVecList.Count - 1; i >= m_nListMaxNum; i--) moveVecList.RemoveAt(i);    
    }


    // 手のモーションを識別する関数
    // 引数：なし
    // 戻り値：なし
    private void HandsMotionIdentify() 
    {
        // 風の生成
        // 風の向き
        Vector3 windVector = new Vector3(0, 0, 0);
        if (IsCreatWind(0, ref windVector)) CreateWind(0, windVector);
        if (IsCreatWind(1, ref windVector)) CreateWind(1, windVector);


    }

    // 手のひらの方向を取得―※出来なかったので親指の付け根の方向を取得
    // 引数：左手か右手か
    // 戻り値：手のひらの方向ベクトル
    private Vector3 GetHandDirection(int handNum) 
    {
        PointListAnnotation point = m_HandLandmark[handNum].GetLandmarkList();
        //手首の位置情報
        Vector3 wrist = point[0].transform.position;
        //　手首から人差し指の付けの根の方向
        Vector3 toIndexVec = point[1].transform.position - wrist;
        toIndexVec.Normalize();// 正規化

        return toIndexVec;
    }

    // 風を生成する条件の関数
    // 引数：右手か左手か
    // 引数：移動距離の情報を返す
    // 戻り値：生成する true しない false
    private bool IsCreatWind(int handNum,ref Vector3 move)
    {
        // 手の情報が登録されていないなら_false
        if(!m_HandLandmark[handNum]) return false;
        List<Vector3> moveVecList = GetMoveVecList(handNum);
        // リストが少ないなら_false
        bool isListUnder = moveVecList.Count < m_nListMaxNum;
        if (isListUnder)return false;

        // 移動距離初期化
        move = new Vector3(0, 0, 0);

        // 移動距離の合計
        for (int i = 0; i < moveVecList.Count - 1; i++) move += moveVecList[i] - moveVecList[i + 1];

        // 最低移動距離を越えたら_false
        bool isOverMoveDistance = move.magnitude > m_fMaxSpeed;
        if (isOverMoveDistance) return false;

        // 最大移動距離を越えなかったら_false
        bool isUnderMoveDistance = move.magnitude < m_fMinSpeed;
        if (isUnderMoveDistance) return false;

        Vector3 moveN = Vector3.Normalize(move);
        float dot = Vector3.Dot(moveN, GetHandDirection(handNum));

        // 手のひらの方向と移動した方向が一緒か
        bool isSameDirection = dot >= 0;
        // 方向が一緒ではないなら_false
        if (!isSameDirection) return false;

        // 手がパーか
        bool isPaperSign = GetHandPose(handNum)==(byte)HandPose.PaperSign;
        Debug.Log("手のポーズ%b"+GetHandPose(handNum));
        // 手がパーでないなら_false
        if (!isPaperSign) return false;

        // 移動距離リストのリセット
        moveVecList.Clear();

        return true;
    }

    // 手のポーズの取得
    // 引数：右手か左手か
    // 戻り値：一ビットずつ指が立っているとtrue
    private byte GetHandPose(int handNum) 
    {
        byte sign = 0;
        //データを取得
        PointListAnnotation LandMarkData = m_HandLandmark[handNum].GetLandmarkList();
        // 手首の位置
        Vector3 wrist = LandMarkData[0].transform.position;
        // 親指
        SetFingerPose(wrist, LandMarkData[4].transform.position, (byte)HandPose.Thumb, ref sign);
        
        // 人差し指
        SetFingerPose(wrist, LandMarkData[8].transform.position, (byte)HandPose.Index, ref sign);
        
        // 中指
        SetFingerPose(wrist, LandMarkData[12].transform.position, (byte)HandPose.Middle, ref sign);
        
        // 薬指
        SetFingerPose(wrist, LandMarkData[16].transform.position, (byte)HandPose.Ring, ref sign);
        
        // 小指
        SetFingerPose(wrist, LandMarkData[20].transform.position, (byte)HandPose.Little, ref sign);
        
        return sign;
    }

    // 指の状態取得設定関数
    // 引数：手首の位置
    // 引数：指先の位置
    // 引数：指の種類
    // 引数：指の状態を保存する変数
    // 戻り値：なし
    private void SetFingerPose(Vector3 wrist,Vector3 finger ,byte hand,ref byte fingerSign) 
    {
        if (isNotBendFinger(wrist,finger)) fingerSign |= hand;
        else fingerSign &= (byte)(0xff ^ hand);
    }

    // 指が曲がっていないか識別する関数
    // 引数：手首の位置
    // 引数：指先の位置
    // 戻り値：曲がっていない true 曲がっている false
    private bool isNotBendFinger(Vector3 wrist,Vector3 finger) 
    {
        Vector3 distance = wrist - finger;
        float length = distance.magnitude;
        float MaxDist = 20;
        // 指先と手首の距離が近いか
         return  length > MaxDist;
    }

    // 風の生成イベントの発行関数
    // 引数：右手か左手か
    // 引数：風の向きと速度
    // 戻り値：なし
    private void CreateWind(int handNum,Vector3 windVec) 
    {
        // 風生成位置
        Vector3 position = m_HandLandmark[0][5].transform.position;
        // 風生成イベントの発行
        OnCreatWinds(position, windVec);
    }

    // 移動距離リストを取得する関数
    // 引数：右手か左手か
    // 戻り値：移動距離のリスト
    private List<Vector3>GetMoveVecList(int handNum) 
    {
        if (handNum == 0) return m_vec3LeftMoveDistanceList;
        else return m_vec3RightMoveDistanceList;
    }

}