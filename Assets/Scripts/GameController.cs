using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Gc;
    public List<GameObject> objectsPrefabs = new();
    public static bool Dragging;
    public static GameObject DraggedObject;
    static Rigidbody2D _draggedObjectRb;
    static Camera _camera;
    static Transform _cameraTransform;
    public LayerMask raycastPlaneLayerMask;
    public LayerMask draggableLayerMask;
    static Vector3 _draggingOffset;
    public static Text InfoText;
    public static List<Rigidbody2D> BallsRigidbodies = new();
    // public static float MaxSpeed = 40;

    // public Transform dummy1Transform;
    // public Transform dummy2Transform;

    void Start()
    {
        Gc = this;
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _camera = _cameraTransform.GetComponent<Camera>();
        InfoText = GameObject.Find("InfoText").GetComponent<Text>();
        CreateBallsList();
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
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            CreateInstance(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            CreateInstance(3);

        if (Input.GetMouseButtonDown(0))
            ProcessClick();

        if (Input.GetMouseButtonUp(0))
            PlaceDraggedObject();

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            Time.timeScale -= .2f;
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
            Time.timeScale += .2f;
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
            Time.timeScale = 0;
    }

    void CreateInstance(int i)
    {
        var a = Instantiate(objectsPrefabs[i]);

         SetDraggedObject(a, Vector3.zero);
    }

    void MoveObject()
    {
        if (!Dragging)
            return;

        Physics.Raycast(GetMouseRay(), out var hit, 100, raycastPlaneLayerMask);

        _draggedObjectRb.position = hit.point + _draggingOffset;
    }

    void ProcessClick()
    {
        var ray = GetMouseRay();

        if (Physics.Raycast(ray, out var hitObject, 100, draggableLayerMask))
        {
            Physics.Raycast(ray, out var hitPlane, 100, raycastPlaneLayerMask);

            var objTransform = hitObject.collider.gameObject.transform;

            SetDraggedObject(objTransform.parent.gameObject, objTransform.position - hitPlane.point);
        }
    }

    Ray GetMouseRay()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }

    void SetDraggedObject(GameObject obj, Vector3 offset)
    {
        DraggedObject = obj;
        _draggingOffset = offset;
        _draggedObjectRb = DraggedObject.GetComponent<Rigidbody2D>();
        Dragging = true;
    }

    void PlaceDraggedObject()
    {
        if (!DraggedObject)
            return;

        Dragging = false;

        if (!_draggedObjectRb.isKinematic)
            _draggedObjectRb.velocity = Vector3.zero;

        _draggedObjectRb.transform.Find("raycast collider")?.GetComponent<RaycastCollider>()?.CheckOverlap();
    }

    void CreateBallsList()
    {
        var balls = GameObject.FindGameObjectsWithTag("ball");

        foreach (GameObject ball in balls)
            BallsRigidbodies.Add(ball.GetComponent<Rigidbody2D>());
    }

}
