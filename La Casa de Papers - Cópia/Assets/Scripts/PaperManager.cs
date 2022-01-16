using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject[] papers;
   
    void Start()
    {
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
                        Debug.Log("lol: " + string.Equals(paper.transform.Find("Quad").GetComponent<Renderer>().sharedMaterial.name.ToString(), papers[i].transform.Find("Quad").GetComponent<Renderer>().sharedMaterial.name.ToString()));
                        //if (string.Equals(paper.transform.Find("Quad").GetComponent<Renderer>().sharedMaterial.name.ToString(), papers[i].transform.Find("Quad").GetComponent<Renderer>().sharedMaterial.name.ToString()))
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

    public void UpdatePapers(GameObject parent, int id, GameObject obj)
    {
        Vector3 local_pos = obj.transform.position;
        foreach(var paper in papers)
        {
            if (paper.transform == parent.transform) {
                //don't update
                continue;
            }

            else if (paper.GetComponent<Paper>().id == id) {
                var o = Instantiate(obj, local_pos, paper.transform.rotation, paper.transform);
                o.transform.position = local_pos;
            }
        }
    }

    // add sheets automatically pls
}
