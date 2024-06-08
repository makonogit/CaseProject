using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ExplosionObstacle : MonoBehaviour
{
    [SerializeField, Header("ノックバックの強さ")]
    private float m_fKnockBackForce = 1.0f;

    [SerializeField, Header("攻撃力")]
    private float m_fAttackPower = 0.0f;

   
    //[SerializeField,Header("s")]

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //プレイヤーと衝突したらノックバックさせる
        if (collision.transform.tag == "Player")
        {
            //方向を求めて方向と力を設定
            Vector3 Direction = transform.position - collision.transform.position;
            collision.transform.GetComponent<CS_Player>().KnockBack(Direction, m_fKnockBackForce);
            ExplotionStar(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーと衝突したらノックバックさせる
        if (collision.transform.tag == "Player")
        {
            //方向を求めて方向と力を設定
            Vector3 Direction = transform.position - collision.transform.position;
            collision.transform.GetComponent<CS_Player>().KnockBack(Direction, m_fKnockBackForce);
            ExplotionStar(collision.gameObject);
        }
    }

    //CS_ExplosionEffectを持っているオブジェクトを探し、爆発を開始させる
    private void ExplotionStar(GameObject _shirius)
    {
        // 再帰的に子オブジェクトからCS_ExplosionEffectを探す
        CS_ExplosionEffect explosionEffect = FindComponentInChildren<CS_ExplosionEffect>(_shirius.transform);

        // 見つかった場合、StartExplosion関数を呼び出す
        if (explosionEffect != null)
        {
            explosionEffect.StartExplosion();
            //Destroy(this.gameObject);
            return;
        }
        else
        {
            Debug.LogError("CS_ExplosionEffectが見つかりませんでした。");
        }
    }

    //指定したコンポーネントがアタッチされているオブジェクトを探索
    //引数:探索を開始するオブジェクトのtransform
    private T FindComponentInChildren<T>(Transform parent) where T : Component
    {
        //孫、ひ孫といった下層のオブジェクトまで捜索できるように再帰的に呼び出す
        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            //見つかったなら抜ける
            if (component != null)
            {
                return component;
            }

            component = FindComponentInChildren<T>(child);//子オブジェクトの探索
            //見つかったなら抜ける
            if (component != null)
            {
                return component;
            }
        }

        return null;
    }
}
