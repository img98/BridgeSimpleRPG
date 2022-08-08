using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager //리소스 매니저에 기생해서, Pooling을 관리하는 매니저

{
    #region pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; } //@Pool_Root 아래에 자기종류에 맞는 풀 산하에 배치하도록 추가해준 transform. 그냥 정리차원에서 만들었다.  

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5) // Count는, 만들어진 original_Root 풀에 대기하고있을, 풀러블(Poolable) 애들의 수 를 의미한다.
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for(int i=0;i<count;i++)
                Push(Create());
        }

        Poolable Create() //풀러블 객체 만드는 함수
        {
            GameObject go = Object.Instantiate<GameObject>(Original); //실제로 생성해야되니 Instantiate
            go.name = Original.name; //Clone안붙게 이름은 그냥 original대로
            return go.GetOrAddComponent<Poolable>(); //Poolable 컴포넌트를 붙여주자.
        }

        public void Push(Poolable poolable) //create로 만든 풀러블들을, 만들었다고 바로 스택에 Push해주는게 아니라 비활성화시키기, original_root산하로 옮기는 등의 과정을 처리하고, 스택에 넣는 기능
        {
            if (poolable == null) //poolalble이 없으면 먼가 잘못된거임
                return;

            poolable.transform.parent = Root; //부모를 root로 달아주기
            poolable.gameObject.SetActive(false); //비활성화시키기
            poolable.IsUsing = false; //poolalble내에 있던 유일한 그녀석. 사용중이 아니라고 기입

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent) //인자parent는 활성화될 풀러블이 어디에 위치할지를 입력해주면된다.
        {
            Poolable poolable;

            if (_poolStack.Count > 0) 
                poolable = _poolStack.Pop();
            else
                poolable = Create(); //만약 스택안에 있는게 없다면, 새로 만들어서 갖다주기
            

            poolable.gameObject.SetActive(true);

            if (parent == null) //DontDestroyOnLoad 해제 용도 (꺼내는 애들이 준비된 5개 이상이면, 새롭게 생성이 된다. 그런데 새롭게 생성이되면 parent가 null이기에 DontDestroyOnLoad에 붙어버린다.)
                poolable.transform.parent = Managers.Scene.CurrentScene.transform; //살짝 꼼수인데, 새로 생성되서 parent가 없는애들은, 씬에 무조건 존재하는 CurrentScene의 산하에 넣겠다는의미.
                                                                                   //이렇게되면 아래 poolable.transform.parent = parent=null;이 되기에, CurrentScene과의 연결은 끊어지고, 그럼 자연스럽게 Scene이 있는 위치와 같은위치에 오게될것이다.
            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>(); //풀매니저는 여러개의 이름(string)을 가진 Pool들을 갖고있을거고, 그로인해 여러종류의 풀을 갖게 될것이다.
    Transform _root; //사실 transform으로하든 GameObject로 하든 상관없다. 어쩌피 오든 오브젝트에는 transform이 있기때문
    
    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void Push(Poolable poolable) //풀링할 애들을 집어넣는 함수. 스택처럼 Push Pop함수를 자주 쓴다고한다.
    {
        string name = poolable.gameObject.name;
        if(_pool.ContainsKey(name)==false) //만약 다쓰고 집어넣으려는데, 이녀석의 풀이 없다면? 예외적인 경우이긴함
        {
            GameObject.Destroy(poolable.gameObject); //그냥 삭제해버리겠다.
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false) //만약 풀이 생성되기 전인 풀러블을, Pop을 요청했다면
            CreatePool(original); //해당 풀을 새로 만들겠다

        return _pool[original.name].Pop(parent);
    }
    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count); //Pool클래스에서 만든 Init을 사용하면 코드 중복이 덜해진다.
        pool.Root.parent = _root;

        _pool.Add(original.name, pool);
    }

    public GameObject GetOriginal(string name) //리소스의 Load에 해당하는 부분, 풀링할애들중에서 원본(프리팹 파일?)을 갖다줘라
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    public void Clear() //씬이 넘어가면, 풀에 넣어놨던 정보들을 날려야할까? 게임마다 다를것이긴한데.. 일단 지금은 mmo목표니 날리도록하자.
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
