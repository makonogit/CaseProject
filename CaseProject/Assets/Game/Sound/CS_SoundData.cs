using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SoundData : MonoBehaviour
{
    [SerializeField, Header("BGM用AudioSource")]
    private AudioSource m_BGMAudioSource;

    [SerializeField, Header("SE用AudioSource")]
    private AudioSource m_SEAudioSource;

    [SerializeField, Header("CS_TimeLimit ※制限時間によってピッチを上げる為")]
    private CS_TimeLimit m_cstimelimit;

    [SerializeField, Header("ピッチを上げる速度")]
    private float m_fPitchUpSpeed = 1.0f;

    [SerializeField, Header("ピッチの最大速度")]
    private float m_fMaxPitchSpeed = 1.5f;

    [SerializeField, Header("ピッチを上げ始める時間の割合 ※0~1")]
    private float m_fPitchUpTime = 0.7f;


    [SerializeField, Header("BGMList")]
    private List<SoundData> m_BGMList = new List<SoundData>();

    [SerializeField, Header("SEList")]
    private List<SoundData> m_SEList = new List<SoundData>();


    private bool m_isGameOverChangeTrigger = false;  //ゲームオーバー時のサウンド変更トリガー 

    // Start is called before the first frame update
    void Start()
    {
        ObjectData.m_csSoundData = this;

       // DontDestroyOnLoad(transform.gameObject);  // このGameObjectをシーン遷移中に破棄しない
    }

    // Update is called once per frame
    void Update()
    {
        if (m_cstimelimit)
        {
            //時間がギリギリになってきたら徐々に音を上げる
            if (m_cstimelimit.GetTimeLimitRatio > m_fPitchUpTime && m_BGMAudioSource.pitch < m_fMaxPitchSpeed)
            {
                m_BGMAudioSource.pitch += m_fPitchUpSpeed * Time.deltaTime;
            }

            //制限時間経過したらゲームオーバー
            if (m_cstimelimit.GetTimeLimitRatio == 1.0f && m_BGMAudioSource.pitch > 1.1f) { PlayBGM("GameOver"); }
        }

    }

    //BGM変更関数
    //引数:Audioclip
    //戻り値:True 成功 False 失敗
    //private bool ChangeBGM(AudioClip audio)
    //{
    //    if(audio == null) { return false; }

    //    m_BGMAudioSource.Stop();
    //    m_BGMAudioSource.pitch = 1.0f;
    //    m_BGMAudioSource.clip = audio;
    //    m_BGMAudioSource.Play();

    //    return true;
    //}


    //SE再生用関数
    //引数：SEの登録名
    public void PlaySE(string sename)
    {

        //同じ名前のSEがあれば再生
        foreach (var se in m_SEList)
        {
            string name = se.m_SoundName;
            if(name == sename) 
            {
                //同じ音を再生中だったら再生しない
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

    //BGM再生用関数
    //引数：SEの登録名
    public void PlayBGM(string bgmname)
    {

        //同じ名前のSEがあれば再生
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

    //BGM再生用関数
    //引数：SEの登録名,再生位置
    public void PlayBGM(string bgmname,float time)
    {

        //同じ名前のSEがあれば再生
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

    //BGM停止関数
    public void StopBGM()
    {
        m_BGMAudioSource.Stop();
    }

    //BGMの再生位置を取得する
    public float BGMTIME
    {
        get
        {
            return m_BGMAudioSource.time;
        }
    }


}

//サウンドデータを保持するクラス
//参照しやすいようにキーと値として保存
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