//------------------------------
// �S���ҁF���� ���o
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class CS_Cloud : MonoBehaviour
{

    [Header("�ϐ�")]
    [SerializeField] private GameObject m_objCloudParticle;
    private float m_fCloudSize = 1.0f;
    [SerializeField] private float m_fMaxSize = 2.0f;
    [SerializeField] private float m_fGrowingValue = 0.1f;
    [SerializeField] private float m_fResetSizeValue = 0.01f;
    [SerializeField] private Vector3 m_vec3StartSize;

    [Header("���̐ݒ�")]
    [SerializeField] private GameObject m_objThunder;                   // ������
    [SerializeField] private float m_fCreateDelayOfThunder = 0.125f;    // �����f�B���C
    [SerializeField] private float m_fThunderMoveSpeed = 10;        // ���������̔{��
    [SerializeField] private float m_fThunderPower = 1;                 // �����̔{��
    private float m_fCreatedThunderTime = 0;                            // �����o�ߎ���
    
    [Header("�J")]
    [SerializeField] private GameObject m_objRain;              // ������
    [SerializeField] private int m_fRainCreatePerSecond = 10;    // �J�̖��b������
    [SerializeField] private float m_fRainCreateRange = 25;      // �������锼�a
    [SerializeField] private float m_fDecrease = 0.01f;            // �T�C�Y������
    private float m_fRequiredCreateNum = 0.0f;                  // �K�v�Ȑ�����

    // Start is called before the first frame update
    private void Start()
    {
        //CS_HandSigns.OnCreateRains += EventRain;
        //CS_HandSigns.OnCreateThunders += CreateThunder;
    }

    // Update is called once per frame
    private void Update()
    {
        m_fCreatedThunderTime += Time.deltaTime;
    }
    private void OnParticleCollision(GameObject other)
    {
        bool isSameTheObject = other == m_objCloudParticle;
        bool isNotMaxofCloudSize = m_fCloudSize <= m_fMaxSize;
        bool isGrowing = isSameTheObject && isNotMaxofCloudSize;

        if (isGrowing) Growing();
        if (!isNotMaxofCloudSize) SceneManager.LoadScene("ResultScene"); // SizeReset();
        
    }
    // �_���傫���Ȃ�֐�
    // �����F�Ȃ�
    // �߂�l�F�Ȃ�
    private void Growing() 
    {
        // 1�b�Ԃɑ傫���Ȃ�l
        float growingRate = m_fGrowingValue * Time.deltaTime;
        m_fCloudSize += growingRate;
        transform.localScale = m_vec3StartSize * m_fCloudSize;
    }
    
    // �_�T�C�Y�����Z�b�g
    // �����F�Ȃ�
    // �߂�l�F�Ȃ�
    private void SizeReset()
    {
        transform.localScale = (m_vec3StartSize * m_fResetSizeValue);
        m_fCloudSize = m_fResetSizeValue;
    }

    // �J�𐶐�����C�x���g�ݒ�֐�
    // �����F���ɈӖ��Ȃ�
    // �����F���ɈӖ��Ȃ�
    // �߂�l�F�Ȃ�
    private void EventRain(Vector3 pos ,Vector3 dir) 
    {
        // �T�C�Y�����Z�b�g�T�C�Y�����傫����
        bool isSizeOverZero = m_fCloudSize > m_fResetSizeValue;
        if (isSizeOverZero) CreateRain();
    }
    // �J�𐶐�����֐�
    // �����F�Ȃ�
    // �߂�l�F�Ȃ�
    private void CreateRain() 
    {
        // �������鐔��ǉ�����
        m_fRequiredCreateNum += m_fRainCreatePerSecond * Time.deltaTime;
        for (int i = 0; i <= m_fRequiredCreateNum; i++)
        {
            // �����ʒu�̃����_���擾
            float x = Random.Range(0, 7);
            float y = Random.Range(0, 7);
            float radius = Random.Range(0, m_fRainCreateRange);

            Vector3 rad = new Vector3(Mathf.Cos(x), Mathf.Sin(y));
            rad.Normalize();
            Vector3 offset = new Vector3(0, -5.0f);
            // �ʒu�̐ݒ�
            Vector3 Pos = transform.position + offset + rad * radius;
            // �����̐ݒ�
            Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
            // �J�̐���
            GameObject obj = GameObject.Instantiate(m_objRain, Pos, rotation);
            m_fRequiredCreateNum -= 1;

            // �_������������
            m_fCloudSize -= m_fDecrease;
            transform.localScale = m_vec3StartSize * m_fCloudSize;
        }
    }

    // ���𐶐�����֐�
    // �����F�����ʒu
    // �����F����������
    // �߂�l�F�Ȃ�
    private void CreateThunder(Vector3 position, Vector3 direction)
    {
        Debug.Log("�T���_�[");
        if (m_fCreatedThunderTime >= m_fCreateDelayOfThunder)
        {
            
            // �����̐ݒ�
            Quaternion rotation = Quaternion.EulerAngles(0, 0, -180 * Mathf.Deg2Rad);
            // ���̐���
            GameObject obj = GameObject.Instantiate(m_objThunder, transform.position, rotation);
            CS_Thunder cs_thunder = obj.GetComponent<CS_Thunder>();  //���̃X�N���v�g�擾

            cs_thunder.Movement = direction.magnitude * m_fThunderMoveSpeed;
            //cs_wind.WindPower = direction.magnitude * m_fWindPower;

            // ���Ԃ̃��Z�b�g
            m_fCreateDelayOfThunder = 0;
        }
    }

}