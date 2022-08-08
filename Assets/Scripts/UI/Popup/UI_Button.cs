using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup //앞으로 모든 ui는 UI_Base 기반으로 만들거니까, 얘를 상속받게하면됨
{
    enum Buttons // Bind로 체크할때 얘(Buttons)의 내용(PointButton)이랑 같은 이름이 있는지 확인.
    {
        PointButton,
    }
    enum Texts
    {
        PointText,
        ScoreText,
    }
    enum GameObjects
    {
        TestObject,
    }
    enum Images
    {
        ItemIcon,
    }


    public override void Init()
    {
        base.Init(); //내 부모의 init도 호출하기

        Bind<Button>(typeof(Buttons)); //이게 reflection문법인데, 정확히 enum을 넘긴건 아니고, enum타입을 넘긴거래
        Bind<TextMeshProUGUI>(typeof(Texts)); // Generic을 쓰는이유=Texts를 갖다쓸건데, 거기서 TextMeshProUGUI라는 클래스를 쓰는애를 맵핑해주세요 라는 뜻
        Bind<GameObject>(typeof(GameObjects)); //컴포넌트가 아니라, 게임타입을 갖다 맵핑하는 경우도 만들어보자.
        Bind<Image>(typeof(Images));

        // Get<TextMeshProUGUI>((int)Texts.ScoreText).text = "Test Get";
        //GetText((int)Texts.ScoreText).text = "Text Get v2."; //참고로 (int를 쓰는이유는 Texts.ScoreText를 int버전으로 받아들이기위함 이라고 이해하면된다.)

        GameObject go = GetImage((int)Images.ItemIcon).gameObject; //gameObject로 불러온 이유는, ItemIcon이라는 오브젝트에 UI_EventHandler를 추가하거나, UI_EventHandler로 변화를 줄것이기때문이다.
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);
    }

    int _score = 0;
    public void OnButtonClicked(PointerEventData data) 
    {
        _score++;
        GetText((int)Texts.ScoreText).text = $"점수 : {_score}";
    }
}
