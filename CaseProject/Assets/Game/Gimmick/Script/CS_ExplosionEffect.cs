//-----------------------------------------------
//�S���ҁF��������
//�����̏���
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ExplosionEffect : MonoBehaviour
{
    [SerializeField, Header("�����A�j���[�V�����̃g���K�[��")]
    private string m_triggerName;


    private bool isExplotion = false;
    // Start is called before the first frame update
    void Start()
    {
        
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
     
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���������̂��v���C���[�^�O�H
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�v���C���[����������");
            GetComponent<Animator>().SetTrigger(m_triggerName);//�A�j���[�V�����Đ�
            isExplotion = true;
        }
    }
}
