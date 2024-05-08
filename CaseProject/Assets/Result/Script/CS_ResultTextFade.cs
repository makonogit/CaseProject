using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ResultTextFade : MonoBehaviour
{
    [SerializeField, Header("フェードインにかかる時間")]
    private float m_fadeInDuration = 2f; // フェードインにかかる時間（秒）
    private float m_currentAlpha = 0f;
    private float m_fadeTimer = 0f;
    [SerializeField, Header("フェードインが始まる状態")]
    private CS_ResultController.RESULT_STATE m_startFadeState;
    [SerializeField, Header("フェードインが終わってから次の状態")]
    private CS_ResultController.RESULT_STATE m_finishFadeState;
    private CS_ResultController m_rController;

    private SpriteRenderer m_sRenderer;

    bool mFadeFinish = false;
    // Start is called before the first frame update
    void Start()
    { 
        m_rController = GameObject.Find("ResultCtrl").GetComponent<CS_ResultController>();
        m_sRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //テキストのフェードイン状態じゃないなら終了
        if(m_rController.ResultState != m_startFadeState) { return; }

        m_fadeTimer += Time.deltaTime;

        //フェードが終了しているなら更新終了
        if (mFadeFinish)
        {
            bool goNext = IsNext();//次に行くか確認

            if (goNext)
            {
                m_rController.ResultState = m_finishFadeState;
                Destroy(this.gameObject);
            }
            return;  
        }

        m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);
        if (m_fadeTimer > m_fadeInDuration)
        {
            if (m_finishFadeState == CS_ResultController.RESULT_STATE.NONE) return;

            mFadeFinish = true;//フェード終了
            m_fadeTimer = 0.0f;
            return;
        }

        Color color = m_sRenderer.color;
        color.a = m_currentAlpha;
        m_sRenderer.color = color;
    }

    private bool IsNext()
    {
        //１秒経ったならtrue
        if (m_fadeTimer > 1.0f)
        {
            return true;
        }
        return false;
    }
}
