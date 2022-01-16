using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using OVRTouchSample;

public class ControllerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform grab_spot;
    public Transform carrossel;
    public OVRInput.Controller controller;
    private Color resetColor;

    private GameObject selectedObject = null;
    public Transform resetTransform;

    private float lineMaxLength = 10f;
    
    private bool grabbingObject = false;
    private GameObject grabbedObject = null;
    private GameObject highlight = null;
    public GameObject corkboard = null;
    private Vector3 hitPoint = new Vector3();
    private Collider collider = null;
    private bool isHighlighting = false;
    private bool isDragging = false;
    private Vector3 lastDragPoint = new Vector3();

    private Vector3 highlight_pos = new Vector3();

    void Start()
    {
       //resetTransform = GameObject.Find("Environment").transform;
       //corkboard = GameObject.Find("Corkboard_reset");
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        
        SelectObject(transform.position, transform.forward, lineMaxLength);

        float rightIndexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        //Debug.Log("index: " + rightIndexTrigger);


        /* if(isPinning && rightIndexTrigger > 0.5f && rightHandTrigger > 0.5f)
            return; */

        if (rightIndexTrigger < 0.5f || !grabbingObject){
            isHighlighting = false;
        }

        if(OVRInput.Get(OVRInput.Button.One, controller)){
            Drag(OVRInput.GetLocalControllerPosition(controller));
            //Debug.Log("Initial pos: " + OVRInput.GetLocalControllerPosition(controller));

        }

        if(rightIndexTrigger > 0.5f && grabbingObject && grabbedObject.GetComponent<Highlighter>() != null && highlight == null){
            //Debug.Log("selected: " + selectedObject);
            if(selectedObject != null && selectedObject.layer == 3){
                Highlight();
            }
        }

        if(rightIndexTrigger > 0.5f && grabbingObject && grabbedObject.GetComponent<Eraser>() != null){
            if(highlight!=null){
                Debug.Log("highlight: " + highlight.name);
                Erase();
            }
        }

        if(rightIndexTrigger > 0.5f && selectedObject != null && selectedObject.tag == "Corkboard" && grabbingObject && grabbedObject.layer == 3){
            PinToCorkboard(); //wip
        }

        if(rightIndexTrigger > 0.5f && selectedObject != null && selectedObject.tag == "Placeholder" && grabbingObject && grabbedObject.layer == 3){
            PinToPlaceholder(); //wip
        }

        float rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        
        if(selectedObject != null && !grabbingObject && rightHandTrigger > 0.5f && rightIndexTrigger < 0.5f){
            HoldObject();
        }
        //rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);

        if(grabbingObject && rightHandTrigger <= 0.5f){
            ReleaseObject(resetTransform, grabbedObject.transform.position);
        }

        if(!OVRInput.Get(OVRInput.Button.One, controller)){
            isDragging = false;
            //Debug.Log("Final pos: " + OVRInput.GetLocalControllerPosition(controller));

        }

    }

    private void SelectObject(Vector3 targetPos, Vector3 direction, float length){
        //Debug.Log("Selecting");
        RaycastHit hit;
        Ray ray = new Ray(targetPos, direction);

        Vector3 endPosition = targetPos + (length * direction);

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("hit! " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Highlight")
            {
                if(isHighlighting){
                    selectedObject = null;
                    //Debug.Log("COLLISION");
                }
                selectedObject = hit.collider.gameObject.transform.parent.gameObject;
                highlight = hit.collider.gameObject;
                hitPoint = hit.point;
                OVRInput.SetControllerVibration(0.5f, 0.5f, controller);
            }
            else if (hit.collider.gameObject.name != "Plane")
            {
                //hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                selectedObject = hit.collider.gameObject;
                hitPoint = hit.point;
                highlight = null;
                OVRInput.SetControllerVibration(0.5f, 0.5f, controller);
            }
        }
        else
        {
            selectedObject = null;
            OVRInput.SetControllerVibration(0.0f, 0.0f, controller);
        }
    }

    private void HoldObject(){
        //Debug.Log("hold!");
        if(selectedObject.tag == "Corkboard" || selectedObject.tag=="Placeholder") return;
        selectedObject.transform.parent = grab_spot;
        selectedObject.transform.position = grab_spot.position;
        collider = selectedObject.GetComponent<Collider>();
        collider.enabled = false;
        grabbedObject = selectedObject;
        grabbingObject = true;
    }

    private void ReleaseObject(Transform parent, Vector3 pos, bool pin = false){
        //Debug.Log("release!");
        collider.enabled = true;
        if(pos != null)
            grabbedObject.transform.position = pos;
        if(selectedObject != null && selectedObject.tag == "Corkboard")
            grabbedObject.transform.rotation = corkboard.transform.rotation;
        else if(pin && parent != resetTransform)
            grabbedObject.transform.rotation = parent.rotation;
        grabbedObject.transform.parent = parent;
        //selectedObject = null;
        grabbingObject = false;
    }

    private void PinToCorkboard(){
        //Debug.Log("pinning");
        ReleaseObject(corkboard.transform, hitPoint, true);
    }

    private void Highlight(){
        //Debug.Log("Highlighting");
        GameObject obj = null;
        if (!isHighlighting)
        {
            obj = grabbedObject.GetComponent<Highlighter>().Highlight(hitPoint, selectedObject);
            isHighlighting = true;
            highlight_pos = selectedObject.transform.InverseTransformPoint(hitPoint);
        } else
        {
            var hit_point = selectedObject.transform.InverseTransformPoint(hitPoint);
            var pos = selectedObject.transform.TransformPoint(new Vector3(hit_point.x, highlight_pos.y, highlight_pos.z));
            obj = grabbedObject.GetComponent<Highlighter>().Highlight(pos, selectedObject);
            //obj.transform.position = new Vector3(obj.transform.position.x, highlight_pos.y, highlight_pos.z);
        }

        
        GameObject.Find("Manager").GetComponent<PaperManager>().UpdatePapers(selectedObject, obj);
    }

    private void PinToPlaceholder(){
        //Debug.Log("pinning to main cluster");
        ReleaseObject(selectedObject.transform, selectedObject.transform.position, true);
    }

    private void Erase(){
        GameObject.Find("Manager").GetComponent<PaperManager>().UpdatePapers(selectedObject, highlight, true);
        grabbedObject.GetComponent<Eraser>().Erase(highlight);
        highlight = null;
    }

    private void Drag(Vector3 pos){
        if(!isDragging){
            lastDragPoint = pos;
            isDragging = true;
            return;
        }

        else{
            var target = carrossel;
            if(pos.x > lastDragPoint.x){
                //rotate right
                target.RotateAround(target.position, Vector3.up, 40 * Time.deltaTime);
            }
            else{
                //rotate left
                target.RotateAround(target.position, Vector3.up, -40 * Time.deltaTime);
            }

            lastDragPoint = pos;
        }
    }
}
