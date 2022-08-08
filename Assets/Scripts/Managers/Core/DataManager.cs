using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value> //key랑 value는 무슨 정해진 타입이 있는게아님, 오히려 타입을 너가 정해서 넣으라는뜻. 애초에 이건 인터페이스니까, 이런식으로 만들라는 가이드문
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();

    public void Init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict(); //!!LoadJson이 끝나면 StatData타입이 반환될테니, 우리 거기서 추가로 MakeDict()를 진행하여 dictionary를 만들어준다
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key,Value> //Loader는 우리가 불러올 데이터의 타입을 의미함. 즉, <키와밸류를 들고있는 애의 타입, 키의 타입, 밸류의 타입>
    {
        //데이터를 파싱하는 함수임

        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}"); 
        return JsonUtility.FromJson<Loader>(textAsset.text); //유니티는 Json을 알아서 파싱해주는 라이브러리가 있다.

    }



    //Data는 항상 들고있어야 하기에  Clear하지않는다.
}
