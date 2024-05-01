//-----------------------------------------------
//�S���ҁF��������
//�^�[�Q�b�g�Ƃ̋����ɉ����č폜����
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DestroyObject : MonoBehaviour
{
    [SerializeField, Header("�^�[�Q�b�g")]
    private GameObject m_targetObject;
    [SerializeField, Header("�폜���鋗��")]
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
