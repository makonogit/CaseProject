//-----------------------------------------------
//担当者：中川直登
//ゲームオーバーイベント
//-----------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CS_TitleHandler;

public class CS_GameOverEvent : MonoBehaviour
{
    [Header("処理")]
    [SerializeField] private bool m_bIsGameOver;
    [SerializeField] private CS_SceneManager m_sceneManager;

    [SerializeField] private List<Vector3> m_Directions = new List<Vector3>();
    [SerializeField] private List<float> m_Time = new List<float>();
    [SerializeField] private bool m_bClap;
    [SerializeField] private bool m_bSceneChanged;


    [Header("演出")]
    [SerializeField] private float m_fWaitTime ;
    [SerializeField] private float m_fOverTimeMax ;
    [SerializeField] private float m_fOverTime ;
    [SerializeField] private AnimationCurve m_curGradientRaito;
    [SerializeField] private Gradient m_graGaugeColor;
    [SerializeField] private Image m_imgBackGround ;
    [SerializeField] private TextMeshProUGUI m_tmpguiGameOver ;

    // Start is called before the first frame update
    private void Start()
    {
        // イベントの登録
        CS_HandSigns.OnCreateWinds += SetMoveDirection;
        CS_HandSigns.OnClap += SetClapFlag;
        CS_TimeLimit.OnTimeOver += SetGameOverFlag;
        InitParameter();
        if (m_sceneManager == null) Debug.LogError("変数<m_sceneManager>が設定されていません。");
    }

    // Update is called once per frame
    private void Update()
    {
        // リストの更新
        TimeOverRemoveList();
        // リロード
        if (IsReload) ChangeScene(CS_SceneManager.SCENE.GAME);
        // セレクトに戻る
        if (IsReturn) ChangeScene(CS_SceneManager.SCENE.SELECT);
        // 演出
        if (m_bIsGameOver) GameOverDirection();
    }

    // OnDestroy is called before this script is Destroyed
    private void OnDestroy()
    {
        // イベントの破棄
        CS_TimeLimit.OnTimeOver -= SetGameOverFlag;
        CS_HandSigns.OnClap -= SetClapFlag;
        CS_HandSigns.OnCreateWinds -= SetMoveDirection;
        
    }

    // 変数の初期化
    // 引き数：なし
    // 戻り値：なし
    private void InitParameter()
    {
        m_bIsGameOver = false;
        m_bClap = false;
        m_fOverTime = 0;
        m_bSceneChanged = false;
    }

    // ゲームオーバーのフラグ設定
    // 引き数：なし
    // 戻り値：なし
    private void SetGameOverFlag() 
    {
        m_bIsGameOver = true;
    }

    // ゲームオーバーの演出関数
    // 引き数：なし
    // 戻り値：なし
    private void GameOverDirection() 
    { 
        m_fOverTime += Time.deltaTime;
        float value = m_curGradientRaito.Evaluate(GetOverTimeRatio);
        UnityEngine.Color color = m_tmpguiGameOver.color;
        color.a = value;
        m_tmpguiGameOver.color = color;
        color = m_graGaugeColor.Evaluate(value);

        m_imgBackGround.color = color;
    }

    // 進んだ時間の割合
    private float GetOverTimeRatio
    {
        get
        {
            float ratio = m_fOverTime/m_fOverTimeMax;
            if (ratio > 1) ratio = 1;
            return ratio;
        }
    }
    // 演出が終わったか
    // 戻り値：終わった True
    private bool IsEndDirection
    {
        get
        {
            float Max = m_fOverTimeMax + m_fWaitTime;
            // 時間を過ぎていない場合 falseで抜ける
            if (m_fOverTime < Max) return false;
            return true;
        }
    }

    // リロードするか
    // 戻り値：リロードする True
    private bool IsReload
    {
        get 
        {
            // 時間を過ぎていない場合 falseで抜ける
            if (!IsEndDirection) return false;
            // 両手を振ったか
            if (!IsCrossSwing()) return false;
            return true;
        }
    }

    // セレクトに戻るか
    // 戻り値：セレクトに戻る True
    private bool IsReturn
    {
        get
        {
            // 時間を過ぎていない場合 falseで抜ける    
            if (!IsEndDirection) return false;// 両手を振ったか
            if (!m_bClap) return false;
            return true;
        }
    }

    // 両手をほぼ同時に振った時
    // 引き数；なし
    // 戻り値：同時に振った時 True
    bool IsCrossSwing()
    {
        for (int i = 0; i < m_Directions.Count - 1; i++)
        {
            float dot = Vector3.Dot(m_Directions[i], m_Directions[i + 1]);
            // 風の向きが反対ならTrue
            if (dot < 0) return true;
        }

        return false;
    }

    // 手をスウィングした時の方向情報を保存する関数
    // 引き数：位置情報
    // 引き数：移動方向
    // 戻り値：なし
    void SetMoveDirection(Vector3 position, Vector3 direction)
    {
        // リストに追加
        m_Directions.Add(direction);
        m_Time.Add(Time.time);
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
        // 規定時間
        const float RegulationTime = 1.0f;
        bool isTimeOver = diff > RegulationTime;
        // 規定時間を超えたらリストから排除
        if (isTimeOver)
        {
            m_Directions.RemoveAt(0);
            m_Time.RemoveAt(0);
        }
    }


    // 拍手したのをフラグに設定する
    // 引数：手の情報のリスト（この関数では使わない）
    // 戻り値：なし
    void SetClapFlag(List<HandInformation> handInformation) 
    {
        // 演出が終わるまでフラグを立てない
        if (!IsEndDirection) return;
        m_bClap = true;
    }

    // シーンを変える
    // 引数：シーン名( 列挙型 )
    // 戻り値：なし
    void ChangeScene(CS_SceneManager.SCENE scene) 
    {
        // シーンをチェンジしているなら抜ける
        if (m_bSceneChanged) return;
        // シーン読込み
         m_sceneManager.LoadScene(scene);
        m_bSceneChanged = true;
    }
}