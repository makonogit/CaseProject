//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;

public class CS_Thunder : MonoBehaviour 
{
    private Transform m_tThisTransform; //自身のTransform

    // 移動距離
    float m_fMovement;

    [SerializeField]
    private float m_fMaxTime = 0.5f;
    private float m_nowTime = 0.0f;

    public float Movement
    {
        set
        {
            m_fMovement = value;
        }
        get
        {
            return m_fMovement;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        m_tThisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        m_tThisTransform.Translate(0,Movement*Time.deltaTime,0);
        m_nowTime += Time.deltaTime;

        // 時間経過で削除する
        if (m_nowTime > m_fMaxTime) Destroy(this.gameObject);
    }

    // OnDestory is called when this object is destroyed
    private void OnDestroy()
    {   
    }
}