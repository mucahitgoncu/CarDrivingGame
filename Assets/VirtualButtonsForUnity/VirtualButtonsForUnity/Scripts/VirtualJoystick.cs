using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public enum VirtualJoystickType { Fixed, Floating }

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform centerArea = null;
    [SerializeField] private RectTransform handle = null;
    [InputControl(layout = "Vector2")]
    [SerializeField] private string stickControlPath;
    [SerializeField] private float movementRange = 100f;

    protected VirtualJoystickType joystickType = VirtualJoystickType.Fixed;
    protected bool _hideOnPointerUp = false;
    protected bool _centralizeOnPointerUp = true;
    private Canvas canvas;
    protected RectTransform baseRect = null;
    protected OnScreenStick handleStickController = null;
    protected CanvasGroup bgCanvasGroup = null;
    protected Vector2 initialPosition = Vector2.zero;
    private Vector2 input;

    public static VirtualJoystick instance;

    protected virtual void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        baseRect = GetComponent<RectTransform>();
        bgCanvasGroup = centerArea.GetComponent<CanvasGroup>();
        handleStickController = handle.gameObject.AddComponent<OnScreenStick>();
        handleStickController.movementRange = movementRange;
        handleStickController.controlPath = stickControlPath;

        Vector2 center = new Vector2(0.5f, 0.5f);
        centerArea.pivot = center;
        handle.anchorMin = center;
        handle.anchorMax = center;
        handle.pivot = center;
        handle.anchoredPosition = Vector2.zero;

        initialPosition = centerArea.anchoredPosition;

        if (joystickType == VirtualJoystickType.Fixed)
        {
            centerArea.anchoredPosition = initialPosition;
            bgCanvasGroup.alpha = 1;
        }
        else if (joystickType == VirtualJoystickType.Floating)
        {
            if (_hideOnPointerUp) bgCanvasGroup.alpha = 0;
            else bgCanvasGroup.alpha = 1;
        }

        instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerEventData constructedEventData = new PointerEventData(EventSystem.current);
        constructedEventData.position = handle.position;
        handleStickController.OnPointerDown(constructedEventData);

        if (joystickType == VirtualJoystickType.Floating)
        {
            centerArea.anchoredPosition = GetAnchoredPosition(eventData.position);

            if (_hideOnPointerUp)
                bgCanvasGroup.alpha = 1;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (joystickType == VirtualJoystickType.Floating)
        {
            handleStickController.OnDrag(eventData);
        }

        // Update the input vector based on joystick position
        input = handle.anchoredPosition / movementRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType == VirtualJoystickType.Floating)
        {
            if (_centralizeOnPointerUp)
                centerArea.anchoredPosition = initialPosition;

            if (_hideOnPointerUp) bgCanvasGroup.alpha = 0;
            else bgCanvasGroup.alpha = 1;
        }

        PointerEventData constructedEventData = new PointerEventData(EventSystem.current);
        constructedEventData.position = Vector2.zero;

        handleStickController.OnPointerUp(constructedEventData);
        input = Vector2.zero; // Reset input when joystick is released
    }

    public Vector2 GetInput()
    {
        return input;
    }

    protected Vector2 GetAnchoredPosition(Vector2 screenPosition)
    {
        Camera cam = (canvas.renderMode == RenderMode.ScreenSpaceCamera) ? canvas.worldCamera : null;
        Vector2 localPoint = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
        {
            Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
            return localPoint - (centerArea.anchorMax * baseRect.sizeDelta) + pivotOffset;
        }

        return Vector2.zero;
    }
}
