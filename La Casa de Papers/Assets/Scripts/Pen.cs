using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public GameObject prefab;
    public Transform tipPosition;
    public int clickCounter = 0;
    private GameObject line = null;
    private bool newLine = true;

    private List<GameObject> lines = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawLine(Vector3 pos, GameObject parent){
        //Debug.Log("Drawing line");
        if(parent == null) return;
        if(clickCounter > 1 && line != null){
            DrawLine(line.GetComponent<LineRenderer>().GetPosition(1), parent);
            DrawLine(pos, parent);
        }

        if(clickCounter > 0){
            UpdateLine(pos, parent);
            //clickCounter = 0;
        }
        if(clickCounter == 0 && parent == null || clickCounter == 0 && pos == null) return;
        var obj = new GameObject();
        var start = Instantiate(obj, pos, parent.transform.rotation, parent.transform);
        Destroy(obj);
        line = Instantiate(prefab, pos, parent.transform.rotation, parent.transform);
        line.GetComponent<LineRenderer>().SetPosition(0, pos);
        line.GetComponent<LineRenderer>().SetPosition(1, pos);
        line.GetComponent<LineManager>().SetStartingPoint(start);
        line.GetComponent<LineManager>().SetEndPoint(start);
        lines.Insert(0, line);

        clickCounter++;
    }

    public void UpdateLine(Vector3 pos, GameObject parent){
        if(parent == null){
            line.GetComponent<LineRenderer>().SetPosition(1, tipPosition.position);
            line.GetComponent<LineManager>().SetStartingPoint(tipPosition.gameObject);
            line.GetComponent<LineManager>().SetEndPoint(tipPosition.gameObject);
            clickCounter = 1;
        } else {
            var obj = new GameObject();
            var end = Instantiate(obj, pos, parent.transform.rotation, parent.transform);
            line.GetComponent<LineRenderer>().SetPosition(1, end.transform.position);
            clickCounter = 0;
            Destroy(obj);
            line.GetComponent<LineManager>().SetEndPoint(end);
        }
    }

    public void DeleteLastLine(){
        if(line!=null){
            Destroy(line);
            line = null;
            lines.RemoveAt(0);
        } else if (lines.Count > 0){
            Destroy(lines[0]);
            lines.RemoveAt(0);
        }

    } 
    
    //i don't remember what I wrote this for so yolo
    public void NewLine(){
        newLine = true;
        clickCounter = 0;
        DeleteLastLine();
    }
}