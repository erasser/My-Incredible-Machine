using UnityEngine;
using UnityEngine.EventSystems;
using static GameController;

public class UiButtonDelete : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        SelectedPart.Delete();
    }
}
