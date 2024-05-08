using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_ResultController : MonoBehaviour
{
    static private bool m_gameOverFg = false;

    [SerializeField, Header("GAMEOVERとCLEARのテキストオブジェ")]
    [Header("0:GAMEOVER 1:CLEAR")]
    private GameObject[] m_texts;

    [SerializeField, Header("Scene")]
    static private string m_sceneName;

    [SerializeField, Header("完成してからの待機時間")]
    private float m_fWaitTime = 3.0f;

    private float m_fNowTime = 0.0f;

    public enum STAGE_TYPE
    {
        STAGE1 = 0,
    };

    public enum RESULT_STATE
    {
       
        MOVESTAR_SERIUS,  //星の移動
        BORN_LINE,  //ラインの出現
        FRAME,      //枠出現
        TEXT_FADE_IN,//星座完成の文字のオブジェクトをフェードイン
        NIGHT_FADE_IN,//第〇夜の文字の画像のオブジェクトをフェードイン
        BOOK_FADE_IN,//本のフェードイン
        GO_SELECT_SCENE,
        NONE//特になし
    };

    private static STAGE_TYPE m_stageType = STAGE_TYPE.STAGE1;//ステージの種類

    private RESULT_STATE m_resultState = RESULT_STATE.MOVESTAR_SERIUS;//リザルトの状態

    //リザルト状態のセッターゲッター
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

    //リザルトに行く関数
    //引数：ゲームオーバーか、現在シーンの名前
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
