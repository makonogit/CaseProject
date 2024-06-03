//-----------------------------------------------
//担当者：中島愛音
//星パワーを集める：シリウス本体(CS_Playerがあるとこ)にアタッチ
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_StarPower : MonoBehaviour
{
    [Header("シリウス本体(CS_Playerがあるとこ)にアタッチ")]

    [Header("星のパーツ画像リスト")]
    [Header("0:Power2")]
    [Header("1:Power3")]
    [Header("2:Power4")]
    [Header("3:Power5")]
    [Header("4:PowerFace")]

    [SerializeField] private List<Sprite> m_starSprites = new List<Sprite>();

    public int nowSprite = 0;

    [SerializeField, Header("星のキャラクター")]
    private GameObject m_starChild;

    [SerializeField, Header("星の欠片のタグ")]
    private string m_sPieceTag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(m_sPieceTag))
        {
            Destroy(collision.gameObject);//欠片オブジェクトを消去

            SpriteRenderer starChildRender = m_starChild.GetComponent<SpriteRenderer>();
            if(nowSprite == m_starSprites.Count -1)
            {
                //星の子の顔オブジェクトを作成
                GameObject childObject = new GameObject("StarChildFace");

                //顔オブジェクトを親オブジェクトの子に設定
                childObject.transform.parent = m_starChild.transform;

                //顔オブジェクトにSpriteRendererコンポーネントを追加
                SpriteRenderer faceRender = childObject.AddComponent<SpriteRenderer>();

                //顔オブジェクトのスプライトを設定
                faceRender.sprite = m_starSprites[nowSprite];

                //顔オブジェクトの位置を親オブジェクトに対して調整
                childObject.transform.localPosition = Vector3.zero;
                childObject.transform.localScale = new Vector3(1f, 1f, 1f);
               
                faceRender.sortingOrder = starChildRender.sortingOrder + 1;
                Destroy(this);
                return;
            }

            //スプライトを設定
            starChildRender.sprite = m_starSprites[nowSprite];
            nowSprite++;//スプライト番号加算
        }
    }
}
