//-----------------------------------------------
//�S���ҁF��������
//�o�q�̏���
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TwinsStar : MonoBehaviour
{
    [SerializeField, Header("�o�q�̃I�u�W�F�N�g")]
    //���̑o�q�I�u�W�F�N�g
    private GameObject[] m_twinsObject = new GameObject[2];

    [SerializeField, Header("�V���E�X�{�̂̃X�v���C�g�����_���[")]
    private SpriteRenderer m_srSubstance;
    [SerializeField, Header("�V���E�X�̎w�̃X�v���C�g�����_���[�i���E�ǂ����ł��j")]
    private SpriteRenderer m_srFinger;

    [SerializeField, Header("������")]
    private float m_fAmplitude = 3.0f;  // �U�ꕝ
    [SerializeField, Header("�X�s�[�h")]
    private float m_fSpeed = 1.0f;  //�ړ��X�s�[�h

    [SerializeField, Header("�W�����v�͂�+�␳�l")]
    private float m_fJumpPlusPower = 0.0f;
    [SerializeField, Header("�W�����v�͂�-�␳�l")]
    private float m_fJumpMinusPower = 0.0f;

    private Vector3 m_initialPosition;//�����ʒu
    private float m_fElapsedTime = 0.0f;//�o�ߎ���

    private Rigidbody2D m_rb;//���W�b�h�{�f�B
    private Vector3 m_prevVelocity;

    private Vector3 m_prevPosition;

    private int m_nNowStar = 0;//���ݓ����Ă��鐯�ԍ�

    bool m_isMoveingStar = true;



    //�W�����v�̕␳�l���擾
    public float JumpPower
    {
        get
        {
            if (m_nNowStar == 0)
            {
                return m_fJumpPlusPower;
            }
            else
            {
                return -m_fJumpPlusPower;
            }
        }
    }

    public bool IsMoveingStar
    {
        get
        {
            return m_isMoveingStar;
        }
    }

    // Start is called before the first frame update��
    void Start()
    {
        m_initialPosition = m_twinsObject[0].transform.localPosition;

        //�o�q��OrderInLayer��ݒ�
        SpriteRenderer spriteRenderer0 = m_twinsObject[0].GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer1 = m_twinsObject[1].GetComponent<SpriteRenderer>();
        int fingerOrder = m_srFinger.sortingOrder;
        spriteRenderer0.sortingOrder = fingerOrder - 1;
        spriteRenderer1.sortingOrder = fingerOrder - 2;

        m_rb = GetComponent<Rigidbody2D>();
        m_prevVelocity = m_rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //�W�����v�␳����
        AddJumpPower();

        if (!m_isMoveingStar) { return; }

        // �o�ߎ��Ԃ��X�V
        m_fElapsedTime += Time.deltaTime;

        //�E�΂߂ɓ���
        float x = m_fAmplitude * Mathf.Sin(m_fElapsedTime * m_fSpeed - Mathf.PI);
        float y = m_fAmplitude * Mathf.Sin(m_fElapsedTime * m_fSpeed - Mathf.PI);
        Vector2 newPosition = m_initialPosition + new Vector3(x, y, 0);

        //���̈ʒu�X�V
        m_twinsObject[m_nNowStar].transform.localPosition = newPosition;

        //���C���[�̍X�V����
        SpriteRenderer spriteRenderer = m_twinsObject[m_nNowStar].GetComponent<SpriteRenderer>();

        int backOrder = m_srSubstance.sortingOrder - 1;//�V���E�X�̓��̂̌��̃��C���[�ԍ�
        int frontOrder = m_srFinger.sortingOrder - 1;//�V���E�X�̎w�̂ЂƂ��̃��C���[�ԍ�
        //�O�̃|�W�V������荶���ɂ���H
        if (newPosition.x < m_prevPosition.x)
        {
            spriteRenderer.sortingOrder = frontOrder;//�w�̃��C���[�̂ЂƂ��̃��C���[�ɐݒ�
        }
        else
        {
            spriteRenderer.sortingOrder = backOrder;//���̂̃��C���[�̂ЂƂ��̃��C���[�ɐݒ�
        }

        m_prevPosition = newPosition;//�|�W�V�����f�[�^�X�V

        //initialPosition�Ƃ̃x�N�g�������AinitializePosition���E���ɂ��邩�A������0.1�ȉ��Ȃ�speed��0�ɂ��鏈��
        float distance = Vector3.Distance(newPosition, m_initialPosition);
        if (newPosition.x > m_initialPosition.x && distance <= 0.1f && spriteRenderer.sortingOrder == frontOrder)
        {
            m_twinsObject[m_nNowStar].transform.localPosition = m_initialPosition;
            m_isMoveingStar = false;
        }
    }

    private void AddJumpPower()
    {
        //�O�̑��x��茻�݂̑��x���傫���Ȃ�͂�+�␳����
        bool isAddPower = m_prevVelocity.y <= 0.0f && m_rb.velocity.y > 0.0f;
        if (isAddPower)
        {
            m_rb.AddForce(Vector3.up * m_fJumpPlusPower, ForceMode2D.Impulse);
            Debug.Log("�{�␳");
        }

        m_prevVelocity = m_rb.velocity;
    }

    //��̐��̃��C���[�����ւ���
    public void SwapStar()
    {
        SpriteRenderer spriteRenderer0 = m_twinsObject[m_nNowStar].GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer1 = m_twinsObject[(m_nNowStar + 1) % 2].GetComponent<SpriteRenderer>();
        int tmp = spriteRenderer0.sortingOrder;
        spriteRenderer0.sortingOrder = spriteRenderer1.sortingOrder;
        spriteRenderer1.sortingOrder = tmp;
        
    }

    //���̈ړ����ăX�^�[�g
    public void RestartMoveStar()
    {
        m_isMoveingStar = true;
        m_nNowStar++;
        m_nNowStar = m_nNowStar % 2;
        m_fElapsedTime = 0.0f;
    }
}
