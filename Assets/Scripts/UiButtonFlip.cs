using UnityEngine;
using UnityEngine.EventSystems;
using static GameController;

public class UiButtonFlip : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        SelectedPart.Flip();
    }
}
