﻿//-----------------------------------------------
//担当者：中川直登
//タイムリミット
//-----------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_TimeLimit : MonoBehaviour
{
    [Header("タイムリミット")]
    [SerializeField] private float  m_fTimeLimit;
    [SerializeField] private float  m_fNowTime;
    [SerializeField] private AnimationCurve m_acurveSpeed;
    [SerializeField] private Gradient m_graGaugeColor;
    private Image m_imgGauge;
    [SerializeField] private Image m_imgLeftSideGauge;
    [SerializeField] private Image m_imgRightSideGauge;
    [SerializeField] private Image m_imgLeftMark;
    [SerializeField] private Image m_imgRightMark;
    private float m_fAlpha = 1.0f;
    // イベント
    public delegate void EventTimeLimit();
    public static event EventTimeLimit OnTimeOver;

    // Start is called before the first frame update
    private void Start()
    {
        // イベントの登録
        CS_TimeLimit.OnTimeOver += SetTimeScaleZero;
        //InitParameter();
        m_imgGauge = GetComponent<Image>();
        if (m_imgGauge == null) Debug.LogError("nullComponent:Imageのコンポーネントを取得できませんでした。");
    }

    // Update is called once per frame
    private void Update()
    {
        // 時間計算
        m_fNowTime += Time.deltaTime;
        // ゲージのサイズ更新
        ChangeGaugeLength(GetTimeLimitRatio);
        // イベントの発行
        if(IsTimeOver)OnTimeOver();
    }
    
    // OnDestroy is called before this script is Destroyed
    private void OnDestroy()
    {
        // イベントの破棄
        CS_TimeLimit.OnTimeOver -= SetTimeScaleZero;
    }

    // 変数の初期化
    // 引き数：なし
    // 戻り値：なし
    private void InitParameter() 
    {
        m_fNowTime = 0.0f;
        m_fAlpha = 1.0f;
    }

    // ゲージの長さを更新する
    // 引き数：長さ 0～1
    // 戻り値：なし
    private void ChangeGaugeLength(float value) 
    {
        if(value >1.0f)value = 1.0f;
        if(value <0.0f)value = 0.0f;
        
        // ゲージのサイズ更新
        Vector2 size = m_imgGauge.rectTransform.localScale;
        size.x = m_acurveSpeed.Evaluate(value);
        m_imgGauge.rectTransform.localScale = size;
        // 色更新
        ChangeUiColor(value);
        // 位置更新
        Vector3 pos = size * m_imgGauge.rectTransform.sizeDelta * 0.5f;
        pos.y = m_imgGauge.rectTransform.localPosition.y;
        m_imgRightSideGauge.rectTransform.localPosition = pos;
        pos.x *=-1.0f;// 反転
        m_imgLeftSideGauge.rectTransform.localPosition = pos;
    }

    // ゲージの色を更新する
    // 引き数：色 0～1
    // 戻り値：なし
    private void ChangeUiColor(float value) 
    {
        Color color = m_graGaugeColor.Evaluate(value);
        m_imgGauge.color = color;
        m_imgLeftSideGauge.color = color;
        m_imgRightSideGauge.color = color;
        m_imgLeftMark.color = color;
        m_imgRightMark.color = color;

        // 値が0以下なら透明化
        if (IsGaugeMini(m_acurveSpeed.Evaluate(value))) GraduallyTransparent(color);
    }

    // ゲージが真ん中か
    // 引き数：ゲージの値
    // 戻り値：なし
    private bool IsGaugeMini(float value) 
    {
        return value <= 0.0f;
    }

    // 徐々に透明になる関数
    // 引き数：色
    // 戻り値：なし
    private void GraduallyTransparent(Color color)
    {
        float speed = 1.0f;
        m_fAlpha -= speed * Time.deltaTime;
        //透明にする
        color.a = m_fAlpha;
        m_imgGauge.color = color;
        m_imgLeftSideGauge.color = color;
        m_imgRightSideGauge.color = color;
    }

    // タイムスケ―ルをゼロにする関数
    // 引き数：なし
    // 戻り値：なし
    private void SetTimeScaleZero()
    {
        //Time.timeScale = 0.0f;
    }

    // 時間を超過したか
    // 戻り値：超過した true
    private bool IsTimeOver
    {
        get 
        {
            return m_fNowTime > m_fTimeLimit;
        }
    }

    // 現在時刻の割合を取得
    // 戻り値:0～1
    public float GetTimeLimitRatio 
    {
        get 
        {
            float ratio =  m_fNowTime / m_fTimeLimit;
            // 1より大きくなったら
            if(ratio > 1.0f) ratio = 1.0f;
            return ratio;
        }
    }
}