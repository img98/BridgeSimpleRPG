using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager // 이미 싱글톤 매니저가 있으니, 얘는 컴포넌트가 아닌 일반적인 C#파일로 만들것
{
    public Action KeyAction = null; //일종의 delegate
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false; //MouseAction에서 클릭인지 알기위한 변수
    float _pressedTime = 0; //Click과 PressedUp을 구분할수 있기위해, 몇초동안 누르고있었는지 기록

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //UI를 눌렀다면 return;
            return;

        if (Input.anyKey && KeyAction != null) //anykey가 있고(&&) keyAciton이 null이 아니면
            KeyAction.Invoke();

        if(MouseAction!=null)
        {
            if(Input.GetMouseButton(0)) //마우스0 = 왼쪽버튼
            {
                if(!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true; //마우스를 눌렀으면 true로 바꿔주고, 왼쪽버튼을 때면 GetMouseButton이 false가 될테니 else문으로 이동한다.
            }
            else
            {
                if(_pressed)
                {
                    if(Time.time<_pressedTime + 0.2f) //누르기시작한 때에서 0.2초 안에 뗏으면 클릭으로 인정하겠다.
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }

                _pressed = false;
                _pressedTime = 0;
            }
        }
    }
    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
