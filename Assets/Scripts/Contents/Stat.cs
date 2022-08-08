using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;

    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;

    [SerializeField]
    protected float _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {//스탯은 나중에 서버데이터 연동해서 값을 넣어줘야하는데, 지금은 멀었으니까 수동으로 넣어주자.
        _level = 1;
        _hp = 100;
        _maxHp = 100; 
        _attack = 10;
        _defense = 5;
        _moveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat attacker) //공격은, 공격자가 상대에게 신호만 주고, 공격받는사람이 스탯을 변형시키는 로직이 더 낫다.
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if (Hp <= 0)
        {

            Hp = 0; //이게 교수코드
            //Hp = MaxHp; //다시 피채워서 풀에 돌려줘야되지않나?
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker as PlayerStat;
        if (playerStat!=null)
        {
            playerStat.Exp += 15; //나중엔 몬스터DB에 있는 (몬스터마다 다른) 경험치를 올려주면되지만, 지금은 DB만들기도 번거로우니 그냥 15로 고정.
        }
        
        Managers.Game.Despawn(gameObject); //gameObject => 지금 Game을 들고있는 주인

    }
}
