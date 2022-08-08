using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10; // 어쩌피 내부에서 사용하는 깊이의 정도일뿐이니 굳이 0일필요는 없다. 혹은 1~9를 다른걸로 미리 지정해둬도 되겠다.

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();//사실 팝업의 기능은 UI_Popup이라는 컴포넌트 하에 존재한다. 그러니 스택의 타입을 그저 GameObject가 아닌 UI_Popup으로 해주자. 
    UI_Scene _sceneUI = null; //사실 sceneUI는 저장안해도 되는데, 혹시 딴데 쓸지도 모르니까 임시적으로 담는 변수를 만들어두자.

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true) //외부에서 ui를 팝업시킬때 순서에 맞춰 캔버스order를 기입해달라는 뜻
    {
        // UI가 생성될때 필요한 코드이므로, UI_Popup과 UI_Scene에 필요하겠다. 

        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay; //렌더모드는 그냥 이거 써라
        canvas.overrideSorting = true; // 부모와 별개로 자신만의 sortOrder를 갖겠다.

        if (sort == true)
        {
            canvas.sortingOrder = _order;
;           _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base //프리팹엔 없는걸 동적으로 붙이기위한 함수
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup //UI를 팝업 시키는(여는) 기능
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; //이름을 써주지 않아도, 넣은 파일의 type을 추출할수있으니, name으로 <T>를 사용하겠다는말(우리가 프리팹이랑 파일타입을 같게해주서 ex.UI_Popup 가능한 코드다.)

        GameObject go= Managers.Resource.Instantiate($"UI/Popup/{name}"); //팝업도 결국은 프리팹에 있는걸 생성하는거니까
        T popup = Util.GetOrAddComponent<T>(go); //생성한 애한테 GetOrAddComponent
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);
        
        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene //UI를 여는 기능. 그렇지만 SceneUI이기에 스택에 들어가진 않겠다.
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; //이름을 써주지 않아도, 넣은 파일의 type을 추출할수있으니, name으로 <T>를 사용하겠다는말(우리가 프리팹이랑 파일타입을 같게해주서 ex.UI_Popup 가능한 코드다.)

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI= Util.GetOrAddComponent<T>(go); //생성한 애한테 GetOrAddComponent
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform); //Root를 프로퍼티 문법으로 만들어서 바로 사용가능함.

        return sceneUI;
    }

    public void ClosePopupUI(UI_Popup popup) // 잘못된걸 지우지못하게, 지울 이름을 입력하는 버전
    {
        if (_popupStack.Count == 0)
            return;
        if (_popupStack.Peek() == popup)
        {
            Debug.Log($"Close Popuo Fail! : this time u need to close {_popupStack.Peek()}, but u requested {popup}"); // 오바해서 좀 길게늘림
            return;
        }

        ClosePopupUI();
    }
        public void ClosePopupUI() // 팝업된UI 지우는 기능
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject); //popup.gameObject란, 해당순서의 UI_Popup을 물고있는 오브젝트를 의미한다
        popup = null; //삭제했으니 popup을 다시건드리면 에러남. 아예null로 처리해버리자

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }


}
