//-----------------------------------------------
//担当者：中島愛音
//爆発の処理：爆発のエフェクトオブジェクトにアタッチ
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ExplosionEffect : MonoBehaviour
{
    [Header("爆発のエフェクトオブジェクトにアタッチ")]
    [Header("シリウスの持っている星の子オブジェクトにExplosionEffectPrefabを持たせる")]
    [SerializeField, Header("爆発アニメーションのトリガー名")]
    private string m_triggerName;

    [SerializeField, Header("爆発したときの減速度(0.0～1.0)")]
    private float m_fDeceleration = 1.0f;

    [SerializeField, Header("シリウス本体のRigidBody2D")]
    private Rigidbody2D m_rb;

    private bool isExplotion = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isExplotion) { return; }

        Animator animator = GetComponent<Animator>();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("AC_Explosion")) { return; }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //アニメーションが終了した？
        if (stateInfo.normalizedTime >= 0.9f)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
     
   
    public void StartExplosion()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Animator>().SetTrigger(m_triggerName);//アニメーション再生
        isExplotion = true;
        //減速させる
        Vector3 playerVel = m_rb.velocity;
        playerVel *= m_fDeceleration;
        m_rb.velocity = playerVel;
        Debug.Log("爆発開始");
    }

   
}
