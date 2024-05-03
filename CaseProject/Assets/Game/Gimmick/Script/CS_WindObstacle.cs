//------------------------------------
//担当者：菅眞心
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------
//風で動く障害物のクラス
//------------------------------------
public class CS_WindObstacle : MonoBehaviour
{
    [SerializeField, Header("レーン用のエッジコライダー")]
    private EdgeCollider2D m_edcolLaneEdge;

    [SerializeField, Header("障害物のRigidbody")]
    private Rigidbody2D m_objRigidBody;

    private Vector2 m_v2LeftPoint;  //左端のレーン座標
    private Vector2 m_v2RightPoint; //右端のレーン座標

    // Start is called before the first frame update
    void Start()
    {

        Vector2 ObjPos = new Vector2(transform.position.x, transform.position.y);
        //レーンの両端の座標を取得
        m_v2LeftPoint = ObjPos + m_edcolLaneEdge.points[0];
        m_v2RightPoint = ObjPos + m_edcolLaneEdge.points[1];

        Debug.Log(m_v2LeftPoint);
        Debug.Log(m_v2RightPoint);

    }

    // Update is called once per frame
    void Update()
    {

        //左右のポイントとの距離を計算
        float leftdistance = Vector2.Distance(m_v2LeftPoint, transform.position);
        float rightdistance = Vector2.Distance(m_v2RightPoint, transform.position);

        //端に来たら座標を固定
        if(leftdistance < 0) { transform.localPosition = new Vector3(m_v2LeftPoint.x, m_v2LeftPoint.y, 0.0f); }
        if(rightdistance < 0) { transform.localPosition = new Vector3(m_v2RightPoint.x, m_v2RightPoint.y, 0.0f); }

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag != "Wind") { return; }

        CS_Wind cswind = collision.transform.GetComponent<CS_Wind>();

        if (!cswind) { return; }

        Debug.Log("風");

        //風の向きを取得
        CS_Wind.E_WINDDIRECTION direction = cswind.WindDirection;

        //方向によって風を与える
        switch(direction)
        {
            case CS_Wind.E_WINDDIRECTION.NONE:
                break;
            case CS_Wind.E_WINDDIRECTION.LEFT:
                m_objRigidBody.AddForce(Vector2.right, ForceMode2D.Force);
                break;
            case CS_Wind.E_WINDDIRECTION.RIGHT:
                m_objRigidBody.AddForce(Vector2.left, ForceMode2D.Force);
                break;
        }
       
    }

}
