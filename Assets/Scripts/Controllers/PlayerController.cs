using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    PlayerStat _stat;

    bool _stopSkill = false;

    //GetMask뒤에 계속 string이 들어가는게 맘에 안들어서 Define에 아예 레이어별로 숫자를 정의해줬다.
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetOrAddComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

    }

    protected override void UpdateMoving()
    {
        //몬스터가 내 사정거리보다 가까우면 공격
        if(_lockTarget!=null)
        {
            _destPos = _lockTarget.transform.position; //목표위치는 타겟팅된 몬스터 위치
             float distance = (_destPos - transform.position).magnitude; //목표 몬스터까지의 거리
            if (distance <= 1)  //사정거리에 들어오면
            {
                State = Define.State.Skill; //스킬사용으로 상태바꿈
                return; //아래코드가 더 돌지않게 return으로 끝
            }
        }

        //이동로직
        Vector3 dir = _destPos - transform.position; //방향벡터 구하기
        dir.y = 0;//다른 콜라이더타고 올라가지않게 0으로 높이조정
        if (dir.magnitude < 0.1f) // 도착했다면 //nma.Move가 정밀도가 그리높진않아서, 도착의 기준을 널널히해줬다.
            State = Define.State.Idle;
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))  //인자= 레이저쏘는 위치가 한 배꼽쯤으로(돌뿌리도 못간다체크하면 안되니까), 방향, 레이저길이(코앞에있다는걸 알기위해 짧게), 막힐 레이어
            {
                if(!Input.GetMouseButton(0)) //마우스를 떼지않았으면 계속 벽에 비비는 모션으로 남을수있게(디아블로처럼) 조건문 달아줌
                    State = Define.State.Idle; //Idle로 만들어 서있는 애니메이션으로 바꿈
                return; //앞에 벽이있으니 움직이지않고 그냥끝
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude); //이동거리의 제한을 안해주면 끝날때 도착점에 수렴하지 못하고 부들부들거릴수 있음.
                                                                                              //clamp(value,min,max) => value가 min보다 작으면 min으로 max보다 크면 max값으로 넣어준다.

            transform.position = transform.position + dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

    }
    protected override void UpdateSkill()
    {
        if (_lockTarget != null) //이 조건문이 필수다. 지금 몬스터가 poolable상태가 아니기에, 몬스터를 despawn하면 아래_lockTarget들이 null이 되어버려 크래쉬가남
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
                
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }


    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == Define.State.Die) //얘는 업데이트가 아니라 콜백형식이라 이렇게 따로 추가 해준거라는데?
            return;
        switch(State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp) //꾹 누르던게 한번이라도 떼지면 정지하겠다.
                        _stopSkill = true;
                }
                break;
        }


    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt) //꾹누르는중에도 OnMouseEvent는 계속 들어갈건데, 만약 아래 코드가 좀 꼬이면, 행동도중 에러가 날테니 서기, 달리기만 관리하는 녀석버전으로 따로 빼냈다.
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);//레이캐스트한해서 히트했다는 결과를 저장

        switch (evt)
        {
            case Define.MouseEvent.PointerDown: //마우스를 누른순간
                {
                    if (raycastHit)
                    {   //Moving을 시작
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press: //마우스를 누르고있는 동안
                {                    
                    if (_lockTarget == null && raycastHit) 
                        _destPos = hit.point; //목적지 좌표를 계속 업데이트
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }

    }
}
