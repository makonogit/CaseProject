//------------------------------
// 担当者：中島　愛音
//カメラの初期化
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SelectCamera : MonoBehaviour
{
    [SerializeField, Header("カメラの初期位置に合わせるオブジェクト")]
    private GameObject m_cameraPosObj;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 target = m_cameraPosObj.transform.position;
        transform.position = new Vector3(target.x, target.y, this.transform.position.z);
    }

   
}
