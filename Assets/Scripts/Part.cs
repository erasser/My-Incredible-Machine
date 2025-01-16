using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class Part : MonoBehaviour
{
    public float rotationStep;
    public bool canBeFlipped;
    Outline _outline;
    Rigidbody2D _rb2D;
    Vector3 _positionBeforeSimulation;
    Quaternion _rotationBeforeSimulation;

    List<Vector3> _debugPositionHistory = new();

    void Awake()
    {
        PrepareOutline();

        _rb2D = GetComponent<Rigidbody2D>();

        if (_rb2D.bodyType == RigidbodyType2D.Dynamic)
            PartsWithDynamicRigidBodies2D.Add(this);

        StopSimulation();
    }

    void PrepareOutline()
    {
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineWidth = 3;
        _outline.enabled = false;
    }

    public void ToggleCollisionEffect(bool enable)
    {
        _outline.OutlineColor = Color.red;
        _outline.enabled = enable;
    }

    public void ToggleSelectionEffect(bool enable)
    {
        _outline.OutlineColor = Color.green;
        _outline.enabled = enable;
    }

    public void Rotate()
    {
        if (rotationStep == 0)
            return;

        transform.Rotate(Vector3.back, rotationStep, Space.World);
    }

    public void Flip()
    {
        if (!canBeFlipped)
            return;

        transform.Rotate(Vector3.up, 180, Space.World);

        GetComponent<Conveyor>()?.Flip();
    }

    public void Delete()
    {
        ClearSelection();

        if (PartsWithDynamicRigidBodies2D.Contains(this))
            PartsWithDynamicRigidBodies2D.Remove(this);

        Destroy(gameObject);
    }

    public void StartSimulation()
    {
        _rb2D.bodyType = RigidbodyType2D.Dynamic;

        RestoreStateBeforeSimulation();
    }

    public void StopSimulation()
    {
        SaveStateBeforeSimulation(); // ---- to se musí jindy

        _rb2D.bodyType = RigidbodyType2D.Kinematic;
        _rb2D.velocity = Vector3.zero;
    }

    void SaveStateBeforeSimulation()
    {
        var tr = transform;
        _positionBeforeSimulation = tr.position;
        _rotationBeforeSimulation = tr.rotation;
    }

    void RestoreStateBeforeSimulation()
    {
        var tr = transform;
        tr.position = _positionBeforeSimulation;
        tr.rotation = _rotationBeforeSimulation;
    }

    void Update()
    {
        _debugPositionHistory.Add(_rb2D.position);
        
        if (Input.GetMouseButtonDown(1))
        {
            foreach (Vector3 v in _debugPositionHistory)
                print(v);
        }
    }

}
