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

    // Start is called before the first frame update
    void Start()
    {
        m_sRenderer = GetComponent<SpriteRenderer>();
        m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, 0f); // 最初は透明にする
    }

    // Update is called once per frame
    void Update()
    {
        m_fadeTimer += Time.deltaTime;
        if (m_fadeTimer < m_fadeInDuration)
        {
            m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);
            m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, m_currentAlpha);
        }
        else
        {
            Destroy(this);
        }
    }
}
