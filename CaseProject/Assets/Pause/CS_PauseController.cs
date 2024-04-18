//------------------------------
// �S���ҁF�����@����
// �|�[�Y
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_PauseController : MonoBehaviour
{
    [SerializeField, Header("�|�[�Y��CanvasPrefab")]
    private GameObject m_pauseCanvasPrefab;

    [SerializeField, Header("�n���h�T�C��")]
    private CS_HandSigns m_handSigns;

    private HandLandmarkListAnnotation[] m_HandLandmark = new HandLandmarkListAnnotation[2];

    private GameObject m_pauseScreen;

    private static bool m_isPause = false;

    void Start()
    {
        m_isPause = false;
    }

    private void Update()
    {
        if (isPause()){ return; }

        //�肪T�̃|�[�Y�Ȃ�Pause()���Ă�
        //�n���h�}�[�N���擾
        m_HandLandmark = m_handSigns.HandMark;

        //null�Ȃ�return
        if (m_HandLandmark[0] == null) { return; }

        //T�|�[�Y����Ȃ��Ȃ�I��
        if (!m_handSigns.IsTPose()) { return; }

        Pause();
    }

    //�|�[�Y�֐�
    public void Pause()
    {
        //���łɃ|�[�Y���Ȃ�I��
        if (m_isPause) { return; }

        Time.timeScale = 0f;
        m_isPause = true;

        //�|�[�Y��ʐ���
        CreatePauseCanvas();
    }

    //�|�[�Y��ʐ����֐�
    private void CreatePauseCanvas()
    {
        if(m_pauseCanvasPrefab != null)
        {
            //�|�[�Y��ʍ쐬
           m_pauseScreen = Instantiate(m_pauseCanvasPrefab);
           m_pauseScreen.GetComponent<Canvas>().worldCamera = Camera.main;
            //�|�[�Y�T�C���ݒ�
            FindText(m_pauseScreen.transform);
        }
        else
        {
            Debug.LogError("�|�[�Y��Canvas�v���n�u���w�肳��Ă��܂���");
        }
    }


    private void FindText(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // �q�I�u�W�F�N�g�̖��O�� "Point Annotation(Clone)" �ł��邩�ǂ������m�F
            if (child.GetComponent<CS_PauseEvent>() != null)
            {
                child.GetComponent<CS_PauseEvent>().SetHandSignData(m_handSigns);
            }

            // �q�I�u�W�F�N�g�����̎q�������Ă���ꍇ�A�ċA�I�ɂ����̎q�I�u�W�F�N�g������
            if (child.childCount > 0)
            {
                FindText(child);
            }
        }
    }

    //�|�[�Y�֐����g�����ɂ��ւ�炸�~�܂�Ȃ��Ƃ��Ɏg�p
    public static bool isPause()
    {
        return m_isPause;
    }

    //�ĊJ
    public void Restart()
    {
        Time.timeScale = 1f;
        m_isPause = false;

        //�|�[�Y��ʂ�����
        Destroy(m_pauseScreen);
    }

    //���Ȃ���
    public void TryAgain()
    {
        Time.timeScale = 1f;
        //���݂̃V�[�����Ăяo��
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   


}
