using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10;

    [SerializeField]
    float _attackRange = 2;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetOrAddComponent<Stat>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle()
    {

        //TODO :플레이어 몬스터 총괄 매니저생기면 옮길것
        //GameObject player = GameObject.FindGameObjectWithTag("Player"); //이것보단 GetPlayer로 게임매니저에서 player를 리턴하는 함수를 만드는게 좀더 최적화될거임
        GameObject player = Managers.Game.GetPlayer();
        if (!player.IsValid()) //player==null이였던걸, 플레이어 despawn하고나면 idle상태를 유지하기위해 valid로 바꾸었다.
        {
            return;

        }

        float distance = (player.transform.position - transform.position).magnitude; //플레이어와 나까지의 거리
        if (distance <= _scanRange) //플레이어가 내 애드범위에 있으면
        {
            _lockTarget = player; //타겟팅잡고

            State = Define.State.Moving; //그쪽으로 이동하겠다.

            return;
        }

    }

    protected override void UpdateMoving()
    {
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)  //사정거리에 들어오면
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position); //사정거리안에 들어와 공격중이면 더이상 앞으로 더가지않게 (이걸로 밀치기 해결)

                State = Define.State.Skill; //스킬사용으로 상태바꿈
                return; //아래코드가 더 돌지않게 return으로 끝
            }
        }

        //이동로직
        Vector3 dir = _destPos - transform.position; //방향벡터 구하기
        if (dir.magnitude < 0.1f) // 도착했다면 //nma.Move가 정밀도가 그리높진않아서, 도착의 기준을 널널히해줬다.
            State = Define.State.Idle;
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            nma.SetDestination(_destPos); //목적지까지 찾아가는 길찾기 라이브러리이다.
            nma.speed = _stat.MoveSpeed;

            //transform.position = transform.position + dir.normalized * moveDist; //NavMeshAgent를 안쓸때, 하드코딩으로 움직이던(포지션을 바꿔주던) 코드
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }
    }

    protected override void UpdateSkill()
    {
        if (_lockTarget.IsValid())
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }

    }
    
    void OnHitEvent()
    {
        if (_lockTarget.IsValid())
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat); //타겟한테, 내_stat을 담은, 공격받음 이벤트가 일어나게 한다.

            if (targetStat.Hp>0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;
            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }

    }
}
