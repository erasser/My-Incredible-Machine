using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Gc;
    public List<GameObject> menuObjectsPrefabs;
    public static bool Dragging;
    public static Part DraggedObject;
    static Rigidbody2D _draggedObjectRb;
    static Camera _camera;
    static Transform _cameraTransform;
    static Camera _cameraUi3D;
    public LayerMask raycastPlaneLayerMask;
    public LayerMask draggableLayerMask;
    LayerMask _ui3DLayerMask;
    static Vector3 _draggingOffset;
    public static Text InfoText;
    public static readonly List<Rigidbody2D> BallsRigidbodies2D = new();
    public static Part SelectedPart;
    Vector2 _lastOnObjectClickCoordinates;
    public UiButtonRotate buttonRotate;
    public UiButtonFlip buttonFlip;
    public UiButtonDelete buttonDelete;
    GameObject _uiButtonPlay;
    GameObject _uiButtonStop;
    Vector2 _menu3DStartingOffset = new(- 80, 40);
    float _menu3DVerticalOffset = 14;
    float _menu3DPartsScale = 5;
    static Transform _ui3DTransform;
    public static readonly List<Part> PartsWithDynamicRigidBodies2D = new();

    void Start()
    {
        Gc = this;
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _camera = _cameraTransform.GetComponent<Camera>();
        _cameraUi3D = GameObject.Find("Camera UI 3D").GetComponent<Camera>();
        _ui3DTransform = GameObject.Find("UI 3D").transform;
        _ui3DLayerMask = 1 <<_ui3DTransform.gameObject.layer;
        InfoText = GameObject.Find("InfoText").GetComponent<Text>();
        _uiButtonPlay = GameObject.Find("button play");
        _uiButtonStop = GameObject.Find("button stop");
        _uiButtonStop.SetActive(false);

        CreateBallsList();

        HideTransformButtons();

        CreateMenu3DItems();
    }

    void Update()
    {
        ProcessControls();

        MoveObject();  // TODO: Move to fixed upd.

        // var camX = (2500 - Screen.width) / 230f * 10;
        // _cameraUi3D.transform.position = new(camX, _cameraUi3D.transform.position.y, _cameraUi3D.transform.position.z);

        // InfoText.text = camX.ToString();
    }

    void ProcessControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProcessSceneCast();
            ProcessMenu3DCast();
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

    void MoveObject()
    {
        if (!Dragging)
            return;

        _draggedObjectRb.position = GetMouseCastHit().point + _draggingOffset;
    }

    RaycastHit GetMouseCastHit()
    {
        Physics.Raycast(GetMouseRay(), out var hit, 100, raycastPlaneLayerMask);

        return hit;
    }

    void ProcessMenu3DCast()
    {
        if (Physics.Raycast(_cameraUi3D.ScreenPointToRay(Input.mousePosition), out var hit, 100, _ui3DLayerMask))
            CreatePart(hit.collider.transform.parent.GetComponent<Menu3DPart>());
    }

    void CreatePart(Menu3DPart menuPart)
    {
        var newPart = Instantiate(menuPart.partPrefab, GetMouseCastHit().point, menuPart.partPrefab.transform.rotation);

        if (newPart.CompareTag("ball"))
            BallsRigidbodies2D.Add(newPart.GetComponent<Rigidbody2D>());            

        SetDraggedObject(newPart, Vector3.zero);
    }

    void ProcessSceneCast()
    {
        var ray = GetMouseRay();

        if (Physics.Raycast(ray, out var hitObject, 100, draggableLayerMask))
        {
            var objTransform = hitObject.collider.gameObject.transform;

            SetDraggedObject(objTransform.parent.GetComponent<Part>(), objTransform.position - GetMouseCastHit().point);

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

        // if (!_draggedObjectRb.isKinematic)
            // _draggedObjectRb.velocity = Vector3.zero;

        // _draggedObjectRb.transform.Find("raycast collider")?.GetComponent<RaycastCollider>()?.CheckOverlap();
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

    void CreateMenu3DItems()  // TODO: Bude jim to chtít dát vhodnější collider (a nebo snad ani ne)
    {
        for (int i = 0; i < menuObjectsPrefabs.Count; ++i)
        {
            var part = menuObjectsPrefabs[i];
            Vector3 pos = new(_menu3DStartingOffset.x, _menu3DStartingOffset.y - i * _menu3DVerticalOffset, 0);
            var item = Instantiate(part, _ui3DTransform, false);

            item.transform.localPosition = pos;
            item.transform.rotation = part.transform.rotation * Quaternion.Euler(0, 30, 0);
            item.transform.localScale *= _menu3DPartsScale;

            item.layer = (int)Mathf.Log(_ui3DLayerMask.value, 2);
        }
    }

    public void StartWorldSimulation()
    {
        foreach (Part part in PartsWithDynamicRigidBodies2D)
            part.StartSimulation();
        
        _uiButtonPlay.SetActive(false);
        _uiButtonStop.SetActive(true);
    }

    public void StopWorldSimulation()
    {
        foreach (Part part in PartsWithDynamicRigidBodies2D)
            part.StopSimulation();
        
        _uiButtonStop.SetActive(false);
        _uiButtonPlay.SetActive(true);
    }

}
