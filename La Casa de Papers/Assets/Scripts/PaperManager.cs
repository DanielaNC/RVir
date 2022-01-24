using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> papers;
    private Transform env;
    public GameObject linePrefab;
   
    void Awake()
    {
        env = GameObject.Find("Environment").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        /*papers = GameObject.FindObjectsOfType(typeof(Paper), true) as GameObject[];
        
        foreach (var paper in papers)
        {
            if (paper != null)
            {
                int j = 0;
                if(paper.GetComponent<Paper>() == null)
                    paper.AddComponent<Paper>();

                if (paper.GetComponent<Paper>().id == -1)
                {

                    for (int i = 0; i < papers.Length; i++)
                    {
                        
                        if (paper.transform.Find("Quad").GetComponent<Renderer>().sharedMaterial == papers[i].transform.Find("Quad").GetComponent<Renderer>().sharedMaterial)
                        {
                            if (papers[i].GetComponent<Paper>() != null && papers[i].GetComponent<Paper>().id != -1)
                            {
                                paper.GetComponent<Paper>().id = papers[i].GetComponent<Paper>().id;
                                break;
                            }
                        }
                    }

                    paper.GetComponent<Paper>().id = j;
                    j++;
                }
            }
        }*/
    }

    public void UpdatePaperConnection(GameObject line){
        var start = line.GetComponent<LineManager>().startPos;
        var end = line.GetComponent<LineManager>().endPos;

        int startId = start.transform.parent.GetComponent<Paper>().id;
        int endId = end.transform.parent.GetComponent<Paper>().id;
        GameObject o = null;

        if(startId == endId){
            foreach(var paper in papers)
            {
                if(paper.transform == start.transform.parent.gameObject.transform)
                    continue;

                else if (paper.GetComponent<Paper>().id == startId){
                    var pos = paper.transform.position;
                    var rot = paper.transform.rotation;
                    var parent = paper.transform.parent;

                    paper.transform.position = line.transform.parent.gameObject.transform.position;

                    var newStart = new GameObject();
                    newStart.transform.position = start.transform.position;
                    newStart.transform.parent = paper.transform;
                    var newEnd = new GameObject();
                    newEnd.transform.position = end.transform.position;
                    newEnd.transform.parent = paper.transform;
                    o = Instantiate(linePrefab, line.transform.position, paper.transform.rotation, paper.transform);
                    o.GetComponent<LineRenderer>().material = line.GetComponent<LineRenderer>().material;
                    o.GetComponent<LineRenderer>().SetPosition(0, newStart.transform.position);
                    o.GetComponent<LineRenderer>().SetPosition(1, newEnd.transform.position);
                    o.GetComponent<LineManager>().SetStartingPoint(newStart);
                    o.GetComponent<LineManager>().SetEndPoint(newEnd);
                    paper.transform.position = pos;
                    paper.transform.rotation = rot;
                    paper.transform.parent = parent;
                }
            }
        }

        else{
            foreach(var paper in papers)
            {
                if (paper.GetComponent<Paper>().id == startId){
                    var newStart = new GameObject();
                    newStart.transform.position = start.transform.position;
                    newStart.transform.parent = paper.transform;
                    foreach(var paper2 in papers){
                        if(paper.GetComponent<Paper>().id == endId){
                            var newEnd = new GameObject();
                            newEnd.transform.position = end.transform.position;
                            newEnd.transform.parent = paper2.transform;
                            o = Instantiate(linePrefab, line.transform.position, paper.transform.rotation, paper.transform);
                            o.GetComponent<LineRenderer>().material = line.GetComponent<LineRenderer>().material;
                            o.GetComponent<LineManager>().SetEndPoint(newEnd);
                            o.GetComponent<LineRenderer>().SetPosition(1, newEnd.transform.position);
                            o.GetComponent<LineManager>().SetStartingPoint(newStart);
                            o.GetComponent<LineRenderer>().SetPosition(0, newStart.transform.position);
                        }
                    }
                }
            }
        }

    }

    public void UpdatePapers(GameObject parent, GameObject obj, bool flag = false)
    {
        if(obj == null) return;
        if(obj.tag == "Line") {
            UpdatePaperConnection(obj);
            return;
        }
        Vector3 local_pos = obj.transform.position;
        if(parent != null && parent.GetComponentInChildren<Paper>() == null) {
            return;
        }
        int id = parent.GetComponentInChildren<Paper>().id;
        foreach(var paper in papers)
        {
            if (paper.GetComponent<Paper>().id == id) {
                var paperActive = paper.active;
                paper.SetActive(true);
                var rotation = paper.transform.rotation;
                var pos = paper.transform.position;
                var parentPos = paper.transform.parent;

                paper.transform.rotation = parent.transform.rotation;
                paper.transform.position = parent.transform.position;
                //paper.transform.parent = parent.transform.parent;

                if(!flag){
                    if(obj.tag == "Highlight"){
                        var o = Instantiate(obj, local_pos, paper.transform.rotation, paper.transform);
                        o.tag = "Highlight";
                    }
                }

                else{
                    for(int i =0; i < paper.transform.childCount; i++){
                        var child = paper.transform.GetChild(i);
                        if(child.gameObject.tag == obj.tag && child.position == obj.transform.position){
                            if(obj.tag == "Highlight")
                                Destroy(child.gameObject);
                            else if(obj.tag == "Line")
                                child.gameObject.GetComponent<LineManager>().DeleteSelf();
                        }
                    }
                }

                paper.transform.rotation = rotation;
                paper.transform.position = pos;
                paper.transform.parent = parentPos;
                paper.SetActive(paperActive);
            }
        }
    }

    public void addPaper(GameObject paper){
        papers.Add(paper);
    }

    public void removePaper(GameObject paper){
        foreach(var item in papers){
            if(item.GetComponent<Paper>().id == paper.GetComponent<Paper>().id){
                if(item.transform == paper.transform){
                    papers.Remove(item);
                    return;
                }
            }
        }
    }

}
