using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ResultBookFade : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�C���ɂ����鎞��")]
    private float m_fadeInDuration = 2f; // �t�F�[�h�C���ɂ����鎞�ԁi�b�j
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
        //�{�̃t�F�[�h�C����Ԃ���Ȃ��Ȃ�I��
        if(m_rController.ResultState != CS_ResultController.RESULT_STATE.BOOK_FADE_IN) { return; }

        m_fadeTimer += Time.deltaTime;
        m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);
        if (m_fadeTimer > m_fadeInDuration)
        {
            //�Z���N�g�ւ̑J�ڏ�Ԃɂ��ď���
            m_rController.ResultState = CS_ResultController.RESULT_STATE.GO_SELECT_SCENE;
            Destroy(this);
            return;
        }
        Debug.Log("�{�̕s�����Q��");
        Color color = m_sRenderer.color;
        color.a = m_currentAlpha;
        m_sRenderer.color = color;
    }
}
