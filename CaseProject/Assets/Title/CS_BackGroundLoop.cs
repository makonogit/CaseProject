//-----------------------------------------------
//’S“–ÒF’†“‡ˆ¤‰¹
//”wŒi‚ÌƒVƒtƒgˆ—
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_BackGroundLoop : MonoBehaviour
{
    [SerializeField,Header("”wŒi‰æ‘œ3–‡")]
    private GameObject[] m_backGrounds = new GameObject[3]; //”wŒi‚Ì”z—ñ
    [SerializeField, Header("ƒS[ƒ‹")]
    private GameObject m_goalBackGround;
    [SerializeField, Header("ƒS[ƒ‹‚ªƒoƒbƒNƒOƒ‰ƒEƒ“ƒh‚Ì‰½–‡–Úæ‚©")]
    private int m_GoalBackNum;

    [SerializeField, Header("ƒ^ƒCƒgƒ‹ŠÇ—ƒXƒNƒŠƒvƒg")]
    private CS_TitleHandler m_titleHandler;

    private Camera mainCamera;
    private float m_backgroundHeight; //”wŒi‚Ì‚‚³

    private void Awake()
    {
        mainCamera = Camera.main;//ƒƒCƒ“ƒJƒƒ‰‚ğæ“¾
        //”wŒi‰æ‘œ‚Ì‚‚³‚ğæ“¾
        m_backgroundHeight = m_backGrounds[0].GetComponent<SpriteRenderer>().bounds.size.y;

        //”wŒi‰æ‘œ‚ğc‚É“™ŠÔŠu‚É•À‚×‚é
        for (int i = 1; i < m_backGrounds.Length; i++)
        {
            Vector3 newPosition = m_backGrounds[0].transform.position + Vector3.up * i * m_backgroundHeight;
            m_backGrounds[i].transform.position = newPosition;
        }

        m_goalBackGround.transform.position = m_backGrounds[0].transform.position + Vector3.up * m_GoalBackNum * m_backgroundHeight;
    }
   

    void Update()
    {
        //”wŒi‚ª–³‚¢‚È‚çI—¹
        if (m_backGrounds[0] == null) { return; }

        if (m_titleHandler.TitleState == CS_TitleHandler.TITLE_STATE.STOP)
        {
            DestroyBackObjcts();//”wŒi‚ğÁ‚·
            return;
        }

        //”wŒi‚ğƒXƒNƒ[ƒ‹‚³‚¹‚é
        for (int i = 0; i < m_backGrounds.Length; i++)
        {
            GameObject background = m_backGrounds[i];
            //ƒJƒƒ‰‚ªŒ»İ‚Ì”wŒiŠO‚Éo‚½‚çA”wŒi‚ğˆê”Ôã‚ÉƒVƒtƒg
            if (background.transform.position.y + m_backgroundHeight * 1.5f < mainCamera.transform.position.y)
            {
                ShiftBackGround(background, i);
            }
        }
        
    }

   void ShiftBackGround(GameObject back,int num)
   {
        //ˆê”Ôã‚É‚ ‚é”wŒi‰æ‘œ‚Ì—v‘f”Ô†‚ğæ“¾
        int top = (num + (m_backGrounds.Length -1)) % m_backGrounds.Length;
        //V‚µ‚¢ˆÊ’u‚ğİ’è
        Vector3 newPos = m_backGrounds[top].transform.position + Vector3.up * m_backgroundHeight;
        m_backGrounds[num].transform.position = newPos;
   }

    void DestroyBackObjcts()
    {
        for (int i = 0; i < m_backGrounds.Length; i++)
        {
            Destroy(m_backGrounds[i]);
        }
        m_backGrounds[0] = null;
    }
}
