using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }


    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects)); //지난번에 했던 바인드함수

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridPanel.transform) //내 transform이 들고있는 모든 자식들을 순회
            Managers.Resource.Destroy(child.gameObject); // 기존 프리팹에서 예시용으로 들고있던 모든 자식들(인벤_아이템)을 삭제해주세요=초기화
        
        //여기부턴 패널에 아이템 채우는 로직, 실제 인벤토리 정보를 참고해서 작성하면됨
        for(int i=0;i<8;i++) //아직 정보라고 할거도없으니 그냥 8개정도만 만들어보자.
        {
            // GameObject item = Managers.Resource.Instantiate("UI/SubItem/UI_Inven_Item"); //이렇게 프리팹의 경로를 길게 쓰는게 귀찮으니 ShowPopupItem처럼 subItem을 만드는 함수를 만들어서 적용시키자
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform).gameObject; //MakeSubItem을통해 컴포넌트를 뽑아와서, 걔의 주인(gameObject)를 뽑아서 Item에 대입
            //item.transform.SetParent(gridPanel.transform); //부모자식 설정하는것도 MakeSubItem에 인자로 넣어서 기능추가하면 좋을것같다.

            //UI_Inven_Item invenItem = Util.GetOrAddComponent<UI_Inven_Item>(item); //만든 아이템자식들에게 UI_Inven_Item 컴포넌트를 붙여 기능을 부여 or 걔네를 Get해서 다른데 사용할수있다.
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>(); //GetOrAddComponent기능을 item 즉, 그냥 gameobject에서도 쉽게 불러올수있게 만들려고 extension으로 확장시켰다.
            invenItem.SetInfo($"{i}번째 집행검");
        }

    }
}
