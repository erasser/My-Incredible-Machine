using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public List<GameObject> objectsPrefabs = new();
    bool _dragging;
    GameObject _draggedObject;
    Rigidbody _draggedObjectRb;
    Transform _draggedObjectTransform;
    Camera _camera;
    Transform _cameraTransform;
    public LayerMask raycastPlaneLayerMask;
    public LayerMask draggableLayerMask;
    Vector3 _draggingOffset;  // offset between cursor and dragged object

    void Start()
    {
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _camera = _cameraTransform.GetComponent<Camera>();
        // _raycastPlaneMask = LayerMask.NameToLayer("raycast plane");
    }

    void Update()
    {
        ProcessControls();

        MoveObject();  // TODO: Move to fixed upd.
    }

    void ProcessControls()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CreateInstance(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            CreateInstance(1);

        if (Input.GetMouseButtonDown(0))
            ProcessClick();

        if (Input.GetMouseButtonUp(0))
        {
            _dragging = false;
            if (_draggedObject && !_draggedObjectRb.isKinematic)
                _draggedObjectRb.velocity = Vector3.zero;
        }
    }

    void CreateInstance(int i)
    {
         SetDraggedObject(Instantiate(objectsPrefabs[i]), Vector3.zero);
    }

    void MoveObject()
    {
        if (!_dragging)
            return;

        Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, 100, raycastPlaneLayerMask);

        // _draggedObjectTransform.position = hit.point + _draggingOffset;
        // Physics.SyncTransforms();
        _draggedObjectRb.position = hit.point + _draggingOffset;
    }

    void ProcessClick()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hitObject, 100, draggableLayerMask))
        {
            Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hitPlane, 100, raycastPlaneLayerMask);

            // _draggedObject = hitObject.collider.gameObject;
            // _draggedObjectTransform = _draggedObject.transform;
            // _draggingOffset = _draggedObjectTransform.position - hitPlane.point;
            // _dragging = true;
            // _draggedObjectRb = _draggedObject.GetComponent<Rigidbody>();

            SetDraggedObject(hitObject.collider.gameObject, hitObject.collider.gameObject.transform.position - hitPlane.point);
        }

    }

    void SetDraggedObject(GameObject obj, Vector3 offset)
    {
        _draggedObject = obj;
        _draggedObjectTransform = _draggedObject.transform;
        _draggingOffset = offset;
        _dragging = true;
        _draggedObjectRb = _draggedObject.GetComponent<Rigidbody>();
    }

    void SetDraggedObjectx(GameObject o)
    {
        _draggedObject = o;
        _draggedObjectTransform = _draggedObject.transform;
        _draggingOffset = Vector3.zero;
        _dragging = true;
        _draggedObjectRb = _draggedObject.GetComponent<Rigidbody>();
    }
    
    void SetDraggedObjecty(RaycastHit hit)
    {
        _draggedObject = hit.collider.gameObject;
        _draggedObjectTransform = _draggedObject.transform;
        // _draggingOffset = _draggedObjectTransform.position - hit.point;
        _dragging = true;
        _draggedObjectRb = _draggedObject.GetComponent<Rigidbody>();

        // SetDraggedObject(hit.collider.gameObject);
    }

}
