//------------------------------
// 担当者：中川 直登
//------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//------------------------------
// 敵を生成するスクリプト
//------------------------------
public class CS_EnemysGenerater : MonoBehaviour
{
    [Header("敵のオブジェ")]
    [SerializeField] private GameObject m_objEnemy;
    [SerializeField] private float m_fGenerateDelay;    // 生成にかかる時間
    private float m_fGenerateTime;                      // 生成時間

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 時間を計算
        m_fGenerateTime += Time.deltaTime;
        // 生成時間を経過したか
        bool isGenerate = m_fGenerateTime > m_fGenerateDelay;
        if (isGenerate) GenerateEnemy(m_objEnemy);
    }

    // 敵を生成する関数
    // 引数：敵オブジェ
    // 戻り値：なし
    private void GenerateEnemy(GameObject enemy)
    {
        // 角度設定
        Quaternion rotation = Quaternion.EulerAngles(0, 0, 0);
        // 位置設定
        Vector3 position = transform.position;
        // 敵の生成
        GameObject.Instantiate(enemy, position, rotation);
        // 時間のリセット
        m_fGenerateTime = 0;
    }
}
