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

        //http://www.theappguruz.com/blog/add-collider-to-line-renderer-unity
        /*if(startPos != null && endPos != null){
            float lineLength = Vector3.Distance (startPos.transform.position, endPos.transform.position);
            col.size = new Vector3 (lineLength, 0.1f, 1f); 
            Vector3 midPoint = (startPos.transform.position + endPos.transform.position)/2;
            col.transform.position = midPoint;
            float angle = (Mathf.Abs (startPos.transform.position.y - endPos.transform.position.y) / Mathf.Abs (startPos.transform.position.x - endPos.transform.position.x));
            if((startPos.transform.position.y<endPos.transform.position.y && startPos.transform.position.x>endPos.transform.position.x) || (endPos.transform.position.y<startPos.transform.position.y && endPos.transform.position.x>startPos.transform.position.x))
            {
                angle*=-1;
            }
            angle = Mathf.Rad2Deg * Mathf.Atan (angle);
            col.transform.Rotate (0, 0, angle);
        }*/
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