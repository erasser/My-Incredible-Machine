using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Gc;
    public List<GameObject> objectsPrefabs = new();
    public static bool Dragging;
    public static Part DraggedObject;
    static Rigidbody2D _draggedObjectRb;
    static Camera _camera;
    static Transform _cameraTransform;
    static Camera _cameraUi3D;
    public LayerMask raycastPlaneLayerMask;
    public LayerMask draggableLayerMask;
    public LayerMask ui3DLayerMask;
    static Vector3 _draggingOffset;
    public static Text InfoText;
    public static readonly List<Rigidbody2D> BallsRigidbodies2D = new();
    public static Part SelectedPart;
    Vector2 _lastOnObjectClickCoordinates;
    public UiButtonRotate buttonRotate;
    public UiButtonFlip buttonFlip;
    public UiButtonDelete buttonDelete;
    Vector2 _menu3DStartingOffset = new(- 100, 50);
    Vector2 _menu3DVerticalOffset = new(0, 15);
    static Transform _ui3DTransform;

    void Start()
    {
        Gc = this;
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _camera = _cameraTransform.GetComponent<Camera>();
        _cameraUi3D = GameObject.Find("Camera UI 3D").GetComponent<Camera>();
        _ui3DTransform = GameObject.Find("UI 3D").transform;
        InfoText = GameObject.Find("InfoText").GetComponent<Text>();

        CreateBallsList();

        HideTransformButtons();

        // CreateMenu3DItems();
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
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            CreateInstance(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            CreateInstance(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            CreateInstance(6);

        if (Input.GetMouseButtonDown(0))
        {
            ProcessMenu3DCast();
            ProcessSceneCast();
        }

        if (Input.GetMouseButtonUp(0))
            ProcessTouchReleased();

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            Time.timeScale -= .2f;
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
            Time.timeScale += .2f;
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
            Time.timeScale = 0;
    }

    void CreateInstance(int i)
    {
        var part = Instantiate(objectsPrefabs[i]);

        if (part.CompareTag("ball"))
            BallsRigidbodies2D.Add(part.GetComponent<Rigidbody2D>());            

        SetDraggedObject(part.GetComponent<Part>(), Vector3.zero);
    }

    void MoveObject()
    {
        if (!Dragging)
            return;

        Physics.Raycast(GetMouseRay(), out var hit, 100, raycastPlaneLayerMask);

        _draggedObjectRb.position = hit.point + _draggingOffset;
    }

    void ProcessMenu3DCast()
    {
        if (Physics.Raycast(_cameraUi3D.ScreenPointToRay(Input.mousePosition), out var hit, 100, ui3DLayerMask))
        {
            // instantiate
        }
    }

    void ProcessSceneCast()
    {
        var ray = GetMouseRay();

        if (Physics.Raycast(ray, out var hitObject, 100, draggableLayerMask))
        {
            Physics.Raycast(ray, out var hitPlane, 100, raycastPlaneLayerMask);

            var objTransform = hitObject.collider.gameObject.transform;

            SetDraggedObject(objTransform.parent.GetComponent<Part>(), objTransform.position - hitPlane.point);

            _lastOnObjectClickCoordinates = Input.mousePosition;
        }
        else if (!EventSystem.current.IsPointerOverGameObject())
            ClearSelection();
    }

    void ProcessTouchReleased()
    {
        if (!Dragging)
            return;

        Dragging = false;

        if (!_draggedObjectRb.isKinematic)
            _draggedObjectRb.velocity = Vector3.zero;

        _draggedObjectRb.transform.Find("raycast collider")?.GetComponent<RaycastCollider>()?.CheckOverlap();
    }

    Ray GetMouseRay()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }

    void SetDraggedObject(Part part, Vector3 offset)
    {
        DraggedObject = part;
        _draggingOffset = offset;
        _draggedObjectRb = DraggedObject.GetComponent<Rigidbody2D>();
        Dragging = true;
        SelectPart(part);
    }

    void CreateBallsList()
    {
        var balls = GameObject.FindGameObjectsWithTag("ball");

        foreach (GameObject ball in balls)
            BallsRigidbodies2D.Add(ball.GetComponent<Rigidbody2D>());
    }

    public static void SelectPart(Part part)
    {
        ClearSelection();
        SelectedPart = part;
        part.ToggleSelectionEffect(true);

        ShowUiButtons();
    }

    public static void ClearSelection()
    {
        if (!SelectedPart)
            return;

        SelectedPart.ToggleSelectionEffect(false);
        SelectedPart = null;

        HideTransformButtons();
    }

    static void ShowUiButtons()
    {
        if (SelectedPart.rotationStep != 0)
            Gc.buttonRotate.gameObject.SetActive(true);

        if (SelectedPart.canBeFlipped)
            Gc.buttonFlip.gameObject.SetActive(true);

        Gc.buttonDelete.gameObject.SetActive(true);
    }

    static void HideTransformButtons()
    {
        Gc.buttonRotate.gameObject.SetActive(false);
        Gc.buttonFlip.gameObject.SetActive(false);
        Gc.buttonDelete.gameObject.SetActive(false);
    }

    void CreateMenu3DItems()
    {
        // foreach (GameObject prefab in objectsPrefabs)
        for (int i = 0; i < objectsPrefabs.Count; ++i)
        {
            var item = Instantiate(objectsPrefabs[i], _menu3DStartingOffset + i * _menu3DVerticalOffset, objectsPrefabs[i].transform.rotation, _ui3DTransform);
        }
    }

}
