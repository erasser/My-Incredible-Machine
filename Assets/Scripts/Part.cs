using UnityEngine;
using static GameController;

public class Part : MonoBehaviour
{
    public float rotationStep;
    public bool canBeFlipped;
    Outline _outline;

    void Awake()
    {
        PrepareOutline();
    }

    void PrepareOutline()
    {
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineWidth = 3;
        _outline.enabled = false;
        
        // print("◘" + _outline + ", id: " + GetInstanceID());
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

        Destroy(gameObject);
    }

}
