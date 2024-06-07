//-----------------------------------------------
//�S���ҁF�����S
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CS_StageData : MonoBehaviour
{
    [Header("�X�e�[�W�f�[�^")]
    public List<World> m_Worlds;

    private int m_nMaxWorld = 0;    //���[���h�ő吔
    private int m_nMaxStage = 0;    //�X�e�[�W�ő吔

    private void Start()
    {
        //���[���h�ő吔�̕ۑ�
        m_nMaxWorld = m_Worlds.Count;
        m_nMaxStage = m_Worlds[0].Stagedata.Count;
    }

    //�X�e�[�W���Getter,Setter
    public int WORLDMAX
    {
        get
        {
            return m_nMaxWorld;
        }
    }

    public int STAGEMAX
    {
        get
        {
            return m_nMaxStage;
        }
        set
        {
            m_nMaxStage = value;
        }
    }
}

//-----------------------------------------------
//�X�e�[�W�f�[�^�N���X
//-----------------------------------------------
[System.Serializable]
public class StageData
{
    public Sprite m_sSelectStageSprite; //�Z���N�g��ʂ̃X�e�[�W�X�v���C�g
    public GameObject m_gStagePrefab;   //�X�e�[�W�̃v���n�u
    public StageData(Sprite selectsprite,GameObject stagepbj)
    {
        m_gStagePrefab = stagepbj;
        m_sSelectStageSprite = selectsprite;
    }
}

//-----------------------------------------------
//���[���h�f�[�^�N���X
//-----------------------------------------------
[System.Serializable]
public class World
{
    public List<StageData> Stagedata;

    public World(List<StageData> stagedata)
    {
        Stagedata = stagedata;
    }
}


//-----------------------------------------------
//�X�e�[�W���Q�Ɨp�N���X(�V�[���Ԃ̕ϐ������)
//-----------------------------------------------
public static class StageInfo
{
    [Header("���[���h�ԍ�")]
    public static int m_nWorldNum = 0;
    [Header("�X�e�[�W�ԍ�")]
    public static int m_nStageNum = 0;

    //-------------------------------------
    // �X�e�[�W�f�[�^�Z�b�g�֐�
    // �����F���[���h�ԍ�
    // �����F�X�e�[�W�ԍ�
    //-------------------------------------
    public static void SetStageData(int _worldnum,int _stagenum)
    {
        m_nWorldNum = _worldnum;
        m_nStageNum = _stagenum;
    }


    //---------------------------------------
    // ���[���h�ԍ�Getter
    //---------------------------------------
    public static int World
    {
        get
        {
            return m_nWorldNum;
        }
    }


    //---------------------------------------
    // �X�e�[�W�ԍ�Getter
    //---------------------------------------
    public static int Stage
    {
        get
        {
            return m_nStageNum;
        }
       
    }

}

//-------------------------------------------
// �X�e�[�W�I�u�W�F�N�g���Ǘ�
// �Ⴄ���@�ŊǗ�������������,�܂��̂�����
//-------------------------------------------
public static class ObjectData
{

    [Header("���̎qTransform")]
    public static Transform m_tStarChildTrans;

    [Header("PlayerTransform")]
    public static Transform m_tPlayerTrans;

    [Header("�J��������X�N���v�g")]
    public static CS_CameraControl m_csCamCtrl;
}