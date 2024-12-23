using UnityEngine;

public class Part : MonoBehaviour
{
    Outline _outline;

    void Start()
    {
        PrepareOutline();
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

}
