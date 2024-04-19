//------------------------------
// 担当者：中島　愛音
// 遷移
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_PauseEvent : MonoBehaviour
{
    [SerializeField, Header("衝突と取る距離")]
    private float m_hitDistance = 90.0f;

    [SerializeField, Header("遷移先")]
    private string m_nextSceneName;

    private HandLandmarkListAnnotation[] m_handLandmark = new HandLandmarkListAnnotation[2];

    private GameObject m_mainCanvas;

    private List<GameObject> m_handPoints = new List<GameObject>(); // 見つけたプレハブを格納するリスト

    private CS_HandSigns m_handSigns;
    // Start is called before the first frame update
    void Start()
    {
        m_mainCanvas = GameObject.Find("Main Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        //メインキャンバスからPoint Annotation(Clone)を捜索
        FindPointAnnotationClonePrefabs(m_mainCanvas.transform);
        if (m_handPoints.Count == 0) { return; }

        Vector2 uiObjectScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        Debug.Log("テキストのスクリーン座標" + uiObjectScreenPos);
        bool isColliding = false;
        foreach (GameObject handPoint in m_handPoints)
        {
            // m_handPointsのオブジェクトのスクリーン座標を取得
            Vector2 handPointScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, handPoint.transform.position);

            // 当たり判定を取得
            isColliding = Vector2.Distance(uiObjectScreenPos, handPointScreenPos) < m_hitDistance; // 100fは当たり判定の範囲を表す値

            // 当たり判定の結果に応じた処理を行う（例えば、当たっていたら何かしらの処理をする）
            if (isColliding)
            {
                break;
            }
        }

        if (!isColliding) { return; }

        m_handLandmark = m_handSigns.HandMark;

        CS_PauseController pauseController = GameObject.Find("PauseController").GetComponent<CS_PauseController>();

        if(m_handLandmark[0] != null)
        {
            if (m_handSigns.GetHandPose(0) == (byte)CS_HandSigns.HandPose.RockSign)
            {
                if(m_nextSceneName == "None") 
                {
                    pauseController.Restart();
                    return;
                }
                SceneManager.LoadScene(m_nextSceneName);
            }
        }
        if (m_handLandmark[1] != null)
        {
            if (m_handSigns.GetHandPose(1) == (byte)CS_HandSigns.HandPose.RockSign)
            {
                if (m_nextSceneName == "None")
                {
                    pauseController.Restart();
                    return;
                }
                SceneManager.LoadScene(m_nextSceneName);
            }
        }


    }

    public void SetHandSignData(CS_HandSigns _signs)
    {
        m_handSigns = _signs;
        if(m_handSigns == null)
        {
            Debug.Log("ハンドサインがない");
        }
    }
    private void FindPointAnnotationClonePrefabs(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // 子オブジェクトの名前が "Point Annotation(Clone)" であるかどうかを確認
            if (child.name == "Point Annotation(Clone)")
            {
                m_handPoints.Add(child.gameObject); // プレハブをリストに追加
            }

            // 子オブジェクトが他の子を持っている場合、再帰的にそれらの子オブジェクトを検索
            if (child.childCount > 0)
            {
                FindPointAnnotationClonePrefabs(child);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HIT");
    }


}
