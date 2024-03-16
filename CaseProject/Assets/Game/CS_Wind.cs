//------------------------------
//担当者:菅眞心
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Wind : MonoBehaviour
{

    private Transform m_tThisTransform; //自身のTransform

    [SerializeField,Header("移動量")]
    private float m_fMovment = 1.0f;

    [SerializeField,Header("風の強さ")]
    private float m_fWindPower = 1.0f;

    //風のgetter,setter
    public float WindPower
    {
        set
        {
            m_fWindPower = value;
        }
        get
        {
            return m_fWindPower;
        }
    }

    public float Movement
    {
        set
        {
            m_fMovment = value;
        }
        get
        {
            return m_fMovment;
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
        m_tThisTransform.Translate(m_fMovment,0.0f,0.0f);
    }
}
