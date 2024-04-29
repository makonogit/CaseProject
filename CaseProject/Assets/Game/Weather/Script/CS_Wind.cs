//------------------------------
//担当者:菅眞心
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Mediapipe.CopyCalculatorOptions.Types;

public class CS_Wind : MonoBehaviour
{

    [SerializeField]private GameObject m_objWind;
    private Vector3 m_vec3CameraPos;
    private float m_fWindPower = 1.0f;

    float m_nowTime = 0.0f;

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

    public Vector3 SetCameraPos 
    {
        set { m_vec3CameraPos = value; }
    }

    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update()
    {
        m_nowTime += Time.deltaTime;
        const float deleteTime = 3.0f;
        if (m_nowTime > deleteTime) Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 風かを名前で判断
        if (collision.gameObject.name != this.name)return;
        float ThisScale = this.transform.localScale.x;
        float OtherScale = collision.transform.localScale.x;

        // 風のScaleXで向きと強さを判定する
        float addPower = OtherScale + ThisScale;
        float subPower = OtherScale - ThisScale;


        // 同じ方向か判断
        if (addPower * addPower < subPower * subPower) return;

        // 相手が弱かった時
        //if(ThisScale*ThisScale < OtherScale * OtherScale) { }

        float gosa = 1.0f;
        bool isUnder = addPower < -gosa;
        bool isOver = addPower > gosa;
        // 風の力が同じくらいかをScaleXで判定
        if (isOver && isUnder) return;

        Vector3 pos = m_vec3CameraPos;
        pos.z = 0;
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        GameObject.Instantiate(m_objWind, pos, rotation);
    }

}
