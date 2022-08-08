using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다.

    #region Contents
    GameManagerEx _game = new GameManagerEx();
    public static GameManagerEx Game { get { return Instance._game; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager(); //각각 기능 매니저와 Managers를 연결
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();


    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null) //만약 @Managers가 없어 null이 들어온다면 새로 @managers를 만들어주자
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go); // 중요한거니 왠만하면 삭제하지 말라는 문법
            s_instance = go.GetComponent<Managers>();


            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._sound.Init();

        }
    }

    public static void Clear()
    {
        //Data는 항상 들고있어야 하기에 clear하지않는다.
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Clear(); // 혹시 다른매니저에서 pool을 쓰고있었을지도 모르니 맨 마지막에 삭제
    }
}
