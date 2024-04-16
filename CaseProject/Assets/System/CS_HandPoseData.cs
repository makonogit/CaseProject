//-----------------------------------------------
//�S���ҁF�����S
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity;
using UnityEngine;


public class CS_HandPoseData : MonoBehaviour
{

    //����̃f�[�^
    private HandLandmarkListAnnotation[] m_HandLandmark = new HandLandmarkListAnnotation[2];

    public PointListAnnotation LeftHandData
    {
        get
        {
            return m_HandLandmark[0].GetLandmarkList();
        }
    }

    public PointListAnnotation RightHandData
    {
        get
        {
            return m_HandLandmark[1].GetLandmarkList();
        }
    }

    //�w�̃f�[�^
    public struct Fingerindex
    {
        public bool thumb;     //�e�w
        public bool index;     //�l�����w
        public bool middle;    //���w
        public bool ring;      //��w
        public bool little;    //���w

        public Fingerindex(bool t,bool i,bool m,bool r ,bool l)
        {
            thumb = t;
            index = i;
            middle = m;
            ring = r;
            little = l;
        }
    }

    //�|�[�Y�f�[�^
    public readonly Dictionary<string, Fingerindex> PoseData = new Dictionary<string, Fingerindex>
    {
        {"Zero",new Fingerindex(false,false,false,false,false) }, 
        {"One",new Fingerindex(false,true,false,false,false) },
        {"Two",new Fingerindex(false,true,true,false,false) }, 
        {"Three",new Fingerindex(false,true,true,true,false) },
        {"For",new Fingerindex(false,true,true,true,true) },
        {"Five",new Fingerindex(true,true,true,true,true) },
        {"Six",new Fingerindex(true,false,false,false,false) },
        {"Seven",new Fingerindex(true,true,false,false,false) },
        {"Eight",new Fingerindex(true,true,true,false,false) },
        {"Nine",new Fingerindex(true,true,true,true,false) },
    };

    private string m_sKey;  //�L�[�̖��O


