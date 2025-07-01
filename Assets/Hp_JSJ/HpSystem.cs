using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpSystem : MonoBehaviour
{
    [Header("Health Info")]
    public float maxHp = 100f;
    public float curHp;

    public event Action<float, float> OnHpChanged;

    private void Awake()
    {
        curHp = maxHp;
    }

    public void TakeDamage(float amount)
    {
        if (amount < 0) return;

        curHp -= amount;
        curHp = Mathf.Max(curHp, 0);

        OnHpChanged?.Invoke(curHp, maxHp);

        if (UIHpDisplay.Instance != null) { UIHpDisplay.Instance.RegisterAndShowHpUI(this); }

        if (curHp <= 0)
        {
            // Die Logic 구현 (어차피 다른 곳에서도 이 스크립트 쓸거니까 overring하는 게 나을 수도?)
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (amount < 0) return;

        curHp += amount;
        curHp = Mathf.Min(curHp, maxHp);

        OnHpChanged?.Invoke(curHp, maxHp);

        if (UIHpDisplay.Instance != null) { UIHpDisplay.Instance.RegisterAndShowHpUI(this); }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " is Dead :(");
    }

    //public float GetHpPercentage()
    //{
    //    if (maxHp == 0) return 0;
    //    return curHp / maxHp;
    //}

    public void SetMaxHp(float newMaxHealth)
    {
        if (newMaxHealth <= 0) return;

        maxHp = newMaxHealth;

        curHp = Mathf.Min(curHp, maxHp);
        OnHpChanged?.Invoke(curHp, maxHp);
    }

    public void SetCurHp(float newHp)
    {
        curHp = Mathf.Clamp(newHp, 0, maxHp);
        OnHpChanged?.Invoke(curHp, maxHp);
    }
}
