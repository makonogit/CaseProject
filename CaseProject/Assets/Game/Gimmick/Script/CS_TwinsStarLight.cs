using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TwinsStarLight : MonoBehaviour
{
    [SerializeField, Header("最大スケール")]
    private float m_fMaxScale = 2.0f; //最大スケール
    private float m_fMinScale = 0.0f; //最小スケール
    [SerializeField, Header("スケーリング速度")]
    public float m_fScalingSpeed = 1.0f; //スケール変化速度

    private bool scalingUp = true; // スケールを拡大中か縮小中か

    [SerializeField]
    private CS_TwinsStar m_twinsStar;

    private void Start()
    {
        if (!m_twinsStar) { Debug.LogWarning("CS_TwinsStarLightのCS_TwinsStarがありません。"); }

        transform.localScale = Vector3.one * m_fMinScale;
    }

    void Update()
    {
        //星の移動処理中なら終了
        if (m_twinsStar.IsMoveingStar) { return; }

        // 現在のスケールを取得
        Vector3 nowScale = transform.localScale;

        if (scalingUp)
        {
            //スケールを拡大
            nowScale += Vector3.one * m_fScalingSpeed * Time.deltaTime;

            //スケールが最大値に達したら縮小モードに切り替え
            if (nowScale.x >= m_fMaxScale)
            {
                nowScale = Vector3.one * m_fMaxScale;
                scalingUp = false;

                m_twinsStar.SwapStar();//二つの星のレイヤーを入れ替える
            }
        }
        else
        {
            //スケールを縮小
            nowScale -= Vector3.one * m_fScalingSpeed * Time.deltaTime;

            //スケールが最小値に達したら拡大モードに切り替え
            if (nowScale.x <= m_fMinScale)
            {
                nowScale = Vector3.one * m_fMinScale;
                scalingUp = true;
                m_twinsStar.RestartMoveStar();//星の移動を再開
            }
        }

        //スケールを更新
        transform.localScale = nowScale;
    }
}
