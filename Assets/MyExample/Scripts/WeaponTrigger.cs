using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
   
    void Start()
    {
        OnDeactiveWeapon();
    }

    void Update()
    {
        
    }
    public BoxCollider weaponCol;

    public void OnActiveWeapon()
    {
        // dagger의 충돌체를 활성화 하고싶다.
        weaponCol.enabled = true;        
    }

    public void OnDeactiveWeapon()
    {
        // dagger의 충돌체를 비활성화 하고싶다.
        weaponCol.enabled = false;
    }
}
