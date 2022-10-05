using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetMaterialTex : MonoBehaviour
{
    public Texture2D emssionTex;
    private void OnValidate()
    {
        if (emssionTex != null) return;
        Material mat = GetComponent<SpriteRenderer>().sharedMaterial;
        mat.SetTexture("_EmissionMap", emssionTex);
    }
}
