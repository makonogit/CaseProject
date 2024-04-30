//-----------------------------------------------
//担当者：井上想哉
//敵クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_EnemyController : MonoBehaviour
{
    [SerializeField, Header("オブジェクト")]
    private GameObject m_ShotPrefab;

    //方向
    //private enum Direction
    //{
    //    LEFT = 0,
    //    RIGHT = 1
    //}

    //private Direction m_isStartSide;  //開始位置

    private Transform m_PlayerTr;   //playerのトランスフォーム
    private Transform m_Trans;  //自分のTransform

    private int m_iCount;

    [SerializeField, Header("移動量")]
    private float m_fMove = 3.0f;

    //[SerializeField, Header("攻撃力")]
    private float m_fAtack = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Trans = this.transform;
        // プレイヤーのTransformを取得
        m_PlayerTr = GameObject.FindGameObjectWithTag("Player").transform;
        ////方向を決める
        //m_isStartSide = m_trans.position.x < 0 ? Direction.LEFT : Direction.RIGHT;
        ////方向によって向きを変える
        //m_trans.localScale = m_isStartSide == Direction.LEFT ? 
        //    new Vector3(m_trans.localScale.x * -1, m_trans.localScale.y) : m_trans.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        m_iCount += 1;

        // （ポイント）
        // 600フレームごとに砲弾を発射する
        if (m_iCount % 600 == 0)
        {
            GameObject shell = Instantiate(m_ShotPrefab, transform.position, Quaternion.identity);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        }

        // プレイヤーとの距離が100.0f以上だったら実行しない
        if (Vector2.Distance(transform.position, m_PlayerTr.position) > 100.0f)
        {
            return;
        }

        // プレイヤーに向けて進む
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(m_PlayerTr.position.x, m_PlayerTr.position.y),
            m_fMove * Time.deltaTime);

        //右か左に移動する
        //if(m_isStartSide == Direction.LEFT)
        //{
        //    m_trans.position = new Vector3(m_trans.position.x + m_fMove, m_trans.position.y);
        //}
        //else
        //{
        //    m_trans.position = new Vector3(m_trans.position.x - m_fMove, m_trans.position.y);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ////プレイヤーと衝突したら消滅
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーのHPを削る
            //CS_FryShip ship = collision.gameObject.transform.GetComponent<CS_FryShip>();
            //ship.HP -= m_fAtack;
            Destroy(this.gameObject);
            //追記：中島2024.04.03
            //ゲームオーバーフラグをtrue
            //CS_ResultController.GaneOverFlag = true;
            //SceneManager.LoadScene("Result");
            
        }

        //雨と接触したら
        if (collision.gameObject.tag == "Rain")
        {

        }

    }


}
