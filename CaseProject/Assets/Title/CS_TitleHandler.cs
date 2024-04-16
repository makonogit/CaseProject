//------------------------------
// �S���ҁF�����@����
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;


public class CS_TitleHandler : MonoBehaviour
{
    [SerializeField, Header("�n���h�T�C��")]
    private CS_HandSigns m_handSigns;

    private HandLandmarkListAnnotation[] m_HandLandmark = new HandLandmarkListAnnotation[2];

    [SerializeField, Header("GAME,END���S")]
    [Header("0GAME")]
    [Header("1END")]
    private GameObject[] m_txLogos;

    [SerializeField, Header("���̃V�[���̖��O")]
    private string m_nextSceneName;

    public enum TITLE_STATE
    {
        CLOUD_EXCLUSION,//�_�r��
        SELECT_NEXT_SCENE,//���̃V�[���I��
        GO_GAME_SCENE,
        GAME_END
    }

    private TITLE_STATE m_titleState = TITLE_STATE.CLOUD_EXCLUSION;

    public TITLE_STATE TitleState
    {
        set
        {
            m_titleState = value;
        }
        get
        {
            return m_titleState;
        }
    }

    private bool m_isChangeSceneInpossible = false;
    public bool IsChangeSceneImpossible
    {
        get
        {
            return m_isChangeSceneInpossible;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_HandLandmark = m_handSigns.HandMark;
    }

    // Update is called once per frame
    void Update()
    {
        //�_���ǂ����ԂȂ�I��
        if(TitleState == TITLE_STATE.CLOUD_EXCLUSION) { return; }

        //�I����ԂȂ�A�v�������
        if(TitleState == TITLE_STATE.GAME_END) { UnityEditor.EditorApplication.isPlaying = false; }
        //GO_GAME_SCENE��ԂȂ玟�̃V�[����
        if (TitleState == TITLE_STATE.GO_GAME_SCENE) { SceneManager.LoadScene(m_nextSceneName); }

        //�n���h�}�[�N���擾
        m_HandLandmark = m_handSigns.HandMark;
        
        if (m_HandLandmark[0] != null) 
        {
            bool rockSign = m_handSigns.GetHandPose(0) == (byte)CS_HandSigns.HandPose.RockSign;
            if (m_handSigns.GetHandPose(0)==(byte)CS_HandSigns.HandPose.RockSign)
            {
                m_isChangeSceneInpossible = true;//�V�[���J�ډ\�ɂ���
            }
            else 
            {
                m_isChangeSceneInpossible = false; //�V�[���J�ڕs�\�ɂ���
            }
        }
        else if (m_HandLandmark[1] != null)
        {
            //GetHandPose(handNum)==(byte)HandPose.PaperSign
            if (m_handSigns.GetHandPose(1) == (byte)CS_HandSigns.HandPose.RockSign)
            {
                m_isChangeSceneInpossible = true; //�V�[���J�ډ\�ɂ���
            }
            else
            {
                m_isChangeSceneInpossible = false;//�V�[���J�ڕs�\�ɂ���
            }
        }
    }

    public void LogoActiveTrue()
    {
        //���S�̊�����true
        m_txLogos[0].SetActive(true);
        m_txLogos[1].SetActive(true);

        TitleState = TITLE_STATE.SELECT_NEXT_SCENE;//�^�C�g����Ԃ�I���\��Ԃɂ���
    }

    //�V�[���̃��[�h
    public void GoNextScene(string _nextSceneName)
    {
        SceneManager.LoadScene(_nextSceneName);
    }

    //�Q�[���I��
    public void GameEnd()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
