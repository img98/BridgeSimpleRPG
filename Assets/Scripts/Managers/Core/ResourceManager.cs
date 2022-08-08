using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T: Object // 단,T는 Object형식 이라는 뜻
    {
        //로드를 하기전에 풀링되어있는 녀석인지 확인부터해보자.
        if(typeof(T)==typeof(GameObject)) //타입이 gameObject면 프리팹일 가능성이 높으니, 이경우를 조건으로
        {
            string name = path;
            int index = name.LastIndexOf('/'); //아래 Instantiate에 나와있듯, Prefabs/{path} 이렇게 /가 들어간다. 그러므로 마지막 /뒤에 나오는게 필요한 name이다.
            if (index >= 0) //발견했다면
                name = name.Substring(index + 1); // '/' 뒷칸부터 텍스트를 가져온게 이름.

            GameObject go = Managers.Pool.GetOriginal(name); //풀링매니저를 통해 original을 불러와서
            if (go != null) //찾아서 있었다면 그걸 return
                return go as T; 
        }

        return Resources.Load<T>(path); //Resources.Load는 이미 제공하는 라이브러리
    }

    public GameObject Instantiate(string path, Transform parent = null) // prefab을 넣을 부모(상위)오브젝트를 설정가능, 안쓸거면 null
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}"); // 여기서의 Load는 위 T Load를 만들어서 쓸수있는것. 원래라면 Resources.Load를 써야지
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 혹시 풀링된 애가 있다면 걔로 생성
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name; //복사본 이름을 그냥 오리지널이랑 같게한다.(클론 안뜨게)
        return go; 
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return ;
        //바로 삭제하지말고, 만약 풀링이 필요하다면->풀링 매니저한테 위탁. 필요하면 쉽게 꺼낼수있게
        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable!=null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }


}
