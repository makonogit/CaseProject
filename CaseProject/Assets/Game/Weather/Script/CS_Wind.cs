//------------------------------
//�S����:�����S
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

    private bool m_IsWindEnd = false;   //���̒[���ǂ���
    
    [SerializeField, Header("Animator")]
    private Animator m_ThisAnim;

   // [SerializeField, Header("Playerscript")]
    private CS_Player m_player;                            // �v���C���[��script �ǉ��F��

    //���̌���
    public enum E_WINDDIRECTION
    {
        NONE,   //�Ȃ�
        LEFT,   //��
        RIGHT,  //�E
        UP      //��
    }

    [SerializeField,Header("���̌���")]
    //���̌����ϐ�
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

    //����getter,setter
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

    // CS_Player��ݒ肷��֐�
    // �������FCS_Player
    // �߂�l�F�Ȃ�
    public void SetCS_Player(CS_Player player) 
    {
        m_player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_bCreated =false;

        //�I�[��������A�j���[�V�������Đ�
        if (m_IsWindEnd) { this.GetComponent<Animator>().SetBool("End", true); }

        //    if (!m_player) { Debug.LogWarning("Player��script���ݒ肳��Ă��܂���"); }
        //    //�v���C���[�̈ړ��֐��𒼐ڌĂяo��
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
    
    // ���̕���j�����邩
    // �������F�Ȃ�
    // �߂�l�F�j�� True
    private bool IsDestroyThisObject() 
    {
        bool isTimeOver = m_nowTime > m_fDeleteTime;
        return isTimeOver && m_bDelete;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�ɏՓ˂����畗�̉e����^���� �ǉ��F��
        if (collision.transform.tag == "Player") InfluencePlayer(collision);

        // �����𖼑O�Ŕ��f
        if (collision.gameObject.name != this.name) return;

        CS_Wind other = collision.gameObject.GetComponent<CS_Wind>();

        // ������̕��𐶐����邩
        if (!IsCreateUpperWind(other)) return;

        // ������̕��̐���
        float addPower = this.m_fWindPower + other.m_fWindPower;
        CreateWindUpper(addPower,other);
    }

    // ������̕��𐶐����邩
    // �������F����������
    // �߂�l�F���Ȃ� True
    private bool IsCreateUpperWind(CS_Wind other) 
    {    
        // �������������f
        if (!IsFacingDirection(other)) { return false; }

        // ���̗͂��������炢�����f
        if (!IsTolerance(other)) { return false; }

        // �����ς݂�
        if (m_bCreated || other.m_bCreated) { return false; }
        
        return true;
    }

    // ���̕��������������Ă��邩
    // �������F����������
    // �߂�l�F�������Ă���Ȃ� True
    private bool IsFacingDirection(CS_Wind other) 
    {
        bool isSameDirection = this.m_eWindDirection == other.m_eWindDirection;
        if (isSameDirection) return false;
        // ���E�����݂̂�
        if(!this.IsHorizontal())return false;
        if(!other.IsHorizontal())return false;
        return true;
    }
    // ���E������
    // �����F�Ȃ�
    // �߂�l�F���E�Ȃ�True
    private bool IsHorizontal() 
    {
        if (this.m_eWindDirection == E_WINDDIRECTION.NONE) return false;
        if (this.m_eWindDirection == E_WINDDIRECTION.UP) return false;
        return true;
    }
    
    // ���̗͂��������炢�����f
    // �������F��������
    // �߂�l�F�������炢�Ȃ�True
    private bool IsTolerance(CS_Wind other) 
    {
        float addPower = this.m_fWindPower + other.m_fWindPower;
        const float tolerance = 100.0f;// ���e�͈�
        bool isTolerance = addPower < tolerance && addPower > -tolerance;
        return isTolerance;
    }

    // �v���C���[�ɉe����^����֐�
    // �������F�R���W����
    // �߂�l�F�Ȃ�
    private void InfluencePlayer(Collider2D collision) 
    {
        //collision.transform.GetComponent<CS_Player>().WindMove(m_eWindDirection, m_fWindPower);
        //Destroy(this.gameObject);
        m_player.WindMove(this.m_eWindDirection, this.m_fWindPower);
        // ������̂𑁂�����
        const float lastTime = 0.25f;
        m_nowTime = m_fDeleteTime - lastTime;
    }

    // ������̕��𐶐�����
    // �������F���̗�
    // �������F��������
    // �߂�l�F�Ȃ�
    private void CreateWindUpper(float windPower,CS_Wind other) 
    {
        // �ʒu�Ǝp��
        Vector3 pos = this.transform.position;
        pos += other.transform.position;
        pos *= 0.5f;
        Vector3 offset = new Vector3(0,-1,0);
        Quaternion rotation = Quaternion.Euler(0,0,0);

        // �I�u�W�F�N�g����
        GameObject createdWind = GameObject.Instantiate(m_objWind, pos + offset, rotation);

        // ���̋����{��
        const float magnification = 2.0f;
        // ���̏�Ԑݒ�
        CS_Wind cswind = createdWind.GetComponent<CS_Wind>();
        cswind.WindDirection = E_WINDDIRECTION.UP;              //������ɐݒ�@�ǉ��F��
        cswind.m_fWindPower = Mathf.Abs(windPower * magnification);
        
        const float UpperDeleteTime = 2.0f;
        cswind.m_fDeleteTime = UpperDeleteTime;
        cswind.SetCS_Player(m_player);

        // �R���|�[�l���g����A�N�e�B�u�ɂȂ�̂�True�ɂ��Ă��܂�
        cswind.enabled = true;
        createdWind.GetComponent<BoxCollider2D>().enabled = true;
        
        // ���𐶐�����
        other.m_bCreated = true;
        m_bCreated = true;
    }

}
