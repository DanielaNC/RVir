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

    private GameObject selectedObject = null;
    public Transform resetTransform;

    private bool grabbingObject = false;
    private GameObject grabbedObject = null;
    private GameObject corkboard = null;
    private Vector3 hitPoint = new Vector3();
    private Collider collider = null;

    void Start()
    {
       resetTransform = GameObject.Find("Environment").transform;
       corkboard = GameObject.Find("Corkboard");
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        
        SelectObject(transform.position, transform.forward, lineMaxLength);

        float rightIndexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);

        if(selectedObject != null && selectedObject.name == "default" && grabbingObject && grabbedObject.layer == 3){
            PinPaper(); //wip
        }

        float rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        
        if(selectedObject != null && !grabbingObject && rightHandTrigger > 0.5f){
            HoldObject();
        }

        rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);

        if(grabbingObject && rightHandTrigger <= 0.5f){
            ReleaseObject();
        }

    }

    private void SelectObject(Vector3 targetPos, Vector3 direction, float length){
        //Debug.Log("Selecting");
        RaycastHit hit;
        Ray ray = new Ray(targetPos, direction);

        Vector3 endPosition = targetPos + (length * direction);

        if(Physics.Raycast(ray, out hit)){
            Debug.Log("hit! " + hit.collider.gameObject.name);
            if(hit.collider.gameObject.name != "Plane"){
                //hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                selectedObject = hit.collider.gameObject; 
                hitPoint = hit.point;        
            }
        }
        else
            selectedObject = null;
    }

    private void HoldObject(){
        //Debug.Log("hold!");
        if(selectedObject.transform.parent == corkboard.transform) return;
        selectedObject.transform.parent = grab_spot;
        selectedObject.transform.position = grab_spot.position;
        collider = selectedObject.GetComponent<Collider>();
        collider.enabled = false;
        grabbedObject = selectedObject;
        grabbingObject = true;
    }

    private void ReleaseObject(bool flag = false){
        //Debug.Log("release!");
        collider.enabled = true;
        grabbedObject.transform.parent = resetTransform;
        if(flag)
            grabbedObject.transform.parent = corkboard.transform;
        //selectedObject = null;
        grabbingObject = false;
    }

    private void PinPaper(){
        Debug.Log("pinning");
        Vector3 pos = new Vector3(hitPoint.x, hitPoint.y, -1.43f);
        grabbedObject.transform.position = pos;
        ReleaseObject(true);
    }
}
