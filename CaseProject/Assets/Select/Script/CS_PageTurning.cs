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

  
    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        CS_HandSigns.OnCreateWinds += PageTurning;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Flip()
    {
        // ���݂�Scale���擾���AX�������ɔ��]������
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }


    
    //�y�[�W�߂���̃A�j���[�V�����𔭓�
    private void PageTurningAnimation()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("pageTurningAnim")) { return; }
        GetComponent<Animator>().SetTrigger("pageTurningAnim");
    }

    //�{���߂���
    void PageTurning(Vector3 _position, Vector3 _direction)
    {
        bool isFlip = (_direction.x < 0.0f && !isFacingRight) || (_direction.x > 0.0f && isFacingRight);

        Debug.Log("���E����" + _direction.x);

        if (isFlip) { Flip(); }
        PageTurningAnimation();//�A�j���[�V�������s
    }
}
