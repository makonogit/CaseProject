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



    [SerializeField, Header("本のTransForm")]
    private Transform m_tBockTrans;

    [SerializeField, Header("本の移動速度")]
    private float m_fMoveSpeed = 1.0f;
    [SerializeField, Header("本の最大移動量")]
    private float m_fMaxMove = 2.0f;

    private float m_fMove = 0.0f;   //本の移動量

    [SerializeField, Header("TitleLogoのSpriteRenderer")]
    private SpriteRenderer m_TitileRenderer;

    private float m_fTitleLogoAlpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_TitileRenderer) { Debug.LogWarning("TitleLogoのSpriteRendererが設定されていません"); }

        m_fTitleLogoAlpha = m_TitileRenderer.color.a;

        m_sRenderer = GetComponent<SpriteRenderer>();
        
        m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, 0f); // 最初は透明にする

        if (!m_tBockTrans) { Debug.LogWarning("本のTransFormが設定されていません"); }

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
            //最大移動量に達するまで横移動(本を開くモーションの時にずらす)
            if(m_fMove > m_fMaxMove) 
            {
                return; 
            }
            
            //Destroy(this);
        }






    }
}
