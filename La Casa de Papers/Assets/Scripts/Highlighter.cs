using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab = null;
    public float defaultScale = 0.01f;

    public bool isOverlapping(GameObject parent, Vector3 pos){

        for(int j = 0; j< parent.transform.childCount; j++){
            var child = parent.transform.GetChild(j);
            if(child.gameObject.tag == "Highlight"){
                float x_min_1 = parent.transform.InverseTransformPoint(pos).x - defaultScale/2;
                float x_max_1 = parent.transform.InverseTransformPoint(pos).x + defaultScale/2;
                float x_min_2 = parent.transform.InverseTransformPoint(child.position).x - defaultScale/2;
                float x_max_2 = parent.transform.InverseTransformPoint(child.position).x + defaultScale/2;
                float y_min_1 = parent.transform.InverseTransformPoint(pos).y - defaultScale/2;
                float y_max_1 = parent.transform.InverseTransformPoint(pos).y + defaultScale/2;
                float y_min_2 = parent.transform.InverseTransformPoint(child.position).y - defaultScale/2;
                float y_max_2 = parent.transform.InverseTransformPoint(child.position).y + defaultScale/2;

                if((x_min_1 < x_min_2 && x_min_2 < x_max_1) || (x_min_2 < x_min_1 && x_min_1 < x_max_2)){
                    if((y_min_1 < y_min_2 && y_min_2 < y_max_1) || (y_min_2 < y_min_1 && y_min_1 < y_max_2))
                        return true;
                }
            }
        }

        return false;
    }

    public GameObject Highlight(Vector3 pos, GameObject parent){
        if(!isOverlapping(parent, pos)){
        var o = Instantiate(prefab, pos, parent.transform.rotation, parent.transform);
        o.tag = "Highlight";
        return o;
        }

        return null;
    }
}