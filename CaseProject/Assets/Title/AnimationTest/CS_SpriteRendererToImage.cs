using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_SpriteRendererToImage : MonoBehaviour
{
    private SpriteRenderer m_sRenderer;
    private Image m_image;
    // Start is called before the first frame update
    void Start()
    {
        m_sRenderer = GetComponent<SpriteRenderer>();
        m_image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        m_image.sprite = m_sRenderer.sprite;
    }
}
