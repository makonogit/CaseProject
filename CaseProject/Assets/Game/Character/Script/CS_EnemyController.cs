//-----------------------------------------------
//�S���ҁF���z��
//�G�N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_EnemyController : MonoBehaviour
{
    [SerializeField, Header("�I�u�W�F�N�g")]
    private GameObject m_ShotPrefab;

    //����
    //private enum Direction
    //{
    //    LEFT = 0,
    //    RIGHT = 1
    //}

    //private Direction m_isStartSide;  //�J�n�ʒu

    private Transform m_PlayerTr;   //player�̃g�����X�t�H�[��
    private Transform m_Trans;  //������Transform

    private int m_iCount;

    [SerializeField, Header("�ړ���")]
    private float m_fMove = 3.0f;

    //[SerializeField, Header("�U����")]
    private float m_fAtack = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Trans = this.transform;
        // �v���C���[��Transform���擾
        m_PlayerTr = GameObject.FindGameObjectWithTag("Player").transform;
        ////���������߂�
        //m_isStartSide = m_trans.position.x < 0 ? Direction.LEFT : Direction.RIGHT;
        ////�����ɂ���Č�����ς���
        //m_trans.localScale = m_isStartSide == Direction.LEFT ? 
        //    new Vector3(m_trans.localScale.x * -1, m_trans.localScale.y) : m_trans.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        m_iCount += 1;

        // �i�|�C���g�j
        // 600�t���[�����ƂɖC�e�𔭎˂���
        if (m_iCount % 600 == 0)
        {
            GameObject shell = Instantiate(m_ShotPrefab, transform.position, Quaternion.identity);
            Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        }

        // �v���C���[�Ƃ̋�����100.0f�ȏゾ��������s���Ȃ�
        if (Vector2.Distance(transform.position, m_PlayerTr.position) > 100.0f)
        {
            return;
        }

        // �v���C���[�Ɍ����Đi��
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(m_PlayerTr.position.x, m_PlayerTr.position.y),
            m_fMove * Time.deltaTime);

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
            Destroy(this.gameObject);
            //�ǋL�F����2024.04.03
            //�Q�[���I�[�o�[�t���O��true
            //CS_ResultController.GaneOverFlag = true;
            //SceneManager.LoadScene("Result");
            
        }

        //�J�ƐڐG������
        if (collision.gameObject.tag == "Rain")
        {

        }

    }


}
