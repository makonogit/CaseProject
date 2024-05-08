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



    [SerializeField, Header("�{��TransForm")]
    private Transform m_tBockTrans;

    [SerializeField, Header("�{�̈ړ����x")]
    private float m_fMoveSpeed = 1.0f;
    [SerializeField, Header("�{�̍ő�ړ���")]
    private float m_fMaxMove = 2.0f;

    private float m_fMove = 0.0f;   //�{�̈ړ���

    [SerializeField, Header("TitleLogo��SpriteRenderer")]
    private SpriteRenderer m_TitileRenderer;

    private float m_fTitleLogoAlpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_TitileRenderer) { Debug.LogWarning("TitleLogo��SpriteRenderer���ݒ肳��Ă��܂���"); }

        m_fTitleLogoAlpha = m_TitileRenderer.color.a;

        m_sRenderer = GetComponent<SpriteRenderer>();
        
        m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, 0f); // �ŏ��͓����ɂ���

        if (!m_tBockTrans) { Debug.LogWarning("�{��TransForm���ݒ肳��Ă��܂���"); }

    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�g�����S������ (�ǉ��F��)
        if(m_TitileRenderer.color.a > 0.0f)
        {
            m_fTitleLogoAlpha -= m_fadeInDuration / 2 * Time.deltaTime;
            m_TitileRenderer.color = new Color(m_TitileRenderer.color.r, m_TitileRenderer.color.g, m_TitileRenderer.color.b, m_fTitleLogoAlpha);
            return;
        }


        //�{���t�F�[�h�C��������(���l)
        m_fadeTimer += Time.deltaTime;
        if (m_fadeTimer < m_fadeInDuration)
        {
            m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);
            m_sRenderer.color = new Color(m_sRenderer.color.r, m_sRenderer.color.g, m_sRenderer.color.b, m_currentAlpha);
        }
        else
        {
            //�ő�ړ��ʂɒB����܂ŉ��ړ�(�{���J�����[�V�����̎��ɂ��炷)
            if(m_fMove > m_fMaxMove) 
            {
                return; 
            }
            
            //Destroy(this);
        }






    }
}
