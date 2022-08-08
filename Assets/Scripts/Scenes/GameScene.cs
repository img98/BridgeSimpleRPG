using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    protected override void Init()
    {
        base.Init(); //start가 없지만 BaseScene에 Awake로 Init을 돌리는 구문이 있어서, 여기도 start없어도 작동한다.

        SceneType = Define.Scene.Game;
        
        //Managers.UI.ShowSceneUI<UI_Inven>(); //게임신이 켜지면 강제로 인벤토리UI도 켜지게하던 코드. 사실 지금은 별내용이 없기에 지워도된다.
        
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        //Managers.Game.Spawn(Define.WorldObject.Monster, "Knight"); //이제 한마리 수동으로 만들지 않고, 스포닝풀을 사용할것이다.
        GameObject go = new GameObject { name = "SpawningPool" }; //스포닝풀이란 오브젝트를 만들고
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>(); //그 오브젝트에 실제로 SpawningPool스크립트를 달아준다. 이래야 기능있는 스포닝풀이 생성된것.
        pool.SetKeepMonsterCount(5);
    }



    public override void Clear()
    {

    }
}
    