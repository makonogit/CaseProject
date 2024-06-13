//-----------------------------------------------
//�S���ҁF�����S
//�R�C���M�~�b�N
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Coin : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W�̗\����")]
    private GameObject m_gPredictionLine;

    //�\������SpiterenderRender���X�g
    private List<SpriteRenderer> m_srPredictionLineList = new List<SpriteRenderer>();

    [SerializeField, Header("�\�����̕\������")]
    private float m_fPredictionViewTime = 3.0f;

    //�\�����̕\���t���O
    private bool m_isPredictionView = false;

    [SerializeField,Header("�\�����̓����x")]
    private float m_fPredictionalpha = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //�q�I�u�W�F�N�g��SpriRenderer��S�Ď擾
        //m_srPredictionLineList =�@m_gPredictionLine.GetComponentsInChildren<SpriteRenderer>(m_srPredictionLineList);
        for (int i = 0; i < m_gPredictionLine.transform.childCount; i++)
        {
            Transform childtrans = m_gPredictionLine.transform.GetChild(i);
            SpriteRenderer sr = childtrans.GetComponent<SpriteRenderer>();
            m_srPredictionLineList.Add(sr);
        }

        //�\�������\����
        foreach (var sr in m_srPredictionLineList)
        {
            sr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (!m_isPredictionView) { return; }
        
        //���Ԍv��
        m_fPredictionViewTime -= Time.deltaTime;
        

        //���Ԍo�߂�����\������S�Ĕ�\���ɂ��ďI��
        if(m_fPredictionViewTime < 0.0f)
        {
            //�\�������\����
            foreach (var sr in m_srPredictionLineList)
            {
                sr.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            Destroy(this.gameObject);
        }

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //�v���C���[���Փ˂�����A�C�e���̋@�\�𖳌���
        if(collision.transform.tag == "Player")
        {
            //SE�Đ�
            ObjectData.m_csSoundData.PlaySE("Coin");

            this.GetComponent<CapsuleCollider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            //�\�����̕\��
            foreach(var sr in m_srPredictionLineList)
            {
                sr.color = new Color(1.0f, 1.0f, 1.0f, m_fPredictionalpha);
            }

            m_isPredictionView = true;
        }
    }

}
