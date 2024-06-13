//-----------------------------------------------
//担当者：菅眞心
//ゲームオーバー時の処理
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //一旦、あとでSceneManagerに変更

public class CS_GameOver : MonoBehaviour
{
    [SerializeField, Header("ハンドトラッキング制御スクリプト")]
    private CS_HandSigns m_csHandsign;

    [SerializeField, Header("GameOverのAnimator")]
    private Animator m_aGameOverAnim;

    private bool m_isXDir = true;

    private bool m_isDir = false;

    [SerializeField, Header("CS_Creater")]
    private CS_Creater m_cscreater;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_csHandsign) { Debug.LogWarning("CS_HandSignが設定されていません"); }
        if (!m_aGameOverAnim) { Debug.LogWarning("Animatorが設定されていません"); }

        //CS_HandSigns.OnCreateWinds -= CS_Creater.CreateWinds;

        //風のイベント消去
        m_cscreater.DeleteEvent();
        //イベント登録
        CS_HandSigns.OnCreateWinds += CloseBook;

    }

    private void OnDestroy()
    {

        //イベント解除
        CS_HandSigns.OnCreateWinds -= CloseBook;
    }


    //本を閉じる処理
    void CloseBook(Vector3 _position, Vector3 _direction)
    {
        //SE再生
        ObjectData.m_csSoundData.PlaySE("Book");

        //めくった向きを保存
        m_isDir = (_direction.x < 0.0f && !m_isXDir) || (_direction.x > 0.0f && m_isXDir);

        //アニメーション再生
        m_aGameOverAnim.SetTrigger("Finish");
        //Debug.Log(isDir);

    }

    void SceneChange()
    {
        //向きによってシーン移動
        if (m_isDir) { SceneManager.LoadScene("SelectScene"); }
        else { SceneManager.LoadScene("GameScene"); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
