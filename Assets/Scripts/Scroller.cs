using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    private float offset;
    private Material backgroundMaterial;

    void Start()
    {
        backgroundMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offset += (scrollSpeed * Time.deltaTime) / 10f;
        backgroundMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
