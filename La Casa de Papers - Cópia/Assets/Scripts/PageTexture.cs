using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PageTexture
{
    public int id;
    public Material material;

    public PageTexture(int _id, Material _material)
    {
        id = _id;
        material = _material;
    }
}
