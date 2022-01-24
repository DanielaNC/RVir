using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Document : MonoBehaviour
{
    public GameObject[] documentPages;
    public string title = "";
    public string authors = "";
    public Material hideCover;
    public Material backCover;
    public Material cover;
    public int id = 0;
    public Transform carrossel;

    public Transform trans;

    public void Open(){
        this.GetComponent<Renderer>().sharedMaterial = hideCover;
        this.transform.Find("Quad").GetComponent<Renderer>().sharedMaterial = hideCover;
        this.transform.GetComponent<Collider>().enabled = false;
    }

    public void Close(){
        this.transform.position = trans.position;
        this.transform.rotation = trans.rotation;
        this.GetComponent<Renderer>().sharedMaterial = backCover;
        this.transform.Find("Quad").GetComponent<Renderer>().sharedMaterial = cover;
        this.transform.GetComponent<Collider>().enabled = true;

        foreach(var page in documentPages){
            page.transform.position = this.transform.position;
            page.transform.rotation = this.transform.rotation;
            page.transform.parent = this.transform;
            page.SetActive(false);
        }
    }

    public void UpdatePages(GameObject[] pages){
        for(int i = 0; i < pages.Length; i++){
            var obj = Instantiate(pages[i], documentPages[i].transform.position, documentPages[i].transform.rotation, this.transform);
            obj.SetActive(false);
            documentPages[i] = obj;
        }

        foreach(var page in pages)
            Destroy(page);
    }
}
