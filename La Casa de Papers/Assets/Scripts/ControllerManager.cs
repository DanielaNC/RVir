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
    public Transform cam;
    public Transform resetTransform;
    public GameObject corkboard = null;
    public Transform penRotation = null;

    public OVRInput.Controller controller;
    private Color resetColor;

    private GameObject selectedObject = null;

    private float lineMaxLength = 10f;
    
    private bool grabbingObject = false;
    private GameObject grabbedObject = null;
    private GameObject lastGrabbedObject = null;
    private GameObject highlight = null;
    private GameObject line = null;
    private Vector3 hitPoint = new Vector3();
    private Collider collider = null;
    private bool isHighlighting = false;
    private bool isDraggingX = false;
    private bool isDraggingY = false;
    private bool hasDeletedLastLine = false;
    private Vector3 lastDragPoint = new Vector3();
    private bool openMenu = false;

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
        
        float rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);

        if(selectedObject != null && selectedObject.tag == "Menu"){
            if(rightIndexTrigger > 0.5f || rightHandTrigger > 0.5f || OVRInput.Get(OVRInput.Button.One, controller) || OVRInput.Get(OVRInput.Button.Two, controller)){
                OpenCloseMenu(selectedObject);
                return;
            }
        }

        if(selectedObject != null && selectedObject.tag == "Button"){
            if(rightIndexTrigger > 0.5f || rightHandTrigger > 0.5f || OVRInput.Get(OVRInput.Button.One, controller) || OVRInput.Get(OVRInput.Button.Two, controller)){
                TriggerButton(selectedObject);
                return;
            }
        }

        if (rightIndexTrigger < 0.5f || !grabbingObject){
            isHighlighting = false;
            FreeHandDeleteLine();
        }

        if (rightIndexTrigger > 0.5f && !grabbingObject && selectedObject != null && selectedObject.tag == "Document"){
            OpenDocument();
        }

        if (rightIndexTrigger > 0.5f && !grabbingObject && selectedObject != null && selectedObject.transform.parent == carrossel){
            CloseDocument();
        }

        if(OVRInput.Get(OVRInput.Button.One, controller) && (grabbingObject && grabbedObject.GetComponent<Pen>() != null)){
            FreeHandDeleteLine();
        }

        if(OVRInput.Get(OVRInput.Button.Two, controller)){
            RotateCarousselY(cam.InverseTransformPoint(grab_spot.position));
            //Debug.Log("Initial pos: " + OVRInput.GetLocalControllerPosition(controller));

        }

        if(rightIndexTrigger > 0.5f && grabbingObject && grabbedObject.GetComponent<Highlighter>() != null && highlight == null){
            //Debug.Log("selected: " + selectedObject);
            if(selectedObject != null && selectedObject.layer == 3){
                Highlight();
            }
        }

        if(rightIndexTrigger > 0.5f && grabbingObject && grabbedObject.GetComponent<Pen>() != null && (selectedObject == null || selectedObject.layer == 3 )){ // 3: paper
            hasDeletedLastLine = false;
            FreeHandDraw();
        }

        if(OVRInput.Get(OVRInput.Button.One, controller) && grabbingObject && grabbedObject.GetComponent<Pen>() != null){
            DeleteLine();
        } 

        if(rightIndexTrigger > 0.5f && grabbingObject && grabbedObject.GetComponent<Eraser>() != null){
            if(highlight!=null){
                EraseHighlight();
            }

            if(line!=null)
                EraseLine();
        }

        if(rightHandTrigger <= 0.5f && selectedObject != null && selectedObject.tag == "Corkboard" && grabbingObject && grabbedObject.layer == 3){
            PinToCorkboard();
        }

        if(rightHandTrigger <= 0.5f && selectedObject != null && selectedObject.tag == "Placeholder" && grabbingObject && grabbedObject.layer == 3){
            PinToPlaceholder();
        }

        if(OVRInput.Get(OVRInput.Button.One, controller) && !grabbingObject){
            RotateCarousselX(cam.InverseTransformPoint(grab_spot.position));
            //Debug.Log("Initial pos: " + OVRInput.GetLocalControllerPosition(controller));

        }

        
        if(selectedObject != null && !grabbingObject && rightHandTrigger > 0.5f && rightIndexTrigger < 0.5f){
            if(selectedObject.transform.parent.gameObject.tag == "Document") return;
            HoldObject();
        }
        //rightHandTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);

        if(grabbingObject && rightHandTrigger <= 0.5f){
            ReleaseObject(resetTransform, grabbedObject.transform.position);
        }

        if(!OVRInput.Get(OVRInput.Button.One, controller)){
            isDraggingX = false;
            //Debug.Log("Final pos: " + OVRInput.GetLocalControllerPosition(controller));

        }

        if(!OVRInput.Get(OVRInput.Button.Two, controller)){
            isDraggingY = false;
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

            else if (hit.collider.gameObject.tag == "Line")
            {
                selectedObject = hit.collider.gameObject.transform.parent.gameObject;
                line = hit.collider.gameObject;
                hitPoint = hit.point;
                OVRInput.SetControllerVibration(0.5f, 0.5f, controller);
            }

            else if (hit.collider.gameObject.name != "Plane")
            {
                //hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                selectedObject = hit.collider.gameObject;
                hitPoint = hit.point;
                highlight = null;
                OVRInput.SetControllerVibration(0.0f, 0.0f, controller);
            }

            else{
                selectedObject = null;
                OVRInput.SetControllerVibration(0.0f, 0.0f, controller);
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
        if(selectedObject.tag == "Pen")
            selectedObject.transform.rotation = penRotation.rotation;
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
        if(selectedObject != null && parent.gameObject.tag == "Corkboard")
            grabbedObject.transform.rotation = corkboard.transform.rotation;
        else if(pin && parent != resetTransform)
            grabbedObject.transform.rotation = parent.rotation;
        grabbedObject.transform.parent = parent;
        if(pin)
            grabbedObject.transform.parent = parent.transform.parent.gameObject.transform;
        //selectedObject = null;
        if(grabbedObject.GetComponent<Paper>() != null)
            grabbedObject.GetComponent<Paper>().isPinned = parent;
        grabbingObject = false;
        lastGrabbedObject = grabbedObject;
        grabbedObject = null;
    }

    private void PinToCorkboard(){
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
        ReleaseObject(selectedObject.transform, selectedObject.transform.position, true);
    }

    private void EraseHighlight(){
        GameObject.Find("Manager").GetComponent<PaperManager>().UpdatePapers(highlight.transform.parent.gameObject, highlight, true);
        grabbedObject.GetComponent<Eraser>().Erase(highlight);
        highlight = null;
    }

    private void EraseLine(){
        line.GetComponent<LineManager>().DeleteSelf();
        line = null;
    }

    private void RotateCarousselX(Vector3 pos){
        if(!isDraggingX){
            lastDragPoint = pos;
            isDraggingX = true;
            return;
        }

        else{
            var target = carrossel;
            if(pos.x > lastDragPoint.x){
                //rotate right
                carrossel.gameObject.GetComponent<CarousselManager>().RotateCarousselX(40.0f);
            }
            else{
                //rotate left
                carrossel.gameObject.GetComponent<CarousselManager>().RotateCarousselX(-40.0f);
            }

            lastDragPoint = pos;
        }
    }

    private void RotateCarousselY(Vector3 pos){
        if(!isDraggingY){
            lastDragPoint = pos;
            isDraggingY = true;
            return;
        }

        else{
            var target = carrossel;
            if(pos.y > lastDragPoint.y){
                //rotate right
                carrossel.gameObject.GetComponent<CarousselManager>().RotateCarousselY(40.0f);
            }
            else{
                //rotate left
                carrossel.gameObject.GetComponent<CarousselManager>().RotateCarousselY(-40.0f);
            }

            lastDragPoint = pos;
        }
    }

    private void FreeHandDraw(){
        var o = grabbedObject.GetComponent<Pen>().DrawLine(hitPoint, selectedObject);
        GameObject.Find("Manager").GetComponent<PaperManager>().UpdatePapers(selectedObject, o);
    }

    private void DeleteLine(){
        // FIXME
        grabbedObject.GetComponent<Pen>().NewLine();
    }

    private void FreeHandDeleteLine(){
        if(!hasDeletedLastLine){
            if(grabbingObject && grabbedObject.GetComponent<Pen>() != null)
                grabbedObject.GetComponent<Pen>().NewLine();
            else if(lastGrabbedObject != null && lastGrabbedObject.GetComponent<Pen>() != null)
                lastGrabbedObject.GetComponent<Pen>().NewLine();
            hasDeletedLastLine = true;
        }
    }

    private void OpenDocument(){
        selectedObject.GetComponent<Document>().Open();
        GameObject.FindObjectOfType<CarousselManager>().OpenDocument(selectedObject);
    }

    private void CloseDocument(){
        //selectedObject.transform.parent.gameObject.GetComponent<Document>().Close();
        GameObject.FindObjectOfType<CarousselManager>().CloseDocument();
    }

    private void OpenCloseMenu(GameObject menu){
        if(menu == null || menu.GetComponent<MenuManager>()==null) return;
        if(openMenu)
            menu.GetComponent<MenuManager>().Close();
        else
            menu.GetComponent<MenuManager>().Open();

        openMenu = !openMenu;
    }

    private void TriggerButton(GameObject button){
        if(button == null || button.GetComponent<MenuButton>()==null) return;
        button.transform.parent.gameObject.transform.parent.gameObject.GetComponent<MenuManager>().TriggerButton(button.GetComponent<MenuButton>().id);
    }
}
