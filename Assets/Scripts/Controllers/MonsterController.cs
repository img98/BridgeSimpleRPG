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

        //TODO :�÷��̾� ���� �Ѱ� �Ŵ�������� �ű��
        //GameObject player = GameObject.FindGameObjectWithTag("Player"); //�̰ͺ��� GetPlayer�� ���ӸŴ������� player�� �����ϴ� �Լ��� ����°� ���� ����ȭ�ɰ���
        GameObject player = Managers.Game.GetPlayer();
        if (!player.IsValid()) //player==null�̿�����, �÷��̾� despawn�ϰ����� idle���¸� �����ϱ����� valid�� �ٲپ���.
        {
            return;

        }

        float distance = (player.transform.position - transform.position).magnitude; //�÷��̾�� �������� �Ÿ�
        if (distance <= _scanRange) //�÷��̾ �� �ֵ������ ������
        {
            _lockTarget = player; //Ÿ�������

            State = Define.State.Moving; //�������� �̵��ϰڴ�.

            return;
        }

    }

    protected override void UpdateMoving()
    {
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)  //�����Ÿ��� ������
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position); //�����Ÿ��ȿ� ���� �������̸� ���̻� ������ �������ʰ� (�̰ɷ� ��ġ�� �ذ�)

                State = Define.State.Skill; //��ų������� ���¹ٲ�
                return; //�Ʒ��ڵ尡 �� �����ʰ� return���� ��
            }
        }

        //�̵�����
        Vector3 dir = _destPos - transform.position; //���⺤�� ���ϱ�
        if (dir.magnitude < 0.1f) // �����ߴٸ� //nma.Move�� ���е��� �׸������ʾƼ�, ������ ������ �γ��������.
            State = Define.State.Idle;
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            nma.SetDestination(_destPos); //���������� ã�ư��� ��ã�� ���̺귯���̴�.
            nma.speed = _stat.MoveSpeed;

            //transform.position = transform.position + dir.normalized * moveDist; //NavMeshAgent�� �Ⱦ���, �ϵ��ڵ����� �����̴�(�������� �ٲ��ִ�) �ڵ�
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
            targetStat.OnAttacked(_stat); //Ÿ������, ��_stat�� ����, ���ݹ��� �̺�Ʈ�� �Ͼ�� �Ѵ�.

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