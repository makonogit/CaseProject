//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;

public class CS_HandBone : MonoBehaviour
{

    private HandLandmarkListAnnotation m_HandLandmark;
    [SerializeField] private List<Transform> m_Bones = new List<Transform>();

    private void Start()
    {
        Search(transform);
    }
    
    private void Update()
    {
        if (m_HandLandmark != null) SetHand();
        else 
        {
            m_HandLandmark = transform.parent.GetComponent<HandLandmarkListAnnotation>();
        }
    }

    // ボーンを探しリストに追加
    // 引数：親のトランスフォーム
    // 戻り値：なし
    private void Search(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            m_Bones.Add(child);
            Search(child);
        }
    }

    private void SetHand() 
    {
        transform.position = m_HandLandmark[0].transform.position;
        // L
        SetPos(1, 17);
        // R
        SetPos(6, 13);
        // M
        SetPos(11, 9);
        // I
        SetPos(16, 5);
        // T
        SetPos(21, 1);
       
    }

    private void SetPos(int startNum,int listNum) 
    {
        for(int i = 0; i <4; i++) 
        {
            Vector3 pos = m_HandLandmark[listNum + i].transform.position;
            Vector3 rotation = m_Bones[i + startNum].position;
            rotation = pos - rotation;
            float z = Mathf.Atan2(rotation.y, rotation.x)*Mathf.Rad2Deg;
            //Debug.Log(z);
            rotation = new Vector3(0,0,z);
            m_Bones[i + startNum + 1].position = pos;
            m_Bones[i + startNum].rotation = Quaternion.Euler(rotation);
        }
    }
}