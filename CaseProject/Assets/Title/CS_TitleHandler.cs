//------------------------------
// �S���ҁF�����@����
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

    //�V�[���̃��[�h
    public void GoNextScene(string _nextSceneName)
    {
        SceneManager.LoadScene(_nextSceneName);
    }

    //�Q�[���I��
    public void GameEnd()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
