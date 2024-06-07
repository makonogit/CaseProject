//-----------------------------------------------
//�S���ҁF�����S
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class CS_SceneManager : MonoBehaviour
{
    //�V�[���p�萔
    public enum SCENE
    {
        TITLE,  //�^�C�g��
        SELECT, //�Z���N�g
        GAME,   //�Q�[��
        POSE,   //�|�[�Y
        RESULT  //���U���g
    }
    
    [SerializeField,Header("NowLoading�I�u�W�F�N�g")]
    private GameObject m_LoadingScreen; //NowLoading�\���p

    [SerializeField, Header("StageData�X�N���v�g")]
    private CS_StageData m_csStagedata;

    [SerializeField, Header("globallight(���]�p)")]
    private Light2D m_lGlobalLight;

    private bool m_IsLightChange = false;   //���]�t���O

    //�N���A�󋵊Ǘ��p
    private Dictionary<int, bool> StageClearData = new Dictionary<int, bool>();

    //���[�h��ҋ@���鎞��(�����i�K�ł̓��[�h����u�̈�)
    [SerializeField,Header("���[�f�B���O�ҋ@����")]
    private float m_fLoadWaitTime = 1000.0f;  
    private float m_fLoadTime = 0.0f;
   
    //�V�[���ǂݍ��݊֐�
    //�����FScene�萔
    public IEnumerator LoadingScene(SCENE _scene)
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
            if(_scene == SCENE.GAME)
            {
                //�o�^���ꂽ�X�e�[�W�I�u�W�F�N�g�𐶐�
                GameObject StageObj = m_csStagedata.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_gStagePrefab;
                if (!StageObj) { Debug.LogWarning("�X�e�[�W�I�u�W�F�N�g���o�^����Ă��܂���"); }
                Instantiate(StageObj);
                //m_csStagegata.
            }

            Debug.Log("�V�[���̓ǂݍ��݂��������܂����B");
        }

    }

    //-------------------------
    //�V�[���ǂݍ��݊֐�
    //�����FScene�萔
    //-------------------------
    public void LoadScene(SCENE _scene)
    {
        SceneManager.LoadScene((int)_scene);
    }


    //-------------------------------------
    //��ԍŏ��Ɏ��s�����֐�
    //�Q�[���V�[���������ꍇ�ɏ���
    //-------------------------------------
    private void Awake()
    {
        int SceneNum = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("World" + StageInfo.World + "Stage" + StageInfo.Stage);

        //�V�[�������C�g�̓ǂݍ���
        ObjectData.m_lGlobalLight = m_lGlobalLight;

        //���݂̃V�[�����Q�[����������X�e�[�W�𐶐�
        if((SCENE)SceneNum == SCENE.GAME)
        {
            //�o�^���ꂽ�X�e�[�W�I�u�W�F�N�g�𐶐�
            GameObject StageObj = m_csStagedata.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_gStagePrefab;
            if (!StageObj) { Debug.LogWarning("�X�e�[�W�I�u�W�F�N�g���o�^����Ă��܂���"); }
            Instantiate(StageObj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //LoadScene(SCENE.GAME);
        //Debug.Log("��݂���");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightChange(float ChangeSpeed,float Maxintensity)
    {
        
        if(m_lGlobalLight.intensity < Maxintensity)
        {
            m_lGlobalLight.intensity += ChangeSpeed * Time.deltaTime;
        }
        else
        {
            //���]�t���O���I��
            m_IsLightChange = true;
        }
        
        //���邳�����Ƃɖ߂�
        if(m_IsLightChange)
        {
            m_lGlobalLight.intensity -= ChangeSpeed * Time.deltaTime;
        }

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
