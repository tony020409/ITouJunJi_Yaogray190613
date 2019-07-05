using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICoordinateDrag:MonoBehaviour,IPointerDownHandler,IDragHandler
{
    public RectTransform rect;
    public RectTransform point;
    public Slider sliderX;
    public Slider sliderY;
    public void OnPointerDown(PointerEventData eventData)
    {
        Press(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Press(eventData);
    }

    void Press(PointerEventData eventData)
    {

        Vector2 uiPosition;
        //https://www.youtube.com/watch?v=uSnZuBhOA2U
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, eventData.position, null, out uiPosition))
        {
            var size = rect.sizeDelta;
            var radius = size.x/2;
            if(uiPosition.sqrMagnitude<=radius*radius)
            {
                uiPosition = new Vector2(uiPosition.x/(size.x),uiPosition.y/(size.y));
                position.x = Mathf.Clamp(uiPosition.x,-0.5f,0.5f);
                position.y = Mathf.Clamp(uiPosition.y,-0.5f,0.5f);
                UpdatePosition(position);
                sliderX.value = position.x;
                sliderY.value = position.y;
            }
        }
    }
    public Vector2 position;
    public System.Action<Vector2> onPositionChanged;
    void UpdatePosition(Vector2 position)
    {
        if(onPositionChanged!=null)
            onPositionChanged(position);
        var size = rect.sizeDelta;
        position.x*=size.x;
        position.y*=size.y;
        point.anchoredPosition = position;
    }

    public float xRate
    {
        set
        {
            position.x = value;
            UpdatePosition(position);
        }
    }
    public float yRate
    {
        set
        {
            position.y = value;
            UpdatePosition(position);
        }
    }

    public void ResetPosition()
    {
        position = Vector2.zero;
        UpdatePosition(position);
        sliderX.value = position.x;
        sliderY.value = position.y;
    }
}