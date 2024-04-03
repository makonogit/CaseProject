//-----------------------------------------------
//担当者：菅眞心
//敵クラス
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_EnemyController : MonoBehaviour
{
    //方向
    //private enum Direction
    //{
    //    LEFT = 0,
    //    RIGHT = 1
    //}

    //private Direction m_isStartSide;  //開始位置

    private Transform m_trans;  //自分のTransform

    //[SerializeField, Header("移動量")]
    //private float m_fMove = 0.5f;

    //[SerializeField, Header("攻撃力")]
    //private float m_fAtack = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_trans = this.transform;
        ////方向を決める
        //m_isStartSide = m_trans.position.x < 0 ? Direction.LEFT : Direction.RIGHT;
        ////方向によって向きを変える
        //m_trans.localScale = m_isStartSide == Direction.LEFT ? 
        //    new Vector3(m_trans.localScale.x * -1, m_trans.localScale.y) : m_trans.localScale;

    }

    // Update is called once per frame
    void Update()
    {
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
            //Destroy(this.gameObject);
            //追記：中島2024.04.03
            //ゲームオーバーフラグをtrue
            CS_ResultController.GaneOverFlag = true;
            //SceneManager.LoadScene("Result");
            
        }

        //雨と接触したら
        if (collision.gameObject.tag == "Rain")
        {

        }

    }


}
