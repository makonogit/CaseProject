using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LineController : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�C���ɂ����鎞��")]
    private float m_fadeInDuration = 2f; // �t�F�[�h�C���ɂ����鎞�ԁi�b�j
    private float m_currentAlpha = 0f;
    private float m_fadeTimer = 0f;
    private CS_ResultController m_rController;
   

    private SpriteRenderer[] m_childRenderers; // �q�I�u�W�F�N�g��Renderer�R���|�[�l���g

    void Start()
    {
        // �q�I�u�W�F�N�g��Renderer�R���|�[�l���g���擾
        m_childRenderers = GetComponentsInChildren<SpriteRenderer>();

        Debug.Log("�J�E���g��" + m_childRenderers.Length);

        // �S�Ă̎q�I�u�W�F�N�g�̓����x��0�i���S�����j�ɐݒ�
        foreach (SpriteRenderer renderer in m_childRenderers)
        {
            SetTransparency(renderer, 0f);
        }
    }

    void Update()
    {
        bool allOpaque = true;//�S�ĕs����
        m_fadeTimer += Time.deltaTime;
        m_currentAlpha = Mathf.Lerp(0f, 1f, m_fadeTimer / m_fadeInDuration);

        if (m_fadeTimer > m_fadeInDuration)
        {
            Destroy(this);
            return;
        }
        // �S�Ă̎q�I�u�W�F�N�g�̓����x�����X�ɕs�����ɂ��Ă���
        foreach (SpriteRenderer renderer in m_childRenderers)
        {

            SetTransparency(renderer, m_currentAlpha);
        }

        if (allOpaque) 
        { 
            //�X�e�[�g��ς��ď���
        }
    }

    // �����x��ݒ肷��w���p�[�֐�
    private void SetTransparency(SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    //���U���g�R���g���[���[�̃Z�b�g
    //�����FCS_ResultController�^
    public void SetResControlloer(CS_ResultController _resCtrl)
    {
        m_rController = _resCtrl;
    }
}
