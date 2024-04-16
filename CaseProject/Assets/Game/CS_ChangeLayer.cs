//-----------------------------------------------
//担当者：井上想哉
//レイヤーチェンジクラス
//-----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ChangeLayer : MonoBehaviour
{
    [SerializeField,Header("拡大・縮小係数")]
    private float m_fScaleFactor = 1.3f; // 拡大・縮小のスケールファクター

    //状態管理変数
    private int[] m_nScale = new int[3]{ -1, 0, 1 };
    private int m_nScaleState = 1;

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
    }

    [SerializeField, Header("レイヤー情報")]
    private LayerData[] m_Layer =　new LayerData[3];

    private int m_nOldLayer = 1;
    private int m_nNowLayer = 1;    //現在のレイヤー

    [SerializeField, Header("手のデータスクリプト")]
    private CS_HandPoseData m_handposedata;

    private bool m_isPush = false;
    
    //ハンドトラッキングからの入力に変更する部分
    public KeyCode scaleKey01 = KeyCode.Space; // 拡大・縮小を行うキー
    public KeyCode scaleKey02 = KeyCode.Space; // 拡大・縮小を行うキー
    //---

    void Update()
    {

        int pushdata = m_handposedata.PUSHDATA;
        if ((pushdata == 1 || pushdata == 0) && !m_isPush)
        {
            m_isPush = true;

            //更新前のレイヤーを保存
            m_nOldLayer = m_nNowLayer;

            //現在のレイヤーを更新
            m_nNowLayer = pushdata == 1 ? m_nNowLayer + 1 : m_nNowLayer - 1;

            //レイヤーの移動制限
            if(m_nNowLayer > 2) { m_nNowLayer = 2; }
            if(m_nNowLayer < 0) { m_nNowLayer = 0; }
        }


        if(m_isPush)
        {
            //レイヤーが更新されていたら
            if(m_nOldLayer < m_nNowLayer)    //押し
            {
                if(m_nNowLayer == 2)
                {
                    m_Layer[m_nNowLayer].LayerObj.SetActive(false);
                    
                    m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale =
                    Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer].Scale, Time.deltaTime);

                    m_Layer[m_nOldLayer - 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.position, m_Layer[m_nOldLayer].Pos, Time.deltaTime);
                }

                if (m_nNowLayer == 1)
                {
                    m_Layer[m_nNowLayer - 1].LayerObj.SetActive(true);
                }

                m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                    Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);

                m_Layer[m_nOldLayer].LayerObj.transform.position =
                    Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);
            }

            if (m_nOldLayer > m_nNowLayer)    //引き
            {
                if (m_nNowLayer == 0)
                {
                    m_Layer[m_nOldLayer + 1].LayerObj.SetActive(false);
                }

                if (m_nNowLayer == 1)
                {
                    m_Layer[m_nNowLayer - 1].LayerObj.SetActive(true);
                }

                m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                    Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);

                m_Layer[m_nOldLayer].LayerObj.transform.position =
                   Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);

            }

            if(m_Layer[m_nOldLayer].LayerObj.transform.localScale == m_Layer[m_nNowLayer].LayerObj.transform.localScale &&
               m_Layer[m_nOldLayer].LayerObj.transform.position == m_Layer[m_nNowLayer].LayerObj.transform.position)
            {
                m_isPush = false;
            }

        }

        {
            //// 全てのオブジェクトを取得
            //GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

            //// 指定したキーが押されたら
            //if (Input.GetKeyDown(scaleKey01))
            //{
            //    if (m_nScale[2] > m_nScale[m_nScaleState])
            //    {
            //        m_nScaleState++;
            //        // 各オブジェクトに対して処理を行う
            //        foreach (GameObject obj in objects)
            //        {
            //            // オブジェクトがプレイヤーでない場合に拡大・縮小を行う
            //            if (!obj.CompareTag("Player"))
            //            {
            //                // オブジェクトのスケールを変更する
            //                obj.transform.localScale *= m_fScaleFactor;
            //            }
            //        }

            //    }

            //}

            //if (Input.GetKeyDown(scaleKey02))
            //{
            //    if (m_nScale[0] < m_nScale[m_nScaleState])
            //    {
            //        m_nScaleState--;
            //        // 各オブジェクトに対して処理を行う
            //        foreach (GameObject obj in objects)
            //        {
            //            // オブジェクトがプレイヤーでない場合に拡大・縮小を行う
            //            if (!obj.CompareTag("Player"))
            //            {
            //                // オブジェクトのスケールを変更する
            //                obj.transform.localScale /= m_fScaleFactor;
            //            }
            //        }
            //    }

            //}
        }
    }
}
