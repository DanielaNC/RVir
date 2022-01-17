using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject[] papers;
    private Transform env;
   
    void Start()
    {
        env = GameObject.Find("Environment").transform;
        papers = GameObject.FindGameObjectsWithTag("Paper");
        //Debug.Log("Length: " + papers.Length);
        int j = 0;
        foreach(var paper in papers)
        {
            if (paper != null)
            {
                paper.AddComponent<Paper>();

                if (paper.GetComponent<Paper>().id == -1)
                {
                    Debug.Log("Material: " + paper.transform.Find("Quad").GetComponent<Renderer>().sharedMaterial.name.ToString());
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

                    if (paper.GetComponent<Paper>().id == -1)
                    {
                        paper.GetComponent<Paper>().id = j;
                        j++;
                    }
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        papers = GameObject.FindGameObjectsWithTag("Paper");
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
                        
                        if (string.Equals(paper.transform.Find("Quad").GetComponent<Renderer>().sharedMaterial.name.ToString(), papers[i].transform.Find("Quad").GetComponent<Renderer>().sharedMaterial.name.ToString()))
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
        }
    }

    public void UpdatePapers(GameObject parent, GameObject obj, bool flag = false)
    {
        if(obj == null) return;
        Vector3 local_pos = obj.transform.position;
        if(parent != null && parent.GetComponentInChildren<Paper>() == null) {
            return;
        }
        int id = parent.GetComponentInChildren<Paper>().id;
        foreach(var paper in papers)
        {
            if(paper.transform == parent.transform)
                continue;

            else if (paper.GetComponent<Paper>().id == id) {
                var rotation = paper.transform.rotation;
                var pos = paper.transform.position;
                paper.transform.rotation = parent.transform.rotation;
                paper.transform.position = parent.transform.position;

                if(!flag){
                    var o = Instantiate(obj, local_pos, paper.transform.rotation, paper.transform);
                    o.tag = "Highlight";
                }

                else{
                    // SOMETHING IS WRONG HELP
                    for(int i =0; i < paper.transform.childCount; i++){
                        var child = paper.transform.GetChild(i);
                        if(child.gameObject.tag == obj.tag && child.position == obj.transform.position){
                            Destroy(child.gameObject);
                        }
                    }
                }
                paper.transform.rotation = rotation;
                paper.transform.position = pos;
            }
        }
    }

}
