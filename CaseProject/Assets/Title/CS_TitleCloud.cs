//------------------------------
// �S���ҁF�����@����
// �_�Ɏ肪�����������̏���
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleCloud : MonoBehaviour
{
    [SerializeField,Header("�ړ���")]
    private float moveForce = 5f; // �ړ������
    [SerializeField, Header("�^�[�Q�b�g�I�u�W�F�N�g��")]
    private string targetObjectName = "Point Annotation"; // �ڕW�ƂȂ�I�u�W�F�N�g�̖��O

    private Rigidbody2D rb;

    private bool hasCollided = false; // ���łɏՓ˂������ǂ����̃t���O

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //�肪���������H
        if (!hasCollided && collision.gameObject.name == targetObjectName)
        {
            // �Փ˂��������Ƌt������x���ɗ͂�������
            Vector2 collisionNormal = collision.contacts[0].normal;
            Vector2 forceDirection = new Vector2(-collisionNormal.x, 0f).normalized;
            rb.AddForce(forceDirection * moveForce, ForceMode2D.Impulse);
            hasCollided = true;
        }

    }
}
