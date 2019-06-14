using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using LuaInterface;
public class EventTriggerListener: UnityEngine.EventSystems.EventTrigger
{
	public delegate void VoidDelegate();
	public delegate void BoolDelegate(bool state);
	public delegate void FloatDelegate(float delta);
	public delegate void VectorDelegate(Vector2 delta);
	public delegate void ObjectDelegate(GameObject obj);
	public delegate void KeyCodeDelegate(KeyCode key);

	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;
	public VoidDelegate onDrag;
	public VoidDelegate onEndDrag;
 
	public LuaFunction luaOnDrag;
	public LuaFunction luaOnEndDrag;

	static public EventTriggerListener Get(GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) 
			listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (onClick != null) 
			onClick();
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (onDown != null) 
			onDown();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (onEnter != null) 
			onEnter();
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (onExit != null) 
			onExit();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (onUp != null) 
			onUp();
	}

	public override void OnSelect(BaseEventData eventData)
	{
		if (onSelect != null) 
			onSelect();
	}

	public override void OnUpdateSelected(BaseEventData eventData)
	{
		if (onUpdateSelect != null) 
			onUpdateSelect();
	}

	public override void OnDrag (PointerEventData eventData)
	{
		if (onDrag != null)
		{
			onDrag ();
		}
		if (luaOnDrag != null) {
			luaOnDrag.Call ();
		}
	}

	public override void OnEndDrag (PointerEventData eventData)
	{
		if (onEndDrag != null)
		{
			onEndDrag ();
		}

		if (luaOnEndDrag != null) {
			luaOnEndDrag.Call ();
		}
	}
}
