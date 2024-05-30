﻿//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using OpenCvSharp;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using static CS_HandSigns;


public class CS_HandSigns : MonoBehaviour
{
    //両手のデータ
    [SerializeField]
    private List<HandLandmarkListAnnotation> m_HandLandmark = new List<HandLandmarkListAnnotation>();

    public List<HandLandmarkListAnnotation> HandMark
    {
        get
        {
            return m_HandLandmark;
        }
    }
    // 手の移動距離
    [Header("動きリスト")]
    [SerializeField] private List<List<Vector3>> m_vec3MoveDistanceList = new List<List<Vector3>>();
    [SerializeField] private List<List<Vector3>> m_vec3AngularList = new List<List<Vector3>>();
    [SerializeField] private List<bool> m_bIsLeftHandList;
    [SerializeField] private List<Vector3> m_vec3Forward;

    // リストの最大数
    [Header("リストの最大数")]
    [SerializeField] private int m_nListMaxNum = 8;

    [Header("手の速度の判定範囲")]
    [SerializeField]private float m_fMinSpeed = 5.0f;
    [SerializeField]private float m_fMaxSpeed = 100.0f;
    [SerializeField] private float m_fMinAngularSpeed = 20.0f;
    [SerializeField] private float m_fMaxAngularSpeed = 100.0f;
    
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
    public static event EHHandSigns OnCreateWinds;

    // 雷生成イベント
    public static event EHHandSigns OnCreateThunders;

    // 雨生成イベント
    public static event EHHandSigns OnCreateRains;

    // Tポーズイベント
    public static event EHHandSigns OnTPose;
    
  
    //--------------------------

