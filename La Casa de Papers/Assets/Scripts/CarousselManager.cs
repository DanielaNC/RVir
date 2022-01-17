using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarousselManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion defaultRotation;

    public Transform[] slots = new Transform[6];

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
            if(child.gameObject.tag == "Placeholder" && angle < 0)
                child.gameObject.GetComponent<Collider>().enabled = false;
            if(child.gameObject.tag == "Placeholder" && angle < 0)
                child.gameObject.GetComponent<Collider>().enabled = true;
            child.RotateAround(child.transform.position, child.transform.right, angle * Time.deltaTime);
        }
    }

    public GameObject CloseDocument(){
        //TODO
        return null;
    }

    public void OpenDocument(GameObject document){
        //TODO
    }
}
