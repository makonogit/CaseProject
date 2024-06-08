using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ExplosionObstacle : MonoBehaviour
{
    [SerializeField, Header("�m�b�N�o�b�N�̋���")]
    private float m_fKnockBackForce = 1.0f;

    [SerializeField, Header("�U����")]
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

    //�����蔻��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�v���C���[�ƏՓ˂�����m�b�N�o�b�N������
        if (collision.transform.tag == "Player")
        {
            //���������߂ĕ����Ɨ͂�ݒ�
            Vector3 Direction = transform.position - collision.transform.position;
            collision.transform.GetComponent<CS_Player>().KnockBack(Direction, m_fKnockBackForce);
            ExplotionStar(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�ƏՓ˂�����m�b�N�o�b�N������
        if (collision.transform.tag == "Player")
        {
            //���������߂ĕ����Ɨ͂�ݒ�
            Vector3 Direction = transform.position - collision.transform.position;
            collision.transform.GetComponent<CS_Player>().KnockBack(Direction, m_fKnockBackForce);
            ExplotionStar(collision.gameObject);
        }
    }

    //CS_ExplosionEffect�������Ă���I�u�W�F�N�g��T���A�������J�n������
    private void ExplotionStar(GameObject _shirius)
    {
        // �ċA�I�Ɏq�I�u�W�F�N�g����CS_ExplosionEffect��T��
        CS_ExplosionEffect explosionEffect = FindComponentInChildren<CS_ExplosionEffect>(_shirius.transform);

        // ���������ꍇ�AStartExplosion�֐����Ăяo��
        if (explosionEffect != null)
        {
            explosionEffect.StartExplosion();
            //Destroy(this.gameObject);
            return;
        }
        else
        {
            Debug.LogError("CS_ExplosionEffect��������܂���ł����B");
        }
    }

    //�w�肵���R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��T��
    //����:�T�����J�n����I�u�W�F�N�g��transform
    private T FindComponentInChildren<T>(Transform parent) where T : Component
    {
        //���A�Б��Ƃ��������w�̃I�u�W�F�N�g�܂ő{���ł���悤�ɍċA�I�ɌĂяo��
        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            //���������Ȃ甲����
            if (component != null)
            {
                return component;
            }

            component = FindComponentInChildren<T>(child);//�q�I�u�W�F�N�g�̒T��
            //���������Ȃ甲����
            if (component != null)
            {
                return component;
            }
        }

        return null;
    }
}
