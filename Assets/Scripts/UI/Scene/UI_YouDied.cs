using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_YouDied : UI_Scene
{
    enum Buttons // Bind�� üũ�Ҷ� ��(Buttons)�� ����(PointButton)�̶� ���� �̸��� �ִ��� Ȯ��.
    {
        BackToLogin,
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.BackToLogin).gameObject.BindEvent(BackToLoginButtonPressed);

        //temp �հ� background�� Youdied ��Ʈ ���̵������� �������� ���ڴµ�.. �ڷ�ƾ�ۿ� ���̾���?
    }
    public void BackToLoginButtonPressed(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scene.Login);
    }
}
