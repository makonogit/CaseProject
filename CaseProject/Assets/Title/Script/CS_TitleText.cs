using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleText : MonoBehaviour
{
    [SerializeField, Header("�V�[����")]
    private CS_TitleHandler.TITLE_STATE m_sceneState;

    [SerializeField, Header("TitleHandler")]
    private CS_TitleHandler m_titleHandler;

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�J�ډ\?
        if (m_titleHandler.IsChangeSceneImpossible)
        {
            //�V�[����ɍs����悤�Ƀ^�C�g����Ԃ�ݒ�
            m_titleHandler.TitleState = m_sceneState;
        }
    }
}
