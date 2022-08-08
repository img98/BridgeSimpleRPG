using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f); //캐릭터로부터 얼마나 떨어져있는가. //아까 신에서 돌려보고 y=-6, z=5라는값을 찾았다.

    [SerializeField]
    GameObject _player = null; //컴포넌트 드래그드랍안했을때를 위한 초기화

    public void SetPlayer(GameObject player) { _player = player; } //사실이런건 카메라매니저 같은걸 만드는게맞긴한데, 없으니까 그냥 대충하자

    void Start()
    { 
        
    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid()==false)
                return;
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block)) //플레이어 위치에서, 캠방향으로, 레이캐스트해서 Block이 콜리젼 된다면
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.9f; //hit좌표와 플레이어 좌표의 차로 방향과 크기를 알수있고, 대충 0.8을 곱해서 벽보다 조금 앞으로 나오게한다. 
                //transform.position = _player.transform.position + _delta.normalized * dist; //normalized 한 이유는, 크기가 아니라 방향만을 갖고오기 위해서(크기는 dist에 담았다)
                transform.position = Vector3.Slerp(transform.position, _player.transform.position + _delta.normalized * dist, 0.5f); //카메라 줌인아웃을 부드럽게 하고싶다.
            }
            else //벽이 없다면 그냥 이동
            {
                //매틱마다 플레이어 좌표를 기준으로 카메라 위치를 계산해야한다. 플레이어를 드래그드랍으로 불러오자.(아직 플레이어를 어떻게 다룰지 결정하지 못해서 임시방편)
                transform.position = _player.transform.position + _delta; // 플레이어의 위치에 델타위치를 더해준게 = 카메라의 위치
                transform.LookAt(_player.transform);//카메라가 상대의 좌표를 주시하도록 강제
            }

        }

    }

    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
