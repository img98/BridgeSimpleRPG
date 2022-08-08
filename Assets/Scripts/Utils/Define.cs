using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,

    }
    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

public enum Layer
    {
        Monster = 8,
        Ground =9,
        Block =10,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press, //마우스를 누르고있는 상태
        PointerDown, // 꾹 누르기를 시작하는 순간 (press와 동시에 일어난다.)
        PointerUp, //꾹 누르고있다가, 떼는 순간
        Click,
        
    }

    public enum CameraMode
    {
        QuarterView,
    }
}
