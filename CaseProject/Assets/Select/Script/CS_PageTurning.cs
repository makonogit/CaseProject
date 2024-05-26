//------------------------------
// �S���ҁF�����@����
//�@�y�[�W�߂���
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;

public class CS_PageTurning : MonoBehaviour
{
    [SerializeField, Header("�n���h�T�C��")]
    private CS_HandSigns m_handSigns;

    [SerializeField, Header("�X�e�[�W�I���X�N���v�g")]
    private CS_StageSelect m_csStageSelect;

    private bool isFacingRight = true;

    //---------------------------------------
    // �X�e�[�W���\���A�j���[�V�����p
    [SerializeField, Header("�X�e�[�W���X�N���v�g")]
    private CS_StageData m_csStageData;
    [SerializeField, Header("�X�e�[�W���\���X�s�[�h")]
    private float m_fStageViewSpeed = 1.0f;
    private bool m_IsStageUpdate = false;   //�X�e�[�W�X�V�t���O
    private GameObject m_gSelectStageObj;   //�X�e�[�W���I�u�W�F�N�g
    private float m_fStageAlpha = 0.0f;     //�X�e�[�W���X�v���C�g���l
    private SpriteRenderer m_srStageSprite; //�X�e�[�W���X�v���C�g

    // Start is called before the first frame update
    void Start()
    {
        CS_HandSigns.OnCreateWinds += PageTurning;
    }

    // Update is called once per frame
    void Update()
    {
        //�X�e�[�W���\��
        if(m_IsStageUpdate) { StageView();}

    }

    private void Flip()
    {
        // ���݂�Scale���擾���AX�������ɔ��]������
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }



    //�y�[�W�߂���̃A�j���[�V�����𔭓�
    private void PageTurningAnimation()
    {
        //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AM_PageTurning")) { return; }
        GetComponent<Animator>().SetTrigger("pageTurningAnim");
    }

    //�{���߂���
    void PageTurning(Vector3 _position, Vector3 _direction)
    {
        //�X�e�[�W�`�撆��������X�V���Ȃ�
        if(m_IsStageUpdate) { return; }
        //Destroy�g�������Ȃ��I�ς������I
        Destroy(m_gSelectStageObj); //�O��̃X�e�[�W������
        m_fStageAlpha = 0.0f;       //�X�e�[�W���̃��l��������


        bool isFlip = (_direction.x < 0.0f && !isFacingRight) || (_direction.x > 0.0f && isFacingRight);

        if (isFlip)
        {
            m_csStageSelect.StageUpdate(-1);        //�X�e�[�W�X�V
            Flip();
        }
        else 
        { 
            m_csStageSelect.StageUpdate(1);         //�X�e�[�W�X�V
        }


        //---------------------------------------------
        // �X�e�[�W���̃X�v���C�g�����_�[��o�^
        GameObject SelectStageObj = m_csStageData.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_gSelectStagePrefab;
        if (!SelectStageObj) { Debug.LogWarning("�Z���N�g�p�X�e�[�W�I�u�W�F�N�g���ݒ肳��Ă��܂���"); }
        m_srStageSprite = SelectStageObj.GetComponent<SpriteRenderer>();
        m_gSelectStageObj = Instantiate(SelectStageObj);

        m_IsStageUpdate = true;                     //�X�e�[�W�X�V�t���O

        PageTurningAnimation();//�A�j���[�V�������s
    }

    //----------------------------------------------
    //�@�X�e�[�W���\��(Animator�C�x���g�ŌĂяo��)
    //----------------------------------------------
    void StageView()
    {
        //SpriteRenderer���o�^����Ă��Ȃ�������X�V���Ȃ�
        if (!m_srStageSprite) { return; }

        //�X�e�[�W���̃��l��1�ɂȂ�����(�\�����ꂽ��)�I��
        if(m_fStageAlpha >= 1.0f)
        {
            m_IsStageUpdate = false;
            return;
        }

        //�X�e�[�W���̃��l���X�V
        m_fStageAlpha += m_fStageViewSpeed * Time.deltaTime;
        m_srStageSprite.color = new Color(1.0f, 1.0f, 1.0f, m_fStageAlpha);

    }
}
