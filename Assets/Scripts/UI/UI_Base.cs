using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>(); //클래스가 Type인, 유니티 오브젝트들을 배열로 저장해주세요 라는의미.

    public abstract void Init(); //어쩌피 UI_base 자체로 쓸일은 없기에, virutal 대신 abstract를 사용하자. abstract = 이름만달아놓고 내용은 없으니, 하위에서 상속받은 애들에서 내용을 구현해라.
 
    private void Start() //상위객체에서 Start를 해줬으니, 아래객체들은 Start를 안써도되고, 그냥 Init()만 정의해주면된다.
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object//연결을 해주는데 enum에 있는 내용들을 전부 알아야되니, 런타임과 함께 확인이 가능한 reflection문법을 사용하자.
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; //인자로만든 str배열크기의, 유니티엔진 오브젝트 타입의 배열을 만든다.
        _objects.Add(typeof(T), objects);
        //이제 사용해야할 object들을 찾았으니 하나하나 돌며 맵핑해주면된다.
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject)) //gameObject로 찾으면 기존 FindChild가 안먹혀서, 분기를 만들어서 다른 find를 써야됨 
            {
                objects[i] = Util.FindChild(gameObject, names[i], true);
            }
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to Bind ({names[i]})");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null; //꺼낸애들을 담아놓을 임시 배열
        if (_objects.TryGetValue(typeof(T), out objects) == false) // 만약 꺼낼수가 없다면(실패) null
            return null;

        return objects[idx] as T; //성공했다면 T타입으로 리턴
    }
    protected TextMeshProUGUI GetText(int idx) //앞으론 Get을쓸때, 타입마다 불러올때 Get<타입> 치기 귀찮으니 그냥 여러버전의 함수로 만들어두자
    { return Get<TextMeshProUGUI>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }


    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click) //인자는 (대상, 행동, 어떤기능을 원하는지)
    {
        /* UI_EventHandler evt = go.GetComponent<UI_EventHandler>(); //위에서 찾은 go에서 UI_EventHandler라는 컴포넌트를 evt에 담은것
        if (evt == null) // 만약 오브젝트에 UI_EventHandler컴포넌트가 안달려있다면, 직접 달아준다.
            evt = go.AddComponent<UI_EventHandler>(); */ //이 내용이 GetOrAddComponent의 내용이다.
        UI_EventHandler evt=Util.GetOrAddComponent<UI_EventHandler>(go);

        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;

            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }

}

