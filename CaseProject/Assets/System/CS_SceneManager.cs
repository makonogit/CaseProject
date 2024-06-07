//-----------------------------------------------
//担当者：菅眞心
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class CS_SceneManager : MonoBehaviour
{
    //シーン用定数
    public enum SCENE
    {
        TITLE,  //タイトル
        SELECT, //セレクト
        GAME,   //ゲーム
        POSE,   //ポーズ
        RESULT  //リザルト
    }
    
    [SerializeField,Header("NowLoadingオブジェクト")]
    private GameObject m_LoadingScreen; //NowLoading表示用

    [SerializeField, Header("StageDataスクリプト")]
    private CS_StageData m_csStagedata;

    [SerializeField, Header("globallight(明転用)")]
    private Light2D m_lGlobalLight;

    private bool m_IsLightChange = false;   //明転フラグ

    //クリア状況管理用
    private Dictionary<int, bool> StageClearData = new Dictionary<int, bool>();

    //ロードを待機する時間(初期段階ではロードが一瞬の為)
    [SerializeField,Header("ローディング待機時間")]
    private float m_fLoadWaitTime = 1000.0f;  
    private float m_fLoadTime = 0.0f;
   
    //シーン読み込み関数
    //引数：Scene定数
    public IEnumerator LoadingScene(SCENE _scene)
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
            if(_scene == SCENE.GAME)
            {
                //登録されたステージオブジェクトを生成
                GameObject StageObj = m_csStagedata.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_gStagePrefab;
                if (!StageObj) { Debug.LogWarning("ステージオブジェクトが登録されていません"); }
                Instantiate(StageObj);
                //m_csStagegata.
            }

            Debug.Log("シーンの読み込みが完了しました。");
        }

    }

    //-------------------------
    //シーン読み込み関数
    //引数：Scene定数
    //-------------------------
    public void LoadScene(SCENE _scene)
    {
        SceneManager.LoadScene((int)_scene);
    }


    //-------------------------------------
    //一番最初に実行される関数
    //ゲームシーンだった場合に処理
    //-------------------------------------
    private void Awake()
    {
        int SceneNum = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("World" + StageInfo.World + "Stage" + StageInfo.Stage);

        //シーン内ライトの読み込み
        ObjectData.m_lGlobalLight = m_lGlobalLight;

        //現在のシーンがゲームだったらステージを生成
        if((SCENE)SceneNum == SCENE.GAME)
        {
            //登録されたステージオブジェクトを生成
            GameObject StageObj = m_csStagedata.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_gStagePrefab;
            if (!StageObj) { Debug.LogWarning("ステージオブジェクトが登録されていません"); }
            Instantiate(StageObj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //LoadScene(SCENE.GAME);
        //Debug.Log("よみこみ");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightChange(float ChangeSpeed,float Maxintensity)
    {
        
        if(m_lGlobalLight.intensity < Maxintensity)
        {
            m_lGlobalLight.intensity += ChangeSpeed * Time.deltaTime;
        }
        else
        {
            //明転フラグをオン
            m_IsLightChange = true;
        }
        
        //明るさをもとに戻す
        if(m_IsLightChange)
        {
            m_lGlobalLight.intensity -= ChangeSpeed * Time.deltaTime;
        }

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
