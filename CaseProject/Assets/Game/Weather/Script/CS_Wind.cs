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

    float m_nowTime = 0.0f;


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
        // �v���C���[�ɏՓ˂����畗�̉e����^���� �ǉ��F��
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<CS_Player>().WindMove(m_eWindDirection, m_fWindPower);
            Destroy(this.gameObject);
        }

        // �����𖼑O�Ŕ��f
        if (collision.gameObject.name != this.name)return;
        float ThisScale = this.transform.localScale.x;
        float OtherScale = collision.transform.localScale.x;

        // �������������f
        bool isThisDirection = ThisScale < 0;
        bool isOtherDirection = OtherScale < 0;
        bool isSameDirection = (isThisDirection && isOtherDirection) || (!isThisDirection && !isOtherDirection);
        if (isSameDirection) return;

        // ���̗͂��������炢�����f
        float addPower = OtherScale + ThisScale;
        const float tolerance = 1.0f;// ���e�͈�
        bool isTolerance = addPower < tolerance && addPower > -tolerance;
        if (!isTolerance) return;

        // ������̕��̐���
        Vector3 pos = m_vec3CameraPos;
        pos.z = 0;
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        CS_Wind cswind = m_objWind.GetComponent<CS_Wind>();
        cswind.WindDirection = E_WINDDIRECTION.UP;   //������ɐݒ�@�ǉ��F��
        cswind.m_fWindPower = addPower;
        cswind.enabled = true;                                  //�X�N���v�g�I�t�ɂȂ�Ӗ��킩���
        m_objWind.GetComponent<BoxCollider2D>().enabled = true;
        GameObject.Instantiate(m_objWind, pos, rotation);
        
        Destroy(collision.gameObject);
        Destroy(this.gameObject);

    }



}
