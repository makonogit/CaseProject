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

    private Vector3 m_v3StartPos;       //�J�n�ʒu

    private bool m_isWindMove = false;  //���ɉe�����󂯂Ă��邩
    private float m_fWindPower;         //���̉e����
    private Quaternion m_qTargetAngle;  //�e���ɂ���ČX���p�x
    private Vector2 m_v2TargetVelocity; //�e���ɂ���Đi�ޕ���



    // Start is called before the first frame update
    void Start()
    {
        m_tThisTransform = this.transform;
        m_v3StartPos = m_tThisTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //�D�̂ӂ�ӂ핂���Ă���\��
        float newY = m_v3StartPos.y + Mathf.Sin(Time.time * m_fFloatSpeed) * m_fFloatHight;
        m_tThisTransform.position = new Vector3(m_tThisTransform.position.x, newY, m_tThisTransform.position.z);

        //====���ɉe������鏈��===
        if(m_isWindMove)
        {
            m_tThisTransform.rotation = Quaternion.Lerp(m_tThisTransform.rotation, m_qTargetAngle,Time.deltaTime * m_fWindPower);

            Vector2 currentpos = (Vector2)transform.position;

            Vector2 targetpos = currentpos + m_v2TargetVelocity * Time.deltaTime;

            transform.position = Vector2.Lerp(currentpos, targetpos, Time.deltaTime * (m_fWindPower * 100.0f));
        }

    }

    //�������蔲����u��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
        {
            //���̉e���͂��擾����
            CS_Wind wind = collision.transform.GetComponent<CS_Wind>();
            m_fWindPower = wind.WindPower;

            //�Փ˂��������x�N�g�����擾
            Vector2 direction = collision.transform.position - transform.position;
            direction.Normalize();

            //�x�N�g������p�x���v�Z
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90.0f; //�I�u�W�F�N�g����]�ς݂Ȃ̂Œ���
            m_qTargetAngle = Quaternion.Euler(0, 0, angle);

            //�i�s�����̎擾
            m_v2TargetVelocity = direction * -10.0f;

            //���̉e�����󂯂�
            m_isWindMove = true;
        }
    }

    //���蔲������
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�������蔲������e�����Ȃ���
        if (collision.gameObject.name == "Wind" || collision.gameObject.name == "Wind(Clone)")
        {
            m_isWindMove = false;
            Destroy(collision.gameObject);
        }
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

