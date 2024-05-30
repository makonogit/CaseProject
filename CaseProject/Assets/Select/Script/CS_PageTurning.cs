//------------------------------
// 担当者：中島　愛音
//　ページめくり
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;

public class CS_PageTurning : MonoBehaviour
{
    [SerializeField, Header("ハンドサイン")]
    private CS_HandSigns m_handSigns;

    [SerializeField, Header("ステージ選択スクリプト")]
    private CS_StageSelect m_csStageSelect;

    [SerializeField, Header("シーンマネージャー")]
    private CS_SceneManager m_csSceneManager;

    [SerializeField, Header("本のAnimator")]
    private Animator m_ABookAnimator;

    private bool isFacingRight = true;

    //---------------------------------------
    // ステージ情報表示アニメーション用
    [SerializeField, Header("ステージ情報スクリプト")]
    private CS_StageData m_csStageData;
    [SerializeField, Header("ステージ情報表示用スプライト")]
    private SpriteRenderer m_srStageInfo;

    // [SerializeField, Header("ステージ情報表示スピード")]
    //private float m_fStageViewSpeed = 1.0f;
    //private bool m_IsStageUpdate = false;   //ステージ更新フラグ
    //rivate GameObject m_gSelectStageObj;   //ステージ情報オブジェクト
    //private float m_fStageAlpha = 0.0f;     //ステージ情報スプライトα値
    //private SpriteRenderer m_srStageSprite; //ステージ情報スプライト

    // Start is called before the first frame update
    void Start()
    {
        CS_HandSigns.OnCreateWinds += PageTurning;
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ情報表示
        //if(m_IsStageUpdate) { StageView();}

        //本を閉じる
        if (m_handSigns.IsClap() && m_ABookAnimator.GetBool("Finish") == false) { m_ABookAnimator.SetBool("Finish", true); }

    }

    private void Flip()
    {
        // 現在のScaleを取得し、X軸方向に反転させる
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }



    //ページめくりのアニメーションを発動
    private void PageTurningAnimation()
    {
        //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AM_PageTurning")) { return; }
        m_ABookAnimator.SetTrigger("pageTurningAnim");
    }

    //本をめくる
    void PageTurning(Vector3 _position, Vector3 _direction)
    {

        bool isFlip = (_direction.x < 0.0f && !isFacingRight) || (_direction.x > 0.0f && isFacingRight);

        if (isFlip)
        {
            if (m_csStageSelect.StageUpdate(-1) == -1) { return; }        //ステージ更新
            Flip();
        }
        else 
        {
            if (m_csStageSelect.StageUpdate(1) == -1) { return; }         //ステージ更新
        }

        //m_srStageSprite = SelectStageObj.GetComponent<SpriteRenderer>();
        //m_gSelectStageObj = Instantiate(SelectStageObj);

        //m_IsStageUpdate = true;                     //ステージ更新フラグ

        PageTurningAnimation();//アニメーション実行

        //---------------------------------------------
        // ステージ情報のスプライトレンダーを登録
        Sprite SelectStageSprite = m_csStageData.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_sSelectStageSprite;
        if (!SelectStageSprite) { Debug.LogWarning("セレクト用ステージスプライトが設定されていません"); }
        m_srStageInfo.sprite = SelectStageSprite;
    }

    //----------------------------------------------
    //　ステージ情報表示(つかってない)
    //----------------------------------------------
    void StageView()
    {
        //SpriteRendererが登録されていなかったら更新しない
        //if (!m_srStageSprite) { return; }

        ////ステージ情報のα値が1になったら(表示されたら)終了
        //if(m_fStageAlpha >= 1.0f)
        //{
        //    m_IsStageUpdate = false;
        //    return;
        //}

        ////ステージ情報のα値を更新
        //m_fStageAlpha += m_fStageViewSpeed * Time.deltaTime;
        //m_srStageSprite.color = new Color(1.0f, 1.0f, 1.0f, m_fStageAlpha);

    }

    //--------------------------------
    //　本を閉じた後の処理(シーン移動)
    //---------------------------------
    void CloseBook()
    {
        CS_HandSigns.OnCreateWinds -= PageTurning;
        m_csSceneManager.LoadScene(CS_SceneManager.SCENE.GAME);
    }
}
