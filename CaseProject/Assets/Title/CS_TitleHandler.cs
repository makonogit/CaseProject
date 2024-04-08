//------------------------------
// 担当者：中島　愛音
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class CS_TitleHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
