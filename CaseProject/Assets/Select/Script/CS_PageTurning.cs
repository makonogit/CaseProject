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

  
    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        CS_HandSigns.OnCreateWinds += PageTurning;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Flip()
    {
        // 現在のScaleを取得し、X軸方向に反転させる
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }


    
    //ページめくりのアニメーションを発動
    private void PageTurningAnimation()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("pageTurningAnim")) { return; }
        GetComponent<Animator>().SetTrigger("pageTurningAnim");
    }

    //本をめくる
    void PageTurning(Vector3 _position, Vector3 _direction)
    {
        bool isFlip = (_direction.x < 0.0f && !isFacingRight) || (_direction.x > 0.0f && isFacingRight);

        Debug.Log("左右方向" + _direction.x);

        if (isFlip) { Flip(); }
        PageTurningAnimation();//アニメーション実行
    }
}
