using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); }  } //혹시나 외부에서 현재scene의 타입을 필요로하면 쓸 함수

    public void LoadScene(Define.Scene type) //우린 enum목록값으로 씬타입을 이용하고 있으니,
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type) //Define.Scene type은 말그대로 Define값이지 string이 아님, 그래서 문자열로 활용할수 없기에, Define값을 문자열로 번역해주는 함수를 만들자.
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type); //이렇게 동적으로 쓰는걸 reflection타입이라 하나봄.
        return name;
    }

    public void Clear() //원래 얘를 그냥 LoadScene에 넣어도 되는데, Managers에서 모든 Clear를 담당하게 하려고 따로 만듬
    {
        CurrentScene.Clear();
    }
}
