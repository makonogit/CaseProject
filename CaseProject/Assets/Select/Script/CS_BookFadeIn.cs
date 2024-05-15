//-----------------------------------------------
//担当者：中島愛音
//本のフェードイン処理
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BookFadeIn : MonoBehaviour
{
    private SpriteRenderer m_sRenderer;

    [SerializeField,Header("フェードインにかかる時間")]
    private float m_fadeInDuration = 2f; // フェードインにかかる時間（秒）
    private float m_currentAlpha = 0f;
    private float m_fadeTimer = 0f;

    [SerializeField, Header("TitleLogoのSpriteRenderer")]
    private SpriteRenderer m_TitileRenderer;

    private float m_fTitleLogoAlpha = 1.0f;

    [SerializeField, Header("turning用本")]
    private GameObject m_turningBook; //フェード後開いた状態の本

    // Start is called before the first frame update
    void Start()
    {
        if (!m_TitileRenderer) { Debug.LogWarning("TitleLogoのSpriteRendererが設定されていません"); }

        m_fTitleLogoAlpha = m_TitileRenderer.color.a;

        m_sRenderer = GetComponent<SpriteRenderer>();
        
        m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, 0f); // 最初は透明にする

       
        if(!m_turningBook) { Debug.LogWarning("ページめくり用の本のGameObjectが設定されていません"); }
        m_turningBook.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //タイトルロゴを消す (追加：菅)
        if(m_TitileRenderer.color.a > 0.0f)
        {
            m_fTitleLogoAlpha -= m_fadeInDuration / 2 * Time.deltaTime;
            m_TitileRenderer.color = new Color(m_TitileRenderer.color.r, m_TitileRenderer.color.g, m_TitileRenderer.color.b, m_fTitleLogoAlpha);
            return;
        }


        //本をフェードインさせる(α値)
        m_fadeTimer += Time.deltaTime;
        if (m_fadeTimer < m_fadeInDuration)
        {
            m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);
            m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, m_currentAlpha);
        }
        else
        {
            //本を開くスクリプトを追加
            CS_OpenBook openBook = this.gameObject.AddComponent<CS_OpenBook>();
            openBook.TurningBook = m_turningBook;
            Destroy(this);
        }






    }
}
