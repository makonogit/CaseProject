using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LineController : MonoBehaviour
{
    [SerializeField, Header("フェードインにかかる時間")]
    private float m_fadeInDuration = 2f; // フェードインにかかる時間（秒）
    private float m_currentAlpha = 0f;
    private float m_fadeTimer = 0f;
    private CS_ResultController m_rController;
   

    private SpriteRenderer[] m_childRenderers; // 子オブジェクトのRendererコンポーネント

    void Start()
    {
        // 子オブジェクトのRendererコンポーネントを取得
        m_childRenderers = GetComponentsInChildren<SpriteRenderer>();

        Debug.Log("カウント数" + m_childRenderers.Length);

        // 全ての子オブジェクトの透明度を0（完全透明）に設定
        foreach (SpriteRenderer renderer in m_childRenderers)
        {
            SetTransparency(renderer, 0f);
        }
    }

    void Update()
    {
        bool allOpaque = true;//全て不透明
        m_fadeTimer += Time.deltaTime;
        m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);

        if (m_fadeTimer > m_fadeInDuration)
        {
            Destroy(this);
            return;
        }
        // 全ての子オブジェクトの透明度を徐々に不透明にしていく
        foreach (SpriteRenderer renderer in m_childRenderers)
        {

            SetTransparency(renderer, m_currentAlpha);
        }

        if (allOpaque) 
        { 
            //ステートを変えて消去
        }
    }

    // 透明度を設定するヘルパー関数
    private void SetTransparency(SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    //リザルトコントローラーのセット
    //引数：CS_ResultController型
    public void SetResControlloer(CS_ResultController _resCtrl)
    {
        m_rController = _resCtrl;
    }
}
