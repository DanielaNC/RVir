using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab = null;
    public float defaultScale = 0.005f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Highlight(Vector3 pos, GameObject parent){
        var o = Instantiate(prefab, pos, parent.transform.rotation, parent.transform);
    }
}
