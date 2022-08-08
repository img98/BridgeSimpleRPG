using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos;

    [SerializeField]
    protected Define.State _state = Define.State.Idle; //초기 상태는 Idle로 설정 //_state를 이제 안쓰는데 이거 필요한가?=>필요하다. State가 결국은 _state의 상태를 바꿔주는 용도이기에.

    [SerializeField]
    protected GameObject _lockTarget;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public virtual Define.State State //상속받은애들중 추가하고싶은 내용있는 애들은 이거갖고 바꾸라는뜻
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0); //인자= 틀어줄 모션, transition넘어가는데 걸릴시간, 레이어(신경안쓸거니 -1), normalizedTimeOffset(0으로하면 모션이 끝나도 처음으로 돌아가서 다시튼다는데?)
                    break;
            }
        }
    }
    public void Start()
    {
        Init();

    }

    void Update()
    {

        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public abstract void Init();

    protected virtual void UpdateDie() { } //모든 캐릭터에 업데이트 로직이 같다는 보장은 없으니, 인터페이스만 파두자
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }

}
