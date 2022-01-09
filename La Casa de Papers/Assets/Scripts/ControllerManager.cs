using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using OVRTouchSample;

public class ControllerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform grab_spot;
    public OVRInput.Controller controller;

    public float lineWidth = 0.1f;
    public float lineMaxLength = 10f;

    public bool left_toggled = true;
    public bool right_toggled = true;

    private GameObject selectedObject = null;
    private Transform resetTransform = null;

    void Start()
    {
       resetTransform = GameObject.Find("Environment").transform;
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        
        float rightIndexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);

        if(rightIndexTrigger > 0.5f){
            SelectObject(transform.position, transform.forward, lineMaxLength);
        }

        float rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        
        if(selectedObject != null && rightHandTrigger > 0.2f){
            HoldObject();
        }

        if(selectedObject != null && rightHandTrigger <= 0.2f){
            ReleaseObject();
        }
    }

    private void SelectObject(Vector3 targetPos, Vector3 direction, float length){
        Debug.Log("Selecting");
        RaycastHit hit;
        Ray ray = new Ray(targetPos, direction);

        Vector3 endPosition = targetPos + (length * direction);

        if(selectedObject != null)
            selectedObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);

        if(Physics.Raycast(ray, out hit)){
            Debug.Log("hit! " + hit.collider.gameObject.name);
            if(hit.collider.gameObject.name != "Plane"){
                hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                selectedObject = hit.collider.gameObject;
                resetTransform = selectedObject.transform;           
            }
        }
        else
            selectedObject = null;
    }

    private void HoldObject(){
        Debug.Log("hold!");
        selectedObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        selectedObject.transform.parent = grab_spot;
        selectedObject.transform.position = grab_spot.position;
    }

    private void ReleaseObject(){
        Debug.Log("release!");
        selectedObject.transform.parent = null;
        selectedObject = null;
        resetTransform = null;
    }
}
