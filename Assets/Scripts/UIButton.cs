using UnityEngine;
using UnityEngine.EventSystems;
using static GameController;

public class UiButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        if (SelectedPart)
        {
            if (name == "button rotate")
                SelectedPart.Rotate();

            if (name == "button flip")
                SelectedPart.Flip();

            if (name == "button delete")
                SelectedPart.Delete();
        }
    }
}
