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
    }

    [SerializeField, Header("レイヤー情報")]
    private LayerData[] m_Layer = new LayerData[3];

    private int m_nOldLayer = 1;    //前のレイヤー
    private int m_nNowLayer = 1;    //現在のレイヤー

    [SerializeField, Header("手のデータスクリプト")]
    private CS_HandPoseData m_handposedata;

    private bool m_isPush = false;      //押し引きの判定

    private float m_fTimer = 0.0f;      //クールタイム

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
            if (m_nNowLayer > 2) { m_nNowLayer = 2; }
            if (m_nNowLayer < 0) { m_nNowLayer = 0; }
        }


        if (m_isPush)
        {
            m_fTimer += Time.deltaTime;

            //レイヤーが更新されていたら
            if (m_nOldLayer < m_nNowLayer)    //押し
            {
                if (m_nNowLayer == 2)
                {
                    //奥のレイヤーを非アクティブにする
                    m_Layer[m_nNowLayer].LayerObj.SetActive(false);

                    //レイヤーを１つ後ろにする
                    m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer - 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer - 1].LayerObj.transform.position, m_Layer[m_nOldLayer].Pos, Time.deltaTime);

                    m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);

                }

                if (m_nNowLayer == 1)
                {
                    //手前のレイヤーをアクティブにする
                    m_Layer[m_nNowLayer - 1].LayerObj.SetActive(true);

                    //レイヤーを１つ後ろにする
                    m_Layer[m_nNowLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.localScale, m_Layer[m_nOldLayer + 1].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.position, m_Layer[m_nOldLayer + 1].Pos, Time.deltaTime);

                    m_Layer[m_nNowLayer + 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer + 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer + 2].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer + 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer + 1].LayerObj.transform.position, m_Layer[m_nOldLayer + 2].Pos, Time.deltaTime);
                }

            }

            if (m_nOldLayer > m_nNowLayer)    //引き
            {
                if (m_nNowLayer == 0)
                {
                    //手前のレイヤーを非アクティブにする
                    m_Layer[m_nNowLayer].LayerObj.SetActive(false);

                    //レイヤーを１つ前にする
                    m_Layer[m_nOldLayer + 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer + 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer + 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer + 1].LayerObj.transform.position, m_Layer[m_nOldLayer].Pos, Time.deltaTime);

                    m_Layer[m_nOldLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.localScale, m_Layer[m_nNowLayer].Scale, Time.deltaTime);
                    m_Layer[m_nOldLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nOldLayer].LayerObj.transform.position, m_Layer[m_nNowLayer].Pos, Time.deltaTime);
                }

                if (m_nNowLayer == 1)
                {
                    //奥のレイヤーをアクティブにする
                    m_Layer[m_nNowLayer + 1].LayerObj.SetActive(true);

                    //レイヤーを1つ前にする
                    m_Layer[m_nNowLayer].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.localScale, m_Layer[m_nOldLayer - 1].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer].LayerObj.transform.position, m_Layer[m_nOldLayer - 1].Pos, Time.deltaTime);

                    m_Layer[m_nNowLayer - 1].LayerObj.transform.localScale =
                        Vector3.Lerp(m_Layer[m_nNowLayer - 1].LayerObj.transform.localScale, m_Layer[m_nOldLayer - 2].Scale, Time.deltaTime);
                    m_Layer[m_nNowLayer - 1].LayerObj.transform.position =
                        Vector3.Lerp(m_Layer[m_nNowLayer - 1].LayerObj.transform.position, m_Layer[m_nOldLayer - 2].Pos, Time.deltaTime);
                }

            }

            //5秒たったら
            if (m_fTimer > 5f)
            {
                m_fTimer = 0.0f;
                m_isPush = false;
            }
        }
    }
}
