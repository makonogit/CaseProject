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

    static private string m_sceneName;

    // Start is called before the first frame update
    void Start()
    {
       if(m_gameOverFg)
       {
            m_texts[0].SetActive(true);
       }
       else
       {
            m_texts[1].SetActive(true);
       }
    }

    // Update is called once per frame
    void Update()
    {

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
