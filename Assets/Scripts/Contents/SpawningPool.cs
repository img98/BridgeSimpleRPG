using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _monsterCount = 0;
    [SerializeField]
    int _reserveCount = 0;
    [SerializeField]
    int _keepMonsterCount = 0;
    [SerializeField]
    Vector3 _spawnPos;
    [SerializeField]
    float _spawnRadius = 15.0f;
    [SerializeField]
    float _spawnTime = 5.0f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; } 

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount; //몬스터카운트 함수는 OnSpawnEvent를 구독하여 Invoke를 받을것임
    }

    void Update()
    {
        while(_reserveCount+_monsterCount<_keepMonsterCount ) 
        {
            StartCoroutine("ReserveSpawn");
        }
    }
    IEnumerator ReserveSpawn() //Managers.Game.Spawn처럼 그전 스폰만 하는게 아니라, 스폰위치, 생성시간 기능까지 추가해줄거라 새로 함수를 만드는것이다.
    {
        _reserveCount++;

        // 0초에서 지정스폰시간 사이에, 스폰시키고 싶은데 시간을 어떻게 조정할수있을까 => 코루틴을 이용하면 좋다.
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        //위 랜덤시간이 지나면 아래 코드 작동
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster,"Knight");


        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>(); //생성위치가 유효한지 확인하려면 길찾기가 효율적임. 길찾기하려고 NMA를 달아줬다.
        
        Vector3 randPos;
        while (true) 
        {
            Vector3 randDir = Random.insideUnitSphere * (Random.Range(0, _spawnRadius)); //Random.insideUnitSphere => 랜덤 벡터를 만드는데 굉장히 유용하다!. 여기에 Radius를 곱해 크기까지 만들어줬다.
            randDir.y = 0; //3d이기는 하나 땅밑이나 하늘에 스폰시킬게 아니니 y축은 0으로 밀어줘야됨

            randPos = _spawnPos + randDir; //스폰의 중심점 + 랜덤방향벡터 = 랜덤으로 스폰될 위치
            NavMeshPath path = new NavMeshPath();
            if(nma.CalculatePath(randPos, path)) //갈수있는지 확인
                break;
        }

        obj.transform.position = randPos; //길찾기를 통해 이 랜덤pos는 쓸수있는곳임을 알았으니, 이제 너는 거기서 태어나라.
        _reserveCount--;

    }
}
