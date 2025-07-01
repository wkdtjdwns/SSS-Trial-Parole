using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUpdater : MonoBehaviour
{
    public enum InteractionType
    {
        Deal,
        Heal
    }

    [Header("Interaction Info")]
    [SerializeField] private InteractionType type;
    [SerializeField] private float amount = 10f;
    //[SerializeField] private bool destroyOnUse = true;

    [Header("Optional: Target Tag")]
    [SerializeField] private string targetTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) { return; }

        HpSystem hpSystem = other.GetComponent<HpSystem>();

        if (hpSystem != null)
        {
            switch (type)
            {
                case InteractionType.Deal:
                    hpSystem.TakeDamage(amount);
                    Debug.Log($"{other.name}���� {amount} ������ :)");
                    break;

                case InteractionType.Heal:
                    hpSystem.Heal(amount);
                    Debug.Log($"{other.name}���� {amount} �� :)");
                    break;
            }

            //if (destroyOnHit)
            //{
            //    Destroy(gameObject);
            //}
        }
    }
}
