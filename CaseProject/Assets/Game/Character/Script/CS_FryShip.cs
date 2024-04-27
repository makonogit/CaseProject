//------------------------------------
//�S���ҁF�����S
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_FryShip : MonoBehaviour
{
    private Transform m_tThisTransform;  //���g��Transform

    [SerializeField, Header("�����Ă��鍂��")]
    private float m_fFloatHight = 0.1f;
    [SerializeField, Header("�����Ă��鑬�x")]
    private float m_fFloatSpeed = 1.0f;
    [SerializeField, Header("�ړ���")]
    private float m_fMove = 1.2f;

    private Vector3 m_v3StartPos;       //�J�n�ʒu

    private bool m_isWindMove = false;  //���ɉe�����󂯂Ă��邩
    private float m_fWindPower;         //���̉e����
    private Quaternion m_qTargetAngle;  //�e���ɂ���ČX���p�x
    private Vector2 m_v2TargetVelocity; //�e���ɂ���Đi�ޕ���
    private Vector3 m_v3WindVec;        //���̕���
    private float m_fSpeed = 0;

    [SerializeField,Header("�ő�HP")]
    private const float m_MaxHP = 100.0f;
    private float m_HP;                   //HP




    public float HP
    {
        set
        {
            m_HP = value;
        }
        get
        {
            return m_HP;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CS_HandSigns.OnCreateWinds += MoveByWind;
        m_tThisTransform = this.transform;
        m_v3StartPos = m_tThisTransform.position;

        m_HP = m_MaxHP; //HP��ݒ�

    }

    // Update is called once per frame
    void Update()
    {
        //�D�̂ӂ�ӂ핂���Ă���\��
        Vector3 move = new Vector3(m_fMove * Time.deltaTime, fluffy, 0);
        m_tThisTransform.position += move +m_v3WindVec*Time.deltaTime;

        ////====���ɉe������鏈��===
        //if(!m_isWindMove)
        //{
        //    //m_tThisTransform.rotation = Quaternion.Lerp(m_tThisTransform.rotation, m_qTargetAngle,Time.deltaTime * m_fWindPower);

        //    m_fSpeed += 2.5f *Time.deltaTime;
            
        //}
        //Vector2 currentpos = (Vector2)transform.position;

        //Vector2 targetpos = currentpos + m_v2TargetVelocity * m_fSpeed * Time.deltaTime;

        //transform.position = Vector2.Lerp(currentpos, targetpos, Time.deltaTime * (m_fWindPower * 100.0f));

        //m_fSpeed = Mathf.Min(0, m_fSpeed);

        if (m_v3WindVec.magnitude > 0) 
        {
            float length = m_v3WindVec.magnitude - 1.0f * Time.deltaTime;
            if(length < 0)length = 0;
            m_v3WindVec = m_v3WindVec.normalized * length;
        }
    }
    // �D���ӂ�ӂ킷�鎞�̈ړ��l��Ԃ�
    private float fluffy{
        get {
            return Mathf.Sin(Time.time * m_fFloatSpeed) * m_fFloatHight;
        }
    }


    ////�������蔲����u��
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
    //    {
    //        //���̉e���͂��擾����
    //        CS_Wind wind = collision.transform.GetComponent<CS_Wind>();
    //        m_fWindPower = wind.WindPower;

    //        //�Փ˂��������x�N�g�����擾
    //        Vector2 direction = collision.transform.position - transform.position;
    //        direction.Normalize();

    //        //�x�N�g������p�x���v�Z
    //        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //        //angle -= 90.0f; //�I�u�W�F�N�g����]�ς݂Ȃ̂Œ���
    //        // m_qTargetAngle = Quaternion.Euler(0, 0, angle);

    //        //�i�s�����̎擾
    //        m_v2TargetVelocity = direction;
    //        m_fSpeed = (m_fWindPower * -1);
    //        //���̉e�����󂯂�
    //        m_isWindMove = true;
    //    }

    //    if(collision.gameObject.tag == "Cloud")
    //    {
    //        Debug.Log("aaaaaaaaaaaaaaaaaaa");
    //    }

    //}

    ////���蔲������
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    //�������蔲������e�����Ȃ���
    //    if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
    //    {
    //        m_isWindMove = false;
    //        Destroy(collision.gameObject);
    //    }
    //}

    // ���ɂ���ē����D���֐�
    // �����F���ɈӖ��Ȃ�
    // �����F���̕���
    // �߂�l�F�Ȃ�
    private void MoveByWind(Vector3 pos,Vector3 dir) 
    {
        Vector3 normal = dir.normalized;
        normal.z = 0;
        normal.Normalize();

        float x = normal.x * normal.x;
        float y = normal.y * normal.y;
        
        // ����������
        bool isVertical =  x < y;
        if (isVertical) normal.x = 0;
        else normal.y = 0;
        normal.Normalize();

        float power = dir.magnitude * 0.5f;
        m_v3WindVec = normal*power;

    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.name == "Wind")
    //    {
         
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
        
    //}
}

