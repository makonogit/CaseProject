using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_ResultController : MonoBehaviour
{
    static private bool m_gameOverFg = false;

    [SerializeField, Header("GAMEOVER��CLEAR�̃e�L�X�g�I�u�W�F")]
    [Header("0:GAMEOVER 1:CLEAR")]
    private GameObject[] m_texts;

    [SerializeField, Header("Scene")]
    static private string m_sceneName;

    [SerializeField, Header("�������Ă���̑ҋ@����")]
    private float m_fWaitTime = 3.0f;

    private float m_fNowTime = 0.0f;

    public enum STAGE_TYPE
    {
        STAGE1 = 0,
    };

    public enum RESULT_STATE
    {
       
        MOVESTAR_SERIUS,  //���̈ړ�
        BORN_LINE,  //���C���̏o��
        FRAME,      //�g�o��
        TEXT_FADE_IN,//���������̕����̃I�u�W�F�N�g���t�F�[�h�C��
        NIGHT_FADE_IN,//��Z��̕����̉摜�̃I�u�W�F�N�g���t�F�[�h�C��
        BOOK_FADE_IN,//�{�̃t�F�[�h�C��
        GO_SELECT_SCENE,
        NONE//���ɂȂ�
    };

    private static STAGE_TYPE m_stageType = STAGE_TYPE.STAGE1;//�X�e�[�W�̎��

    private RESULT_STATE m_resultState = RESULT_STATE.MOVESTAR_SERIUS;//���U���g�̏��

    //���U���g��Ԃ̃Z�b�^�[�Q�b�^�[
    public RESULT_STATE ResultState
    {
        set
        {
            m_resultState = value;
        }
        get
        {
            return m_resultState;
        }
    }
    public STAGE_TYPE StageType
    {
        get
        {
            return m_stageType;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        switch(ResultState)
        {
            case RESULT_STATE.GO_SELECT_SCENE:
                m_fNowTime += Time.deltaTime;
                if(m_fNowTime >= m_fWaitTime)
                {
                    SceneManager.LoadScene("SelectScene");
                }
                break;
        }
    }

    static public bool GameOverFlag
    {
        set
        {
            m_gameOverFg = value;
        }
        get
        {
            return m_gameOverFg;
        }
    }

    //���U���g�ɍs���֐�
    //�����F�Q�[���I�[�o�[���A���݃V�[���̖��O
    static public void GoResult(bool _gameOver, string _sceneName)
    {
        m_gameOverFg = _gameOver;
        m_sceneName = _sceneName;
        SceneManager.LoadScene("ResultScene");
    }

    public void OtherScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void Continue()
    {
        SceneManager.LoadScene(m_sceneName);
    }
}