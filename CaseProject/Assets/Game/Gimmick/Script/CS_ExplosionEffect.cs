//-----------------------------------------------
//担当者：中島愛音
//爆発の処理
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ExplosionEffect : MonoBehaviour
{
    [SerializeField, Header("爆発アニメーションのトリガー名")]
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
        //アニメーションが終了した？
        if (stateInfo.normalizedTime >= 0.9f)
        {
            Destroy(this.gameObject);
        }
    }
     
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //当たったのがプレイヤータグ？
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが当たった");
            GetComponent<Animator>().SetTrigger(m_triggerName);//アニメーション再生
            isExplotion = true;
        }
    }
}
