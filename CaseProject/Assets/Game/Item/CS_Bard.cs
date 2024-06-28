using UnityEngine;
//------------------------------------
//担当者：菅眞心
//------------------------------------

//------------------------------------
//鳥クラス
//------------------------------------
public class CS_Bard : CS_Obstacle
{
    [SerializeField, Header("移動速度")]
    private float m_fMoveSpeed = 0.1f;

    //鳥の向き
    private Vector3 m_v3Directon = Vector3.right;

    private void Start()
    {
        //オブジェクトのスケールによって向きを設定
        if(transform.localScale.x < 0) { m_v3Directon = Vector3.left; }
    }

    private void Update()
    {
        //向いている方向に移動
        transform.Translate(m_v3Directon * m_fMoveSpeed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Stage")
        {
            m_v3Directon.x *= -1;
            transform.localScale = new Vector3(m_v3Directon.x, transform.localScale.y, 1.0f);
        }
    }

}
