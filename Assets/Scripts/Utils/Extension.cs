using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension //굳이 하나의 스크립트에서만 해당 함수기능을 호출할수있는 것이 아니라, this를 붙인곳에서도 호출할수있게 확장시킴 = extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
    
    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
