//------------------------------------
//担当者：菅眞心
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//------------------------------------
//プレイヤークラス
//------------------------------------
public class CS_Player : MonoBehaviour
{
    private float m_fMovement = 0.0f;       //移動量
    private Vector3 m_v3DestinationPos;     //移動目的地

    private float m_fStartHeight;           //開始時の高さ
    private bool m_IsStartFry = false;      //浮遊開始したか

    [SerializeField, Header("自分のTransForm")]
    private Transform m_tThisTrans;

    [SerializeField, Header("自分のRigidbody")]
    private Rigidbody2D m_rThisRigidbody;

    [SerializeField, Header("星の子TransForm")]
    private Transform m_tStarChildTrans;

    [SerializeField, Header("シリウスのアニメーター")]
    private Animator m_aThisAnimator;

    [SerializeField, Header("最大上昇速度")]
    private float m_fMaxUpSpeed = 10.0f;

    [SerializeField, Header("上昇倍率")]
    private float m_fUpPower = 5.0f;

    [SerializeField, Header("減速倍率")]
    private float m_fSpeedDown = 30.0f;

    [SerializeField, Header("左右の移動速度")]
    private float m_fleftright = 3.0f;

    [SerializeField, Header("壁に当たった時の跳ね返り")]
    private float m_fWallReflect = 2.0f; 


    [SerializeField, Header("Effect表示用オブジェクトTransform")]
    private Transform m_tEffectTrans;

    [SerializeField, Header("エフェクト表示用Animator")] 
    private Animator m_aEffectAnim;

    [SerializeField, Header("ステージのGlobalLight")]
    private Light2D m_lGlobalLight;


    private bool m_isUpTrigger = false;                //上昇中か
    private bool m_isSpeedDownTrigger = false;         //減速中か
    private Vector3 m_v3NowUpPower = Vector3.zero;     //現在の上昇率

    //--------------------
    //　手の動く処理用
    //--------------------
    [SerializeField, Header("手のTransForm")]
    private Transform m_tHandTrans;

    [SerializeField, Header("手の浮遊幅")]
    private float m_fHandFryLength = 0.1f;

