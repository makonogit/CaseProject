//-----------------------------------------------
//担当者：中島愛音
//上昇星
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_RisingStar : MonoBehaviour
{
    private Rigidbody2D m_rb;//リジッドボディ
    private Vector3 m_prevVelocity;//前のリジッドボディの速度
    private Vector3 m_backUpVelocity;//保存用リジッドボディの速度
    int m_nAccumulateCount = 0;//溜めカウント
    float m_fDescentCount = 0;//下降時のカウント
    bool m_isAccumulate = false;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_prevVelocity = m_rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //下降中かつ溜め中じゃない？
        if (m_rb.velocity.y < 0.0f && !m_isAccumulate)
        {
            m_fDescentCount += 0.5f;

            if(m_fDescentCount >= 100.0f) { m_fDescentCount = 100.0f; }
        }
        //前の速度より現在の速度が大きいなら力を+補正する
        bool isReady = m_prevVelocity.y <= 0.0f && m_rb.velocity.y > 0.0f;

        if(isReady && !m_isAccumulate)
        {
            m_isAccumulate = true;//溜めをtrue
            m_backUpVelocity = m_rb.velocity;//現在の速度を保存
        }
        m_prevVelocity = m_rb.velocity;

        if (!m_isAccumulate) { return; }


        //現在の速度を0にして溜めカウントを加算
        Vector3 velocity = m_rb.velocity;
        Debug.Log("速度" + m_rb.velocity);
        velocity.y = 0.0f;
        m_rb.velocity = velocity;
        m_nAccumulateCount++;

      
        //倍率を設定
        float magnification = 1.0f;
        magnification = (m_fDescentCount / 100f > 0.33f) ? 2f : magnification;
        magnification = (m_fDescentCount / 100f > 0.66f) ? 3f : magnification;

        //溜めカウントが一定以上？
        if (m_nAccumulateCount >= (int)m_fDescentCount)
        {
            m_rb.AddForce(Vector2.up * (1 + (magnification /10f)), ForceMode2D.Impulse);//力を加える
            //カウント初期化
            m_nAccumulateCount = 0;
            m_fDescentCount = 0;
            m_isAccumulate = false;//溜めをfalse
        }
    }
}