    // Start is called before the first frame update
    private void Start()
    {
        OnCreateWinds += NullEvent;
        OnTPose += NullEvent;
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
            // 子オブジェを持っていない時false
            bool isHaveChild = transform.childCount > 0;
            if (!isHaveChild)return false;

            return true;
        }
    }
   
    // 手の情報を取得する関数
    // 引数：なし
    // 戻り値：なし
    private void TakeHandsStates() 
    {
        Transform parentTransform = transform;
        int i = 0;
        m_HandLandmark.Clear();        
        m_vec3Forward.Clear(); 
        m_bIsLeftHandList.Clear();
        foreach (Transform child in parentTransform) SetLists(child,ref i);
        return;
    }

    // 取得した値を変数に設定する関数
    // 引き数：トランスフォーム
    // 引き数：ループ回数
    // 戻り値：なし
    private void SetLists(Transform child,ref int num) 
    {
        // 子オブジェから手の情報を探す
        HandLandmarkListAnnotation hand = child.GetComponent<HandLandmarkListAnnotation>();
        if (!hand) return;// 取得できなかった

        // 手の情報を設定する
        m_HandLandmark.Add(hand);

        if (m_vec3MoveDistanceList.Count <= num) m_vec3MoveDistanceList.Add(new List<Vector3>());
        if (m_vec3AngularList.Count <= num) m_vec3AngularList.Add(new List<Vector3>());
        bool LeftSide = false;
        // 前方向を設定
        m_vec3Forward.Add(GetForward(num, ref LeftSide));
        m_bIsLeftHandList.Add(LeftSide);

        // リストに追加
        AddHandPointList(m_HandLandmark[num].GetLandmarkList(), num);
        AddHandAngularList(m_HandLandmark[num].GetLandmarkList(), num);
        num++;
    }

    // 手のポイントリストに追加する
    // 引数：設定する手の位置情報
    // 引数：左手か右手か
    // 戻り値：なし
    private void AddHandPointList(PointListAnnotation point,int handNum) 
    {
        List<Vector3> moveVecList = m_vec3MoveDistanceList[handNum];
        // 距離を求める
        Vector3 handPos = point[0].transform.position;

        // リストに追加
        moveVecList.Insert(0, handPos);

        // リストが多くなった分消す
        for (int i = moveVecList.Count - 1; i >= m_nListMaxNum; i--) moveVecList.RemoveAt(i);    
    }

    // 手の角度をリストに追加
    // 引数：設定する手の位置情報
    // 引数：左手か右手か
    // 戻り値：なし
    private void AddHandAngularList(PointListAnnotation point,int handNum) 
    {
        List<Vector3> moveVecList = m_vec3AngularList[handNum];
        
        // 手の方向を求める
        Vector3 handDir = point[9].transform.position;
        handDir -= point[0].transform.position;
        handDir.Normalize();
        
        // リストに追加
        moveVecList.Insert(0, Quaternion.LookRotation(m_vec3Forward[handNum], handDir).eulerAngles);
        // リストが多くなった分消す
        for (int i = moveVecList.Count - 1; i >= m_nListMaxNum; i--) moveVecList.RemoveAt(i);
    }

    // 手のモーションを識別する関数
    // 引数：なし
    // 戻り値：なし
    private void HandsMotionIdentify() 
    {
        for (int i = 0;i< m_HandLandmark.Count;i++)
        {
            // 手の情報が登録されていないなら次に進む
            if (!m_HandLandmark[i]) continue;
                                    
            // リストが少ないなら次に進む
            bool isListUnder = m_vec3MoveDistanceList[i].Count < m_nListMaxNum;
            if (isListUnder) continue;

            // 手の回転速度
            Vector3 vec = GetHandMovement(i);

            // 風の生成
            if (IsCreateWind(i, vec)) CreateWind(i, vec);
        }
        // 両手でTポーズ
        if (IsTPose()) OnTPose(Vector3.zero, Vector3.zero);
    }


    // 手のひらの方向を取得する
    // 引き数：手のリスト番号
    // 戻り値：前方向を返す
    private Vector3 GetForward(int handNum,ref bool LeftSide) 
    {
        //手首の位置情報
        Vector3 wrist = m_HandLandmark[handNum][0].transform.position;
        //親指の付けね
        Vector3 ThumbDir = m_HandLandmark[handNum][1].transform.position;
        ThumbDir -= wrist;
        ThumbDir.Normalize();

        // 人差し指の付け根
        Vector3 indexDir = m_HandLandmark[handNum][5].transform.position;
        indexDir -= wrist;
        indexDir.Normalize();
        // 小指の付け根
        Vector3 pinkyDir = m_HandLandmark[handNum][17].transform.position;
        pinkyDir -= wrist;
        pinkyDir.Normalize();

        Vector3 forward = Vector3.Cross(indexDir,pinkyDir);
        forward = forward.normalized;
        LeftSide = Vector3.Dot(ThumbDir, forward) < -0.0001f;
        if (LeftSide) forward *= -1;
        return forward;
    }

    // 手の動きを取得
    // 引数：右手か左手か
    // 戻り値：動いた距離
    public Vector3 GetHandMovement(int handNum) 
    {
        Vector3 move = new Vector3(0, 0, 0);
        List<Vector3> moveVecList = m_vec3MoveDistanceList[handNum];
        
        // 移動距離の合計
        for (int i = 0; i < moveVecList.Count - 1; i++) move += LimitMovement(moveVecList[i] - moveVecList[i + 1]);
        return move;
    }

    // 異常な移動量を判断する関数
    // 引き数：距離
    // 戻り値：異常ならゼロを返す
    public Vector3 LimitMovement(Vector3 move) 
    {
        const float limit = 10.0f;
        if (move.magnitude > limit) { return Vector3.zero; }
        return move;
    }
    // 手の動きを取得
    // 引数：右手か左手か
    // 戻り値：動いた距離
    public Vector3 GetHandAngularSpeed(int handNum)
    {
        Vector3 move = new Vector3(0, 0, 0);
        List<Vector3> moveVecList = m_vec3AngularList[handNum];
        // 移動距離の合計
        for (int i = 0; i < moveVecList.Count - 1; i++) move += moveVecList[i] - moveVecList[i + 1];
        return move;
    }

    // 手のひらの方向を取得―※出来なかったので親指の付け根の方向を取得
    // 引数：左手か右手か
    // 戻り値：手のひらの方向ベクトル
    public Vector3 GetHandDirection(int handNum) 
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
    // 引数：移動距離
    // 戻り値：生成する true しない false
    private bool IsCreateWind(int handNum,Vector3 move)
    {
        List<Vector3> moveVecList = m_vec3MoveDistanceList[handNum];
        
        // 手がパーか
        bool isPaperSign = GetHandPose(handNum)==(byte)HandPose.PaperSign;
        // 手がパーでないなら_false
        if (!isPaperSign) return false;

        // 回転量を取得
        float Pitch = GetHandAngularSpeed(handNum).x;
        if (!IsHandMovement(handNum,move) && !IsRotate(Pitch)) return false;
                        
        // 移動距離リストのリセット
        moveVecList.Clear();

        return true;
    }
    private bool IsHandMovement(int handNum,Vector3 move) 
    {
        // 手のひらの向きの判定
        float yaw = m_vec3AngularList[handNum][0].y;

        if (!IsPalmFacingSideways(yaw)) { return false; }
        // 動いたか
        if (!IsMoving(move)) { return false; }
        // 手のひらの方向と移動した方向が一緒か
        bool isPositiveX = move.x >= 0;
        bool isPalmDirection = (m_bIsLeftHandList[handNum] && isPositiveX) || ((!m_bIsLeftHandList[handNum] && !isPositiveX));
        if (!isPalmDirection) { return false; }
        return true;
    }
    // 手のひらが横を向いているか
    // 引き数：ヨー
    // 戻り値：向いているならTrue
    private bool IsPalmFacingSideways(float yaw) 
    {
        const float LeftUnder = 60.0f;
        const float LeftTop = 120.0f;
        const float RightUnder = 240.0f;
        const float RightTop = 300.0f;
        bool isRight = yaw > RightUnder && yaw < RightTop;
        bool isLeft = yaw > LeftUnder && yaw < LeftTop;
        return isLeft || isRight;
    }

    // 動いたか
    // 引き数：移動距離
    // 戻り値：動いたなら true
    private bool IsMoving(Vector3 move) 
    {
        // 最低移動距離を越えたら_false
        if (move.magnitude > m_fMaxSpeed) return false; 
        // 最大移動距離を越えなかったら_false
        if (move.magnitude < m_fMinSpeed) return false; 
        return true;
    }
    
    // 回転したか
    // 引き数：回転量
    // 戻り値：一定量回転したなら true
    private bool IsRotate(float move) 
    {
        // 最低移動距離を越えたら_false
        if (move > m_fMaxAngularSpeed ) return false;
        // 最大移動距離を越えなかったら_false
        if (move < m_fMinAngularSpeed ) return false;
        
        return true;
    }

    // 風の生成イベントの発行関数
    // 引数：右手か左手か
    // 引数：風の向きと速度
    // 戻り値：なし
    private void CreateWind(int handNum, Vector3 windVec)
    {
        // 風生成位置
        Vector3 position = m_HandLandmark[handNum][5].transform.position;

        Vector3 offset = m_vec3Forward[handNum];
        const float value = 1.5f;
        offset *= value;
        // 倍率
        const float magnification = 0.1f;
        float Pitch = GetHandAngularSpeed(handNum).x * magnification;
        Vector3 dir = new Vector3(-1, 0, 0) * Mathf.Abs(Pitch);
        if (m_bIsLeftHandList[handNum]) dir*=-1;
        // 風生成イベントの発行
        OnCreateWinds(position + offset, windVec + dir);
    }

    // 両手でTポーズをしているか条件関数
    // 引数：なし
    // 戻り値：ポーズをとっている true
    public bool IsTPose() 
    {
        // 両手の情報があるか,ないなら終わる
        bool isNullOfHandsInfo = m_HandLandmark.Count<2;
        if (isNullOfHandsInfo) return false;
        
        // 両手がパーではないなら
        bool isPaperSign = GetHandPose(0) == (byte)HandPose.PaperSign;
        if (!isPaperSign) return false;
        
        // もう片方も
        isPaperSign = GetHandPose(1) == (byte)HandPose.PaperSign;
        if (!isPaperSign) return false;
        

        // 指先が第一関節に近いか
        float length = 30;
        

        // 中指の先ともう片方の手の中指の第一関節との距離
        Vector3 dis = m_HandLandmark[0][12].transform.position;
        dis -= m_HandLandmark[1][9].transform.position;

        // 中指の先ともう片方の手の中指の第一関節との距離
        Vector3 dis1 = m_HandLandmark[1][12].transform.position;
        dis1 -= m_HandLandmark[0][9].transform.position;

        bool isNaerHandToFinger = dis.magnitude <= length;
        bool isNaerHandToFinger1 = dis1.magnitude <= length;

        if (!isNaerHandToFinger && !isNaerHandToFinger1) return false;

        // 手の方向が垂直関係か

        // 許容範囲
        float tolerance = 0.5f;
        
        // 手の方向（正規化）
        Vector3 handvec = m_HandLandmark[0][12].transform.position;
        handvec -= m_HandLandmark[0][9].transform.position;
        handvec.Normalize();
        
        // 手の方向（正規化）
        Vector3 handvec1 = m_HandLandmark[1][12].transform.position;
        handvec1 -= m_HandLandmark[1][9].transform.position;
        handvec1.Normalize();
        
        // 内積で０に近ければ垂直
        float dot = Vector3.Dot(handvec, handvec1);

        // 手と手の重なった角度が許容範囲ないか
        bool isVertical = dot < tolerance && dot > -tolerance;
        if (!isVertical) return false;
        
        return true;
    }

    // 手のポーズの取得
    // 引数：右手か左手か
    // 戻り値：一ビットずつ指が立っているとtrue
    public byte GetHandPose(int handNum) 
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
        float MaxDist = 1;
        // 指先と手首の距離が近いか
         return  length > MaxDist;
    }

    //押し引き関数
    //引数：手の左右
    //戻り値：-1 判定不可　0:引いた　1:押した
    public int PushHand()
    {
        // 両手の情報があるか,ないなら終わる
        bool isNullOfHandsInfo = !m_HandLandmark[0] || !m_HandLandmark[1];
        if (isNullOfHandsInfo) return -1;

        // 両手がパーではないなら
        bool isPaperSign = GetHandPose(0) == (byte)HandPose.PaperSign;
        if (!isPaperSign) return -1;

        //両手の移動量を取得
        Vector3 Lefthandmove = GetHandMovement(0).normalized;
        Vector3 Righthandmove = GetHandMovement(1).normalized;

        //前に押し出した
        if(Lefthandmove.z > 0.8f && Righthandmove.z > 0.8f) { return 1; }
        //後ろに引いた
        if (Lefthandmove.z < 0 && Righthandmove.z < 0) { return 0; }

        return -1;
    }

    private void NullEvent(Vector3 pos , Vector3 dir)
    {/*Nothing*/}
    
}