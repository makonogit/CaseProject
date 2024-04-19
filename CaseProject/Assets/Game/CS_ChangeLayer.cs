//-----------------------------------------------
//担当者：井上想哉
//レイヤーチェンジクラス
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ChangeLayer : MonoBehaviour
{
    //レイヤーごとのデータ
    [System.Serializable]
    struct LayerData
    {
        //[SerializeField]
        public Vector3 Scale;
        //[SerializeField]
        public Vector3 Pos;
        //[SerializeField]
        public GameObject LayerObj;

        public LayerData(Vector3 scale,Vector3 pos,GameObject obj)
        {
            Scale = scale;
            Pos = pos;
            LayerObj = obj;
        }

    }

    [SerializeField, Header("レイヤー情報")]
    private List<LayerData> m_Layer = new List<LayerData>();

    private int m_nOldLayer = 2;    //前のレイヤー
    private int m_nNowLayer = 2;    //現在のレイヤー

    private const int m_MoveMax = 3;   //最大レイヤー数

    [SerializeField, Header("手のデータスクリプト")]
    private CS_HandSigns m_handsigns;

    private bool m_isPush = false;      //押し引きの判定

    private float m_fTimer = 0.0f;      //クールタイム

    void Update()
    {
        
        int pushdata = 1;

        if(!m_isPush) pushdata = m_handsigns.PushHand();

        if ((pushdata == 1 && m_nNowLayer < m_MoveMax) || 
            (pushdata == 0 && m_nNowLayer > 1))
        {
            m_isPush = true;

            //更新前のレイヤーを保存
            m_nOldLayer = m_nNowLayer;
            Debug.Log(pushdata);

            //現在のレイヤーを更新
            if(pushdata == 1)
            {
                m_nNowLayer++;
                for (int i = m_Layer.Count - 1; i < 1; i++)
                {
                    Debug.Log("I" + i);
                    m_Layer[i] = new LayerData(m_Layer[i].Scale, m_Layer[i].Pos, m_Layer[i - 1].LayerObj);
                }
            }
            if(pushdata == 0) 
            {
                m_nNowLayer--;
                for (int i = 0; i < m_Layer.Count - 1; i++)
                {
                    m_Layer[i] = new LayerData(m_Layer[i].Scale, m_Layer[i].Pos, m_Layer[i + 1].LayerObj);
                }
            }

            //レイヤーの移動制限
            //if (m_nNowLayer > m_LayerMax) { m_nNowLayer = m_LayerMax; }
            //if (m_nNowLayer < 0) { m_nNowLayer = 0; }

        }


        if (LayerChange())
        {
            m_isPush = false;
        }
 
    }


    private bool LayerChange()
    {
        //レイヤーが更新されていなかったら終了
     //   if (m_nOldLayer == m_nNowLayer) { return; }

        //サイズと位置の更新
        for(int i = 0; i<m_Layer.Count;i++)
        {
            if(m_Layer[i].LayerObj == null) { continue; }
            m_Layer[i].LayerObj.transform.localScale =
                Vector3.Lerp(m_Layer[i].LayerObj.transform.localScale, m_Layer[i].Scale, Time.deltaTime);
            m_Layer[i].LayerObj.transform.position =
                Vector3.Lerp(m_Layer[i].LayerObj.transform.position, m_Layer[i].Pos, Time.deltaTime);
        }

        if(m_Layer[2].LayerObj.transform.position == m_Layer[2].Pos)
        {
            return true;
        }

        return false;
      
    }

}
