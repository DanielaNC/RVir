using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarousselManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion defaultRotation;

    public GameObject[] slots = new GameObject[7];

    public GameObject prefab = null;

    private GameObject open_document = null;

    private GameObject[] document_pages;

    void Start()
    {
        defaultRotation = transform.rotation;
    }

    public void RotateCarousselX(float angle){
        transform.RotateAround(transform.position, transform.up, angle * Time.deltaTime);
    }

    public void RotateCarousselY(float angle){
        for(int i = 0; i < transform.childCount; i++){
            var child = transform.GetChild(i);
            if(child.gameObject.tag == "Placeholder"){
                Vector3 rot = child.rotation.eulerAngles;
                while(rot.x > 360f)
                    rot.x -= 360f;
                if ( rot.x >= 70f && rot.x <= 359f)
                    child.gameObject.GetComponent<Collider>().enabled = false;

                else
                    child.gameObject.GetComponent<Collider>().enabled = true;
            }

            child.RotateAround(child.transform.position, child.transform.right, angle * Time.deltaTime);
        }
    }

    public void CloseDocument(){
        if(open_document != null){
            open_document.GetComponent<Document>().Close();
            //open_document.GetComponent<Document>().UpdatePages(document_pages);
            /*open_document = null;

            for(int i = 0; i < this.transform.childCount; i++){
                var child = this.transform.GetChild(i);
                if(child.gameObject.tag == "Paper"){
                    GameObject.Find("Manager").GetComponent<PaperManager>().removePaper(child.gameObject);
                    Destroy(child.gameObject);
                }
            }*/
        }
    }

    public void OpenDocument(GameObject obj){
        CloseDocument();
        List<GameObject> children = new List<GameObject>();
        var document = obj.GetComponent<Document>();
        document.Open();
        document_pages = new GameObject[document.documentPages.Length];
        //document.parent.transform = this.transform;

        for(int i = 0; i < document.documentPages.Length; i++){

            document.documentPages[i].SetActive(true);
            document.documentPages[i].transform.parent = this.transform;
            document.documentPages[i].transform.position = slots[i].transform.position;
            document.documentPages[i].transform.rotation = slots[i].transform.rotation;
           
        }

        open_document = obj;
    }
}