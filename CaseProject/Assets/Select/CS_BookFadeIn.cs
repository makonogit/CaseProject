//-----------------------------------------------
//�S���ҁF��������
//�{�̃t�F�[�h�C������
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BookFadeIn : MonoBehaviour
{
    private SpriteRenderer m_sRenderer;

    [SerializeField,Header("�t�F�[�h�C���ɂ����鎞��")]
    private float m_fadeInDuration = 2f; // �t�F�[�h�C���ɂ����鎞�ԁi�b�j
    private float m_currentAlpha = 0f;
    private float m_fadeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_sRenderer = GetComponent<SpriteRenderer>();
        m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, 0f); // �ŏ��͓����ɂ���
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
