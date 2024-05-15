//-----------------------------------------------
//�S���ҁF��������
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_OpenBook : MonoBehaviour
{
    public GameObject m_turningBook;//�y�[�W�߂���p�̃Q�[���I�u�W�F�N�g
    private Animator m_animator;//�A�j���[�^�[

    public GameObject TurningBook
    {
        set
        {
            m_turningBook = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetTrigger("openBook");//�J���A�j���[�V�����Đ�
    }

    private void Update()
    {
       
        // ���݂�AnimatorStateInfo���擾
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        // �A�j���[�V�������I���������ǂ������`�F�b�N
        bool isAnimationFinish = stateInfo.IsName("AC_OpenBook") && stateInfo.normalizedTime >= 0.9f;
        if(!isAnimationFinish) { return; }

        m_animator.speed = 0f;

        // �y�[�W���߂���{�̊�����true�ɐݒ�
        if (m_turningBook != null)
        {
            m_turningBook.SetActive(true);//�y�[�W�߂���p�̃I�u�W�F�N�g�̊�����true
            Destroy(this.gameObject);
        }
    
    }
}
