using UnityEngine;

public class PhysicsPointer : MonoBehaviour
{
    public float defaultLength = 10.0f;

    private LineRenderer lineRenderer = null;

    private void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update(){
        UpdateLength();
    }

    private void UpdateLength(){
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, CalculateEnd());
    }

    private Vector3 CalculateEnd(){
        RaycastHit hit = CreateForwardRaycast();
        Vector3 endPosition = DefaultEnd(defaultLength);

        if(hit.collider){
            //Debug.Log("Hit!" + hit.collider.gameObject.name);
            endPosition = hit.point;
        }

        return endPosition;
    }

    private RaycastHit CreateForwardRaycast(){
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }

    private Vector3 DefaultEnd(float length){
        return transform.position + (transform.forward * length);
    }
}