    [SerializeField, Header("手の浮遊スピード")]
    private float m_fHandFrySpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_tThisTrans) Debug.LogWarning("シリウスのTransFormが設定されていません");
        if (!m_rThisRigidbody) Debug.LogWarning("シリウスのRigidBodyが設定されていません");
        if (!m_tHandTrans) Debug.LogWarning("シリウスの手のTransFormが設定されていません");
        if (!m_aThisAnimator) Debug.LogWarning("シリウスのAnimatorが設定されていません");
        if (!m_tEffectTrans) Debug.LogWarning("EffectオブジェクトのTransformが設定されていません");
        if (!m_aEffectAnim) Debug.LogWarning("Effect用のAnimatorが設定されていません");
        if (!m_tStarChildTrans) Debug.LogWarning("星の子TransFormが設定されていません");
        if (!m_lGlobalLight) Debug.LogWarning("GlobalLightが設定されていません");

        m_fStartHeight = m_tThisTrans.position.y;   // 開始時の高さ

        //管理クラスにデータ保存
        ObjectData.m_tPlayerTrans = m_tThisTrans;
        ObjectData.m_tStarChildTrans = m_tStarChildTrans;
        ObjectData.m_lGlobalLight = m_lGlobalLight;
    }

    // Update is called once per frame
    void Update()
    {
        //離陸したら離陸アニメーションの再生
        if (m_fStartHeight < m_tThisTrans.position.y && !m_IsStartFry) 
        {
            m_IsStartFry = true;
            m_aThisAnimator.SetBool("TakeOff", true); 
        }


        //すり抜け防止(簡易実装)
        if(m_tThisTrans.position.x > 8.8f)
        {
            Vector3 position = m_tThisTrans.position;
            position.x = 7.8f;
            m_tThisTrans.position = position;
        }
        if (m_tThisTrans.position.x < -8.8f)
        {
            Vector3 position = m_tThisTrans.position;
            position.x = -7.8f;
            m_tThisTrans.position = position;
        }


    }

    private void FixedUpdate()
    {
        //手がふわふわ動く処理
        m_tHandTrans.position = new Vector3(m_tHandTrans.position.x, m_tHandTrans.position.y + Wave, 0.0f);

        if (!m_isUpTrigger) { return; }

        ////時間まで上昇し続ける
        //m_fTimeMeasure += Time.deltaTime;

        //if(m_fTimeMeasure < m_fUpTime) 
        //{
        //    //上昇終了したらパラメータを初期化
        //    m_fTimeMeasure = 0.0f;
        //    m_isUpTrigger = false;
        //    return; 
        //}

        //最大速度まで上昇したら減速する
        if(m_rThisRigidbody.velocity.magnitude > m_fMaxUpSpeed) 
        {
            m_rThisRigidbody.velocity = new Vector2(m_rThisRigidbody.velocity.x, m_fMaxUpSpeed);            
            m_isSpeedDownTrigger = true; }

        //減速処理
        if (m_isSpeedDownTrigger)
        {
            m_v3NowUpPower.y -= m_fSpeedDown * Time.deltaTime;

            Debug.Log(m_v3NowUpPower.y);

            //減速しきったら終了
           if(m_v3NowUpPower.y < 0.0f)
           {
               m_isSpeedDownTrigger = false;
               m_isUpTrigger = false;
                return;
           }
            
        }

        m_rThisRigidbody.AddForce(m_v3NowUpPower, ForceMode2D.Force);

    }

    //上昇速度変更
    public float UPPOWER
    {
        set
        {
            m_v3NowUpPower.y += value;
        }
        get
        {
            return m_v3NowUpPower.y;
        }
    }

    //ふわふわ動くようにSin波で値を返す
    private float Wave
    {
        get
        {
            return Mathf.Sin(Time.time * m_fHandFrySpeed) * m_fHandFryLength;
        }
    }

    //------------------------------------
    //ノックバック関数
    //引数：ノックバックの方向,ノックバックの強さ
    //------------------------------------
    public void KnockBack(Vector3 knockbackdirection,float knockbackpower)
    {

        //SE再生
        ObjectData.m_csSoundData.PlaySE("KnockBack");

        //ノックバックの方向と強さを考慮して目的地を設定
        //m_v3DestinationPos = m_tThisTrans.position - (knockbackdirection * knockbackpower);

        //m_tThisTrans.position = m_v3DestinationPos;

        //Effect発火(衝突した位置で再生)
        m_tEffectTrans.position = new Vector3(m_tThisTrans.position.x + knockbackdirection.x, m_tThisTrans.position.y + knockbackdirection.y, 0.0f);
        m_aEffectAnim.SetTrigger("Damage"); //ダメージAnimationを再生

        Vector3 dir = m_tThisTrans.position - (knockbackdirection * knockbackpower);
        dir.y *= -5.0f;

        m_rThisRigidbody.AddForce(dir);
        //m_rThisRigidbody.velocity = m_tThisTrans.position - (knockbackdirection * knockbackpower);

        Debug.Log("ノックバック" + (m_tThisTrans.position - knockbackdirection * knockbackpower));
        
    }

    //------------------------------------
    //風の影響設定関数
    //引数：風の向き
    //引数：風の強さ
    //------------------------------------
    public void WindMove(CS_Wind.E_WINDDIRECTION distination,float windpower)
    {
        
        Vector3 Direction = Vector3.zero;

        float power = windpower;

        m_aThisAnimator.SetBool("Jump", false);

        //向きによって方向を設定
        switch (distination)
        {
            case CS_Wind.E_WINDDIRECTION.NONE:
                Direction = Vector3.zero;
                break;
            case CS_Wind.E_WINDDIRECTION.LEFT:
                Direction = Vector3.right;
                power *= m_fleftright;      //風に倍率をかけて威力を上げる
                //左移動アニメーションを再生
                m_aThisAnimator.SetBool("Left", true);
                break;
            case CS_Wind.E_WINDDIRECTION.RIGHT:
                Direction = Vector3.left;
                power *= m_fleftright;      //風に倍率をかけて威力を上げる
                //右移動アニメーションを再生
                m_aThisAnimator.SetBool("Right", true);
                break;
            case CS_Wind.E_WINDDIRECTION.UP:
                Direction = Vector3.up;

                power *= m_fUpPower;                //風に倍率をかけて威力を上げる
                m_v3NowUpPower = Direction * power; //力を保存
                m_isUpTrigger = true;               //上昇中に設定

                //浮遊アニメーションを再生
                m_aThisAnimator.SetBool("Fry", true);
                return;
                break;
        }

        //m_rThisRigidbody.velocity = m_v3NowUpPower;

        //Debug.Log("力のくわえる向き" + Direction * windpower);
        m_rThisRigidbody.AddForce(Direction * power, ForceMode2D.Force);
        //m_rThisRigidbody.velocity = Direction * power;

    }


    //------------------------------------
    //ゴール関数
    //------------------------------------
    private void OnGoal()
    {
        //アニメーション再生
        m_aThisAnimator.SetTrigger("Gole");
    }


    //-----------------------------------------------
    // 浮遊アニメーションの終了(Animationで呼び出し)
    //-----------------------------------------------
    private void WindAnimEnd()
    {
        m_aThisAnimator.SetBool("Jump", true);
        m_aThisAnimator.SetBool("Fry", false);
    }

    //-----------------------------------------------
    // 右左移動アニメーションの終了(Animationで呼び出し)
    //-----------------------------------------------
    private void MoveAnimEnd()
    {
        m_aThisAnimator.SetBool("Jump", true);
        m_aThisAnimator.SetBool("Left", false);
        m_aThisAnimator.SetBool("Right",false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //壁に当たったらノックバック
        if (collision.transform.tag == "Stage")
        {
            //Debug.Log("壁");
            Vector3 dir = m_tThisTrans.position - collision.transform.position;
            dir.y = 0.0f;

            Vector3 force = m_tThisTrans.position - (dir * m_fWallReflect);

            m_rThisRigidbody.AddForce(force);
            //KnockBack(dir, m_fWallReflect);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゴールに到着したらゴールイベントの発火
        if(collision.transform.tag == "Goal")
        {
            OnGoal();
        }

        //風に当たったら風の影響を受ける
        //if(collision.transform.tag == "Wind")
        //{
        //    CS_Wind wind = transform.GetComponent<CS_Wind>();
        //    if (!wind) return;

        //    float power = wind.WindPower;
        //    Debug.Log(power);
        //    Vector3 Direction = m_tThisTrans.position - collision.transform.position;
        //    WindMove(Direction, power);
        //}

    }


    //private void OnTriggerStay2D(Collider2D collision)
    //{

    //    //風に当たったら風の影響を受ける
    //    if (collision.transform.tag == "Wind")
    //    {
    //        CS_Wind wind = transform.GetComponent<CS_Wind>();
    //        if (!wind) return;

    //        float power = wind.WindPower;
            
    //        Vector3 Direction = m_tThisTrans.position - collision.transform.position;
    //        WindMove(Direction, power);
    //    }

    //}

}
