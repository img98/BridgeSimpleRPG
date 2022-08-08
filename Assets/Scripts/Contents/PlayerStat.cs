using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat //플레이어한테만 특별히 더있을 stat들을 위한 스크립트, 일반 Stat을 상속받아서 level, hp같은건 안써줘도된다.
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp 
    { 
        get { return _exp; } 
        set //여기서 바로 레벨업 체크를 하면, 다른데서 코드를 해당코드를 쓸 일이 적어질거다.
        { 
            _exp = value;
            //레벨업 체크
            //int level = Level; //체크용임시 level , 내 원래 Level
            int level = 1; //체크 level을 1부터 시작하면 만약 경험치가 깎이는 경우를 넣었을때 레벨 다운도 만들수 있을것이다 ,대신 현재 경험치에 맞춰서 아래 while문이 여러번 돌것이다.
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break; //다음 레벨이 없다면, 그냥 끝내기
                if (_exp < stat.totalExp)
                    break;
                level++;
            }
            if(level !=Level) //위코드를 돌고 임시로만든변수level이 내 Level과 다르다면, 레벨업이 일어낫다는 것이니, 나의 Level을 업데이트
            {
                Debug.Log("Level Up!");
                Level = level;
                SetStat(Level);
            }
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        
        _level = 1;
        _exp = 0;
        _defense = 5; //스탯json에 따로 추가안해주긴했는데 새로 만들기 귀찮으니 그냥 상수로 넣어주자.
        _moveSpeed = 5.0f;
        _gold = 0;

        SetStat(_level);
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        //_exp = stat.totalExp; //사실 exp는 위를 정의하다보면 자연히 알수있고, exp가 좀 초과해있는 상태에서 불러오면 오히려 내려가는 현상이 생길수있으니 안써주는게 맞다.
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
        Managers.UI.ShowSceneUI<UI_YouDied>();
        Managers.Resource.Destroy(gameObject);
    }
}
