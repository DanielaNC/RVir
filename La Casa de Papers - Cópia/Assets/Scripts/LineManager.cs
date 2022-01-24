using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startPos = null;
    public GameObject endPos = null;
    public BoxCollider col = null;

    void Start()
    {
        col = new GameObject("Collider").AddComponent<BoxCollider>();
        col.transform.parent = this.GetComponent<LineRenderer>().transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startPos != null){
            this.GetComponent<LineRenderer>().SetPosition(0, startPos.transform.position);
        }

        if(endPos != null){
            this.GetComponent<LineRenderer>().SetPosition(1, endPos.transform.position);
        }
        else{
            endPos = startPos;
        }
    }

    public void SetStartingPoint(GameObject pos){ 
        startPos = pos; 
    }

    public void SetEndPoint(GameObject pos){ endPos = pos; }

    public void DeleteSelf(){
        Destroy(startPos);
        Destroy(endPos);
        Destroy(gameObject);
    }
}