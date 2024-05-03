//------------------------------------
//�S���ҁF�����S
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------
//���[�v�M�~�b�N�N���X
//------------------------------------
public class CS_Warp : MonoBehaviour
{
    [SerializeField, Header("���[�v��I�u�W�F�N�g")]
    private GameObject m_WarpObj;

    private bool m_IsWarp = false;  //���[�v�t���O

    //public bool WARPFLG
    //{
    //    set
    //    {
    //        m_IsWarp = value;   
    //    }
    //    get
    //    {
    //        return m_IsWarp;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�ȊO�ɓ���������I��
        if(collision.transform.tag != "Player") { return; }

        CS_Warp cswarp = m_WarpObj.GetComponent<CS_Warp>();
        
        //���W�����̂܂ܓ���ւ�
        if(!m_IsWarp) collision.transform.position = m_WarpObj.transform.position;

        cswarp.m_IsWarp = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�v���C���[�ȊO�ɓ���������I��
        if (collision.transform.tag != "Player") { return; }

        //���[�v���甲�����烏�[�v��ԏI��
        CS_Warp cswarp = m_WarpObj.GetComponent<CS_Warp>();
        if(!cswarp.m_IsWarp)m_IsWarp = false;
    }


}
