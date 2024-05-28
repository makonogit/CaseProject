//-----------------------------------------------
//�S���ҁF��������
//�㏸��
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_RisingStar : MonoBehaviour
{
    private Rigidbody2D m_rb;//���W�b�h�{�f�B
    private Vector3 m_prevVelocity;//�O�̃��W�b�h�{�f�B�̑��x
    private Vector3 m_backUpVelocity;//�ۑ��p���W�b�h�{�f�B�̑��x
    int m_nAccumulateCount = 0;//���߃J�E���g
    float m_fDescentCount = 0;//���~���̃J�E���g
    bool m_isAccumulate = false;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_prevVelocity = m_rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //���~�������ߒ�����Ȃ��H
        if (m_rb.velocity.y < 0.0f && !m_isAccumulate)
        {
            m_fDescentCount += 0.5f;

            if(m_fDescentCount >= 100.0f) { m_fDescentCount = 100.0f; }
        }
        //�O�̑��x��茻�݂̑��x���傫���Ȃ�͂�+�␳����
        bool isReady = m_prevVelocity.y <= 0.0f && m_rb.velocity.y > 0.0f;

        if(isReady && !m_isAccumulate)
        {
            m_isAccumulate = true;//���߂�true
            m_backUpVelocity = m_rb.velocity;//���݂̑��x��ۑ�
        }
        m_prevVelocity = m_rb.velocity;

        if (!m_isAccumulate) { return; }


        //���݂̑��x��0�ɂ��ė��߃J�E���g�����Z
        Vector3 velocity = m_rb.velocity;
        Debug.Log("���x" + m_rb.velocity);
        velocity.y = 0.0f;
        m_rb.velocity = velocity;
        m_nAccumulateCount++;

      
        //�{����ݒ�
        float magnification = 1.0f;
        magnification = (m_fDescentCount / 100f > 0.33f) ? 2f : magnification;
        magnification = (m_fDescentCount / 100f > 0.66f) ? 3f : magnification;

        //���߃J�E���g�����ȏ�H
        if (m_nAccumulateCount >= (int)m_fDescentCount)
        {
            m_rb.AddForce(Vector2.up * (1 + (magnification /10f)), ForceMode2D.Impulse);//�͂�������
            //�J�E���g������
            m_nAccumulateCount = 0;
            m_fDescentCount = 0;
            m_isAccumulate = false;//���߂�false
        }
    }
}
