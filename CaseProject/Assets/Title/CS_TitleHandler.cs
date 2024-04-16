//------------------------------
// 担当者：中島　愛音
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;


public class CS_TitleHandler : MonoBehaviour
{
    [SerializeField, Header("ハンドサイン")]
    private CS_HandSigns m_handSigns;

    private HandLandmarkListAnnotation[] m_HandLandmark = new HandLandmarkListAnnotation[2];

    [SerializeField, Header("GAME,ENDロゴ")]
    [Header("0GAME")]
    [Header("1END")]
    private GameObject[] m_txLogos;

    [SerializeField, Header("次のシーンの名前")]
    private string m_nextSceneName;

    public enum TITLE_STATE
    {
        CLOUD_EXCLUSION,//雲排除
        SELECT_NEXT_SCENE,//次のシーン選択
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
        //雲をどける状態なら終了
        if(TitleState == TITLE_STATE.CLOUD_EXCLUSION) { return; }

        //終了状態ならアプリを閉じる
        if(TitleState == TITLE_STATE.GAME_END) { UnityEditor.EditorApplication.isPlaying = false; }
        //GO_GAME_SCENE状態なら次のシーンへ
        if (TitleState == TITLE_STATE.GO_GAME_SCENE) { SceneManager.LoadScene(m_nextSceneName); }

        //ハンドマークを取得
        m_HandLandmark = m_handSigns.HandMark;
        
        if (m_HandLandmark[0] != null) 
        {
            bool rockSign = m_handSigns.GetHandPose(0) == (byte)CS_HandSigns.HandPose.RockSign;
            if (m_handSigns.GetHandPose(0)==(byte)CS_HandSigns.HandPose.RockSign)
            {
                m_isChangeSceneInpossible = true;//シーン遷移可能にする
            }
            else 
            {
                m_isChangeSceneInpossible = false; //シーン遷移不可能にする
            }
        }
        else if (m_HandLandmark[1] != null)
        {
            //GetHandPose(handNum)==(byte)HandPose.PaperSign
            if (m_handSigns.GetHandPose(1) == (byte)CS_HandSigns.HandPose.RockSign)
            {
                m_isChangeSceneInpossible = true; //シーン遷移可能にする
            }
            else
            {
                m_isChangeSceneInpossible = false;//シーン遷移不可能にする
            }
        }
    }

    public void LogoActiveTrue()
    {
        //ロゴの活動をtrue
        m_txLogos[0].SetActive(true);
        m_txLogos[1].SetActive(true);

        TitleState = TITLE_STATE.SELECT_NEXT_SCENE;//タイトル状態を選択可能状態にする
    }

    //シーンのロード
    public void GoNextScene(string _nextSceneName)
    {
        SceneManager.LoadScene(_nextSceneName);
    }

    //ゲーム終了
    public void GameEnd()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
