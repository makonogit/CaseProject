using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ResultTextFade : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�C���ɂ����鎞��")]
    private float m_fadeInDuration = 2f; // �t�F�[�h�C���ɂ����鎞�ԁi�b�j
    private float m_currentAlpha = 0f;
    private float m_fadeTimer = 0f;
    [SerializeField, Header("�t�F�[�h�C�����n�܂���")]
    private CS_ResultController.RESULT_STATE m_startFadeState;
    [SerializeField, Header("�t�F�[�h�C�����I����Ă��玟�̏��")]
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

        //�e�L�X�g�̃t�F�[�h�C����Ԃ���Ȃ��Ȃ�I��
        if(m_rController.ResultState != m_startFadeState) { return; }

        m_fadeTimer += Time.deltaTime;

        //�t�F�[�h���I�����Ă���Ȃ�X�V�I��
        if (mFadeFinish)
        {
            bool goNext = IsNext();//���ɍs�����m�F

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

            mFadeFinish = true;//�t�F�[�h�I��
            m_fadeTimer = 0.0f;
            return;
        }

        Color color = m_sRenderer.color;
        color.a = m_currentAlpha;
        m_sRenderer.color = color;
    }

    private bool IsNext()
    {
        //�P�b�o�����Ȃ�true
        if (m_fadeTimer > 1.0f)
        {
            return true;
        }
        return false;
    }
}
