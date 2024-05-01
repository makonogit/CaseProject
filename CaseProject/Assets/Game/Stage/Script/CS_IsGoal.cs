//-----------------------------------------------
//�S���ҁF�����S
//����(�S�[��)�N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_IsGoal : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField, Header("�S�[���I�u�W�F�N�g")]
    private Transform m_tGoleTrans;

    [SerializeField, Header("�v���C���[��Transform")]
    private Transform m_tPlayerTrans;

    [SerializeField, Header("�S�[���ړ����x")]
    private float m_fGoalSpeed = 0.1f;
        
    private bool m_IsGoal = false;

    void Start()
    {
        
        if (!m_tGoleTrans) { Debug.LogWarning("�S�[��Transform���ݒ肳��Ă��܂���"); }
        if (!m_tPlayerTrans) { Debug.LogWarning("�v���C���[Transaform���ݒ肳��Ă��܂���"); }

    }

    // Update is called once per frame
    void Update()
    {
        //�S�[�����ĂȂ�������X�V���Ȃ�
        if (!m_IsGoal) { return; }

        //if(m_tPlayerTrans.position == m_tGoleTrans.position) { return; }

        //if (m_tPlayerTrans.localScale != m_tPlayerTrans.localScale) { return; }

        //�S�[�����W�܂ňړ�
        m_tPlayerTrans.position = Vector3.MoveTowards(m_tPlayerTrans.position, m_tGoleTrans.position, m_fGoalSpeed * Time.deltaTime);

        //�S�[���̐��Ɠ����悤�ɃX�P�[���k��
        m_tPlayerTrans.localScale = Vector3.MoveTowards(m_tPlayerTrans.localScale, m_tGoleTrans.localScale, m_fGoalSpeed / 2 * Time.deltaTime);


    }

    //--------------------------------------------
    // �Ȑ���̈ړ��֐�(�x�W�F�Ȑ�)
    // ����1�F���x
    // ����2�F�n�_
    // ����3�F����_
    // ����4�F�I�_
    //--------------------------------------------
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //�ǋL�F����2024.04.03
            //�Q�[���I�[�o�[�t���O��false�ɐݒ�
            //CS_ResultController.GameOverFlag = false;
            //SceneManager.LoadScene("Result");

            Debug.Log("�S�[��");

            //�S�[������
            m_IsGoal = true;

        }
    }

}
