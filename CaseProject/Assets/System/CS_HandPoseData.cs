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


    //===== �������p�ϐ� ========
    
    private Vector3 previouspos;
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
    private int m_recordNum = 10;           //�ۊǐ�
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

    // Start is called before the first frame update
    void Start()
    {
        m_recordPositions = new Vector3[m_recordNum];   
    }

    // Update is called once per frame
    void Update()
    {
        //��̏����擾
        if (!m_HandLandmark[0] || !m_HandLandmark[1])
        {
            if (transform.childCount > 0)
            {
                HandLandmarkListAnnotation hand = transform.Find("HandLandmarkList Annotation(Clone)").GetComponent<HandLandmarkListAnnotation>();

                if (hand)
                {
                    m_HandLandmark[(int)hand.GetHandedness()] = hand;
                }
            }
        }
        
        //--------------���̏���(�ȈՎ���)-----------------
        if(m_HandLandmark[0] && m_HandLandmark[0].isActive)
        {
            RecordFingerPosition(HandLandmarkListAnnotation.Hand.Left);
            CreateWind(HandLandmarkListAnnotation.Hand.Left);
        }

        if(m_HandLandmark[1] && m_HandLandmark[1].isActive)
        {
            RecordFingerPosition(HandLandmarkListAnnotation.Hand.Right);
            CreateWind(HandLandmarkListAnnotation.Hand.Right);
        }

        //----------------�J�̐�������-----------------------
        if (m_HandLandmark[0] && m_HandLandmark[0].isActive)
        {
           //CreateRain(HandLandmarkListAnnotation.Hand.Left);
        }

        if (m_HandLandmark[1] && m_HandLandmark[1].isActive)
        {
           //CreateRain(HandLandmarkListAnnotation.Hand.Right);
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
        Vector3 currentpos = point[8].transform.position;

        //3�̓_���擾���A���������ԃx�N�g�����v�Z
        Vector3 posA = point[5].transform.position;
        Vector3 posB = point[17].transform.position;
        Vector3 posC = point[0].transform.position;
        Vector3 vecAB = posB - posA;
        Vector3 vecAC = posC - posA;

        //2�̃x�N�g���̊O�ς��擾���A�ʂ̖@���x�N�g�����v�Z
        Vector3 normal = Vector3.Cross(vecAB, vecAC).normalized;

        //�ʂ̖@���x�N�g�����g���Ė����x�N�g�����v�Z
        Vector3 vecDir = Vector3.Cross(normal, vecAB).normalized;
        return vecDir;
    }

    //�|�W�V�����ۊ�
    private void RecordFingerPosition(HandLandmarkListAnnotation.Hand hand)
    {

        if (m_isRecordFinish) { return; }
        //����l�����w�̐�̈ړ��ʂ��v�Z
        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();

        Vector3 currentpos = point[8].transform.position;

        if(hand == HandLandmarkListAnnotation.Hand.Left)
        {
            if(GetHandVector(hand).x < 0.2f)
            {
                return;
            }
        }
        else
        {
            if (GetHandVector(hand).x > 0.2f)
            {
                return;
            }
        }

        float movement = Vector3.Distance(previouspos, currentpos);

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
            m_recordPositions[m_recordIndex] = currentpos;//���݂̎w�̈ʒu��ۑ�
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

        m_fWaitFreamTime += Time.deltaTime; //�ҋ@�t���[�����Z
        if (m_fWaitFreamTime > m_fWaitFream)
        {
            //���݂̈ʒu��ۑ�
            previouspos = currentpos;
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
        if (!m_isRecordFinish) { return; }

        GameObject windobj = m_objWind;
        if (hand == HandLandmarkListAnnotation.Hand.Left)
        {
            // windobj.transform.position = new Vector3(windobj.transform.position.x, point[0].transform.position.y * 0.1f, 0.0f);    //���W��ݒ�

            windobj.transform.position = new Vector3(m_recordPositions[0].x, m_recordPositions[0].y, 0.0f);    //���W��ۊǂ����|�W�V�����z��̍ŏ��ɐݒ�
        }
        else
        {
            //Debug.Log("�E��");
            //windobj.transform.position = new Vector3(windobj.transform.position.x * -1, point[0].transform.position.y * 0.1f, 0.0f);    //���W��ݒ�
        }

        CS_Wind cs_wind = windobj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾

        //--------------���̊p�x�ݒ�--------------------
        windobj.transform.eulerAngles = new Vector3(0.0f, 0.0f, m_windStatus.angle* Mathf.Rad2Deg);
        //Debug.Log("���̊p�x" + m_windStatus.angle);
        //���̃X�s�[�h
        cs_wind.Movement = m_windStatus.speed * 0.01f;

        //Debug.Log(cs_wind.Movement);
        //Debug.Log(m_sKey);

        windobj.GetComponent<SpriteRenderer>().color = Color.green;
        cs_wind.WindPower = 0.2f* m_windStatus.speed;
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
        m_rNowTime += Time.deltaTime;//���ݎ��ԉ��Z
        Fingerindex data;
        data = FingerData(hand);
        Debug.Log(FindKeyByValue(data));
        m_sKey = FindKeyByValue(data);
        //��̃|�[�Y��5?
        if (m_sKey == "Five" && m_rNowTime > m_rIntervalTime)
        {
            m_rNowTime = 0.0f;//���ݎ��ԏ�����
            Debug.Log("�J�̐����J�n");
            //��̃��X�g���擾
            PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();


            //����
            for (uint i = 0; i<m_rCreateFingerNums.Length; i++)
            {
                //�w�ԍ��ɍ������ʒu�ɐ���
                // pos = 
                //Debug.Log("�J�̐����ʒu"+pos);
                m_objRain.transform.position = point[m_rCreateFingerNums[i]].transform.position;//new Vector3(point[(int)m_rCreateFingerNums[i]].transform.position.x, point[(int)m_rCreateFingerNums[i]].transform.position.y, 0.0f); 
                Instantiate(m_objRain);

            }
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
