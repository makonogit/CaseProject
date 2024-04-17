//-----------------------------------------------
//担当者：菅眞心
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_SceneManager : MonoBehaviour
{
    //シーン用定数
    public enum SCENE
    {
        TITLE,  //タイトル
        GAME,   //ゲーム
        POSE,   //ポーズ
        RESULT  //リザルト
    }
    
    [SerializeField,Header("NowLoadingオブジェクト")]
    private GameObject m_LoadingScreen; //NowLoading表示用

    //クリア状況管理用
    private Dictionary<int, bool> StageClearData = new Dictionary<int, bool>();

    //ロードを待機する時間(初期段階ではロードが一瞬の為)
    [SerializeField,Header("ローディング待機時間")]
    private float m_fLoadWaitTime = 1000.0f;  
    private float m_fLoadTime = 0.0f;
   
    //シーン読み込み関数
    //引数：Scene定数
    private IEnumerator LoadScene(SCENE _scene)
    {

        if (!m_LoadingScreen)
        {
            Debug.LogWarning("ローディングスクリーンがありません");
        }

        //NowLoadingを非表示
        m_LoadingScreen.SetActive(false);

        //非同期読み込み開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)_scene);

        //読み込み完了まで待機(ロード待機時間経過を待つ)
        while(!asyncLoad.isDone && m_fLoadTime > m_fLoadWaitTime)
        {
            //ロード時間
            m_fLoadTime += Time.deltaTime;

            //ローディング画面を表示
            m_LoadingScreen.SetActive(true);

            //シーンの読み込み状況を表示
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("シーンの読み込み進行状況: " + (progress * 100) + "%");

            //読み込み失敗処理
            if (asyncLoad.progress < 0.9f && !asyncLoad.allowSceneActivation)
            {
                Debug.LogError("シーンが読み込めませんでした！");
                break;
            }

            yield return null;
        }

        //ローディング画面の非表示
        m_LoadingScreen.SetActive(false);

        //読み込み完了
        if (asyncLoad.isDone)
        {
            Debug.Log("シーンの読み込みが完了しました。");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //void SaveStageClearData(int StageNum,bool isClear)
    //{
    //    StageClearData[StageNum] = isClear;
    //    SaveData("StageClearData", StageClearData);
    //}


    //シーン読み込み関数
    //引数：読み込みデータ名,データ
    public void SaveData<T>(string key,T data)
    {
        string Data = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, Data);
        PlayerPrefs.Save();
    }


    //シーン読み込み関数
    //引数：読み込みデータ名
    //戻り値：データ
    public T LoadData<T>(string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            string Data = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(Data);
        }
        else
        {
            Debug.LogWarning("指定されたデータは保存されていません");
            return default(T);
        }
    }

}
