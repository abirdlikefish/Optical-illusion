using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MyButton : Button
{
    [SerializeField] UnityEvent onPointerDown = new UnityEvent();
    [SerializeField] UnityEvent onPointerUp = new UnityEvent();
    [SerializeField] float longPressTime = 0.6f;
    [SerializeField] float longPressTimer = 0;
    [SerializeField] UnityEvent onStartLongPress = new UnityEvent();
    public ref UnityEvent get_onPointerDown() => ref onPointerDown;
    bool isDown = false;
    bool isPress = false;
    protected override void Awake()
    {
        base.Awake();
        navigation = new Navigation() { mode = Navigation.Mode.None };
    }
    void Update()
    {
        if (isDown)
        {
            if (isPress)
            {
                return;
            }
            longPressTimer += Time.deltaTime;
            if (longPressTimer >= longPressTime)
            {
                isPress = true;
                onStartLongPress.Invoke();
            }
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        isDown = true;
        longPressTimer = 0;
        onPointerDown.Invoke();
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        isDown = isPress = false;
        onPointerUp.Invoke();
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        isDown = isPress = false;
    }
    public void Test()
    {
        print("Long Press");
    }
}
