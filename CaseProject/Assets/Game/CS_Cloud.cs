//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CS_Cloud : MonoBehaviour
{

    [Header("変数")]
    [SerializeField] private GameObject m_objCloudParticle;
    private float m_fCloudSize = 1.0f;
    [SerializeField] private float m_fMaxSize = 2.0f;
    [SerializeField] private float m_fGrowingValue = 0.1f;
    [SerializeField] private float m_fResetSizeValue = 0.01f;
    [SerializeField] private Vector3 m_vec3StartSize;
    [Header("雨")]
    [SerializeField] private GameObject m_objRain;              // 生成物
    [SerializeField] private int m_fRainCreatePerSecond = 10;    // 雨の毎秒生成数
    [SerializeField] private float m_fRainCreateRange = 25;      // 生成する半径
    [SerializeField] private float m_fDecrease = 0.01f;            // サイズ減少率
    private float m_fRequiredCreateNum = 0.0f;                  // 必要な生成数

    // Start is called before the first frame update
    private void Start()
    {
        CS_HandSigns.OnCreateRains += EventRain;
    }

    // Update is called once per frame
    private void Update()
    {
    }
    private void OnParticleCollision(GameObject other)
    {
        bool isSameTheObject = other == m_objCloudParticle;
        bool isNotMaxofCloudSize = m_fCloudSize <= m_fMaxSize;
        bool isGrowing = isSameTheObject && isNotMaxofCloudSize;

        if (isGrowing) Growing();
        if (!isNotMaxofCloudSize) SizeReset();
        
    }
    // 雲が大きくなる関数
    // 引数：なし
    // 戻り値：なし
    private void Growing() 
    {
        // 1秒間に大きくなる値
        float growingRate = m_fGrowingValue * Time.deltaTime;
        m_fCloudSize += growingRate;
        transform.localScale = m_vec3StartSize * m_fCloudSize;
    }
    
    // 雲サイズをリセット
    // 引数：なし
    // 戻り値：なし
    private void SizeReset()
    {
        transform.localScale = (m_vec3StartSize * m_fResetSizeValue);
        m_fCloudSize = m_fResetSizeValue;
    }

    // 雨を生成するイベント設定関数
    // 引数：特に意味なし
    // 引数：特に意味なし
    // 戻り値：なし
    private void EventRain(Vector3 pos ,Vector3 dir) 
    {
        // サイズがリセットサイズよりも大きいか
        bool isSizeOverZero = m_fCloudSize > m_fResetSizeValue;
        if (isSizeOverZero) CreateRain();
    }
    // 雨を生成する関数
    // 引数：なし
    // 戻り値：なし
    private void CreateRain() 
    {
        // 生成する数を追加する
        m_fRequiredCreateNum += m_fRainCreatePerSecond * Time.deltaTime;
        for (int i = 0; i <= m_fRequiredCreateNum; i++)
        {
            // 生成位置のランダム取得
            float x = Random.Range(0, 7);
            float y = Random.Range(0, 7);
            float radius = Random.Range(0, m_fRainCreateRange);

            Vector3 rad = new Vector3(Mathf.Cos(x), Mathf.Sin(y));
            rad.Normalize();
            Vector3 offset = new Vector3(0, -5.0f);
            // 位置の設定
            Vector3 Pos = transform.position + offset + rad * radius;
            // 方向の設定
            Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
            // 雨の生成
            GameObject obj = GameObject.Instantiate(m_objRain, Pos, rotation);
            m_fRequiredCreateNum -= 1;

            // 雲を小さくする
            m_fCloudSize -= m_fDecrease;
            transform.localScale = m_vec3StartSize * m_fCloudSize;
        }
    }
    

}