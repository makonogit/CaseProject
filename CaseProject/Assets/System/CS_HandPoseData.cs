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
    private float m_fSwingThreshold = 0.1f; //�w�̉��U������o����ׂ̓��l
    [SerializeField,Header("���U�肵�Ă���̌��o�f�B���C(sc)")]
    private float m_fSwingDelay = 1.0f;       
    [SerializeField, Header("���I�u�W�F�N�g")]
    private GameObject m_objWind;


    // Start is called before the first frame update
    void Start()
    {
     
        
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
            CreateWind(HandLandmarkListAnnotation.Hand.Left);
        }

        if(m_HandLandmark[1] && m_HandLandmark[1].isActive)
        {
            CreateWind(HandLandmarkListAnnotation.Hand.Right);
        }


        //=====�f�o�b�O�@�|�[�Y�F���m�F==========
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fingerindex data;
            data = FingerData(HandLandmarkListAnnotation.Hand.Left);
            Debug.Log(FindKeyByValue(data));
            
        }
    }


    //���N�����֐�
    //����:��̍��E
    private void CreateWind(HandLandmarkListAnnotation.Hand hand) 
    {

        //Debug.Log(hand);
        
        //����l�����w�̐�̈ړ��ʂ��v�Z
        PointListAnnotation point = m_HandLandmark[(int)hand].GetLandmarkList();

        Vector3 currentpos = point[8].transform.position;

        //�O�̃t���[������̈ړ��ʂ��v�Z
        Vector3 movement = currentpos - previouspos;

        //�������̑��x���v�Z
        float HorizontalSpeed = Mathf.Abs(Vector3.Dot(movement, point[8].transform.right)) / Time.deltaTime;

        m_fSwingTime[(int)hand] += Time.deltaTime; //���U�肵�Ă���̎��Ԃ��v�Z

        if (HorizontalSpeed > m_fSwingThreshold && m_fSwingTime[(int)hand] > m_fSwingDelay)
        {
            //Debug.Log(HorizontalSpeed);

            m_fSwingTime[(int)hand] = 0.0f;
            GameObject windobj = m_objWind;
            if (hand == HandLandmarkListAnnotation.Hand.Left)
            {
               // windobj.transform.position = new Vector3(windobj.transform.position.x, point[0].transform.position.y * 0.1f, 0.0f);    //���W��ݒ�
                windobj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);    //���W��ݒ�
            }
            else
            {
                Debug.Log("�E��");
                windobj.transform.position = new Vector3(windobj.transform.position.x * -1, point[0].transform.position.y * 0.1f, 0.0f);    //���W��ݒ�
            }

            CS_Wind cs_wind = windobj.GetComponent<CS_Wind>();  //���̃X�N���v�g�擾

            //���̎w�̏�Ԃ��擾
            Fingerindex data;
            data = FingerData(hand);
            m_sKey = FindKeyByValue(data);

            float angle = Mathf.Atan2(movement.y, movement.x);
            windobj.transform.eulerAngles = new Vector3(0.0f,0.0f,angle*Mathf.Rad2Deg);
            Debug.Log("���̊p�x" + angle);

            //Debug.Log(cs_wind.Movement);
            Debug.Log(m_sKey);

            //���̐��� �w�̏�Ԃɂ���ĕ��̋�����ύX
            switch (m_sKey)
            {
                case "One":
                    windobj.GetComponent<SpriteRenderer>().color = Color.blue;
                    cs_wind.WindPower = 0.2f;
                    Instantiate(windobj);
                    break;
                case "Two":
                    windobj.GetComponent<SpriteRenderer>().color = Color.green;
                    cs_wind.WindPower = 0.4f;
                    Instantiate(windobj);
                    break;
                case "Three":
                    windobj.GetComponent<SpriteRenderer>().color = Color.yellow;
                    cs_wind.WindPower = 0.6f;
                    Instantiate(windobj);
                    break;
                case "For":
                    windobj.GetComponent<SpriteRenderer>().color = Color.red;
                    cs_wind.WindPower = 0.8f;
                    Instantiate(windobj);
                    break;
                default:
                    //Debug.Log("�����N�����Ȃ�");
                    break;
            }
        }

        //���݂̈ʒu��ۑ�
        previouspos = currentpos;
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
