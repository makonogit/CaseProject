//------------------------------
//’S“–ŽÒ:›áÁS
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Wind : MonoBehaviour
{

    private Transform m_tThisTransform; //Ž©g‚ÌTransform

    [SerializeField,Header("ˆÚ“®—Ê")]
    private float m_fMovment = 1.0f;

    [SerializeField,Header("•—‚Ì‹­‚³")]
    private float m_fWindPower = 1.0f;

    float m_nowTime = 0.0f;

    //•—‚Ìgetter,setter
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
        m_nowTime += Time.deltaTime;
        const float deleteTime = 3.0f;
        if (m_nowTime > deleteTime)
        {
            Destroy(this.gameObject);
        }
    }
}
