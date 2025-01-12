using UnityEngine;
using UnityEngine.EventSystems;
using static GameController;

public class UiButtonRotate : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        SelectedPart.Rotate();
    }
}
