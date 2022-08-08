using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_YouDied : UI_Scene
{
    enum Buttons // Bind로 체크할때 얘(Buttons)의 내용(PointButton)이랑 같은 이름이 있는지 확인.
    {
        BackToLogin,
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.BackToLogin).gameObject.BindEvent(BackToLoginButtonPressed);

        //temp 먼가 background랑 Youdied 멘트 페이드인으로 나왔으면 좋겠는데.. 코루틴밖에 답이없나?
    }
    public void BackToLoginButtonPressed(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scene.Login);
    }
}
