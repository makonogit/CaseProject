//------------------------------
// �S���ҁF�����@����
// �_���Փ˂��Ă��邩
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CheckTouchClouds : MonoBehaviour
{
    [SerializeField, Header("TitleHandler")]
    private CS_TitleHandler m_titleHandler;
    private bool m_isTouchCloud = true;//�_���G��Ă��邩
    private float m_waitTime = 0.0f;//�_�����ׂĉ�ʂ����������̑ҋ@����

    private void Update()
    {
        //�_���G��Ă��Ȃ�
        if (!m_isTouchCloud)
        {
            m_waitTime += Time.deltaTime;//�ҋ@���Ԃ����Z
            
        }

        //�P�b��
        if(m_waitTime>=1.0f)
        {
            m_titleHandler.LogoActiveTrue();//GAME��END�̃��S��\������
            
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cloud"))
        {
            // Cloud�^�O�̃I�u�W�F�N�g��Trigger����o���ꍇ
            // ���ׂĂ�Cloud�I�u�W�F�N�g�����ꂽ���ǂ������m�F����
            CheckIfAllCloudsExited();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Cloud"))
        {
            // Cloud�^�O�̃I�u�W�F�N�g���R���C�_�[�͈͓̔��ɂ���ꍇ
            m_isTouchCloud = true;

            m_waitTime = 0.0f;
        }
    }

    private void CheckIfAllCloudsExited()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        bool isStillTouchingCloud = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Cloud"))
            {
                // �܂�Cloud�ɐG��Ă���I�u�W�F�N�g������ꍇ�̓t���O�𗧂Ă�
                isStillTouchingCloud = true;
                break;
            }
        }

        // ���ׂĂ�Cloud�I�u�W�F�N�g�����ꂽ�ꍇ�Am_isTouchCloud��false�ɐݒ肷��
        m_isTouchCloud = isStillTouchingCloud;
    }

   
}
