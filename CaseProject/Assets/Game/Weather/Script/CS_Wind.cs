//------------------------------
//担当者:菅眞心
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Mediapipe.CopyCalculatorOptions.Types;

public class CS_Wind : MonoBehaviour
{

    [SerializeField]private GameObject m_objWind;
    private Vector3 m_vec3CameraPos;
    private float m_fWindPower = 1.0f;

    private float m_nowTime = 0.0f;
    private bool m_bCreated = false;
    [SerializeField]private bool m_bDelete = true;
    [SerializeField]private float m_fDeleteTime = 3.0f;

    private bool m_IsWindEnd = false;   //風の端かどうか

    [SerializeField, Header("Animator")]
    private Animator m_ThisAnim;

   // [SerializeField, Header("Playerscript")]
   // private CS_Player m_player;                            // プレイヤーのscript 追加：菅

    //風の向き
    public enum E_WINDDIRECTION
    {
        NONE,   //なし
        LEFT,   //左
        RIGHT,  //右
        UP      //上
    }

    [SerializeField,Header("風の向き")]
    //風の向き変数
    private E_WINDDIRECTION m_eWindDirection = E_WINDDIRECTION.NONE;

    public E_WINDDIRECTION WindDirection
    {
        set
        {
            m_eWindDirection = value;
        }
        get
        {
            return m_eWindDirection;
        }
    }

    public bool DeleteFlag 
    {
        set 
        {
            m_bDelete = value;
        }
    }

    //風のgetter,setter
    public float WindPower
    {
        set
        {
            m_fWindPower = value;
        }
        get
        {
            return m_fWindPower;
        }
    }

    public Vector3 SetCameraPos 
    {
        set { m_vec3CameraPos = value; }
    }

    public bool IsWindEnd
    {
        set { m_IsWindEnd = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_bCreated =false;

        //終端だったらアニメーションを再生
        if (m_IsWindEnd) { this.GetComponent<Animator>().SetBool("End", true); }

        //    if (!m_player) { Debug.LogWarning("Playerのscriptが設定されていません"); }
        //    //プレイヤーの移動関数を直接呼び出し
        //    m_player.WindMove(m_eWindDirection, m_fWindPower);

        Debug.Log(m_eWindDirection);

        if (m_eWindDirection == E_WINDDIRECTION.UP) { m_ThisAnim.SetBool("Up", true); }

    }

    // Update is called once per frame
    void Update()
    {

        m_nowTime += Time.deltaTime;


        bool isTimeOver = m_nowTime > m_fDeleteTime;
        if (isTimeOver && m_bDelete) Destroy(this.gameObject);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに衝突したら風の影響を与える 追加：菅
        if (collision.transform.tag == "Player")
        {
            Debug.Log("風ーーーーーーーーーー！");
            //collision.transform.GetComponent<CS_Player>().WindMove(m_eWindDirection, m_fWindPower);
            //Destroy(this.gameObject);
            const float lastTime = 0.25f;
            m_nowTime = m_fDeleteTime - lastTime;
        }

        // 風かを名前で判断
        if (collision.gameObject.name != this.name)return;

        CS_Wind other = collision.gameObject.GetComponent<CS_Wind>();

        // 同じ方向か判断
        bool isSameDirection =  this.m_eWindDirection == other.m_eWindDirection;
        bool isUp = this.m_eWindDirection == E_WINDDIRECTION.UP || other.m_eWindDirection == E_WINDDIRECTION.UP;
        if (isSameDirection && !isUp) { Debug.Log("方向"); return; }

        // 風の力が同じくらいか判断
        float ThisScale = m_fWindPower;//this.transform.localScale.x;
        float OtherScale = other.m_fWindPower;//collision.transform.localScale.x;

        float addPower = OtherScale + ThisScale;
        const float tolerance = 100.0f;// 許容範囲
        Debug.Log(addPower);
        bool isTolerance = addPower < tolerance && addPower > -tolerance;
        if (!isTolerance) { Debug.Log("許容範囲"); return; }

        // 生成済みか
        if (m_bCreated || other.m_bCreated) { Debug.Log("生成済み"); return; }
        // 上方向の風の生成
        Vector3 pos = m_vec3CameraPos;
        pos.y = -2.0f;
        pos.z = 0;
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        GameObject createdWind = GameObject.Instantiate(m_objWind, pos, rotation);
        CS_Wind cswind = createdWind.GetComponent<CS_Wind>();
        cswind.WindDirection = E_WINDDIRECTION.UP;              //上向きに設定　追加：菅
        cswind.m_fWindPower = Mathf.Abs(addPower * 2.0f);
        cswind.enabled = true;                                  //スクリプトオフになる意味わからん
        createdWind.GetComponent<BoxCollider2D>().enabled = true;

        GameObject Player = GameObject.Find("Shirius");
        if (Player != null)
        {
            Player.GetComponent<CS_Player>().WindMove(cswind.m_eWindDirection, cswind.m_fWindPower);
        }

        //Vector3 scale =createdWind.transform.localScale;
        //scale.x = cswind.m_fWindPower;
        //Debug.Log(m_fWindPower);
        //createdWind.transform.localScale = scale;

        other.m_bCreated = true;
        m_bCreated = true;
    }



}
