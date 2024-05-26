//-----------------------------------------------
//�S���ҁF�����S
//����(�S�[��)�N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using UnityEngine.SceneManagement;  //��U����SceneManager���g�p

public class CS_IsGoal : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField, Header("�S�[���I�u�W�F�N�g")]
    private Transform m_tGoleTrans;

    [SerializeField, Header("�v���C���[��Transform")]
    private Transform m_tPlayerTrans;

    [SerializeField, Header("�v���C���[��Rigidbody")]
    private Rigidbody2D m_rPlayerRigid;

    [SerializeField, Header("���̎qTransForm")]
    private Transform m_tStarChild;

    [SerializeField, Header("�J��������X�N���v�g")]
    private CS_CameraControl m_csCamCtrl;

    [SerializeField, Header("�S�[���ړ����x")]
    private float m_fGoalSpeed = 0.1f;
        
    private bool m_IsGoal = false;

    void Start()
    {
        
        if (!m_tGoleTrans) { Debug.LogWarning("�S�[��Transform���ݒ肳��Ă��܂���"); }
        if (!m_tPlayerTrans) { Debug.LogWarning("�v���C���[Transaform���ݒ肳��Ă��܂���"); }
        if (!m_rPlayerRigid) { Debug.LogWarning("�v���C���[��RigidBody���ݒ肳��Ă��܂���"); }
        if (!m_tStarChild) Debug.LogWarning("���̎qTransform���ݒ肳��Ă��܂���");
        if (!m_csCamCtrl) { Debug.LogWarning("�J��������X�N���v�g���ݒ肳��Ă��܂���"); }

    }

    // Update is called once per frame
    void Update()
    {
        //�S�[�����ĂȂ�������X�V���Ȃ�
        if (!m_IsGoal) { return; }

        //if(m_tPlayerTrans.position == m_tGoleTrans.position) { return; }

        //if (m_tPlayerTrans.localScale != m_tPlayerTrans.localScale) { return; }

        //�S�[�����W�܂ňړ�
        m_tStarChild.position = Vector3.MoveTowards(m_tStarChild.position, m_tGoleTrans.position, m_fGoalSpeed * Time.deltaTime);

        //�S�[���̐��Ɠ����悤�ɃX�P�[���k��
        m_tStarChild.localScale = Vector3.MoveTowards(m_tStarChild.localScale,/* m_tGoleTrans.localScale*/Vector3.zero, m_fGoalSpeed / 10 * Time.deltaTime);

        //�ړI�n�ɒB������V�[���J��
        if (m_tPlayerTrans.position == m_tGoleTrans.position) 
        {
            SceneManager.LoadScene("SelectScene");
            return; 
        }
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

            m_csCamCtrl.TARGET = m_tStarChild.gameObject;

            //�d�͂𖳌�
            m_rPlayerRigid.constraints = RigidbodyConstraints2D.FreezeAll;

            //�S�[������
            m_IsGoal = true;

        }
    }

}
