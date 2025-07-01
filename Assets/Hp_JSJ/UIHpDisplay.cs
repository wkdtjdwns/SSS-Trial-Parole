using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHpDisplay : MonoBehaviour
{
    [Header("UI Info")]
    [SerializeField] private GameObject hpUIPrefab;

    [Header("Display Info")]
    [SerializeField] private float displayDuration = 1.0f;
    private Coroutine hideUICoroutine;

    private Dictionary<HpSystem, GameObject> activeHpUIs = new Dictionary<HpSystem, GameObject>();
    private Dictionary<HpSystem, Coroutine> activeHideCoroutines = new Dictionary<HpSystem, Coroutine>();

    public static UIHpDisplay Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this.gameObject); }
        else { Instance = this; }

        if (hpUIPrefab == null)
        {
            Debug.LogError("UIHpDisplay: Hp UI Prefab ¿¬°á X :(", this);
            this.enabled = false;
        }
    }

    public void RegisterAndShowHpUI(HpSystem targetHpSystem)
    {
        if (targetHpSystem == null || hpUIPrefab == null) return;

        GameObject curUIInstance = null;

        if (activeHpUIs.ContainsKey(targetHpSystem))
        {
            curUIInstance = activeHpUIs[targetHpSystem];
        }

        else
        {
            curUIInstance = Instantiate(hpUIPrefab);
            activeHpUIs.Add(targetHpSystem, curUIInstance);

            curUIInstance.transform.SetParent(targetHpSystem.transform);

            curUIInstance.transform.localPosition = new Vector3(0, targetHpSystem.GetComponent<Collider>()?.bounds.size.y ?? 1f, 0);
            curUIInstance.transform.localScale = Vector3.one * 0.01f;
            curUIInstance.name = $"{targetHpSystem.name}_HP_UI";

            targetHpSystem.OnHpChanged += (cur, max) => UpdateHpUI(curUIInstance, cur, max);
        }

        UpdateHpUI(curUIInstance, targetHpSystem.curHp, targetHpSystem.maxHp);
        curUIInstance.SetActive(true);

        if (activeHideCoroutines.ContainsKey(targetHpSystem) && activeHideCoroutines[targetHpSystem] != null)
        {
            StopCoroutine(activeHideCoroutines[targetHpSystem]);
        }

        Coroutine newCoroutine = StartCoroutine(HideUIAfterDelay(curUIInstance, targetHpSystem, displayDuration));
        activeHideCoroutines[targetHpSystem] = newCoroutine;
    }

    private void UpdateHpUI(GameObject uiInstance, float curHp, float maxHp)
    {
        Slider hpSlider = uiInstance.GetComponentInChildren<Slider>();
        TextMeshProUGUI hpText = uiInstance.GetComponentInChildren<TextMeshProUGUI>();

        if (hpSlider != null)
        {
            hpSlider.value = curHp / maxHp;
        }

        if (hpText != null)
        {
            hpText.text = $"HP: {Mathf.CeilToInt(curHp)} / {Mathf.CeilToInt(maxHp)}";
        }
    }

    private IEnumerator HideUIAfterDelay(GameObject uiInstance, HpSystem targetHpSystem, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (uiInstance != null)
        {
            uiInstance.SetActive(false);
        }

        if (activeHideCoroutines.ContainsKey(targetHpSystem))
        {
            activeHideCoroutines[targetHpSystem] = null;
        }
    }

    private void OnDestroy()
    {
        foreach (var entry in activeHpUIs)
        {
            if (entry.Value != null) 
            {
                Destroy(entry.Value);
            }
        }

        activeHpUIs.Clear();
        activeHideCoroutines.Clear();
    }
}
