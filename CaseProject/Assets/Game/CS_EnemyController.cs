//-----------------------------------------------
//�S���ҁF�����S
//�G�N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_EnemyController : MonoBehaviour
{
    //����
    //private enum Direction
    //{
    //    LEFT = 0,
    //    RIGHT = 1
    //}

    //private Direction m_isStartSide;  //�J�n�ʒu

    private Transform m_trans;  //������Transform

    //[SerializeField, Header("�ړ���")]
    //private float m_fMove = 0.5f;

    //[SerializeField, Header("�U����")]
    //private float m_fAtack = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_trans = this.transform;
        ////���������߂�
        //m_isStartSide = m_trans.position.x < 0 ? Direction.LEFT : Direction.RIGHT;
        ////�����ɂ���Č�����ς���
        //m_trans.localScale = m_isStartSide == Direction.LEFT ? 
        //    new Vector3(m_trans.localScale.x * -1, m_trans.localScale.y) : m_trans.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        //�E�����Ɉړ�����
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
        ////�v���C���[�ƏՓ˂��������
        if (collision.gameObject.tag == "Player")
        {
            //�v���C���[��HP�����
            //CS_FryShip ship = collision.gameObject.transform.GetComponent<CS_FryShip>();
            //ship.HP -= m_fAtack;
            //Destroy(this.gameObject);
            //�ǋL�F����2024.04.03
            //�Q�[���I�[�o�[�t���O��true
            CS_ResultController.GaneOverFlag = true;
            //SceneManager.LoadScene("Result");
            
        }

        //�J�ƐڐG������
        if (collision.gameObject.tag == "Rain")
        {

        }

    }


}
