//-----------------------------------------------
//�S���ҁF�����S
//�J��������N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CameraControl : MonoBehaviour
{
    [SerializeField, Header("�Ǐ]�Ώ�")]
    private GameObject m_TargetObj;

    [SerializeField,Header("�J�����ړ�����")]
    private EdgeCollider2D m_LimitPos;

    //�J�����̈ړ�����
    private Vector2 m_v2MaxLimit;
    private Vector2 m_v2MinLimit;

    private Transform m_tThisTrans;
    private Transform m_tTargetTrans;

    private Camera maincamera;

    // Start is called before the first frame update
    void Start()
    {
        //�J�����̃T�C�Y�擾
        maincamera = Camera.main;

        //�ړ������̐ݒ�
        m_v2MinLimit.x = m_LimitPos.points[0].x + maincamera.orthographicSize * maincamera.aspect;
        m_v2MinLimit.y = m_LimitPos.points[1].y + maincamera.orthographicSize;
        m_v2MaxLimit.x = m_LimitPos.points[2].x - maincamera.orthographicSize * maincamera.aspect;
        m_v2MaxLimit.y = m_LimitPos.points[3].y - maincamera.orthographicSize;

        //���W�̐ݒ�
        m_tTargetTrans = m_TargetObj.transform;
        m_tThisTrans = this.transform;

        Debug.Log(maincamera.ViewportToWorldPoint(Vector2.zero));
        Debug.Log(maincamera.ViewportToWorldPoint(Vector2.one));

    }

    // Update is called once per frame
    void Update()
    {

        //�O����W
        Vector3 ClampPosition = new Vector3(m_tTargetTrans.position.x, m_tTargetTrans.position.y, m_tThisTrans.position.z);
        //�ړ�����
        ClampPosition.x = Mathf.Clamp(ClampPosition.x, m_v2MinLimit.x, m_v2MaxLimit.x);
        ClampPosition.y = Mathf.Clamp(ClampPosition.y, m_v2MinLimit.y, m_v2MaxLimit.y);

        m_tThisTrans.position = ClampPosition;
        
    }
}
