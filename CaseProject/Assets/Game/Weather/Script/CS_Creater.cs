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
    [SerializeField] private float m_fWindPower = 1;                // �����̔{��
    


    // Start is called before the first frame update
    private void Start()
    {
        // �C�x���g�ݒ�
        CS_HandSigns.OnCreateWinds += CreateWind;
        
    }
    
    // Update is called once per frame
    private void Update()
    {
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
        Debug.Log("Create Wind");
        
        Vector3 dir = direction;
        dir.y = 0;
        dir.Normalize();

        Vector3 pos =GetWindPosition(dir);
        // �����̐ݒ�
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        // ���̐���
        GameObject obj = GameObject.Instantiate(m_objWind, pos, rotation);
        // �T�C�Y�ύX
        Vector3 scale = obj.transform.localScale;
        scale.x = direction.magnitude * m_fWindPower * dir.x;
        obj.transform.localScale = scale;

        CS_Wind cs_wind = obj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾
        cs_wind.WindDirection = dir.x > 0 ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT;�@//���̌����ݒ�@�ǉ��F�����S
        cs_wind.WindPower = direction.magnitude * m_fWindPower;
        cs_wind.SetCameraPos = this.transform.position;
    }
    
    // ���̐����ʒu�����߂�֐�
    // �����F���̕���
    // �߂�l�F���̐����ʒu
    private Vector3 GetWindPosition(Vector3 direction) 
    {
        Vector3 pos = this.transform.position;
        pos.z = 0;// ���s�͕K�v�Ȃ�
        const float offsetX = 10;
        if (direction.x < 0) pos.x += offsetX;
        else pos.x -= offsetX;

        return pos;
    }
}