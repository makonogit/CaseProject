//------------------------------
//�S����:�����S
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

    //����getter,setter
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
        // �����𖼑O�Ŕ��f
        if (collision.gameObject.name != this.name)return;
        float ThisScale = this.transform.localScale.x;
        float OtherScale = collision.transform.localScale.x;

        // ����ScaleX�Ō����Ƌ����𔻒肷��
        float addPower = OtherScale + ThisScale;
        float subPower = OtherScale - ThisScale;


        // �������������f
        if (addPower * addPower < subPower * subPower) return;

        // ���肪�ォ������
        //if(ThisScale*ThisScale < OtherScale * OtherScale) { }

        float gosa = 1.0f;
        bool isUnder = addPower < -gosa;
        bool isOver = addPower > gosa;
        // ���̗͂��������炢����ScaleX�Ŕ���
        if (isOver && isUnder) return;

        Vector3 pos = m_vec3CameraPos;
        pos.z = 0;
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        GameObject.Instantiate(m_objWind, pos, rotation);
    }

}
