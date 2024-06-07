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

    [SerializeField, Header("�S�[���ړ����x")]
    private float m_fGoalSpeed = 0.1f;


    [SerializeField, Header("�S�[���p�̐����w�i")]
    private SpriteRenderer m_srSign;

    [SerializeField, Header("�S�[���p�̐���UI")]
    private SpriteRenderer m_srSignComplete;

    [SerializeField, Header("�S�[���\�����x")]
    private float m_fGoalSignViewSpeed = 2.0f;

    private float m_fGoalSignAlpha = 0.0f;  //�S�[���p�w�i�̓����x

    private bool m_IsGoalView = false;      //�S�[�����̕\���t���O(�Ȃ񂩂���������Ȃ�������
    private bool m_IsLightChange = false;   //���]�t���O

    private bool m_IsGoal = false;

    void Start()
    {
        
        if (!m_tGoleTrans) { Debug.LogWarning("�S�[��Transform���ݒ肳��Ă��܂���"); }
        if (!m_srSign) { Debug.LogWarning("����Spiterender���ݒ肳��Ă��܂���"); }
        if (!m_srSignComplete) { Debug.LogWarning("��������UI��Spiterender���ݒ肳��Ă��܂���"); }

        m_srSign.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);
        m_srSignComplete.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        //�S�[�����ĂȂ�������X�V���Ȃ�
        if (!m_IsGoal) { return; }

        //if(m_tPlayerTrans.position == m_tGoleTrans.position) { return; }

        //if (m_tPlayerTrans.localScale != m_tPlayerTrans.localScale) { return; }

        //�ړI�n�ɒB������S�[���p�I�u�W�F�N�g�\���J�n
        if (ObjectData.m_tStarChildTrans.position == m_tGoleTrans.position) 
        {
            //���������X�ɖ��邭
            if(!m_IsLightChange && ObjectData.m_lGlobalLight.intensity < 5.0f) { ObjectData.m_lGlobalLight.intensity += 5.0f * Time.deltaTime; } 
            else { m_IsLightChange = true; }

            if (m_IsLightChange) 
            {
                if(ObjectData.m_lGlobalLight.intensity > 1.0f) { ObjectData.m_lGlobalLight.intensity -= 5.0f * Time.deltaTime;  }
                else { m_IsGoalView = true; }
            }
            
        }
        else
        {
            //�S�[�����W�܂ňړ�
            ObjectData.m_tStarChildTrans.position = Vector3.MoveTowards(ObjectData.m_tStarChildTrans.position, m_tGoleTrans.position, m_fGoalSpeed * Time.deltaTime);

            //�S�[���̐��Ɠ����悤�ɃX�P�[���k��
            ObjectData.m_tStarChildTrans.localScale = Vector3.MoveTowards(ObjectData.m_tStarChildTrans.localScale,/* m_tGoleTrans.localScale*/Vector3.zero, m_fGoalSpeed / 10 * Time.deltaTime);
        }


        if (m_IsGoalView)
        {
            Debug.Log("HAIKEI");
            //�S�[���p�̔w�i�����X�ɕ\��
            m_fGoalSignAlpha += m_fGoalSignViewSpeed * Time.deltaTime;

            m_srSign.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);
            m_srSignComplete.color = new Color(1.0f, 1.0f, 1.0f, m_fGoalSignAlpha);

            //�\������������V�[���J��
            if (m_fGoalSignAlpha > 1.0f)
            {
                SceneManager.LoadScene("ScelectScene");
            }
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //�ǋL�F����2024.04.03
            //�Q�[���I�[�o�[�t���O��false�ɐݒ�
            //CS_ResultController.GameOverFlag = false;
            //SceneManager.LoadScene("Result");

            Debug.Log("�S�[��");

            ObjectData.m_csCamCtrl.TARGET = ObjectData.m_tStarChildTrans.gameObject;

            //�d�͂𖳌�
            Rigidbody2D playerd = ObjectData.m_tPlayerTrans.GetComponent<Rigidbody2D>();
            playerd.constraints = RigidbodyConstraints2D.FreezeAll;

            //�S�[������
            m_IsGoal = true;

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
    }

}
