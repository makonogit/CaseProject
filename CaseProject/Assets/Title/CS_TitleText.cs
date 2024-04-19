using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TitleText : MonoBehaviour
{
    [SerializeField, Header("シーン先")]
    private CS_TitleHandler.TITLE_STATE m_sceneState;

    [SerializeField, Header("TitleHandler")]
    private CS_TitleHandler m_titleHandler;

    private void OnTriggerExit2D(Collider2D collision)
    {
        //遷移可能?
        if (m_titleHandler.IsChangeSceneImpossible)
        {
            //シーン先に行けるようにタイトル状態を設定
            m_titleHandler.TitleState = m_sceneState;
        }
    }
}
