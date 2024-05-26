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

    private bool isFacingRight = true;

    //---------------------------------------
    // ステージ情報表示アニメーション用
    [SerializeField, Header("ステージ情報スクリプト")]
    private CS_StageData m_csStageData;
    [SerializeField, Header("ステージ情報表示スピード")]
    private float m_fStageViewSpeed = 1.0f;
    private bool m_IsStageUpdate = false;   //ステージ更新フラグ
    private GameObject m_gSelectStageObj;   //ステージ情報オブジェクト
    private float m_fStageAlpha = 0.0f;     //ステージ情報スプライトα値
    private SpriteRenderer m_srStageSprite; //ステージ情報スプライト

    // Start is called before the first frame update
    void Start()
    {
        CS_HandSigns.OnCreateWinds += PageTurning;
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ情報表示
        if(m_IsStageUpdate) { StageView();}

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
        GetComponent<Animator>().SetTrigger("pageTurningAnim");
    }

    //本をめくる
    void PageTurning(Vector3 _position, Vector3 _direction)
    {
        //ステージ描画中だったら更新しない
        if(m_IsStageUpdate) { return; }
        //Destroy使いたくない！変えたい！
        Destroy(m_gSelectStageObj); //前回のステージを消去
        m_fStageAlpha = 0.0f;       //ステージ情報のα値を初期化


        bool isFlip = (_direction.x < 0.0f && !isFacingRight) || (_direction.x > 0.0f && isFacingRight);

        if (isFlip)
        {
            m_csStageSelect.StageUpdate(-1);        //ステージ更新
            Flip();
        }
        else 
        { 
            m_csStageSelect.StageUpdate(1);         //ステージ更新
        }


        //---------------------------------------------
        // ステージ情報のスプライトレンダーを登録
        GameObject SelectStageObj = m_csStageData.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_gSelectStagePrefab;
        if (!SelectStageObj) { Debug.LogWarning("セレクト用ステージオブジェクトが設定されていません"); }
        m_srStageSprite = SelectStageObj.GetComponent<SpriteRenderer>();
        m_gSelectStageObj = Instantiate(SelectStageObj);

        m_IsStageUpdate = true;                     //ステージ更新フラグ

        PageTurningAnimation();//アニメーション実行
    }

    //----------------------------------------------
    //　ステージ情報表示(Animatorイベントで呼び出し)
    //----------------------------------------------
    void StageView()
    {
        //SpriteRendererが登録されていなかったら更新しない
        if (!m_srStageSprite) { return; }

        //ステージ情報のα値が1になったら(表示されたら)終了
        if(m_fStageAlpha >= 1.0f)
        {
            m_IsStageUpdate = false;
            return;
        }

        //ステージ情報のα値を更新
        m_fStageAlpha += m_fStageViewSpeed * Time.deltaTime;
        m_srStageSprite.color = new Color(1.0f, 1.0f, 1.0f, m_fStageAlpha);

    }
}
