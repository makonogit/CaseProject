//------------------------------
// �S���ҁF�����@����
// �J��
//------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_PauseEvent : MonoBehaviour
{
    [SerializeField, Header("�Փ˂Ǝ�鋗��")]
    private float m_hitDistance = 90.0f;

    [SerializeField, Header("�J�ڐ�")]
    private string m_nextSceneName;

    private HandLandmarkListAnnotation[] m_handLandmark = new HandLandmarkListAnnotation[2];

    private GameObject m_mainCanvas;

    private List<GameObject> m_handPoints = new List<GameObject>(); // �������v���n�u���i�[���郊�X�g

    private CS_HandSigns m_handSigns;
    // Start is called before the first frame update
    void Start()
    {
        m_mainCanvas = GameObject.Find("Main Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        //���C���L�����o�X����Point Annotation(Clone)��{��
        FindPointAnnotationClonePrefabs(m_mainCanvas.transform);
        if (m_handPoints.Count == 0) { return; }

        Vector2 uiObjectScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        Debug.Log("�e�L�X�g�̃X�N���[�����W" + uiObjectScreenPos);
        bool isColliding = false;
        foreach (GameObject handPoint in m_handPoints)
        {
            // m_handPoints�̃I�u�W�F�N�g�̃X�N���[�����W���擾
            Vector2 handPointScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, handPoint.transform.position);

            // �����蔻����擾
            isColliding = Vector2.Distance(uiObjectScreenPos, handPointScreenPos) < m_hitDistance; // 100f�͓����蔻��͈̔͂�\���l

            // �����蔻��̌��ʂɉ������������s���i�Ⴆ�΁A�������Ă����牽������̏���������j
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
            Debug.Log("�n���h�T�C�����Ȃ�");
        }
    }
    private void FindPointAnnotationClonePrefabs(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // �q�I�u�W�F�N�g�̖��O�� "Point Annotation(Clone)" �ł��邩�ǂ������m�F
            if (child.name == "Point Annotation(Clone)")
            {
                m_handPoints.Add(child.gameObject); // �v���n�u�����X�g�ɒǉ�
            }

            // �q�I�u�W�F�N�g�����̎q�������Ă���ꍇ�A�ċA�I�ɂ����̎q�I�u�W�F�N�g������
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
