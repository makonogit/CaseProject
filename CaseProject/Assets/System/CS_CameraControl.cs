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

    [SerializeField, Header("�X�e�[�W�̏��")]
    private EdgeCollider2D m_MaxHeight;

    [SerializeField, Header("�X�e�[�W�f�[�^")]
    private CS_StageData m_csStagedata;

    //�J�����̈ړ�����
    private Vector2 m_v2MaxLimit;
    private Vector2 m_v2MinLimit;

    private Transform m_tThisTrans;
    private Transform m_tTargetTrans;

    private Camera maincamera;

    //�Ǐ]�Ώۂ�ύX
    public GameObject TARGET
    {
        set
        {
            m_TargetObj = value;
            m_tTargetTrans = value.transform;
        }
    }

    
    //�X�e�[�W�ɂ���ĕς��̂�SceneManager�œǂݍ��ݎ��ɕύX
    public float MAXHEIGHT
    {
        set
        {
            m_v2MaxLimit.y = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //�Ǘ��N���X�Ƀf�[�^�ۑ�
        ObjectData.m_csCamCtrl = this;

        //�J�����̃T�C�Y�擾
        maincamera = Camera.main;

        //�ړ������̐ݒ�
        m_v2MinLimit.x = m_LimitPos.points[0].x + maincamera.orthographicSize * maincamera.aspect;
        m_v2MinLimit.y = m_LimitPos.points[1].y + maincamera.orthographicSize;
        m_v2MaxLimit.x = m_LimitPos.points[2].x - maincamera.orthographicSize * maincamera.aspect;
        //m_v2MaxLimit.y = m_LimitPos.points[3].y - maincamera.orthographicSize;

        //�X�e�[�W�̒������擾���ăJ�����̏����ݒ�
        int length = m_csStagedata.m_Worlds[StageInfo.World].Stagedata[StageInfo.Stage].m_nStageLength;
        m_v2MaxLimit.y = ((length * 10) - 5) - maincamera.orthographicSize;

        //EdgeCollider��ݒ�
        Vector2[] points = m_LimitPos.points;
        points[0].y = ((length * 10) - 5);
        points[3].y = ((length * 10) - 5);
        m_LimitPos.points = points;

        m_MaxHeight.offset = new Vector2(0.0f, (length * 10) - 5);


        //���W�̐ݒ�
        m_tTargetTrans = m_TargetObj.transform;
        m_tThisTrans = this.transform;

        //Debug.Log(maincamera.ViewportToWorldPoint(Vector2.zero));
        //Debug.Log(maincamera.ViewportToWorldPoint(Vector2.one));

    }

    // Update is called once per frame
    void Update()
    {

        //�O����W
        Vector3 ClampPosition = new Vector3(m_tTargetTrans.position.x, m_tTargetTrans.position.y, m_tThisTrans.position.z);
        //�ړ�����
        ClampPosition.x = Mathf.Clamp(ClampPosition.x, m_v2MinLimit.x, m_v2MaxLimit.x);
        ClampPosition.y = Mathf.Clamp(ClampPosition.y + 2.5f, m_v2MinLimit.y, m_v2MaxLimit.y);

        m_tThisTrans.position = ClampPosition;
        
    }

    
}
