using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inven_Item : UI_Base // 얘는 독립적인 존재도 아니고, 하나만 존재하는 것도 아니기에 UI_Scene이 아닌 UI_Base를 사용한다.
{
    enum GameObjects
    {
        //사실 양이 많지가 않아서, text랑 Image인데도 그냥 go로 뭉뚱그려서 담아도된다.
        ItemIcon,
        ItemNameText,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));//이러면 각각 text컴포넌트나 image컴포넌트를 물고있는걸 기준으로 찾아다주는게 아니라, 그냥 오브젝트의 이름이 저건지만 판별해서 바인딩해줌.결과는 같은데 로직이 아주조금 다르다.
        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = _name;//위에서 말했듯, 컴포넌트를 바인딩한게 아니기에, text를 바꿔주고싶으면 해당 이름의 오브젝트의 Component를 Get해서 연결해줘야됨

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"아이템 클릭! : {_name}"); }); //바인드는 위에서 했으니 생략가능, Get으로 아이콘을 누르면 로그가 나오게하는 코드
    }

    string _name;
    public void SetInfo(string name) //아이템별 이름을 저장해서 사용하기위함(지금은 따로 받아올곳이 없거 구조가 조악함.)
    {
        _name = name;
    }
}
