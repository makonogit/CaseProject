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

    //private List<HandLandmarkListAnnotation> m_handLandmark = new List<HandLandmarkListAnnotation>();

    [SerializeField, Header("���̃V�[���̖��O")]
    private string m_nextSceneName;

    private Vector3[] m_v3HandDir = new Vector3[2];    //��̌���
    private Vector3[] m_v3HandMove = new Vector3[2];   //��̓���

    //[SerializeField, Header("����̏����ʒu")]
    //[Header("0:�E��̏����ʒu")]
    //[Header("1:����̏����ʒu")]
    //private GameObject[] m_startHandsObjPosition = new GameObject[2];
    //private Vector3[] m_startHandsScreenPosition = new Vector3[2];

    //[SerializeField, Header("�V���E�X���o�Ă��闼��̈ʒu")]
    //[Header("0:�E��̏����ʒu")]
    //[Header("1:����̏����ʒu")]
    //private GameObject[] m_bornSeriusObjPosition = new GameObject[2];


    //[SerializeField, Header("����̔z�u�̔F���\�͈�")]
    //private float m_recognizableDistance = 2.0f;

    bool m_isUpdate = true;

    //��Ԃ��ҋ@��2�̑ҋ@����
    private float m_nowWaitTime = 0.0f;
    private float m_waitTime = 1.0f;

    public enum TITLE_STATE
    {
        SET_HANDS,  //������������ʒu�ɃZ�b�g�ł��Ă��邩
        CALL_SERIUS,//�V���E�X���Ă�
        BORN_SERIUS,//�V���E�X�̓o��
        WAIT1,      //�ҋ@1
        SCROLL,     //��ʃX�N���[����
        STOP,       //�X�N���[���I��
        MAGNIFICATION_SERIUS,//�g��V���E�X
        REDUCTION_SERIUS,//�k���V���E�X
        WAIT2,      //�ҋ@2
        GAME_END
    }

    private TITLE_STATE m_titleState = TITLE_STATE.SET_HANDS;

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
        //m_handLandmark = m_handSigns.HandMark;


        //for(int i =0; i < 2; i++)
        //{
        //    Vector3 worldPos = m_startHandsObjPosition[i].transform.position;
        //    m_startHandsScreenPosition[i] = worldPos;
        //}

        if (!m_handSigns) { Debug.LogWarning("CS_HandSigns.cs���A�^�b�`����Ă��܂���"); }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGoNextScene();//���̃V�[���ւ������ǂ����̏���

        //�X�V���Ȃ��Ȃ�I��
        if (!m_isUpdate) { return; }

        //����̏����擾
        for (int i = 0; i < 2; i++) 
        {
            //���肪�p�[����Ȃ�������I��
            bool is_handpose = m_handSigns.GetHandPose(i) == (byte)CS_HandSigns.HandPose.PaperSign;
            if (!is_handpose) { return; }

            m_v3HandDir[i] = m_handSigns.GetHandDirection(i);
            m_v3HandMove[i] = m_handSigns.GetHandMovement(i);

        }

        switch (m_titleState)
        {
            case TITLE_STATE.SET_HANDS:
                m_titleState = TITLE_STATE.CALL_SERIUS;
                break;
            case TITLE_STATE.CALL_SERIUS:
                m_isUpdate = false;
                m_titleState = TITLE_STATE.BORN_SERIUS;
                break;
        }

        //Debug.Log("Dir0" + m_v3HandDir[0] + "Dir1" + m_v3HandDir[1]);
        //Debug.Log("Move0" + m_v3HandMove[0] + "Move1" + m_v3HandMove[1]);

        {
            ////�n���h�}�[�N���擾
            ////0���E��A�P������Ƃ���
            //m_handLandmark = m_handSigns.HandMark;

            //if (m_handLandmark.Count < 2)
            //{
            //    return;
            //}


            //string[] hand = { "�E��", "����" };
            //PointListAnnotation point1 = m_handLandmark[0].GetLandmarkList();
            //PointListAnnotation point2 = m_handLandmark[1].GetLandmarkList();
            ////�E�肪���葤�ɂ���Ȃ�handLandmark�̒��g�����ւ���
            //if(point1[9].transform.position.x < point2[9].transform.position.x)
            //{
            //    HandLandmarkListAnnotation mark = m_handLandmark[0];
            //    m_handLandmark[0] = m_handLandmark[1];
            //    m_handLandmark[1] = mark;
            //}

            //for (int i = 0; i < 2; i++)
            //{
            //    PointListAnnotation point = m_handLandmark[i].GetLandmarkList();

            //    float dis = Vector2.Distance(point[9].transform.position, m_startHandsScreenPosition[i]);

            //    if (dis > m_recognizableDistance) { return; }
            //}

            //    switch (m_titleState)
            //    {
            //        case TITLE_STATE.SET_HANDS:
            //            for (int i = 0; i < 2; i++)
            //            {
            //                m_startHandsObjPosition[i].transform.position = m_bornSeriusObjPosition[i].transform.position;
            //                m_startHandsScreenPosition[i] = m_startHandsObjPosition[i].transform.position;
            //            }
            //            m_titleState = TITLE_STATE.CALL_SERIUS;
            //            break;
            //        case TITLE_STATE.CALL_SERIUS:
            //            for (int i = 0; i < 2; i++)
            //            {
            //                Destroy(m_startHandsObjPosition[i]);
            //                Destroy(m_bornSeriusObjPosition[i]);
            //            }
            //            m_isUpdate = false;
            //            m_titleState = TITLE_STATE.BORN_SERIUS;
            //            break;
            //    }
        }
    }

 
    //�V�[���̃��[�h
    void CheckGoNextScene()
    {
        //�ҋ@����2?
        if (TitleState != TITLE_STATE.WAIT2) { return; }

        if (m_nowWaitTime <= m_waitTime)
        {
            m_nowWaitTime += Time.deltaTime;//�f���^�^�C�����Z
            return;
        }
        SceneManager.LoadScene("SelectScene");  
    }

    //�Q�[���I��
    public void GameEnd()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
