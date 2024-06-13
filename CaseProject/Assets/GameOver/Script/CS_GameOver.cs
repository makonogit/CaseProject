//-----------------------------------------------
//�S���ҁF�����S
//�Q�[���I�[�o�[���̏���
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //��U�A���Ƃ�SceneManager�ɕύX

public class CS_GameOver : MonoBehaviour
{
    [SerializeField, Header("�n���h�g���b�L���O����X�N���v�g")]
    private CS_HandSigns m_csHandsign;

    [SerializeField, Header("GameOver��Animator")]
    private Animator m_aGameOverAnim;

    private bool m_isXDir = true;

    private bool m_isDir = false;

    [SerializeField, Header("CS_Creater")]
    private CS_Creater m_cscreater;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_csHandsign) { Debug.LogWarning("CS_HandSign���ݒ肳��Ă��܂���"); }
        if (!m_aGameOverAnim) { Debug.LogWarning("Animator���ݒ肳��Ă��܂���"); }

        //CS_HandSigns.OnCreateWinds -= CS_Creater.CreateWinds;

        //���̃C�x���g����
        m_cscreater.DeleteEvent();
        //�C�x���g�o�^
        CS_HandSigns.OnCreateWinds += CloseBook;

    }

    private void OnDestroy()
    {

        //�C�x���g����
        CS_HandSigns.OnCreateWinds -= CloseBook;
    }


    //�{����鏈��
    void CloseBook(Vector3 _position, Vector3 _direction)
    {
        //SE�Đ�
        ObjectData.m_csSoundData.PlaySE("Book");

        //�߂�����������ۑ�
        m_isDir = (_direction.x < 0.0f && !m_isXDir) || (_direction.x > 0.0f && m_isXDir);

        //�A�j���[�V�����Đ�
        m_aGameOverAnim.SetTrigger("Finish");
        //Debug.Log(isDir);

    }

    void SceneChange()
    {
        //�����ɂ���ăV�[���ړ�
        if (m_isDir) { SceneManager.LoadScene("SelectScene"); }
        else { SceneManager.LoadScene("GameScene"); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
