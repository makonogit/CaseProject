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

    [SerializeField, Header("�V�[���}�l�[�W���[")]
    private CS_SceneManager m_csSceneManager;

    [SerializeField, Header("�{��Animator")]
    private Animator m_ABookAnimator;

    private bool isFacingRight = true;

    //---------------------------------------
    // �X�e�[�W���\���A�j���[�V�����p
    [SerializeField, Header("�X�e�[�W���X�N���v�g")]
    private CS_StageData m_csStageData;
    [SerializeField, Header("�X�e�[�W���\���p�X�v���C�g")]
    private SpriteRenderer m_srStageInfo;

    // [SerializeField, Header("�X�e�[�W���\���X�s�[�h")]
    //private float m_fStageViewSpeed = 1.0f;
    //private bool m_IsStageUpdate = false;   //�X�e�[�W�X�V�t���O
    //rivate GameObject m_gSelectStageObj;   //�X�e�[�W���I�u�W�F�N�g
    //private float m_fStageAlpha = 0.0f;     //�X�e�[�W���X�v���C�g���l
    //private SpriteRenderer m_srStageSprite; //�X�e�[�W���X�v���C�g

    // Start is called before the first frame update
    void Start()
    {
        //BGM�̍Đ�
        ObjectData.m_csSoundData.PlayBGM("StartBGM", ObjectData.m_fBGMTime);

        // �C�x���g�o�^
        CS_HandSigns.OnCreateWinds += PageTurning;
    }

    // Update is called once per frame
    void Update()
    {
        //�X�e�[�W���\��
        //if(m_IsStageUpdate) { StageView();}

        //�{�����
        if (m_handSigns.IsClap() && m_ABookAnimator.GetBool("Finish") == false) { m_ABookAnimator.SetBool("Finish", true); }

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
        m_ABookAnimator.SetTrigger("pageTurningAnim");
    }

    //�{���߂���
    void PageTurning(Vector3 _position, Vector3 _direction)
    {
        //SE�Đ�
        ObjectData.m_csSoundData.PlaySE("Book");

        bool isFlip = (_direction.x < 0.0f && !isFacingRight) || (_direction.x > 0.0f && isFacingRight);

        if (isFlip)
        {
            if (m_csStageSelect.StageUpdate(-1) == -1) { return; }        //�X�e�[�W�X�V
            Flip();
        }
        else 
        {
            if (m_csStageSelect.StageUpdate(1) == -1) { return; }         //�X�e�[�W�X�V
        }

        //m_srStageSprite = SelectStageObj.GetComponent<SpriteRenderer>();
        //m_gSelectStageObj = Instantiate(SelectStageObj);

        //m_IsStageUpdate = true;                     //�X�e�[�W�X�V�t���O

        PageTurningAnimation();//�A�j���[�V�������s

        //---------------------------------------------
        // �X�e�[�W���̃X�v���C�g�����_�[��o�^
        Sprite SelectStageSprite = m_csStageData.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_sSelectStageSprite;
        if (!SelectStageSprite) { Debug.LogWarning("�Z���N�g�p�X�e�[�W�X�v���C�g���ݒ肳��Ă��܂���"); }
        m_srStageInfo.sprite = SelectStageSprite;
    }

    //----------------------------------------------
    //�@�X�e�[�W���\��(�����ĂȂ�)
    //----------------------------------------------
    void StageView()
    {
        //SpriteRenderer���o�^����Ă��Ȃ�������X�V���Ȃ�
        //if (!m_srStageSprite) { return; }

        ////�X�e�[�W���̃��l��1�ɂȂ�����(�\�����ꂽ��)�I��
        //if(m_fStageAlpha >= 1.0f)
        //{
        //    m_IsStageUpdate = false;
        //    return;
        //}

        ////�X�e�[�W���̃��l���X�V
        //m_fStageAlpha += m_fStageViewSpeed * Time.deltaTime;
        //m_srStageSprite.color = new Color(1.0f, 1.0f, 1.0f, m_fStageAlpha);

    }

    //--------------------------------
    //�@�{�������̏���(�V�[���ړ�)
    //---------------------------------
    void CloseBook()
    {
        //SE�Đ�
        ObjectData.m_csSoundData.PlaySE("BookClose");
        CS_HandSigns.OnCreateWinds -= PageTurning;
        m_csSceneManager.LoadScene(CS_SceneManager.SCENE.GAME);
    }
}
