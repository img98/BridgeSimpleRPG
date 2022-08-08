using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util //그냥 기능성 함수들을 전부 넣어놓은 스크립트
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component // 오브젝트에서 컴포넌트를 받아오거나 없으면 추가해주세요
    {
        T component = go.GetComponent<T>(); 
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive); // 모든 go는 transform이 있으니, 새로 코드짤필요 없이, 아래 FindChild<T>코드에 필요한 컴포넌트<T>를 Transform으로 쓰면된다.

        if (transform != null)
            return transform.gameObject;
        return null;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        //최상위 부모를 입력하면, 그의 자식들을 쭉 훑어보면서 이름(name)이 같은 자식을 return하거나,이름입력이 없다면 그냥 타입이 맞으면 리턴해주는함수
        //resursive는 재귀적으로, 자식의 자식도 찾을건지 옵션
    {
        if (go == null)
            return null;
        if(recursive==false)
        {
            for(int i =0;i<go.transform.childCount;i++)
            {
                Transform transform = go.transform.GetChild(i); //딱 자기 자식만 데려오는 라이브러리
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {//이름 조건을 통과했으면 컴포넌트를 들고있는지 확인해줘야됨
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
            
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>()) //자식들컴포넌트 가져오는 라이브러리를 통해 하나씩 component에 대입
            {
                if (string.IsNullOrEmpty(name) || component.name == name) //이름이 비어있거나, 내가 찾던거면 그거 보내주기.
                    return component;
            }

        }
        return null;
    }
}
