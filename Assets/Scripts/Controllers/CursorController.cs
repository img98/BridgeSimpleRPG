using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    Texture2D _attackIcon; //자주쓸 아이콘이라 그냥 한번 갖고와서 저장시키겠다.
    Texture2D _handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand,

    }
    CursorType _cursorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack"); //자주쓸 아이콘이라 그냥 한번 갖고와서 저장시키겠다.
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Ground)
            {
                if (_cursorType != CursorType.Attack) //커서이미지가 너무 자주바뀌면(프레임마다) 이미지가 깜빡거림. 그래서 상태가 변했을때만, 커서이미지가 변하게 해주는것.
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 5, 0), CursorMode.Auto); //인자 (이미지, 이미지가 시작될좌표, 마우스모드 )
                    _cursorType = CursorType.Attack;
                }
            }
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }

    }
}
