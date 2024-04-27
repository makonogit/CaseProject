//------------------------------
// �S���ҁF���� ���o
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;

public class CS_Creater : MonoBehaviour 
{ 
   
    [Header("���̐ݒ�")]
    [SerializeField]private GameObject m_objWind;                   // ������
    [SerializeField] private float m_fCreateDelayOfWind = 0.5f;     // �����f�B���C
    [SerializeField] private float m_fWindMoveSpeed = 0.125f;       // ���������̔{��
    [SerializeField] private float m_fWindPower = 1;                // �����̔{��
    private float m_fCreatedWindTime = 0;                           // �����o�ߎ���

    [Header("���̐ݒ�")]
    [SerializeField] private GameObject m_objThunder;                   // ������
    [SerializeField] private float m_fCreateDelayOfThunder = 0.125f;    // �����f�B���C
    [SerializeField] private float m_fThunderMoveSpeed = 10;        // ���������̔{��
    [SerializeField] private float m_fThunderPower = 1;                 // �����̔{��
    private float m_fCreatedThunderTime = 0;                            // �����o�ߎ���

    [Header("�J�̐ݒ�")]
    [SerializeField] private GameObject m_objRain;              // ������
    [SerializeField] private int m_fRainCreatePerSecond= 10;    // �J�̖��b������
    [SerializeField] private float m_fRainCreateRange = 25;      // �������锼�a
    private float m_fRequiredCreateNum = 0.0f;                  // �K�v�Ȑ�����

    // Start is called before the first frame update
    private void Start()
    {
        // �C�x���g�ݒ�
        CS_HandSigns.OnCreateWinds += CreateWind;
        CS_HandSigns.OnCreateThunders += CreateThunder;
        CS_HandSigns.OnCreateRains += CreateRain;
    }
    
    // Update is called once per frame
    private void Update()
    {
        m_fCreatedWindTime += Time.deltaTime;
        m_fCreatedThunderTime += Time.deltaTime;
    }

    // ���̐�������֐��i�w�� subscribe�j
    // �����F�l�����w�̕t�����̈ʒu
    // �����F�i�s����
    // �߂�l�F�Ȃ�
    //
    // �������A�߂�l�̓C�x���g�̔��s���Ō��܂�B
    //
    private void CreateWind(Vector3 position,Vector3 direction) 
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

    // ���𐶐�����֐�
    // �����F�����ʒu
    // �����F����������
    // �߂�l�F�Ȃ�
    private void CreateThunder(Vector3 position,Vector3 direction) 
    {
        if (m_fCreatedThunderTime >= m_fCreateDelayOfThunder) 
        {
            // �����̐ݒ�
            Quaternion rotation = Quaternion.EulerAngles(0, 0, -180*Mathf.Deg2Rad);
            // ���̐���
            GameObject obj = GameObject.Instantiate(m_objThunder, position, rotation);
            CS_Thunder cs_thunder = obj.GetComponent<CS_Thunder>();  //���̃X�N���v�g�擾

            cs_thunder.Movement = direction.magnitude * m_fThunderMoveSpeed;
            //cs_wind.WindPower = direction.magnitude * m_fWindPower;

            // ���Ԃ̃��Z�b�g
            m_fCreateDelayOfThunder = 0;
        }
    }
    // �J�𐶐�����֐�
    // �����F�����ʒu
    // �����F���ɈӖ��Ȃ�
    // �߂�l�F�Ȃ�
    private void CreateRain(Vector3 position, Vector3 direction)
    {
        // �������鐔��ǉ�����
        m_fRequiredCreateNum += m_fRainCreatePerSecond * Time.deltaTime;
        for(int i = 0; i <= m_fRequiredCreateNum; i++) 
        {
            float x = Random.Range(0, 7);
            float y = Random.Range(0, 7);
            float radius = Random.Range(0, m_fRainCreateRange);
            
            Vector3 rad = new Vector3(Mathf.Cos(x), Mathf.Sin(y));
            rad.Normalize();

            Vector3 Pos = position + rad*radius;
            // �����̐ݒ�
            Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
            // �J�̐���
            GameObject obj = GameObject.Instantiate(m_objRain, Pos, rotation);
            m_fRequiredCreateNum -= 1;
        }
    }
    
}