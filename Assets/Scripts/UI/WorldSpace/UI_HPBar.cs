using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar,
    }

    Stat _stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        Transform parent = gameObject.transform.parent; //내.트랜스폼의.부모 (여기서 '내'는 생략가능)
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y); // HP바가 생기는 위치 = 내부모 + 위방향으로*부모의 콜라이더 y축사이즈만큼
        transform.rotation = Camera.main.transform.rotation; //HP바가 카메라방향에 맞게 돌아가도록,(빌보드)

        float ratio = _stat.Hp / (float)_stat.MaxHp; // float를 int로 나누는건 소수를 남길수있기에, 분모만 float캐스팅해줘도 된다.
        SetHPRatio(ratio);
    }

    public void SetHPRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
