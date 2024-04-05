//------------------------------
// �S���ҁF���� ���o
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;
using static CS_HandPoseData;

public class CS_Creater : MonoBehaviour 
{
    [Header("���̃I�u�W�F�N�g")]
    [SerializeField]private GameObject m_objWind;

    

    [ Header("�����f�B���C")]
    [SerializeField] private float m_fCreateDelayOfWind = 0.5f;
    [Header("���̈ړ����x�{��")]
    [SerializeField] private float m_fWindMoveSpeed = 0.125f;
    [Header("���̗͔{��")]
    [SerializeField] private float m_fWindPower = 1;
    // �������o�ߎ���
    private float m_fCreatedWindTime = 0;
    // Start is called before the first frame update
    private void Start()
    {
        // �C�x���g�ݒ�
        CS_HandSigns.OnCreatWinds += CreateWid;
    }
    
    // Update is called once per frame
    private void Update()
    {
        m_fCreatedWindTime += Time.deltaTime;
    }

    // ���̐�������֐��i�w�� subscribe�j
    // �����F�l�����w�̕t�����̈ʒu
    // �����F�i�s����
    // �߂�l�F�Ȃ�
    //
    // �������A�߂�l�̓C�x���g�̔��s���Ō��܂�B
    //
    private void CreateWid(Vector3 position,Vector3 direction) 
    {
        // ���Ԃ��߂�����
        if (m_fCreatedWindTime >= m_fCreateDelayOfWind)
        {
            // �����̐ݒ�
            float angle = Mathf.Atan2(direction.y, direction.x);
            Quaternion rotation = Quaternion.EulerAngles(0, 0, angle);
            // ���̐���
            GameObject obj = GameObject.Instantiate(m_objWind, position, rotation);
            CS_Wind cs_wind = obj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾

            cs_wind.Movement = direction.magnitude * m_fWindMoveSpeed;
            cs_wind.WindPower = direction.magnitude * m_fWindPower;
            
            // ���Ԃ̃��Z�b�g
            m_fCreatedWindTime = 0;
        }
    }
}