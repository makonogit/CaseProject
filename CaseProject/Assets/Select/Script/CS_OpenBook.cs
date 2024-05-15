//-----------------------------------------------
//担当者：中島愛音
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_OpenBook : MonoBehaviour
{
    public GameObject m_turningBook;//ページめくり用のゲームオブジェクト
    private Animator m_animator;//アニメーター

    public GameObject TurningBook
    {
        set
        {
            m_turningBook = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetTrigger("openBook");//開くアニメーション再生
    }

    private void Update()
    {
       
        // 現在のAnimatorStateInfoを取得
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        // アニメーションが終了したかどうかをチェック
        bool isAnimationFinish = stateInfo.IsName("AC_OpenBook") && stateInfo.normalizedTime >= 0.9f;
        if(!isAnimationFinish) { return; }

        m_animator.speed = 0f;

        // ページをめくる本の活動をtrueに設定
        if (m_turningBook != null)
        {
            m_turningBook.SetActive(true);//ページめくり用のオブジェクトの活動をtrue
            Destroy(this.gameObject);
        }
    
    }
}
