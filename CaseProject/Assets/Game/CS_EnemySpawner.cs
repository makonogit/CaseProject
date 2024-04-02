//-----------------------------------------------
//�S���ҁF�����S
//�G�����N���X
//-----------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_EnemySpawner : MonoBehaviour
{
    private float m_fTime = 0.0f;   //���Ԍv��

    [SerializeField, Header("�G�����Ԋu")]
    private float m_fCreateTime = 2.0f;

    [SerializeField, Header("�G�I�u�W�F�N�g")]
    private GameObject EnemyObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_fTime += Time.deltaTime;  //���Ԍv��
        int random = Random.Range(1, 100);

        //�������Ԍo�߂����烉���_���Ȉʒu���琶��
        if(m_fTime > m_fCreateTime)
        {
            GameObject enemy = EnemyObj;
            //�Ƃ肠�����E�ƍ�����
            if(random < 50)
            {
                //��
                enemy.transform.position = new Vector3(-160, 0, 0);
                Instantiate(EnemyObj);
            }
            else
            {
                //�E
                enemy.transform.position = new Vector3(160, 0, 0);
                Instantiate(EnemyObj);
            }

            m_fTime = 0.0f;
        }

    }
}
