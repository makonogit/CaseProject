//-----------------------------------------------
//�S���ҁF�����S
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------
//�X�e�[�W�I���N���X
//-----------------------------------------------
public class CS_StageSelect : MonoBehaviour
{

    //------------------------------------------------
    // 1-1,1-2�ƕ�����₷���悤��1�n�܂�ŊǗ�
    // �z�񎩑̂�0�n�܂�
    [SerializeField, Header("���݂̃��[���h�ԍ�")]
    private int m_nNowWorldNum = 1;

    [SerializeField, Header("���݂̃X�e�[�W�ԍ�")]
    private int m_nNowStageNum = 1;

    [SerializeField, Header("�X�e�[�W���")]
    private CS_StageData m_csStageData;

    [SerializeField, Header("�V�[���}�l�[�W���[")]
    private CS_SceneManager m_csSceneManager;

    //-------------------------------------
    //�@�X�e�[�W��i�߂�
    //�@�����F���X�e�[�W���X�V���邩(-1��������1�߂�)
    //-------------------------------------
    public void StageUpdate(int _stage)
    {
        //���[���h���ő�(5-5)�Œl�����Ȃ�X�V���Ȃ�
        if(m_nNowWorldNum == m_csStageData.WORLDMAX && m_nNowStageNum == m_csStageData.STAGEMAX && _stage > 0) { return; }

        //�X�e�[�W���ŏ�(1-1)�Œl�����Ȃ�X�V���Ȃ�
        if(m_nNowWorldNum == 1 && m_nNowStageNum == 1 && _stage < 0) { return; } 

        m_nNowStageNum += _stage;

        //�o�^����Ă���X�e�[�W�����傫���l�ɂȂ�����X�e�[�W�X�V�������[���h�X�V
        if(m_nNowStageNum > m_csStageData.STAGEMAX && m_nNowWorldNum < m_csStageData.WORLDMAX)
        {
            m_nNowWorldNum++;
            m_nNowStageNum = 1;

            //���[���h�ő吔�̍X�V
            m_csStageData.STAGEMAX = m_csStageData.m_Worlds[m_nNowWorldNum - 1].Stagedata.Count;
        }

        //�X�e�[�W�f�[�^��o�^
        StageInfo.SetStageData(m_nNowWorldNum - 1, m_nNowStageNum - 1);

        Debug.Log("World:" + (StageInfo.World + 1) + "Stage:" + (StageInfo.Stage + 1));
    }

    // Start is called before the first frame update
    void Start()
    {
        //m_csSceneManager.LoadScene(CS_SceneManager.SCENE.GAME);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
