//------------------------------
// 担当者：中島　愛音
// 雲に手が当たった時の処理
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleCloud : MonoBehaviour
{
    [SerializeField,Header("移動力")]
    private float moveForce = 5f; // 移動する力
    [SerializeField, Header("ターゲットオブジェクト名")]
    private string targetObjectName = "Point Annotation"; // 目標となるオブジェクトの名前

    private Rigidbody2D rb;

    private bool hasCollided = false; // すでに衝突したかどうかのフラグ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //手が当たった？
        if (!hasCollided && collision.gameObject.name == targetObjectName)
        {
            // 衝突した方向と逆方向のx軸に力を加える
            Vector2 collisionNormal = collision.contacts[0].normal;
            Vector2 forceDirection = new Vector2(-collisionNormal.x, 0f).normalized;
            rb.AddForce(forceDirection * moveForce, ForceMode2D.Impulse);
            hasCollided = true;
        }

    }
}