    //��̌���
    private enum HandDirection
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }


    //===== �������p�ϐ� ========
    
    private Vector3 m_v3Currentpos;
    private float[] m_fSwingTime = new float[2] { 0.0f, 0.0f };  //���U��f�B���C�v�Z�p

    [SerializeField,Header("���U������o���铬�l")]
    private float m_fSwingThreshold = 100.0f; //�w�̉��U������o����ׂ̓��l
    [SerializeField,Header("���U�肵�Ă���̌��o�f�B���C(sc)")]
    private float m_fSwingDelay = 1.0f;       
    [SerializeField, Header("���I�u�W�F�N�g")]
    private GameObject m_objWind;


    [SerializeField, Header("���W�擾�̑ҋ@�t���[��")]
    private float m_fWaitFream = 0.2f;
    private float m_fWaitFreamTime = 0.0f;

    [SerializeField, Header("�t���[���ۊǐ�")]
    private int m_recordNum = 50;           //�ۊǐ�
    private Vector3[] m_recordPositions;    //�ۊǗp�z��
    private int m_recordIndex = 0;
    private bool m_isRecording = false;    //���R�[�f�B���O�t���O
    private bool m_isRecordFinish = false;    //���R�[�f�B���O�����t���O
    //���̃X�e�[�^�X
    public struct WindStatus
    {
        public float angle;
        public float speed;
        public float distance;
    }
    private WindStatus m_windStatus;
    //===== �J�����p�ϐ� ========
    [SerializeField, Header("��������w�̔ԍ�")]
    private int[] m_rCreateFingerNums = { 4, 8, 12, 16, 20 };

    [SerializeField, Header("���ɉJ���𐶐�����܂ł̎���")]
    private float m_rIntervalTime = 0.3f;//���ɐ�������܂ł̎���

    private float m_rNowTime = 0.0f;//���݂̎���

    [SerializeField, Header("�J�I�u�W�F�N�g")]
    private GameObject m_objRain;

    [SerializeField, Header("�_�I�u�W�F�N�g")]
    private GameObject m_CloudObj;

    //===========���������A�N�V�����p================
    private float[] m_HandDepth = new float[40];
    private int m_nPushData = -1;           //�����������������@0:�����@1:����
    private int m_nPushFream = 0;           //���݂̕ۑ��t���[��
    private const int m_nMaxPushFream = 40; //���t���[���ۑ����邩
    [SerializeField, Header("��̉��������̓��l")]
    private float m_fPushThreshold = 1.0f;
    private bool m_IsSavePush = false;       //�ۑ����I�������

    //���������f�[�^�̃Q�b�^�[
    public int PUSHDATA
    {
        get
        {
            return m_nPushData;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<m_nMaxPushFream;i++)
        {
            m_HandDepth[i] = 0.0f;
        }

        m_recordPositions = new Vector3[m_recordNum];   
    }

    // Update is called once per frame
    void Update()
    {
        //��̏����擾
        if (transform.childCount > 0 && (!m_HandLandmark[0] || !m_HandLandmark[1]))
        {
            HandLandmarkListAnnotation hand = transform.Find("HandLandmarkList Annotation(Clone)").GetComponent<HandLandmarkListAnnotation>();

            if (!m_HandLandmark[0] && (hand[20].transform.position.x < 0.0f))
            {
                m_HandLandmark[0] = hand;
            }

            if (!m_HandLandmark[1] && (hand[20].transform.position.x > 0.0f))
            {
                m_HandLandmark[1] = hand;
            }

        }

        ////--------------���̏���(�ȈՎ���)-----------------
        //if (m_HandLandmark[0] && m_HandLandmark[0].isActive)
        //{
        //    RecordFingerPosition(HandLandmarkListAnnotation.Hand.Left);
        //    CreateWind(HandLandmarkListAnnotation.Hand.Left);
        //}

        //if(m_HandLandmark[1] && m_HandLandmark[1].isActive)
        //{
        //    RecordFingerPosition(HandLandmarkListAnnotation.Hand.Right);
        //    CreateWind(HandLandmarkListAnnotation.Hand.Right);

        //}

        //----------------�J�̐�������-----------------------
        if (m_HandLandmark[0] && m_HandLandmark[0].isActive)
        {
           CreateRain(HandLandmarkListAnnotation.Hand.Left);
            PushHand(HandLandmarkListAnnotation.Hand.Left);
        }

        if (m_HandLandmark[1] && m_HandLandmark[1].isActive)
        {
           CreateRain(HandLandmarkListAnnotation.Hand.Right);
        }

        //=====�f�o�b�O�@�|�[�Y�F���m�F==========
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fingerindex data;
            data = FingerData(HandLandmarkListAnnotation.Hand.Left);
            Debug.Log(FindKeyByValue(data));
        }
    }

    public Vector3 GetHandVector(HandLandmarkListAnnotation.Hand hand)
    {
        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();
        //Vector3 currentpos = point[12].transform.position;

        //���w�̕t��������̒��S���W�Ƃ���
        Vector3 HandPos =point[9].transform.position;

        //3�̓_���擾���A���������ԃx�N�g�����v�Z
        Vector3 posA = point[5].transform.position;
        Vector3 posB = point[17].transform.position;
        Vector3 posC = point[0].transform.position;
        Vector2 vecAB = new Vector2(posB.x - posA.x, posB.y - posA.y);
        Vector2 vecAC = new Vector2(posC.x - posA.x, posC.y - posA.y);
        float cross = vecAB.x * vecAC.y - vecAB.y * vecAC.x;

        //2�̃x�N�g���̊O�ς��擾���A�ʂ̖@���x�N�g�����v�Z
        Vector3 normal = new Vector3(0,0,cross).normalized;

        //��̈ʒu���畽�ʂւ̃x�N�g�����v�Z
        Vector3 toHand = new Vector2(HandPos.x - posA.x, HandPos.y - posA.y);

        //���ʏ�ւ̎ˉe�����߂�
        Vector3 projectedHand = Vector2.Dot(toHand, new Vector2(normal.x,normal.y)) * new Vector2(normal.x,normal.y);

        //���ʂ����̕����x�N�g�������߂�
        Vector3 direction = (projectedHand - toHand).normalized;

        //Debug.Log(direction);
        
        //�ʂ̖@���x�N�g�����g���Ė����x�N�g�����v�Z
     //   Vector3 vecDir = Vector3.Cross(normal, vecAB).normalized;
        return direction;
    }


    private HandDirection GetHandDirection(Vector3 Handvector)
    {
        if(Handvector.x > 0.5f && Handvector.y < -0.5f)
        {
            return HandDirection.LEFT;
        }

        if (Handvector.x > 0.2f && Handvector.y > 0.8f)
        {
            return HandDirection.RIGHT;
        }

        if (Handvector.x > 0.9f && Handvector.y > -0.2f)
        {
            return HandDirection.DOWN;
        }

        if(Handvector.x < -0.8f && Handvector.y > 0.2f)
        {
            return HandDirection.UP;
        }

        return HandDirection.NONE;

    }

    private Vector3 CurrentVector;

    //�|�W�V�����ۊ�
    private void RecordFingerPosition(HandLandmarkListAnnotation.Hand hand)
    {

        if (m_isRecordFinish) { return; }
        //����l�����w�̐�̈ړ��ʂ��v�Z
        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();

        Vector3 Oldpos = point[5].transform.position;
        Vector3 OldVector = GetHandVector(hand);
        
        //�x�N�g�������̌������擾
        HandDirection handrirection = GetHandDirection(OldVector);

        m_fWaitFreamTime += Time.deltaTime; //�ҋ@�t���[�����Z

        Vector3 MoveDirection = Vector3.zero;

        if (Oldpos != m_v3Currentpos)
        {
            MoveDirection = (Oldpos - m_v3Currentpos).normalized;
        }

        //Debug.Log(MoveDirection);

        //�������ō��ɐU������
        if (handrirection == HandDirection.LEFT &&
            OldVector != CurrentVector && MoveDirection.x > 0.0f)
        {
            return;
        }
        ////�E�����ŉE�ɐU������
        //if(handrirection == HandDirection.RIGHT &&
        //    OldVector != CurrentVector && HorizontalDistance < 0.0f)
        //{
        //    return;
        //}
        //�������ŏ�ɐU������
        //if (handrirection == HandDirection.DOWN &&
        //    OldVector != CurrentVector && OldVector.y > CurrentVector.y)
        //{
        //    return;
        //}
        ////������ŉ��ɐU������
        //if (handrirection == HandDirection.UP &&
        //    OldVector != CurrentVector && OldVector.y < CurrentVector.y)
        //{
        //    return;
        //}

        //if(handrirection == HandDirection.NONE) { return; }

        float movement = Vector3.Distance(m_v3Currentpos, Oldpos);

        m_fSwingTime[(int)hand] += Time.deltaTime; //���U�肵�Ă���̎��Ԃ��v�Z
        //-----------------�e�X�g--------------------
        //���̎w�̏�Ԃ��擾
        Fingerindex data;
        data = FingerData(hand);
        m_sKey = FindKeyByValue(data);
        
        //----------------------------------------
        //���������ȏォ���R�[�f�B���O������Ȃ��H
        if (movement > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay &&!m_isRecording)
        {
            m_isRecording = true;//���R�[�h�J�n
           // Debug.Log("���R�[�h�J�n");
        }

        //���R�[�f�B���O�J�n���������Ă��Ȃ��H
        if (m_isRecording && !m_isRecordFinish )
        {
            m_recordPositions[m_recordIndex] = Oldpos;//���݂̎w�̈ʒu��ۑ�
            m_recordIndex++;
            //�ۊǔz��̃T�C�Y�ȏ�H
            if(m_recordIndex >= m_recordPositions.Length)
            {
                m_isRecordFinish = true;//���R�[�h����
                SetWindStatus();//���̃X�e�[�^�X��ݒ�
                m_fSwingTime[(int)hand] = 0.0f;
                //Debug.Log("���R�[�h����");
            }
        }
        if (m_fWaitFreamTime > m_fWaitFream)
        {
            //���݂̈ʒu��ۑ�
            CurrentVector = GetHandVector(hand);
            m_v3Currentpos = point[5].transform.position;
            m_fWaitFreamTime = 0.0f;
        }

    }



    //���̃X�e�[�^�X�̐ݒ�֐�
    private void SetWindStatus()
    {
        Vector3 firstPos = m_recordPositions[0];
        Vector3 finalPos = m_recordPositions[m_recordPositions.Length -1];
        m_windStatus.distance = Vector3.Distance(firstPos, finalPos);//����

        float recordTotalTime = Time.deltaTime * m_recordPositions.Length;//���R�[�h����
        //�ŏ��Ɉʒu����Ō�̈ʒu�܂ł̑��x�ݒ�
        m_windStatus.speed = m_windStatus.distance / recordTotalTime;

        //�ŏ��̃t���[���ƍŌ�̃t���[���̃x�N�g��
        Vector3 movevec = finalPos - firstPos;
       
        //�p�x�ݒ�
        m_windStatus.angle = Mathf.Atan2(movevec.y, movevec.x);
    }
    //���N�����֐�
    //����:��̍��E
    private void CreateWind(HandLandmarkListAnnotation.Hand hand) 
    {
        Fingerindex data;
        data = FingerData(hand);
        m_sKey = FindKeyByValue(data);
        //�肪�p�[��
        bool isFive = (string.Compare(m_sKey, ("Five")) == 0);
        if (!isFive) { return; }
        if (!m_isRecordFinish) { return; }

        GameObject windobj = m_objWind;
        windobj.transform.position = new Vector3(m_recordPositions[0].x, m_recordPositions[0].y, 0.0f);    //���W��ۊǂ����|�W�V�����z��̍ŏ��ɐݒ�
        Debug.Log(m_recordPositions[0]);
        CS_Wind cs_wind = windobj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾

        //--------------���̊p�x�ݒ�--------------------
        windobj.transform.eulerAngles = new Vector3(0.0f, 0.0f, m_windStatus.angle* Mathf.Rad2Deg);
        //Debug.Log("���̊p�x" + m_windStatus.angle);
        //���̃X�s�[�h
        cs_wind.Movement = m_windStatus.speed * 0.01f;

        //Debug.Log(cs_wind.Movement);
        //Debug.Log(m_sKey);

        //windobj.GetComponent<SpriteRenderer>().color = Color.green;
        cs_wind.WindPower = 1.0f* m_windStatus.speed;
        Instantiate(windobj);//���𐶐�����܂�
       
        InitRecord();//���R�[�h�֘A�̕ϐ���������
        return;
        //Debug.Log(hand);

        //����l�����w�̐�̈ړ��ʂ��v�Z
        //PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();

        //Vector3 currentpos = point[8].transform.position;

        ////�O�̃t���[������̈ړ��ʂ��v�Z
        //Vector3 movementvec = currentpos - previouspos; 

        //float movement = Vector3.Distance(currentpos, previouspos);

        ////�������̑��x���v�Z
        ////float HorizontalSpeed = Mathf.Abs(Vector3.Dot(movement, point[8].transform.right)) / Time.deltaTime;

        //m_fSwingTime[(int)hand] += Time.deltaTime; //���U�肵�Ă���̎��Ԃ��v�Z
        ////if(HorizontalSpeed > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay)

        //if (movement > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay)
        //{
        //    //Debug.Log(HorizontalSpeed);

        //    m_fSwingTime[(int)hand] = 0.0f;
        //    GameObject windobj = m_objWind;
        //    if (hand == HandLandmarkListAnnotation.Hand.Left)
        //    {
        //       // windobj.transform.position = new Vector3(windobj.transform.position.x, point[0].transform.position.y * 0.1f, 0.0f);    //���W��ݒ�
        //        windobj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);    //���W��ݒ�(�e�X�g)
        //    }
        //    else
        //    {
        //        //Debug.Log("�E��");
        //        //windobj.transform.position = new Vector3(windobj.transform.position.x * -1, point[0].transform.position.y * 0.1f, 0.0f);    //���W��ݒ�
        //    }

        //    CS_Wind cs_wind = windobj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾

        //    //���̎w�̏�Ԃ��擾
        //    Fingerindex data;
        //    data = FingerData(hand);
        //    m_sKey = FindKeyByValue(data);

        //    //--------------���̊p�x�ݒ�--------------------
        //    float angle = Mathf.Atan2(movementvec.y, movementvec.x);
        //    windobj.transform.eulerAngles = new Vector3(0.0f,0.0f,angle*Mathf.Rad2Deg);
        //    Debug.Log("���̊p�x" + angle);

        //    //Debug.Log(cs_wind.Movement);
        //    Debug.Log(m_sKey);

        //    //���̐��� �w�̏�Ԃɂ���ĕ��̋�����ύX
        //    switch (m_sKey)
        //    {
        //        case "One":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.blue;
        //            cs_wind.WindPower = 0.2f;
        //            Instantiate(windobj);
        //            break;
        //        case "Two":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.green;
        //            cs_wind.WindPower = 0.4f;
        //            Instantiate(windobj);
        //            break;
        //        case "Three":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.yellow;
        //            cs_wind.WindPower = 0.6f;
        //            Instantiate(windobj);
        //            break;
        //        case "For":
        //            windobj.GetComponent<SpriteRenderer>().color = Color.red;
        //            cs_wind.WindPower = 0.8f;
        //            Instantiate(windobj);
        //            break;
        //        default:
        //            //Debug.Log("�����N�����Ȃ�");
        //            break;
        //    }
        //}

        //m_fWaitFreamTime += Time.deltaTime; //�ҋ@�t���[�����Z
        //if (m_fWaitFreamTime > m_fWaitFream)
        //{
        //    //���݂̈ʒu��ۑ�
        //    previouspos = currentpos;
        //    m_fWaitFreamTime = 0.0f;
        //}


    }


    //���������֐�
    //�����F��̍��E
    //�߂�l�F-1 ����s�@0:�������@1:������
    private int PushHand(HandLandmarkListAnnotation.Hand hand)
    {
        Fingerindex data;
        data = FingerData(hand);
        m_sKey = FindKeyByValue(data);

        //����L���Ă��Ȃ�������v�Z���Ȃ�
        if(m_sKey != "Five")
        {
            m_nPushFream = 0;
            return -1;
        }

        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();
        //���w�̕t��������̒��S�Ƃ���Z���W��ۑ�
        float HandPos = point[9].transform.position.z;

        //��̈ʒu�̍������v�Z���āA�z��ɕۑ�
        float HandPosDelta = 0.0f;
        for(int i = 0;i<m_nMaxPushFream;i++)
        {
            int PrevFream = (m_nPushFream - i - 1 + m_nMaxPushFream) % m_nMaxPushFream;
            HandPosDelta += HandPos - m_HandDepth[PrevFream];
        }
        m_HandDepth[m_nPushFream] = HandPos;
        m_nPushFream = (m_nPushFream + 1) % m_nMaxPushFream;

        //�ۑ����I��������̓��������o
        if (!m_IsSavePush && m_nPushFream == 0)
        {
            m_IsSavePush = true;

            //��̈ʒu�����l�𒴂��������m�F
            bool IsLeave = HandPosDelta / m_nMaxPushFream > m_fPushThreshold;
            bool IsPush = HandPosDelta / m_nMaxPushFream < -m_fPushThreshold;
            

            Debug.Log(m_fPushThreshold);
            if (IsPush)
            {
                Debug.Log("������");
                return m_nPushData = 1;
            }
            if (IsLeave)
            {
                Debug.Log("������");
                return m_nPushData = 0;
            }

        }
        else
        {
            m_IsSavePush = false;
            for(int i = 0; i< m_nMaxPushFream;i++)
            {
                m_HandDepth[i] = 0;
            }
        }

        
        return m_nPushData = -1;

        {
            m_HandDepth[m_nPushFream] = point[9].transform.position.z;

            m_nPushFream++;

            //�w��t���[�����L�^���ďI�[�܂ŕۑ������牟����������������Ԃ�
            if (m_nPushFream == m_nMaxPushFream - 1)
            {
                for (int i = 0; i < m_nMaxPushFream - 1; i++)
                {
                    if (m_HandDepth[i] < m_HandDepth[i + 1])
                    {
                        if (m_nPushData != -1 && m_nPushData == 0)
                        {
                            m_nPushFream = 0;
                            m_nPushData = -1;
                            return m_nPushData;
                        }
                        m_nPushData = 1;    //�����Ă���
                    }
                    if (m_HandDepth[i] > m_HandDepth[i + 1])
                    {
                        if (m_nPushData != -1 && m_nPushData == 1)
                        {
                            m_nPushFream = 0;
                            m_nPushData = -1;
                            return m_nPushData;
                        }
                        m_nPushData = 0;    //�����Ă���
                    }
                }

                //�t���[���̃��Z�b�g
                m_nPushFream = 0;

                return m_nPushData;
            }

            return -1;
        }
    }

    private void InitRecord()
    {
        m_isRecordFinish = false;
        m_isRecording = false;
        m_recordIndex = 0;
        //Debug.Log("���R�[�h������");
    }

    //�J�~�炵�֐�
    //����:��̍��E
    private void CreateRain(HandLandmarkListAnnotation.Hand hand)
    {
        //�肪���������Ă��邩
        HandDirection handdirection = GetHandDirection(GetHandVector(hand));
        //Debug.Log(handdirection);
        if (handdirection != HandDirection.DOWN) { return; }

        m_rNowTime += Time.deltaTime;//���ݎ��ԉ��Z
        Fingerindex data;
        data = FingerData(hand);
        //Debug.Log(FindKeyByValue(data));
        m_sKey = FindKeyByValue(data);
        //��̃|�[�Y��5?
        if (m_sKey == "Five" && m_rNowTime > m_rIntervalTime)
        {
            m_rNowTime = 0.0f;//���ݎ��ԏ�����
           // Debug.Log("�J�̐����J�n");
            //��̃��X�g���擾
            PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();


            //����
            float XMinpos = m_CloudObj.transform.position.x - m_CloudObj.transform.localScale.x * 10;
            float XMaxpos = m_CloudObj.transform.position.x + m_CloudObj.transform.localScale.x * 10;
            float RandomXpos = Random.Range(XMinpos, XMaxpos);
            m_objRain.transform.position = new Vector3(RandomXpos, m_CloudObj.transform.position.y + m_CloudObj.transform.localScale.y, 0.0f);
            Instantiate(m_objRain);
        }
    }

    //�w���オ���Ă��邩�̔���֐�
    //����:�E��or����
    //�߂�l:�w�̃f�[�^
    Fingerindex FingerData(HandLandmarkListAnnotation.Hand hand)
    {
        //�f�[�^���擾
        PointListAnnotation LandMarkData = m_HandLandmark[(int)hand].GetLandmarkList();

        Fingerindex fingerdata;

        //�e�w�̕t��������30����Ă����炠���Ă��锻��
        fingerdata.thumb = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[4].transform.position) > 20;
        fingerdata.index = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[8].transform.position) > 20;
        fingerdata.middle = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[12].transform.position) > 20;
        fingerdata.ring = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[16].transform.position) > 20;
        fingerdata.little = Vector3.Distance(LandMarkData[0].transform.position, LandMarkData[20].transform.position) > 20;

        return fingerdata;
    }


    //Dictionary�̒l����L�[���擾����֐�
    //����:Dictionary�̒l
    //�߂�l:Key
    string FindKeyByValue(Fingerindex targetvalue)
    {
        // Dictionary�����[�v���Ďw�肵���l�ɑΉ�����L�[������
        foreach (KeyValuePair<string,Fingerindex> pair in PoseData)
        {
            if(pair.Value.Equals(targetvalue))
            {
                return pair.Key;
            }
        }

        //������Ȃ�������null��Ԃ�
        return null;

    }



}
