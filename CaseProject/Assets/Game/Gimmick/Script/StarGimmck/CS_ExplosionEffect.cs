//-----------------------------------------------
//�S���ҁF��������
//�����̏����F�����̃G�t�F�N�g�I�u�W�F�N�g�ɃA�^�b�`
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ExplosionEffect : MonoBehaviour
{
    [Header("�����̃G�t�F�N�g�I�u�W�F�N�g�ɃA�^�b�`")]
    [Header("�V���E�X�̎����Ă��鐯�̎q�I�u�W�F�N�g��ExplosionEffectPrefab����������")]
    [SerializeField, Header("�����A�j���[�V�����̃g���K�[��")]
    private string m_triggerName;

    [SerializeField, Header("���������Ƃ��̌����x(0.0�`1.0)")]
    private float m_fDeceleration = 1.0f;

    [SerializeField, Header("�V���E�X�{�̂�RigidBody2D")]
    private Rigidbody2D m_rb;

    private bool isExplotion = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isExplotion) { return; }

        Animator animator = GetComponent<Animator>();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("AC_Explosion")) { return; }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //�A�j���[�V�������I�������H
        if (stateInfo.normalizedTime >= 0.9f)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
     
   
    public void StartExplosion()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Animator>().SetTrigger(m_triggerName);//�A�j���[�V�����Đ�
        isExplotion = true;
        //����������
        Vector3 playerVel = m_rb.velocity;
        playerVel *= m_fDeceleration;
        m_rb.velocity = playerVel;
        Debug.Log("�����J�n");
    }

   
}
