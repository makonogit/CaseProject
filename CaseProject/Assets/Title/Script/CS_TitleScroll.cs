//-----------------------------------------------
//�S���ҁF��������
//�J�����̃X�N���[��
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleScroll : MonoBehaviour
{
    private Camera m_camera;
    [SerializeField, Header("�S�[���I�u�W�F�N�g")]
    private GameObject m_goalBackGround;
    private Vector3 m_goalPos;
    [SerializeField, Header("�X�N���[�����x")]
    private float m_scrollSpeed = 1.0f; //�X�N���[�����x
    [SerializeField, Header("�����x")]
    private float m_acceleration = 0.1f; // �����x
    [SerializeField, Header("�������J�n���鋗���̊���(0~1.0)")]
    public float m_decelerationRatio = 0.7f; // �������J�n���鋗���̊���
    [SerializeField, Header("�����J�n���Ă���~�܂�܂ł̎���")]
    float m_decelerationTime = 2.0f;

    [SerializeField, Header("�^�C�g���Ǘ��X�N���v�g")]
    private CS_TitleHandler m_titleHandler;

    private float m_DistanceAll;//�ڕW�܂ł̋���

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        m_goalPos = m_goalBackGround.transform.position;
        m_DistanceAll = Vector3.Distance(m_goalPos, this.transform.position);//�ڕW�܂ł̋����ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C�g���̏�Ԃ��X�N���[���łȂ��Ȃ�I��
        if(m_titleHandler.TitleState != CS_TitleHandler.TITLE_STATE.SCROLL) { return; }

        // �J�����̏�����ւ̈ړ��ʂ��擾
        m_camera.transform.Translate(Vector3.up * m_scrollSpeed * Time.deltaTime);

        float nowDistance = Vector3.Distance(m_goalPos, m_camera.transform.position);//���݂̋���
        float decelerationDistance = m_DistanceAll * (1.0f - m_decelerationRatio);//�������J�n���鋗��

        // �S�[���ɓ��B�������ǂ������`�F�b�N
        Vector2 goalToCamera = m_goalPos - m_camera.transform.position;
        //�ڕW�l��y����H
        if (goalToCamera.y < 0.0f) 
        {
            Vector3 pos = new Vector3(m_goalPos.x, m_goalPos.y, m_camera.transform.position.z);
            m_camera.transform.position = pos; // �S�[���ʒu�ɃJ�������ړ�
            m_scrollSpeed = 0.0f;
            m_titleHandler.TitleState = CS_TitleHandler.TITLE_STATE.STOP;//�X�g�b�v��Ԃ�
        }
        else if (nowDistance < decelerationDistance)
        { 
            //�����J�n�_�܂ł̋���
            float distanceToDecelerate = m_DistanceAll - nowDistance;

            //�������鎞��
            float decelerationTime = m_decelerationTime * (distanceToDecelerate / decelerationDistance);

            //����
            m_scrollSpeed = Mathf.Lerp(m_scrollSpeed, 0, Time.deltaTime / decelerationTime);
        }
        else
        {
            //����
            m_scrollSpeed += m_acceleration * Time.deltaTime;
        }

        //���x�𐧌�
        m_scrollSpeed = Mathf.Clamp(m_scrollSpeed, 0.0f, Mathf.Infinity);
    }
}
