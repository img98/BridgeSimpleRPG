using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }
    public virtual void ClosePopupUI() //UI_Popup을 상속받은 애들이 ClosePopupUI에 쉽게 접근할수있도록, 첨부함.(이게 extension인가?)
    {
        Managers.UI.ClosePopupUI(this);
    }
}
