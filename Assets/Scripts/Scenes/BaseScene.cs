using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown; //씬타입이 어디서든 받아서 확인할순있지만(public) 설정은 상속받은자손만 가능(protected), 그리고 이값은 Unknown으로 설정해두겠다.

    void Awake() // Awake는 GameScene이라는 컴포넌트(스크립트말고 오브젝트 자체를 말하고있다.)를 꺼도 작동한다. 알면좋고
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear(); //여기서 만들건 아니라 abstract문 //이 Scene을 종료시키는게 Clear(말그대로 내앞에서 치워라.)
}
