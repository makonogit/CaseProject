using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SeriusMove : MonoBehaviour
{
    [SerializeField, Header("���U���g�R���g���[���[")]
    private CS_ResultController m_rController;

    [SerializeField, Header("�����̃v���n�u���X�g")]
    private List<GameObject> m_constellationList = new List<GameObject>();
    private GameObject m_constellation;//���̃��U���g�Ŏg�������I�u�W�F�N�g
    private Transform m_targetObj;//�V���E�X���ړ�����ꏊ
    private Vector3 m_targetPos;//�V���E�X���ړ�����ꏊ
    private List<Transform> m_starList = new List<Transform>();//�������ɂ��鐯�̃��X�g

    [SerializeField, Header("���̈ړ��X�s�[�h")]
    private float m_fSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        //����g������������
        m_constellation = Instantiate(m_constellationList[(int)m_rController.StageType],Vector3.zero, Quaternion.identity);
        //m_constellationList.Clear();//���X�g�͂����g��Ȃ��̂ŃN���A

        if(m_constellation == null) { Debug.LogWarning("����������܂���"); }
        Debug.Log(m_constellation.name);
        //�����I�u�W�F�N�g����stars�Ƃ����q�I�u�W�F�N�g��������
        Transform starsInCostellation = m_constellation.transform.Find("stars");

        bool isTarget = starsInCostellation != null;
        if (!isTarget) { Debug.LogWarning("stars������"); }

        m_targetObj = starsInCostellation.Find("targetStar");//�ڕW�ʒu�̃I�u�W�F�N�g
        //�^�[�Q�b�g�����邩
        isTarget = m_targetObj != null;
        m_targetPos = m_constellation.transform.TransformPoint(starsInCostellation.localPosition + m_targetObj.position);
        //m_targetPos = m_constellation.transform.TransformPoint(m_targetPos);
        Debug.Log("�^�[�Q�b�g�ʒu" + m_targetPos);
        if (!isTarget) { Debug.LogWarning("�^�[�Q�b�g������"); }

        //�X�^�[�ƃV���E�X��������
        Transform seriusStar = transform.Find("Star");//Star�I�u�W�F�N�g��������
        isTarget = seriusStar != null;
        if (!isTarget) { Debug.LogWarning("Star������"); }
        Transform sirius = transform.Find("Sirius");//Serius�I�u�W�F�N�g��������
        isTarget = sirius != null;
        if (!isTarget) { Debug.LogWarning("Sirius������"); }
        seriusStar.localScale = m_targetObj.lossyScale;

        //�V���E�X�̐V���ȃ|�W�V�����ݒ�
        Vector3 seriusNewPos = seriusStar.position + Vector3.up * seriusStar.localScale.y;
        seriusNewPos.y += sirius.localScale.y / 2;
        sirius.position = seriusNewPos;
    }

    // Update is called once per frame
    void Update()
    {
        // �ڕW�l�ɋ߂Â����瓞���Ɣ��f
        if (Vector3.Distance(transform.position, m_targetObj.position) < 0.1f)
        {
            NextStateReady();//���̏�Ԃ֍s������
            Destroy(this);
            return;
        }

        //�ړ�
        Move();
       
    }

    //���̏����ɍs�����߂̏���
    private void NextStateReady()
    {
        //�����I�u�W�F�N�g����lines�I�u�W�F�N�g�������o��
        GameObject lines = m_constellation.transform.Find("lines").gameObject;
        //�^�[�Q�b�g�����邩
        bool isTarget = lines != null;
        if (!isTarget) { Debug.LogWarning("lines������"); }
        lines.AddComponent<CS_LineController>();
        lines.GetComponent<CS_LineController>().SetResControlloer(this.m_rController);
        
        //�͂߂���ʉ����Ȃ炵�ă��U���g�̏�Ԃ����C���\����Ԃɂ��ď���
        m_rController.ResultState = CS_ResultController.RESULT_STATE.BORN_LINE;
    }

    //�ړ�����
    private void Move()
    {
       
        Vector3 direction = (m_targetObj.position- transform.position).normalized;

        // �ڕW�l�Ɍ������Ĉړ�
        transform.position += direction * m_fSpeed * Time.deltaTime;
    }
}

