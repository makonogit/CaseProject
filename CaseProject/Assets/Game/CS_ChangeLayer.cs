//-----------------------------------------------
//�S���ҁF���z��
//���C���[�`�F���W�N���X
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ChangeLayer : MonoBehaviour
{
    [SerializeField,Header("�g��E�k���W��")]
    private float m_fScaleFactor = 1.3f; // �g��E�k���̃X�P�[���t�@�N�^�[

    //��ԊǗ��ϐ�
    private int[] m_nScale = new int[3]{ -1, 0, 1 };
    private int m_nScaleState = 1;

    //�n���h�g���b�L���O����̓��͂ɕύX���镔��
    public KeyCode scaleKey01 = KeyCode.Space; // �g��E�k�����s���L�[
    public KeyCode scaleKey02 = KeyCode.Space; // �g��E�k�����s���L�[
    //---

    void Update()
    {
        // �S�ẴI�u�W�F�N�g���擾
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

        // �w�肵���L�[�������ꂽ��
        if (Input.GetKeyDown(scaleKey01))
        {
            if (m_nScale[2] > m_nScale[m_nScaleState])
            {
                m_nScaleState++;
                // �e�I�u�W�F�N�g�ɑ΂��ď������s��
                foreach (GameObject obj in objects)
                {
                    // �I�u�W�F�N�g���v���C���[�łȂ��ꍇ�Ɋg��E�k�����s��
                    if (!obj.CompareTag("Player"))
                    {
                        // �I�u�W�F�N�g�̃X�P�[����ύX����
                        obj.transform.localScale *= m_fScaleFactor;
                    }
                }

            }

        }

        if (Input.GetKeyDown(scaleKey02))
        {
            if (m_nScale[0] < m_nScale[m_nScaleState])
            {
                m_nScaleState--;
                // �e�I�u�W�F�N�g�ɑ΂��ď������s��
                foreach (GameObject obj in objects)
                {
                    // �I�u�W�F�N�g���v���C���[�łȂ��ꍇ�Ɋg��E�k�����s��
                    if (!obj.CompareTag("Player"))
                    {
                        // �I�u�W�F�N�g�̃X�P�[����ύX����
                        obj.transform.localScale /= m_fScaleFactor;
                    }
                }
            }

        }

    }
}
