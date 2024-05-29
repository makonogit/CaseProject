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
            Destroy(this.gameObject);
        }
    }
     
   
    public void StartExplosion()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Animator>().SetTrigger(m_triggerName);//�A�j���[�V�����Đ�
        isExplotion = true;
        Debug.Log("�����J�n");
    }

   
}
