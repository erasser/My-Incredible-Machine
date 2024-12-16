using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<GameObject> objectsPrefabs = new();
    bool _dragging;
    GameObject _draggedObject;
    Rigidbody2D _draggedObjectRb;
    Camera _camera;
    Transform _cameraTransform;
    public LayerMask raycastPlaneLayerMask;
    public LayerMask draggableLayerMask;
    Vector3 _draggingOffset;
    public static Text InfoText;

    void Start()
    {
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _camera = _cameraTransform.GetComponent<Camera>();
        InfoText = GameObject.Find("InfoText").GetComponent<Text>();
        // Time.timeScale = 2;
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

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            Time.timeScale -= .2f;
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
            Time.timeScale += .2f;
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
            Time.timeScale = 0;

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

        _draggedObjectRb.position = hit.point + _draggingOffset;
    }

    void ProcessClick()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitObject, 100, draggableLayerMask))
        {
            Physics.Raycast(ray, out var hitPlane, 100, raycastPlaneLayerMask);
        
            var obj = hitObject.collider.gameObject;
        
            SetDraggedObject(obj, obj.transform.position - hitPlane.point);
        }
    }

    void SetDraggedObject(GameObject obj, Vector3 offset)
    {
        _draggedObject = obj;
        _draggingOffset = offset;
        _draggedObjectRb = _draggedObject.transform.parent.GetComponent<Rigidbody2D>();
        _dragging = true;
    }

}
