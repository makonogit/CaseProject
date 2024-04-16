//-----------------------------------------------
//担当者：菅眞心
//火事クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Fire : MonoBehaviour
{

    [SerializeField, Header("火のHP")]
    private float m_FireHP = 100.0f;

    //火の最大HP
    private float m_MaxHP = 0.0f;

    float FIREHP
    {
        get
        {
            return m_FireHP;
        }
        set
        {
            m_FireHP = value;
        }
    }

    private Vector3 m_Scale;        //火のスケール

    // Start is called before the first frame update
    void Start()
    {
        m_Scale = this.transform.localScale;    //スケールの保存
        m_MaxHP = m_FireHP;                     //最大HPの保存
    }

    // Update is called once per frame
    void Update()
    {
        //HPがなくなったら消去
        if(m_FireHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //雨に当たったら
        if (collision.gameObject.tag == "Rain")
        {
            m_FireHP -= 10.0f;
            //スプライトの縮小(最大HPに合わせて)
            Vector3 DecreaseRate = new Vector3((m_Scale.x / m_MaxHP) * 5.0f,(m_Scale.y / m_MaxHP) * 5.0f, 0.0f);
            this.transform.localScale -= DecreaseRate;
        }
    }

}
