//-----------------------------------------------
//�S���ҁF�����S
//����(�S�[��)�N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CS_IsGoal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //�ǋL�F����2024.04.03
            //�Q�[���I�[�o�[�t���O��false�ɐݒ�
            CS_ResultController.GameOverFlag = false;
            SceneManager.LoadScene("Result");
        }
    }

}
