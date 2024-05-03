//------------------------------
// �S���ҁF�����@����
//�@�y�[�W�߂���
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;

public class CS_PageTurning : MonoBehaviour
{
    [SerializeField, Header("�n���h�T�C��")]
    private CS_HandSigns m_handSigns;

    [SerializeField, Header("�y�[�W�߂�����s����̈ړ���")]
    private float m_handMovement = 5.0f;

    private bool isFacingRight = true;

    private Vector3[] m_midlleF_PrevPos = new Vector3[2];//���w�̃|�W�V����

    private List<HandLandmarkListAnnotation> m_handLandmark = new List<HandLandmarkListAnnotation>();
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //��xnull�ŏ��������Ă���
        
        //�n���h�}�[�N���擾
        //1���E��A0������Ƃ���
        m_handLandmark = m_handSigns.HandMark;

        if (m_handLandmark.Count < 2)
        {
            return;
        }

        //�y�[�W�߂��肪���s���Ȃ�I��
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AM_PageTurning")) { return; }

        for (int i = 0; i < 2; i++)
        {
            if(m_handLandmark[i] == null) { return; }
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AM_PageTurning")) { return; }
            //��̈ړ��x�N�g��
            Vector3 handVec = m_handSigns.GetHandMovement(i);
            PointListAnnotation point1 = m_handLandmark[i].GetLandmarkList();�@//�|�C���g���X�g���擾
            //�y�[�W�̂߂���̃A�j���[�V����
            bool turning = IsPageTurning(handVec,i);
            if (turning)
            {
                //�E�肩���肩�Ŗ{�̂߂��������ς���
                if (handVec.x > 0.0f && isFacingRight) { Flip(); }
                else if (handVec.x < 0.0f && !isFacingRight) { Flip(); }
                PageTurningAnimation();//�A�j���[�V�������s
            }
            m_midlleF_PrevPos[i] = point1[12].transform.position;//���w�̐��ۑ�
        }
        
    }

    private void Flip()
    {
        // ���݂�Scale���擾���AX�������ɔ��]������
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }


    private bool IsPageTurning(Vector3 _moveVec, int handNum)
    {   
        //�ړ���������薢���Ȃ�false
        if (_moveVec.magnitude < m_handMovement) { return false; }

        //-------------���ʒu����ʂ̉E�������������Ƃ�--------------
        PointListAnnotation point1 = m_handLandmark[handNum].GetLandmarkList();�@//�|�C���g���X�g���擾
        //���̃|�W�V�������X�N���[�����W�ɂ���
        Vector3 wristPos = point1[17].transform.position;
        Vector3 screenPos = Camera.main.WorldToViewportPoint(wristPos);
        //�X�N���[�����W�̉E���ɂ��邩
        bool handScreenPosRight = screenPos.x > 0.5f;

        //���w��̈ʒu�Ǝ���X���W�̊֌W�����
        Vector3 middleFingerPos = point1[12].transform.position;
        //�X�N���[���̉E���ɂ���H
        if (handScreenPosRight)
        {
            //������E�Ɉړ������Ȃ�false
            if(IsMoveingRight(m_midlleF_PrevPos[handNum],middleFingerPos,wristPos)) { return false; }
        }
        else //�X�N���[���̍���
        {
            //�E���獶�Ɉړ������Ȃ�false
            if (IsMoveingLeft(m_midlleF_PrevPos[handNum], middleFingerPos, wristPos)) { return false; }
        }
        //----------------------------------------------------------------

        //�p�[�łȂ��Ȃ�false
        if (m_handSigns.GetHandPose(handNum) != (byte)CS_HandSigns.HandPose.PaperSign) { return false; }

        return true;
    }

    private bool IsMoveingLeft(Vector3 _prevPos, Vector3 _currentPos, Vector3 _wristPos)
    {
        return _prevPos.x > _wristPos.x && _currentPos.x <= _wristPos.x;
    }

    private bool IsMoveingRight(Vector3 _prevPos, Vector3 _currentPos, Vector3 _wristPos)
    {
        return _prevPos.x < _wristPos.x && _currentPos.x >= _wristPos.x;
    }

    //�y�[�W�߂���̃A�j���[�V�����𔭓�
    private void PageTurningAnimation()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("pageTurningAnim")) { return; }
        GetComponent<Animator>().SetTrigger("pageTurningAnim");
    }
}
