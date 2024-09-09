using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IBuffable
{
    [SerializeField]
    private ScriptableBuff AttackBuff;

    private ScriptableBuff _buff;
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        Debug.Log("Trigger entered with: " + collision.gameObject.name);
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);

            if (gotHit)
                Debug.Log(collision.name + " hit for " + attackDamage);
            damageable.IsStun = false;
        }
    }
    public void Start()
    {
        ApplyBuff(AttackBuff);
    }
    public void Update()
    {
        if (_buff != null) HandleBuff();
    }
    public void ApplyBuff(ScriptableBuff buff)
    {
        this._buff = buff;
        attackDamage += _buff.Value;
    }

    public void RemoveBuff()
    {
        attackDamage -= _buff.Value;
        _buff = null;
    }

    private float currentEffectTime = 0f;
    public void HandleBuff()
    {
        currentEffectTime += Time.deltaTime;

        if (currentEffectTime >= _buff.Duration) RemoveBuff();

    }
}

