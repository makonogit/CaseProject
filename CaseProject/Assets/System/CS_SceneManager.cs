//-----------------------------------------------
//�S���ҁF�����S
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_SceneManager : MonoBehaviour
{
    //�V�[���p�萔
    public enum SCENE
    {
        TITLE,  //�^�C�g��
        GAME,   //�Q�[��
        POSE,   //�|�[�Y
        RESULT  //���U���g
    }
    
    [SerializeField,Header("NowLoading�I�u�W�F�N�g")]
    private GameObject m_LoadingScreen; //NowLoading�\���p

    //�N���A�󋵊Ǘ��p
    private Dictionary<int, bool> StageClearData = new Dictionary<int, bool>();

    //���[�h��ҋ@���鎞��(�����i�K�ł̓��[�h����u�̈�)
    [SerializeField,Header("���[�f�B���O�ҋ@����")]
    private float m_fLoadWaitTime = 1000.0f;  
    private float m_fLoadTime = 0.0f;
   
    //�V�[���ǂݍ��݊֐�
    //�����FScene�萔
    private IEnumerator LoadScene(SCENE _scene)
    {

        if (!m_LoadingScreen)
        {
            Debug.LogWarning("���[�f�B���O�X�N���[��������܂���");
        }

        //NowLoading���\��
        m_LoadingScreen.SetActive(false);

        //�񓯊��ǂݍ��݊J�n
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)_scene);

        //�ǂݍ��݊����܂őҋ@(���[�h�ҋ@���Ԍo�߂�҂�)
        while(!asyncLoad.isDone && m_fLoadTime > m_fLoadWaitTime)
        {
            //���[�h����
            m_fLoadTime += Time.deltaTime;

            //���[�f�B���O��ʂ�\��
            m_LoadingScreen.SetActive(true);

            //�V�[���̓ǂݍ��ݏ󋵂�\��
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("�V�[���̓ǂݍ��ݐi�s��: " + (progress * 100) + "%");

            //�ǂݍ��ݎ��s����
            if (asyncLoad.progress < 0.9f && !asyncLoad.allowSceneActivation)
            {
                Debug.LogError("�V�[�����ǂݍ��߂܂���ł����I");
                break;
            }

            yield return null;
        }

        //���[�f�B���O��ʂ̔�\��
        m_LoadingScreen.SetActive(false);

        //�ǂݍ��݊���
        if (asyncLoad.isDone)
        {
            Debug.Log("�V�[���̓ǂݍ��݂��������܂����B");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //void SaveStageClearData(int StageNum,bool isClear)
    //{
    //    StageClearData[StageNum] = isClear;
    //    SaveData("StageClearData", StageClearData);
    //}


    //�V�[���ǂݍ��݊֐�
    //�����F�ǂݍ��݃f�[�^��,�f�[�^
    public void SaveData<T>(string key,T data)
    {
        string Data = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, Data);
        PlayerPrefs.Save();
    }


    //�V�[���ǂݍ��݊֐�
    //�����F�ǂݍ��݃f�[�^��
    //�߂�l�F�f�[�^
    public T LoadData<T>(string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            string Data = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(Data);
        }
        else
        {
            Debug.LogWarning("�w�肳�ꂽ�f�[�^�͕ۑ�����Ă��܂���");
            return default(T);
        }
    }

}
