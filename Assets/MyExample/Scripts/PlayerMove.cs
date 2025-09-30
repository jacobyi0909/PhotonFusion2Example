using Fusion;
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

    public override void Spawned()
    {
        base.Spawned();
        cc = GetComponent<NetworkCharacterController>();

        // 만약 입력권한이 없다면 
        if (false == HasInputAuthority)
        {
            // 카메라를 끄고싶다.
            cameraRig.transform.GetChild(0).gameObject.SetActive(false);
        }
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
