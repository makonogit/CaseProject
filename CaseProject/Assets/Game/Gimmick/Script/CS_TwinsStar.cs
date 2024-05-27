//-----------------------------------------------
//担当者：中島愛音
//双子の処理
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_TwinsStar : MonoBehaviour
{
    [SerializeField, Header("双子のオブジェクト")]
    //星の双子オブジェクト
    private GameObject[] m_twinsObject = new GameObject[2];

    [SerializeField, Header("シリウス本体のスプライトレンダラー")]
    private SpriteRenderer m_srSubstance;
    [SerializeField, Header("シリウスの指のスプライトレンダラー（左右どっちでも）")]
    private SpriteRenderer m_srFinger;

    [SerializeField, Header("動く幅")]
    private float m_fAmplitude = 3.0f;  // 振れ幅
    [SerializeField, Header("スピード")]
    private float m_fSpeed = 1.0f;  //移動スピード

    [SerializeField, Header("ジャンプ力の+補正値")]
    private float m_fJumpPlusPower = 0.0f;
    [SerializeField, Header("ジャンプ力の-補正値")]
    private float m_fJumpMinusPower = 0.0f;

    private Vector3 m_initialPosition;//初期位置
    private float m_fElapsedTime = 0.0f;//経過時間

    private Rigidbody2D m_rb;//リジッドボディ
    private Vector3 m_prevVelocity;

    private Vector3 m_prevPosition;

    private int m_nNowStar = 0;//現在動いている星番号

    bool m_isMoveingStar = true;



    //ジャンプの補正値を取得
    public float JumpPower
    {
        get
        {
            if (m_nNowStar == 0)
            {
                return m_fJumpPlusPower;
            }
            else
            {
                return -m_fJumpPlusPower;
            }
        }
    }

    public bool IsMoveingStar
    {
        get
        {
            return m_isMoveingStar;
        }
    }

    // Start is called before the first frame updateｃ
    void Start()
    {
        m_initialPosition = m_twinsObject[0].transform.localPosition;

        //双子のOrderInLayerを設定
        SpriteRenderer spriteRenderer0 = m_twinsObject[0].GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer1 = m_twinsObject[1].GetComponent<SpriteRenderer>();
        int fingerOrder = m_srFinger.sortingOrder;
        spriteRenderer0.sortingOrder = fingerOrder - 1;
        spriteRenderer1.sortingOrder = fingerOrder - 2;

        m_rb = GetComponent<Rigidbody2D>();
        m_prevVelocity = m_rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        //ジャンプ補正処理
        AddJumpPower();

        if (!m_isMoveingStar) { return; }

        // 経過時間を更新
        m_fElapsedTime += Time.deltaTime;

        //右斜めに動く
        float x = m_fAmplitude * Mathf.Sin(m_fElapsedTime * m_fSpeed - Mathf.PI);
        float y = m_fAmplitude * Mathf.Sin(m_fElapsedTime * m_fSpeed - Mathf.PI);
        Vector2 newPosition = m_initialPosition + new Vector3(x, y, 0);

        //星の位置更新
        m_twinsObject[m_nNowStar].transform.localPosition = newPosition;

        //レイヤーの更新処理
        SpriteRenderer spriteRenderer = m_twinsObject[m_nNowStar].GetComponent<SpriteRenderer>();

        int backOrder = m_srSubstance.sortingOrder - 1;//シリウスの胴体の後ろのレイヤー番号
        int frontOrder = m_srFinger.sortingOrder - 1;//シリウスの指のひとつ後ろのレイヤー番号
        //前のポジションより左側にいる？
        if (newPosition.x < m_prevPosition.x)
        {
            spriteRenderer.sortingOrder = frontOrder;//指のレイヤーのひとつ下のレイヤーに設定
        }
        else
        {
            spriteRenderer.sortingOrder = backOrder;//胴体のレイヤーのひとつ下のレイヤーに設定
        }

        m_prevPosition = newPosition;//ポジションデータ更新

        //initialPositionとのベクトルを取り、initializePositionより右側にいるかつ、距離が0.1以下ならspeedを0にする処理
        float distance = Vector3.Distance(newPosition, m_initialPosition);
        if (newPosition.x > m_initialPosition.x && distance <= 0.1f && spriteRenderer.sortingOrder == frontOrder)
        {
            m_twinsObject[m_nNowStar].transform.localPosition = m_initialPosition;
            m_isMoveingStar = false;
        }
    }

    private void AddJumpPower()
    {
        //前の速度より現在の速度が大きいなら力を+補正する
        bool isAddPower = m_prevVelocity.y <= 0.0f && m_rb.velocity.y > 0.0f;
        if (isAddPower)
        {
            m_rb.AddForce(Vector3.up * m_fJumpPlusPower, ForceMode2D.Impulse);
            Debug.Log("＋補正");
        }

        m_prevVelocity = m_rb.velocity;
    }

    //二つの星のレイヤーを入れ替える
    public void SwapStar()
    {
        SpriteRenderer spriteRenderer0 = m_twinsObject[m_nNowStar].GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer1 = m_twinsObject[(m_nNowStar + 1) % 2].GetComponent<SpriteRenderer>();
        int tmp = spriteRenderer0.sortingOrder;
        spriteRenderer0.sortingOrder = spriteRenderer1.sortingOrder;
        spriteRenderer1.sortingOrder = tmp;
        
    }

    //星の移動を再スタート
    public void RestartMoveStar()
    {
        m_isMoveingStar = true;
        m_nNowStar++;
        m_nNowStar = m_nNowStar % 2;
        m_fElapsedTime = 0.0f;
    }
}
