//-----------------------------------------------
//�S���ҁF�����S
//�Ύ��N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Fire : MonoBehaviour
{

    [SerializeField, Header("�΂�HP")]
    private float m_FireHP = 100.0f;

    //�΂̍ő�HP
    private float m_MaxHP = 0.0f;

    float FIREHP
    {
        get
        {
            return m_FireHP;
        }
        set
        {
            m_FireHP = value;
        }
    }

    private Vector3 m_Scale;        //�΂̃X�P�[��

    // Start is called before the first frame update
    void Start()
    {
        m_Scale = this.transform.localScale;    //�X�P�[���̕ۑ�
        m_MaxHP = m_FireHP;                     //�ő�HP�̕ۑ�
    }

    // Update is called once per frame
    void Update()
    {
        //HP���Ȃ��Ȃ��������
        if(m_FireHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�J�ɓ���������
        if (collision.gameObject.tag == "Rain")
        {
            m_FireHP -= 10.0f;
            //�X�v���C�g�̏k��(�ő�HP�ɍ��킹��)
            Vector3 DecreaseRate = new Vector3((m_Scale.x / m_MaxHP) * 5.0f,(m_Scale.y / m_MaxHP) * 5.0f, 0.0f);
            this.transform.localScale -= DecreaseRate;
        }
    }

}
