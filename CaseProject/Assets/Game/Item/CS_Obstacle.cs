//------------------------------------
//�S���ҁF�����S
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//------------------------------------
//��Q���N���X
//------------------------------------
public class CS_Obstacle : MonoBehaviour
{
    [SerializeField, Header("�m�b�N�o�b�N�̋���")]
    private float m_fKnockBackForce = 1.0f;

    [SerializeField, Header("������")]
    private float m_fSpeedDownRate = 1.0f;

    [SerializeField, Header("�U����")]
    private float m_fAttackPower = 0.0f;

    private float m_fNowUpPower = 0.0f;

    [SerializeField, Header("���g��Tarnsform")]
    private Transform m_tThisTrans;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //�v���C���[�ƏՓ˂����猸��������
        if (collision.transform.tag == "Player")
        {
            m_fNowUpPower = collision.transform.GetComponent<CS_Player>().UPPOWER;
            collision.transform.GetComponent<CS_Player>().UPPOWER = -m_fSpeedDownRate;
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�v���C���[�ƏՓ˂����猸��������
        if (collision.transform.tag == "Player")
        {

            collision.transform.GetComponent<CS_Player>().UPPOWER = -collision.transform.GetComponent<CS_Player>().UPPOWER;
            collision.transform.GetComponent<CS_Player>().UPPOWER = m_fNowUpPower;
        }
    }

    //�����蔻��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�ƏՓ˂�����m�b�N�o�b�N������
        if(collision.transform.tag == "Player")
        {
            //���������߂ĕ����Ɨ͂�ݒ�
            Vector3 Direction = m_tThisTrans.position - collision.transform.position;
            collision.transform.GetComponent<CS_Player>().KnockBack(Direction,m_fKnockBackForce);
        }
    }
}
