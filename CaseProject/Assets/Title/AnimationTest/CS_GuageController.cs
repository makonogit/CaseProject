
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class CS_GuageController : MonoBehaviour
{
    private Image m_guageImage;

    [SerializeField, Header("AnimationTestスクリプト")]
    private CS_AnimationTest m_animTest;

    private int m_prevEventNum = -1;

    // Start is called before the first frame update
    void Start()
    {
        m_guageImage = GetComponent<Image>();
        m_guageImage.fillAmount = 0.0f;
        m_prevEventNum = GetComponent<CS_StageData>().GetEventNum();
    }

    // Update is called once per frame
    void Update()
    {
        //イベント数取得
        int allEventNum = GetComponent<CS_StageData>().GetAllEventNum();
        Debug.Log("イベント数" + allEventNum);
        int nowEventNum = GetComponent<CS_StageData>().GetEventNum();

        if(nowEventNum == m_prevEventNum) { return; }

        m_guageImage.fillAmount += 1.0f / (float)allEventNum;
        if(m_guageImage.fillAmount > 3.0f/allEventNum && m_animTest.ManStatus == CS_AnimationTest.MAN_STATUS.BOY)
        {
            m_animTest.ManStatus = CS_AnimationTest.MAN_STATUS.UNCLE;
        }
        else if(m_guageImage.fillAmount > 6.0f / allEventNum && m_animTest.ManStatus == CS_AnimationTest.MAN_STATUS.UNCLE)
        {
            m_animTest.ManStatus = CS_AnimationTest.MAN_STATUS.MACHO;
        }
        else if (m_guageImage.fillAmount >= 1.0f)
        {
            m_animTest.ManStatus = CS_AnimationTest.MAN_STATUS.BOY;
            m_guageImage.fillAmount = 0.0f;
        }

        m_prevEventNum = nowEventNum;
    }
}
