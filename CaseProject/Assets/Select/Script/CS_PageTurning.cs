//------------------------------
// 担当者：中島　愛音
//　ページめくり
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;

public class CS_PageTurning : MonoBehaviour
{
    [SerializeField, Header("ハンドサイン")]
    private CS_HandSigns m_handSigns;

    [SerializeField, Header("ページめくりを行う手の移動量")]
    private float m_handMovement = 5.0f;

    private bool isFacingRight = true;

    private Vector3[] m_midlleF_PrevPos = new Vector3[2];//中指のポジション

    private List<HandLandmarkListAnnotation> m_handLandmark = new List<HandLandmarkListAnnotation>();
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //一度nullで初期化しておく
        
        //ハンドマークを取得
        //1を右手、0を左手とする
        m_handLandmark = m_handSigns.HandMark;

        if (m_handLandmark.Count < 2)
        {
            return;
        }

        //ページめくりが実行中なら終了
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AM_PageTurning")) { return; }

        for (int i = 0; i < 2; i++)
        {
            if(m_handLandmark[i] == null) { return; }
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AM_PageTurning")) { return; }
            //手の移動ベクトル
            Vector3 handVec = m_handSigns.GetHandMovement(i);
            PointListAnnotation point1 = m_handLandmark[i].GetLandmarkList();　//ポイントリストを取得
            //ページのめくりのアニメーション
            bool turning = IsPageTurning(handVec,i);
            if (turning)
            {
                //右手か左手かで本のめくる向きを変える
                if (handVec.x > 0.0f && isFacingRight) { Flip(); }
                else if (handVec.x < 0.0f && !isFacingRight) { Flip(); }
                PageTurningAnimation();//アニメーション実行
            }
            m_midlleF_PrevPos[i] = point1[12].transform.position;//中指の先を保存
        }
        
    }

    private void Flip()
    {
        // 現在のScaleを取得し、X軸方向に反転させる
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }


    private bool IsPageTurning(Vector3 _moveVec, int handNum)
    {   
        //移動距離が一定未満ならfalse
        if (_moveVec.magnitude < m_handMovement) { return false; }

        //-------------手首位置が画面の右側か左側かをとる--------------
        PointListAnnotation point1 = m_handLandmark[handNum].GetLandmarkList();　//ポイントリストを取得
        //手首のポジションをスクリーン座標にする
        Vector3 wristPos = point1[17].transform.position;
        Vector3 screenPos = Camera.main.WorldToViewportPoint(wristPos);
        //スクリーン座標の右側にあるか
        bool handScreenPosRight = screenPos.x > 0.5f;

        //中指先の位置と手首のX座標の関係を取る
        Vector3 middleFingerPos = point1[12].transform.position;
        //スクリーンの右側にある？
        if (handScreenPosRight)
        {
            //左から右に移動したならfalse
            if(IsMoveingRight(m_midlleF_PrevPos[handNum],middleFingerPos,wristPos)) { return false; }
        }
        else //スクリーンの左側
        {
            //右から左に移動したならfalse
            if (IsMoveingLeft(m_midlleF_PrevPos[handNum], middleFingerPos, wristPos)) { return false; }
        }
        //----------------------------------------------------------------

        //パーでないならfalse
        if (m_handSigns.GetHandPose(handNum) != (byte)CS_HandSigns.HandPose.PaperSign) { return false; }

        return true;
    }

    private bool IsMoveingLeft(Vector3 _prevPos, Vector3 _currentPos, Vector3 _wristPos)
    {
        return _prevPos.x > _wristPos.x && _currentPos.x <= _wristPos.x;
    }

    private bool IsMoveingRight(Vector3 _prevPos, Vector3 _currentPos, Vector3 _wristPos)
    {
        return _prevPos.x < _wristPos.x && _currentPos.x >= _wristPos.x;
    }

    //ページめくりのアニメーションを発動
    private void PageTurningAnimation()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("pageTurningAnim")) { return; }
        GetComponent<Animator>().SetTrigger("pageTurningAnim");
    }
}
