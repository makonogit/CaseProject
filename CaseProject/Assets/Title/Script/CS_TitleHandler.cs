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
    [SerializeField, Header("次のシーンの名前")]
    private string m_nextSceneName;
    private List<Vector3> m_Directions = new List<Vector3>();
    private List<float> m_Time = new List<float>();
    
    //状態が待機時2の待機時間
    private float m_nowWaitTime = 0.0f;
    private float m_waitTime = 1.0f;

    public enum TITLE_STATE
    {
        SET_HANDS,  //両手をを初期位置にセットできているか
        CALL_SERIUS,//シリウスを呼ぶ
        BORN_SERIUS,//シリウスの登場
        WAIT1,      //待機1
        SCROLL,     //画面スクロール中
        STOP,       //スクロール終了
        MAGNIFICATION_SERIUS,//拡大シリウス
        REDUCTION_SERIUS,//縮小シリウス
        WAIT2,      //待機2
        GAME_END
    }

    [SerializeField] private TITLE_STATE m_titleState = TITLE_STATE.SET_HANDS;

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
        CS_HandSigns.OnCreateWinds += Swing;       
    }

    

    // Update is called once per frame
    void Update()
    {
        CheckGoNextScene();//次のシーンへいくかどうかの処理
        
        // リストの更新
        TimeOverRemoveList();    
        
        // シリウスを呼ぶ
        if (IsCallSerius()) m_titleState = TITLE_STATE.BORN_SERIUS;
                
    }
    // OnDestroy is called this object is Destroyed
    private void OnDestroy(){
        CS_HandSigns.OnCreateWinds -= Swing;
    }
    
    // 手をスウィングした時の位置情報を保存する関数
    // 引き数：位置情報
    // 引き数：移動方向
    // 戻り値：なし
    void Swing(Vector3 position, Vector3 direction){
        // セットハンド以外なら抜ける
        if (m_titleState != TITLE_STATE.SET_HANDS) return;
        m_Directions.Add(direction);
        m_Time.Add(Time.time);
    }

    // シリウスを呼ぶ判定をする関数
    // 引き数；なし
    // 戻り値：シリウスを呼ぶTrue
    bool IsCallSerius() 
    {
        for (int i = 0; i < m_Directions.Count-1; i++) 
        {
            float dot = Vector3.Dot(m_Directions[i], m_Directions[i + 1]);
            // 風の向きが反対ならTrue
            if (dot < 0) return true;
        }
        
        return false;
    }
    // 規定時間を超えたらリストから排除する関数
    // 引数：なし
    // 戻り値：なし
    void TimeOverRemoveList() 
    {
        // リストがないなら抜ける
        if (m_Time.Count <= 0) return;
        // 時間を超えたか
        float diff = Time.time - m_Time[0];
        const float RegulationTime = 1.0f;
        bool isTimeOver =diff > RegulationTime;
        // 規定時間を超えたらリストから排除
        if (isTimeOver) 
        {
            m_Directions.RemoveAt(0);
            m_Time.RemoveAt(0);
        }
    }

    //シーンのロード
    void CheckGoNextScene()
    {
        //待機時間2?
        if (TitleState != TITLE_STATE.WAIT2) { return; }

        if (m_nowWaitTime <= m_waitTime)
        {
            m_nowWaitTime += Time.deltaTime;//デルタタイム加算
            return;
        }

        //時間の保存
        ObjectData.m_fBGMTime = ObjectData.m_csSoundData.BGMTIME;
        SceneManager.LoadScene("SelectScene");  
    }

    //ゲーム終了
    public void GameEnd()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
