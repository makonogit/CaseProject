//-----------------------------------------------
//担当者：中島愛音
//ターゲットとの距離に応じて削除する
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DestroyObject : MonoBehaviour
{
    [SerializeField, Header("ターゲット")]
    private GameObject m_targetObject;
    [SerializeField, Header("削除する距離")]
    private float m_destroyDistance = 10.0f;

    
    // Update is called once per frame
    void Update()
    {
        float nowDistance = Vector2.Distance(m_targetObject.transform.position, this.transform.position);
        if(nowDistance > m_destroyDistance)
        {
            Destroy(this.gameObject);
        }
    }
}
