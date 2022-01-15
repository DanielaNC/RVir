using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab = null;
    public float defaultScale = 1.00f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Highlight(Vector3 pos, GameObject parent){
        var o = Instantiate(prefab, pos, parent.transform.rotation, parent.transform);
        o.tag = "Highlight";
        return o;
    }
}
