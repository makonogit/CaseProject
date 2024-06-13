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
    [SerializeField] private float m_fWindPowerMagnification = 1;       // �����̔{��

    [SerializeField, Header("Playerscript")]
    private CS_Player m_player;                            // �v���C���[��script �ǉ��F��

    // Start is called before the first frame update
    private void Start()
    {
        // �C�x���g�ݒ�
        CS_HandSigns.OnCreateWinds += CreateWinds;

        if (!m_player) { Debug.LogWarning("Player��script���ݒ肳��Ă��܂���"); }

    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void DeleteEvent()
    {
        //�Q�[���I�[�o�[���ɌĂԁ@�ǉ��F��
        CS_HandSigns.OnCreateWinds -= CreateWinds; 
    }


    private void CreateWinds(Vector3 position, Vector3 direction) 
    {
        //����SE���Đ�
        ObjectData.m_csSoundData.PlaySE("Wind");

        Vector3 pos = position;
        pos.z = 0;
        Vector3 dir = direction;
        dir.y = 0;
        dir.Normalize();
        bool IsLeftHand =dir.x > 0;
        // �����̐ݒ�
        Quaternion rotation = Quaternion.identity;
        // ���̐���
        GameObject obj = GameObject.Instantiate(m_objWind, pos, rotation);
        if (IsLeftHand) obj.transform.localScale = InvertScaleX(obj);
        CS_Wind cswind = obj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾
        cswind.WindDirection = IsLeftHand ? CS_Wind.E_WINDDIRECTION.LEFT : CS_Wind.E_WINDDIRECTION.RIGHT; //���̌����ݒ�@�ǉ��F�����S
        cswind.WindPower = direction.magnitude * m_fWindPowerMagnification;
        cswind.SetCameraPos = this.transform.position;
        cswind.DeleteFlag = true;
        cswind.SetCS_Player(m_player);
    }
    // �X�P�[��X�𔽓]����
    // �������F���]�������I�u�W�F
    // �߂�l�F���]�����l
    private Vector3 InvertScaleX(GameObject obj) 
    {
        Vector3 scale = obj.transform.localScale;
        scale.x *= -1;
        return scale;
    }

    private void OnDestroy()
    {
        // �C�x���g�ݒ�
        CS_HandSigns.OnCreateWinds -= CreateWinds;
    }

    
}