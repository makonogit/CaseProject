using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ResultBookFade : MonoBehaviour
{
    [SerializeField, Header("フェードインにかかる時間")]
    private float m_fadeInDuration = 2f; // フェードインにかかる時間（秒）
    private float m_currentAlpha = 0f;
    private float m_fadeTimer = 0f;
    private CS_ResultController m_rController;

    private SpriteRenderer m_sRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_rController = GameObject.Find("ResultCtrl").GetComponent<CS_ResultController>();
        m_sRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //本のフェードイン状態じゃないなら終了
        if(m_rController.ResultState != CS_ResultController.RESULT_STATE.BOOK_FADE_IN) { return; }

        m_fadeTimer += Time.deltaTime;
        m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);
        if (m_fadeTimer > m_fadeInDuration)
        {
            //セレクトへの遷移状態にして消去
            m_rController.ResultState = CS_ResultController.RESULT_STATE.GO_SELECT_SCENE;
            Destroy(this);
            return;
        }
        Debug.Log("本の不透明渦中");
        Color color = m_sRenderer.color;
        color.a = m_currentAlpha;
        m_sRenderer.color = color;
    }
}
