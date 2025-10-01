using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : NetworkBehaviour
{
    [Networked] float curHP { get; set; }
    public float maxHP = 10f;
    public Slider sliderHP;

    Animator anim;
    ChangeDetector changeDetector;

    void Start()
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (HasStateAuthority)
        {
            curHP = maxHP;
        }
        sliderHP.value = 1f;
        // 태어날 때 애니메이터를 기억하고싶다.
        anim = GetComponentInChildren<Animator>();
    }

    public override void Render()
    {
        base.Render();
        foreach (var change in changeDetector.DetectChanges(this))
        {
            switch(change)
            {
                case nameof(curHP):
                    sliderHP.value = curHP / maxHP;
                    break;
            }
        }

    }

    void Update()
    {
        // 입력권한이 있다. 그리고 마우스 왼쪽 버튼이 눌러지면
        if (HasInputAuthority && Input.GetMouseButtonDown(0))
        {
            // 공격요청
            RPC_ServerPlayAttack();
        }
    }

    // 입력권한이 있는 클라이언트가 서버에게 공격을 요청하고싶다.
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_ServerPlayAttack()
    {
        RPC_ClientPlayAttack();
    }
    // 서버가 모든 클라이언트에게 공격을 요청하고싶다.
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_ClientPlayAttack()
    {
        anim.SetTrigger("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        // dagger와 부딪혔다면
        if (HasInputAuthority && other.gameObject.name.Contains("dagger"))
        {
            // 모든 클라이언트들의 체력을 1 소모하고싶다.  
            RPC_ServerDamage(1f);
            other.enabled = false;
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ServerDamage(float damage)
    {
        curHP = Mathf.Max(0, curHP - 1);
    }
}
