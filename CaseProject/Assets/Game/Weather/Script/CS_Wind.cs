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
    private CS_Player m_player;                            // プレイヤーのscript 追加：菅

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

    // CS_Playerを設定する関数
    // 引き数：CS_Player
    // 戻り値：ない
    public void SetCS_Player(CS_Player player) 
    {
        m_player = player;
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
        if (IsDestroyThisObject()) Destroy(this.gameObject);
    }
    
    // この風を破棄するか
    // 引き数：なし
    // 戻り値：破棄 True
    private bool IsDestroyThisObject() 
    {
        bool isTimeOver = m_nowTime > m_fDeleteTime;
        return isTimeOver && m_bDelete;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーに衝突したら風の影響を与える 追加：菅
        if (collision.transform.tag == "Player") InfluencePlayer(collision);

        // 風かを名前で判断
        if (collision.gameObject.name != this.name) return;

        CS_Wind other = collision.gameObject.GetComponent<CS_Wind>();

        // 上方向の風を生成するか
        if (!IsCreateUpperWind(other)) return;

        // 上方向の風の生成
        float addPower = this.m_fWindPower + other.m_fWindPower;
        CreateWindUpper(addPower,other);
    }

    // 上方向の風を生成するか
    // 引き数：当たった風
    // 戻り値：作るなら True
    private bool IsCreateUpperWind(CS_Wind other) 
    {    
        // 同じ方向か判断
        if (!IsFacingDirection(other)) { return false; }

        // 風の力が同じくらいか判断
        if (!IsTolerance(other)) { return false; }

        // 生成済みか
        if (m_bCreated || other.m_bCreated) { return false; }
        
        return true;
    }

    // 風の方向が向き合っているか
    // 引き数：当たった風
    // 戻り値：向き合ているなら True
    private bool IsFacingDirection(CS_Wind other) 
    {
        bool isSameDirection = this.m_eWindDirection == other.m_eWindDirection;
        if (isSameDirection) return false;
        // 左右方向のみか
        if(!this.IsHorizontal())return false;
        if(!other.IsHorizontal())return false;
        return true;
    }
    // 左右方向か
    // 引数：なし
    // 戻り値：左右ならTrue
    private bool IsHorizontal() 
    {
        if (this.m_eWindDirection == E_WINDDIRECTION.NONE) return false;
        if (this.m_eWindDirection == E_WINDDIRECTION.UP) return false;
        return true;
    }
    
    // 風の力が同じくらいか判断
    // 引き数：向かい風
    // 戻り値：同じくらいならTrue
    private bool IsTolerance(CS_Wind other) 
    {
        float addPower = this.m_fWindPower + other.m_fWindPower;
        const float tolerance = 100.0f;// 許容範囲
        bool isTolerance = addPower < tolerance && addPower > -tolerance;
        return isTolerance;
    }

    // プレイヤーに影響を与える関数
    // 引き数：コリジョン
    // 戻り値：なし
    private void InfluencePlayer(Collider2D collision) 
    {
        //collision.transform.GetComponent<CS_Player>().WindMove(m_eWindDirection, m_fWindPower);
        //Destroy(this.gameObject);
        m_player.WindMove(this.m_eWindDirection, this.m_fWindPower);
        // 消えるのを早くする
        const float lastTime = 0.25f;
        m_nowTime = m_fDeleteTime - lastTime;
    }

    // 上方向の風を生成する
    // 引き数：風の力
    // 引き数：向かい風
    // 戻り値：なし
    private void CreateWindUpper(float windPower,CS_Wind other) 
    {
        // 位置と姿勢
        Vector3 pos = this.transform.position;
        pos += other.transform.position;
        pos *= 0.5f;
        Vector3 offset = new Vector3(0,-1,0);
        Quaternion rotation = Quaternion.Euler(0,0,0);

        // オブジェクト生成
        GameObject createdWind = GameObject.Instantiate(m_objWind, pos + offset, rotation);

        // 風の強さ倍率
        const float magnification = 2.0f;
        // 風の状態設定
        CS_Wind cswind = createdWind.GetComponent<CS_Wind>();
        cswind.WindDirection = E_WINDDIRECTION.UP;              //上向きに設定　追加：菅
        cswind.m_fWindPower = Mathf.Abs(windPower * magnification);
        
        const float UpperDeleteTime = 2.0f;
        cswind.m_fDeleteTime = UpperDeleteTime;
        cswind.SetCS_Player(m_player);

        // コンポーネントが非アクティブになるのでTrueにしています
        cswind.enabled = true;
        createdWind.GetComponent<BoxCollider2D>().enabled = true;
        
        // 風を生成した
        other.m_bCreated = true;
        m_bCreated = true;
    }

}
