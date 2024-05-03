//------------------------------------
//担当者：中川直登
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//------------------------------------
// ブラックホールクラス
//------------------------------------
public class CS_BlackHole : MonoBehaviour
{
    [SerializeField] private float m_fGravity;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rig = collision.GetComponent<Rigidbody2D>();
        if (rig == null) return; 
        // ブラックホールへの方向を求める
        Vector3 dir = this.transform.position;
        dir -= collision.transform.position;
        dir.Normalize();

        dir *= m_fGravity * Time.deltaTime;
        rig.AddForce(dir);
    }
}

