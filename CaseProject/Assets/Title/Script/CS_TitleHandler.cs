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
    [SerializeField, Header("���̃V�[���̖��O")]
    private string m_nextSceneName;
    private List<Vector3> m_Directions = new List<Vector3>();
    private List<float> m_Time = new List<float>();
    
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

    [SerializeField] private TITLE_STATE m_titleState = TITLE_STATE.SET_HANDS;

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
        CS_HandSigns.OnCreateWinds += Swing;       
    }

    

    // Update is called once per frame
    void Update()
    {
        CheckGoNextScene();//���̃V�[���ւ������ǂ����̏���
        
        // ���X�g�̍X�V
        TimeOverRemoveList();    
        
        // �V���E�X���Ă�
        if (IsCallSerius()) m_titleState = TITLE_STATE.BORN_SERIUS;
                
    }
    // OnDestroy is called this object is Destroyed
    private void OnDestroy(){
        CS_HandSigns.OnCreateWinds -= Swing;
    }
    
    // ����X�E�B���O�������̈ʒu����ۑ�����֐�
    // �������F�ʒu���
    // �������F�ړ�����
    // �߂�l�F�Ȃ�
    void Swing(Vector3 position, Vector3 direction){
        // �Z�b�g�n���h�ȊO�Ȃ甲����
        if (m_titleState != TITLE_STATE.SET_HANDS) return;
        m_Directions.Add(direction);
        m_Time.Add(Time.time);
    }

    // �V���E�X���ĂԔ��������֐�
    // �������G�Ȃ�
    // �߂�l�F�V���E�X���Ă�True
    bool IsCallSerius() 
    {
        for (int i = 0; i < m_Directions.Count-1; i++) 
        {
            float dot = Vector3.Dot(m_Directions[i], m_Directions[i + 1]);
            // ���̌��������΂Ȃ�True
            if (dot < 0) return true;
        }
        
        return false;
    }
    // �K�莞�Ԃ𒴂����烊�X�g����r������֐�
    // �����F�Ȃ�
    // �߂�l�F�Ȃ�
    void TimeOverRemoveList() 
    {
        // ���X�g���Ȃ��Ȃ甲����
        if (m_Time.Count <= 0) return;
        // ���Ԃ𒴂�����
        float diff = Time.time - m_Time[0];
        const float RegulationTime = 1.0f;
        bool isTimeOver =diff > RegulationTime;
        // �K�莞�Ԃ𒴂����烊�X�g����r��
        if (isTimeOver) 
        {
            m_Directions.RemoveAt(0);
            m_Time.RemoveAt(0);
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

        //���Ԃ̕ۑ�
        ObjectData.m_fBGMTime = ObjectData.m_csSoundData.BGMTIME;
        SceneManager.LoadScene("SelectScene");  
    }

    //�Q�[���I��
    public void GameEnd()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
