//-----------------------------------------------
//�S���ҁF��������
//���p���[���W�߂�F�V���E�X�{��(CS_Player������Ƃ�)�ɃA�^�b�`
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_StarPower : MonoBehaviour
{
    [Header("�V���E�X�{��(CS_Player������Ƃ�)�ɃA�^�b�`")]

    [Header("���̃p�[�c�摜���X�g")]
    [Header("0:Power2")]
    [Header("1:Power3")]
    [Header("2:Power4")]
    [Header("3:Power5")]
    [Header("4:PowerFace")]

    [SerializeField] private List<Sprite> m_starSprites = new List<Sprite>();

    public int nowSprite = 0;

    [SerializeField, Header("���̃L�����N�^�[")]
    private GameObject m_starChild;

    [SerializeField, Header("���̌��Ђ̃^�O")]
    private string m_sPieceTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(m_sPieceTag))
        {
            Destroy(collision.gameObject);//���ЃI�u�W�F�N�g������

            SpriteRenderer starChildRender = m_starChild.GetComponent<SpriteRenderer>();
            if(nowSprite == m_starSprites.Count -1)
            {
                //���̎q�̊�I�u�W�F�N�g���쐬
                GameObject childObject = new GameObject("StarChildFace");

                //��I�u�W�F�N�g��e�I�u�W�F�N�g�̎q�ɐݒ�
                childObject.transform.parent = m_starChild.transform;

                //��I�u�W�F�N�g��SpriteRenderer�R���|�[�l���g��ǉ�
                SpriteRenderer faceRender = childObject.AddComponent<SpriteRenderer>();

                //��I�u�W�F�N�g�̃X�v���C�g��ݒ�
                faceRender.sprite = m_starSprites[nowSprite];

                //��I�u�W�F�N�g�̈ʒu��e�I�u�W�F�N�g�ɑ΂��Ē���
                childObject.transform.localPosition = Vector3.zero;
                childObject.transform.localScale = new Vector3(1f, 1f, 1f);
               
                faceRender.sortingOrder = starChildRender.sortingOrder + 1;
                Destroy(this);
                return;
            }

            //�X�v���C�g��ݒ�
            starChildRender.sprite = m_starSprites[nowSprite];
            nowSprite++;//�X�v���C�g�ԍ����Z
        }
    }
}
