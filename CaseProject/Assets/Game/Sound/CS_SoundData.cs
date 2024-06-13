using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SoundData : MonoBehaviour
{
    [SerializeField, Header("BGM�pAudioSource")]
    private AudioSource m_BGMAudioSource;

    [SerializeField, Header("SE�pAudioSource")]
    private AudioSource m_SEAudioSource;

    [SerializeField, Header("CS_TimeLimit ���������Ԃɂ���ăs�b�`���グ���")]
    private CS_TimeLimit m_cstimelimit;

    [SerializeField, Header("�s�b�`���グ�鑬�x")]
    private float m_fPitchUpSpeed = 1.0f;

    [SerializeField, Header("�s�b�`�̍ő呬�x")]
    private float m_fMaxPitchSpeed = 1.5f;

    [SerializeField, Header("�s�b�`���グ�n�߂鎞�Ԃ̊��� ��0~1")]
    private float m_fPitchUpTime = 0.7f;


    [SerializeField, Header("BGMList")]
    private List<SoundData> m_BGMList = new List<SoundData>();

    [SerializeField, Header("SEList")]
    private List<SoundData> m_SEList = new List<SoundData>();


    private bool m_isGameOverChangeTrigger = false;  //�Q�[���I�[�o�[���̃T�E���h�ύX�g���K�[ 

    // Start is called before the first frame update
    void Start()
    {
        ObjectData.m_csSoundData = this;

       // DontDestroyOnLoad(transform.gameObject);  // ����GameObject���V�[���J�ڒ��ɔj�����Ȃ�
    }

    // Update is called once per frame
    void Update()
    {
        if (m_cstimelimit)
        {
            //���Ԃ��M���M���ɂȂ��Ă����珙�X�ɉ����グ��
            if (m_cstimelimit.GetTimeLimitRatio > m_fPitchUpTime && m_BGMAudioSource.pitch < m_fMaxPitchSpeed)
            {
                m_BGMAudioSource.pitch += m_fPitchUpSpeed * Time.deltaTime;
            }

            //�������Ԍo�߂�����Q�[���I�[�o�[
            if (m_cstimelimit.GetTimeLimitRatio == 1.0f && m_BGMAudioSource.pitch > 1.1f) { PlayBGM("GameOver"); }
        }

    }

    //BGM�ύX�֐�
    //����:Audioclip
    //�߂�l:True ���� False ���s
    //private bool ChangeBGM(AudioClip audio)
    //{
    //    if(audio == null) { return false; }

    //    m_BGMAudioSource.Stop();
    //    m_BGMAudioSource.pitch = 1.0f;
    //    m_BGMAudioSource.clip = audio;
    //    m_BGMAudioSource.Play();

    //    return true;
    //}


    //SE�Đ��p�֐�
    //�����FSE�̓o�^��
    public void PlaySE(string sename)
    {

        //�������O��SE������΍Đ�
        foreach (var se in m_SEList)
        {
            string name = se.m_SoundName;
            if(name == sename) 
            {
                //���������Đ�����������Đ����Ȃ�
                if(m_SEAudioSource.isPlaying && m_SEAudioSource.clip == se.m_AudioClip)
                { return; }
                //m_SEAudioSource.clip = se.m_AudioClip;
                m_SEAudioSource.volume = se.m_fVolume;
                m_SEAudioSource.PlayOneShot(se.m_AudioClip);
                return;
            }
        }

        return;
    }

    //BGM�Đ��p�֐�
    //�����FSE�̓o�^��
    public void PlayBGM(string bgmname)
    {

        //�������O��SE������΍Đ�
        foreach (var bgm in m_BGMList)
        {
            string name = bgm.m_SoundName;
            if (name == bgmname)
            {
                m_BGMAudioSource.Stop();
                m_BGMAudioSource.pitch = 1.0f;
                m_BGMAudioSource.volume = bgm.m_fVolume;
                m_BGMAudioSource.clip = bgm.m_AudioClip;
                m_BGMAudioSource.Play();
                return;
            }
        }

        return;
    }

    //BGM�Đ��p�֐�
    //�����FSE�̓o�^��,�Đ��ʒu
    public void PlayBGM(string bgmname,float time)
    {

        //�������O��SE������΍Đ�
        foreach (var bgm in m_BGMList)
        {
            string name = bgm.m_SoundName;
            if (name == bgmname)
            {
                m_BGMAudioSource.Stop();
                m_BGMAudioSource.time = time;
                m_BGMAudioSource.pitch = 1.0f;
                m_BGMAudioSource.volume = bgm.m_fVolume;
                m_BGMAudioSource.clip = bgm.m_AudioClip;
                m_BGMAudioSource.Play();
                return;
            }
        }

        return;
    }

    //BGM��~�֐�
    public void StopBGM()
    {
        m_BGMAudioSource.Stop();
    }

    //BGM�̍Đ��ʒu���擾����
    public float BGMTIME
    {
        get
        {
            return m_BGMAudioSource.time;
        }
    }


}

//�T�E���h�f�[�^��ێ�����N���X
//�Q�Ƃ��₷���悤�ɃL�[�ƒl�Ƃ��ĕۑ�
[System.Serializable]
public class SoundData
{
    public string m_SoundName;
    public AudioClip m_AudioClip;
    public float m_fVolume;

    public SoundData(string name, AudioClip audio, float volume)
    {
        m_SoundName = name;
        m_AudioClip = audio;
        m_fVolume = volume;
    }

}