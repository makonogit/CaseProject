//------------------------------
// �S���ҁF�����@����
//�J�����̏�����
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SelectCamera : MonoBehaviour
{
    [SerializeField, Header("�J�����̏����ʒu�ɍ��킹��I�u�W�F�N�g")]
    private GameObject m_cameraPosObj;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 target = m_cameraPosObj.transform.position;
        transform.position = new Vector3(target.x, target.y, this.transform.position.z);
    }

   
}
