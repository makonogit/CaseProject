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
    [SerializeField] private GameObject m_objWind;         // ������
    [SerializeField] private float m_fWindPower = 1;       // �����̔{��

    [SerializeField, Header("Playerscript")]
    private CS_Player m_player;                            // �v���C���[��script �ǉ��F��

    // Start is called before the first frame update
    private void Start()
    {
        // �C�x���g�ݒ�
        CS_HandSigns.OnCreateWinds += CreateWind;

        if (!m_player) { Debug.LogWarning("Player��script���ݒ肳��Ă��܂���"); }

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

        //Vector3 scale = obj.transform.localScale;
        //scale.x = direction.magnitude * m_fWindPower * dir.x;
        //obj.transform.localScale = scale;

        //���̗͂��畗�I�u�W�F�N�g�̐�������
        int nWindObjNum = (int)((direction.magnitude * m_fWindPower * dir.x) / 10) - 1;
        nWindObjNum = Mathf.Abs(nWindObjNum);
        float fWindX = pos.x;

        //���I�u�W�F�N�g�𐶐�����
        while (nWindObjNum > 0)
        {
            //���̃T�C�Y�����炵�Ĕz�u
            fWindX += 4.0f * dir.x;
            Vector3 Pos = new Vector3(fWindX, pos.y, 0.0f);

            GameObject windobj = Instantiate(m_objWind, Pos, rotation);
            CS_Wind cswind = windobj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾
            cswind.WindDirection = dir.x > 0 ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT; //���̌����ݒ�@�ǉ��F�����S
            cswind.WindPower = direction.magnitude * m_fWindPower * dir.x;
            cswind.SetCameraPos = this.transform.position;
            cswind.DeleteFlag = true;

            if (cswind.WindDirection == CS_Wind.E_WINDDIRECTION.LEFT)
            {
                windobj.transform.localScale = new Vector3(windobj.transform.localScale.x * -1, windobj.transform.localScale.y, 1.0f);
            }

            if (nWindObjNum == 1) 
            {
                cswind.IsWindEnd = true;
            }

            nWindObjNum--;

        }

        CS_Wind cs_wind = obj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾
        cs_wind.WindDirection = dir.x > 0 ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT;�@//���̌����ݒ�@�ǉ��F�����S
        cs_wind.WindPower = direction.magnitude * m_fWindPower * dir.x;
        cs_wind.SetCameraPos = this.transform.position;
        cs_wind.DeleteFlag = true;

        //�v���C���[�̈ړ��֐��𒼐ڌĂяo��
        float windpower = direction.magnitude * m_fWindPower * dir.x;
        windpower = Mathf.Abs(windpower);
        m_player.WindMove(cs_wind.WindDirection, windpower);
    }
    
    // ���̐����ʒu�����߂�֐�
    // �����F���̕���
    // �߂�l�F���̐����ʒu
    private Vector3 GetWindPosition(Vector3 direction) 
    {
        Vector3 pos = this.transform.position;
        pos.z = 0;// ���s�͕K�v�Ȃ�
        const float offsetX = 8;
        if (direction.x < 0) pos.x += offsetX;
        else pos.x -= offsetX;

        pos.y -= 2.0f;

        return pos;
    }
}