
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class CS_GuageController : MonoBehaviour
{
    private Image m_guageImage;

    [SerializeField, Header("AnimationTestスクリプト")]
    private CS_AnimationTest m_animTest;

    // Start is called before the first frame update
    void Start()
    {
        m_guageImage = GetComponent<Image>();
        m_guageImage.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_guageImage.fillAmount += 0.1f / 100.0f;
        if(m_guageImage.fillAmount > 0.33f && m_animTest.ManStatus == CS_AnimationTest.MAN_STATUS.BOY)
        {
            m_animTest.ManStatus = CS_AnimationTest.MAN_STATUS.UNCLE;
        }
        else if(m_guageImage.fillAmount > 0.66f && m_animTest.ManStatus == CS_AnimationTest.MAN_STATUS.UNCLE)
        {
            m_animTest.ManStatus = CS_AnimationTest.MAN_STATUS.MACHO;
        }
        else if (m_guageImage.fillAmount >= 1.0f)
        {
            m_animTest.ManStatus = CS_AnimationTest.MAN_STATUS.BOY;
            m_guageImage.fillAmount = 0.0f;
        }
    }
}
