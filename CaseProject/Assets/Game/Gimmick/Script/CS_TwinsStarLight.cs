using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TwinsStarLight : MonoBehaviour
{
    [SerializeField, Header("�ő�X�P�[��")]
    private float m_fMaxScale = 2.0f; //�ő�X�P�[��
    private float m_fMinScale = 0.0f; //�ŏ��X�P�[��
    [SerializeField, Header("�X�P�[�����O���x")]
    public float m_fScalingSpeed = 1.0f; //�X�P�[���ω����x

    private bool scalingUp = true; // �X�P�[�����g�咆���k������

    [SerializeField]
    private CS_TwinsStar m_twinsStar;

    private void Start()
    {
        if (!m_twinsStar) { Debug.LogWarning("CS_TwinsStarLight��CS_TwinsStar������܂���B"); }

        transform.localScale = Vector3.one * m_fMinScale;
    }

    void Update()
    {
        //���̈ړ��������Ȃ�I��
        if (m_twinsStar.IsMoveingStar) { return; }

        // ���݂̃X�P�[�����擾
        Vector3 nowScale = transform.localScale;

        if (scalingUp)
        {
            //�X�P�[�����g��
            nowScale += Vector3.one * m_fScalingSpeed * Time.deltaTime;

            //�X�P�[�����ő�l�ɒB������k�����[�h�ɐ؂�ւ�
            if (nowScale.x >= m_fMaxScale)
            {
                nowScale = Vector3.one * m_fMaxScale;
                scalingUp = false;

                m_twinsStar.SwapStar();//��̐��̃��C���[�����ւ���
            }
        }
        else
        {
            //�X�P�[�����k��
            nowScale -= Vector3.one * m_fScalingSpeed * Time.deltaTime;

            //�X�P�[�����ŏ��l�ɒB������g�僂�[�h�ɐ؂�ւ�
            if (nowScale.x <= m_fMinScale)
            {
                nowScale = Vector3.one * m_fMinScale;
                scalingUp = true;
                m_twinsStar.RestartMoveStar();//���̈ړ����ĊJ
            }
        }

        //�X�P�[�����X�V
        transform.localScale = nowScale;
    }
}
