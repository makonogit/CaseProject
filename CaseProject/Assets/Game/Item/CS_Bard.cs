using UnityEngine;
//------------------------------------
//�S���ҁF�����S
//------------------------------------

//------------------------------------
//���N���X
//------------------------------------
public class CS_Bard : CS_Obstacle
{
    [SerializeField, Header("�ړ����x")]
    private float m_fMoveSpeed = 0.1f;

    //���̌���
    private Vector3 m_v3Directon = Vector3.right;

    private void Start()
    {
        //�I�u�W�F�N�g�̃X�P�[���ɂ���Č�����ݒ�
        if(transform.localScale.x < 0) { m_v3Directon = Vector3.left; }
    }

    private void Update()
    {
        //�����Ă�������Ɉړ�
        transform.Translate(m_v3Directon * m_fMoveSpeed * Time.deltaTime);

    }

}
