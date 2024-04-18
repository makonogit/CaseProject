//------------------------------
// 担当者：中島　愛音
// ポーズ
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_PauseController : MonoBehaviour
{
    [SerializeField, Header("ポーズのCanvasPrefab")]
    private GameObject m_pauseCanvasPrefab;

    [SerializeField, Header("ハンドサイン")]
    private CS_HandSigns m_handSigns;

    private HandLandmarkListAnnotation[] m_HandLandmark = new HandLandmarkListAnnotation[2];

    private GameObject m_pauseScreen;

    private static bool m_isPause = false;

    void Start()
    {
        m_isPause = false;
    }

    private void Update()
    {
        if (isPause()){ return; }

        //手がTのポーズならPause()を呼ぶ
        //ハンドマークを取得
        m_HandLandmark = m_handSigns.HandMark;

        //nullならreturn
        if (m_HandLandmark[0] == null) { return; }

        //Tポーズじゃないなら終了
        if (!m_handSigns.IsTPose()) { return; }

        Pause();
    }

    //ポーズ関数
    public void Pause()
    {
        //すでにポーズ中なら終了
        if (m_isPause) { return; }

        Time.timeScale = 0f;
        m_isPause = true;

        //ポーズ画面生成
        CreatePauseCanvas();
    }

    //ポーズ画面生成関数
    private void CreatePauseCanvas()
    {
        if(m_pauseCanvasPrefab != null)
        {
            //ポーズ画面作成
           m_pauseScreen = Instantiate(m_pauseCanvasPrefab);
           m_pauseScreen.GetComponent<Canvas>().worldCamera = Camera.main;
            //ポーズサイン設定
            FindText(m_pauseScreen.transform);
        }
        else
        {
            Debug.LogError("ポーズのCanvasプレハブが指定されていません");
        }
    }


    private void FindText(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // 子オブジェクトの名前が "Point Annotation(Clone)" であるかどうかを確認
            if (child.GetComponent<CS_PauseEvent>() != null)
            {
                child.GetComponent<CS_PauseEvent>().SetHandSignData(m_handSigns);
            }

            // 子オブジェクトが他の子を持っている場合、再帰的にそれらの子オブジェクトを検索
            if (child.childCount > 0)
            {
                FindText(child);
            }
        }
    }

    //ポーズ関数を使ったにも関わらず止まらないときに使用
    public static bool isPause()
    {
        return m_isPause;
    }

    //再開
    public void Restart()
    {
        Time.timeScale = 1f;
        m_isPause = false;

        //ポーズ画面を消す
        Destroy(m_pauseScreen);
    }

    //やりなおし
    public void TryAgain()
    {
        Time.timeScale = 1f;
        //現在のシーンを呼び出す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   


}
