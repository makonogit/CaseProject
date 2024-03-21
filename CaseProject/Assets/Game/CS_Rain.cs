using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Rain : MonoBehaviour
{
    float m_nowTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_nowTime += Time.deltaTime;
        const float deleteTime = 3.0f;
        if(m_nowTime > deleteTime)
        {
            Destroy(this.gameObject);
        }
    }
}
