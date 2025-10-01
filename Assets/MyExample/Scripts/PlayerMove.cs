using Fusion;
using TMPro;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    public float speed = 3f;
    public float rotSpeed = 200f;
    public GameObject cameraRig;
    public Transform body;
    public Animator anim;
    NetworkCharacterController cc;


    void Start()
    {
    }

    void Update()
    {
    }

    public TextMeshProUGUI textNickname;

    [Networked] string myNickname { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        cc = GetComponent<NetworkCharacterController>();

        // 만약 입력권한이 없다면 
        if (false == HasInputAuthority)
        {
            // 카메라를 끄고싶다.
            cameraRig.transform.GetChild(0).gameObject.SetActive(false);
            textNickname.SetText(myNickname);
        }
        else
        {
            textNickname.SetText(ConnManager.instance.userNickname);
            textNickname.color = Color.red;
            RPC_ServerSetNickname(ConnManager.instance.userNickname);
        }
    }

    // 1. 입력권한이 있는 클라이언트가 서버에게 이름좀 바꿔줘 요청
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ServerSetNickname(string nickname)
    {
        myNickname = nickname;
        RPC_ClientSetNickname(nickname);
    }
    // 2. 요청을 받은 서버가 다른 모든 클라이언트 들에게 이름 바꿔 라고 요청
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ClientSetNickname(string nickname)
    {
        textNickname.SetText(nickname);
    }



    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        Move();
        Rotation();
    }

    public override void Render()
    {
        base.Render();
        anim.SetFloat("Speed", magnitude);

    }

    [Networked] float magnitude { get; set; }

    private void Move()
    {
        if (false == GetInput(out Vector3 direction))
        {
            return;
        }
        direction.Normalize();

        direction = cameraRig.transform.TransformDirection(direction);

        cc.Move(direction * speed * Runner.DeltaTime);

        magnitude = direction.magnitude;

        // 만약 이동중이라면(magnitude가 0보다 크면)
        if (magnitude > 0)
        {
            // direction방향으로 body를 회전하고싶다.
            body.rotation = Quaternion.LookRotation(direction);
        }


    }

    private void Rotation()
    {
        if (GetInput(out NetworkInputData data))
        {
            float mx = data.mouseX;

            cameraRig.transform.eulerAngles += new Vector3(0, mx, 0) * rotSpeed * Runner.DeltaTime;
        }
    }


    bool GetInput(out Vector3 dir)
    {
        if (GetInput(out NetworkInputData data))
        {
            dir = data.direction;
        }
        else
        {
            dir = Vector3.zero;
        }
        return true;
    }

}
