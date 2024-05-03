//------------------------------------
//�S���ҁF�����S
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------
//���œ�����Q���̃N���X
//------------------------------------
public class CS_WindObstacle : MonoBehaviour
{
    [SerializeField, Header("���[���p�̃G�b�W�R���C�_�[")]
    private EdgeCollider2D m_edcolLaneEdge;

    [SerializeField, Header("��Q����Rigidbody")]
    private Rigidbody2D m_objRigidBody;

    private Vector2 m_v2LeftPoint;  //���[�̃��[�����W
    private Vector2 m_v2RightPoint; //�E�[�̃��[�����W

    // Start is called before the first frame update
    void Start()
    {

        Vector2 ObjPos = new Vector2(transform.position.x, transform.position.y);
        //���[���̗��[�̍��W���擾
        m_v2LeftPoint = ObjPos + m_edcolLaneEdge.points[0];
        m_v2RightPoint = ObjPos + m_edcolLaneEdge.points[1];

        Debug.Log(m_v2LeftPoint);
        Debug.Log(m_v2RightPoint);

    }

    // Update is called once per frame
    void Update()
    {

        //���E�̃|�C���g�Ƃ̋������v�Z
        float leftdistance = Vector2.Distance(m_v2LeftPoint, transform.position);
        float rightdistance = Vector2.Distance(m_v2RightPoint, transform.position);

        //�[�ɗ�������W���Œ�
        if(leftdistance < 0) { transform.localPosition = new Vector3(m_v2LeftPoint.x, m_v2LeftPoint.y, 0.0f); }
        if(rightdistance < 0) { transform.localPosition = new Vector3(m_v2RightPoint.x, m_v2RightPoint.y, 0.0f); }

    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag != "Wind") { return; }

        CS_Wind cswind = collision.transform.GetComponent<CS_Wind>();

        if (!cswind) { return; }

        Debug.Log("��");

        //���̌������擾
        CS_Wind.E_WINDDIRECTION direction = cswind.WindDirection;

        //�����ɂ���ĕ���^����
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
