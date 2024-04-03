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

    static public bool GaneOverFlag
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

    public void GoGameScene()
    {
        Debug.Log("クリックされた");
        SceneManager.LoadScene("TestScene");

    }
}
