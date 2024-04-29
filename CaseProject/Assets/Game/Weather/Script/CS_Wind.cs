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



        // 同じ方向か判断
        bool isThisDirection = ThisScale < 0;
        bool isOtherDirection = OtherScale < 0;
        bool isSameDirection = (isThisDirection && isOtherDirection) || (!isThisDirection && !isOtherDirection);
        if (isSameDirection) return;

        // 風の力が同じくらいか判断
        float addPower = OtherScale + ThisScale;
        const float tolerance = 1.0f;// 許容範囲
        bool isTolerance = addPower < tolerance && addPower > -tolerance;
        if (!isTolerance) return;

        // 上方向の風の生成
        Vector3 pos = m_vec3CameraPos;
        pos.z = 0;
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        GameObject.Instantiate(m_objWind, pos, rotation);


        Destroy(collision.gameObject);
        Destroy(this.gameObject);
    }

}
