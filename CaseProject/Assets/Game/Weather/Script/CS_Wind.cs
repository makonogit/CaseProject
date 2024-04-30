//------------------------------
//�S����:�����S
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Mediapipe.CopyCalculatorOptions.Types;

public class CS_Wind : MonoBehaviour
{

    [SerializeField]private GameObject m_objWind;
    private Vector3 m_vec3CameraPos;
    private float m_fWindPower = 1.0f;

    private float m_nowTime = 0.0f;
    private bool m_bCreated = false;
    [SerializeField]private bool m_bDelete = true;
    [SerializeField]private float m_fDeleteTime = 3.0f;
    //���̌���
    public enum E_WINDDIRECTION
    {
        NONE,   //�Ȃ�
        LEFT,   //��
        RIGHT,  //�E
        UP      //��
    }

    [SerializeField,Header("���̌���")]
    //���̌����ϐ�
    private E_WINDDIRECTION m_eWindDirection = E_WINDDIRECTION.NONE;

    public E_WINDDIRECTION WindDirection
    {
        set
        {
            m_eWindDirection = value;
        }
    }

    public bool DeleteFlag 
    {
        set 
        {
            m_bDelete = value;
        }
    }

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
    void Start()
    {
        m_bCreated =false;
    }

    // Update is called once per frame
    void Update()
    {
        m_nowTime += Time.deltaTime;
        

        bool isTimeOver = m_nowTime > m_fDeleteTime;
        if (isTimeOver && m_bDelete) Destroy(this.gameObject);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ɏՓ˂����畗�̉e����^���� �ǉ��F��
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<CS_Player>().WindMove(m_eWindDirection, m_fWindPower);
            //Destroy(this.gameObject);
            const float lastTime = 0.25f;// 
            m_nowTime = m_fDeleteTime - lastTime;
        }

        // �����𖼑O�Ŕ��f
        if (collision.gameObject.name != this.name)return;

        CS_Wind other = collision.gameObject.GetComponent<CS_Wind>();


        // �������������f
        bool isSameDirection =  this.m_eWindDirection == other.m_eWindDirection;
        bool isUp = this.m_eWindDirection == E_WINDDIRECTION.UP || other.m_eWindDirection == E_WINDDIRECTION.UP;
        if (isSameDirection && !isUp) { Debug.Log("����"); return; }

        // ���̗͂��������炢�����f
        float ThisScale = this.transform.localScale.x;
        float OtherScale = collision.transform.localScale.x;

        float addPower = OtherScale + ThisScale;
        const float tolerance = 1.0f;// ���e�͈�
        bool isTolerance = addPower < tolerance && addPower > -tolerance;
        if (!isTolerance) { Debug.Log("���e�͈�"); return; }

        // �����ς݂�
        if (m_bCreated || other.m_bCreated) { Debug.Log("�����ς�"); return; }
        // ������̕��̐���
        Vector3 pos = m_vec3CameraPos;
        pos.z = 0;
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        GameObject createdWind = GameObject.Instantiate(m_objWind, pos, rotation);
        CS_Wind cswind = createdWind.GetComponent<CS_Wind>();
        cswind.WindDirection = E_WINDDIRECTION.UP;   //������ɐݒ�@�ǉ��F��
        cswind.m_fWindPower = addPower;
        cswind.enabled = true;                                  //�X�N���v�g�I�t�ɂȂ�Ӗ��킩���
        createdWind.GetComponent<BoxCollider2D>().enabled = true;

        other.m_bCreated = true;
        m_bCreated = true;
    }



}
