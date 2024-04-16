//------------------------------
// 担当者：中島　愛音
// 雲が衝突しているか
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CheckTouchClouds : MonoBehaviour
{
    [SerializeField, Header("TitleHandler")]
    private CS_TitleHandler m_titleHandler;
    private bool m_isTouchCloud = true;//雲が触れているか
    private float m_waitTime = 0.0f;//雲がすべて画面から消えた後の待機時間

    private void Update()
    {
        //雲が触れていない
        if (!m_isTouchCloud)
        {
            m_waitTime += Time.deltaTime;//待機時間を加算
            
        }

        //１秒後
        if(m_waitTime>=1.0f)
        {
            m_titleHandler.LogoActiveTrue();//GAMEとENDのロゴを表示する
            
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cloud"))
        {
            // CloudタグのオブジェクトがTriggerから出た場合
            // すべてのCloudオブジェクトが離れたかどうかを確認する
            CheckIfAllCloudsExited();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Cloud"))
        {
            // Cloudタグのオブジェクトがコライダーの範囲内にある場合
            m_isTouchCloud = true;

            m_waitTime = 0.0f;
        }
    }

    private void CheckIfAllCloudsExited()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        bool isStillTouchingCloud = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Cloud"))
            {
                // まだCloudに触れているオブジェクトがある場合はフラグを立てる
                isStillTouchingCloud = true;
                break;
            }
        }

        // すべてのCloudオブジェクトが離れた場合、m_isTouchCloudをfalseに設定する
        m_isTouchCloud = isStillTouchingCloud;
    }

   
}
