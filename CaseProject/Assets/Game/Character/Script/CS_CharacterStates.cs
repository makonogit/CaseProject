//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//------------------------------
// キャラクターのステータス管理スクリプト
//------------------------------
public class CS_CharacterStates : MonoBehaviour
{
    [SerializeField] protected uint m_nMaxHP;
    [SerializeField] private uint m_nHP;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // ダメージを受けた処理
    // 引数：ダメージ値
    // 戻り値：なし
    public void HitDamege(uint damage)
    {
        HP -= damage;
    }

    // 体力のSetterGetter
    protected uint HP
    {
        set
        {
            m_nHP = value;
            m_nHP = (uint)Mathf.Clamp(m_nHP, 0, m_nMaxHP);
        }

        get
        {
            return m_nHP;
        }
    }
}
