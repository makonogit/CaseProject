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

    //ハンドトラッキングからの入力に変更する部分
    public KeyCode scaleKey01 = KeyCode.Space; // 拡大・縮小を行うキー
    public KeyCode scaleKey02 = KeyCode.Space; // 拡大・縮小を行うキー
    //---

    void Update()
    {
        // 全てのオブジェクトを取得
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

        // 指定したキーが押されたら
        if (Input.GetKeyDown(scaleKey01))
        {
            if (m_nScale[2] > m_nScale[m_nScaleState])
            {
                m_nScaleState++;
                // 各オブジェクトに対して処理を行う
                foreach (GameObject obj in objects)
                {
                    // オブジェクトがプレイヤーでない場合に拡大・縮小を行う
                    if (!obj.CompareTag("Player"))
                    {
                        // オブジェクトのスケールを変更する
                        obj.transform.localScale *= m_fScaleFactor;
                    }
                }

            }

        }

        if (Input.GetKeyDown(scaleKey02))
        {
            if (m_nScale[0] < m_nScale[m_nScaleState])
            {
                m_nScaleState--;
                // 各オブジェクトに対して処理を行う
                foreach (GameObject obj in objects)
                {
                    // オブジェクトがプレイヤーでない場合に拡大・縮小を行う
                    if (!obj.CompareTag("Player"))
                    {
                        // オブジェクトのスケールを変更する
                        obj.transform.localScale /= m_fScaleFactor;
                    }
                }
            }

        }

    }
}
