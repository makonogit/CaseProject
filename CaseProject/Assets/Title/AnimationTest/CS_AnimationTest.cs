//------------------------------
// �S���ҁF�����@����
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AnimationTest : MonoBehaviour
{
    private Animator m_anim;//�A�j���[�^�[

    public enum MAN_STATUS//�A�j���[�V��������X�v���C�g�̃X�e�[�^�X
    {
        BOY,  //�j��
        UNCLE,//��������
        MACHO //�}�b�`��
    }

    MAN_STATUS m_manState = MAN_STATUS.BOY;

    public MAN_STATUS ManStatus
    {
        set
        {
            m_manState = value;
            SetManStatus(m_manState);
        }
        get
        {
            return m_manState;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetManStatus(MAN_STATUS _state)
    {
        if(_state == MAN_STATUS.BOY)
        {
            m_anim.SetBool("macho", false);
        }
        else if(_state == MAN_STATUS.UNCLE)
        {
            m_anim.SetBool("uncle", true);
        }
        else if (_state == MAN_STATUS.MACHO)
        {
            m_anim.SetBool("uncle", false);
            m_anim.SetBool("macho", true);
        }
    }
}
