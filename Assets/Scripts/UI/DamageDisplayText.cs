using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageDisplayText : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject damageText;
    public static DamageDisplayText instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than 1 instance of DamageDisplayText exists!");
            enabled = false;
        }
        else instance = this;
    }
    private void Start()
    {
        damageText = Resources.Load("DamageDisplayText") as GameObject;
    }

    public void DisplayDmgText(float dmg, Transform pos, bool player = false, bool corruptDmg = false)
    {
        StartCoroutine(DamageDisplay(dmg, pos, player, corruptDmg));
    }
    public IEnumerator DamageDisplay(float dmgAmount, Transform pos , bool player = false, bool corruptDmg = false)
    {
        var dmgDisplayText = Instantiate(damageText, pos.position, Quaternion.identity, GameObject.Find("Canvas").transform);

        var text = dmgDisplayText.GetComponent<TMP_Text>();

        text.text = "-" + dmgAmount.ToString("F1");
        if (corruptDmg) { text.color = new Color(0.5f, 0.1f, 0.8f, 1); }
        else if (player) { text.color = new Color(0.8f, 0.1f, 0.1f, 1); }
        else { text.color = new Color(0.0f, 0.0f, 0.0f, 1); }

        float elapsedTime = 0;
        Vector3 startingPos = pos.position;
        Vector3 finalPos = pos.position + (pos.up * 2);
        while (elapsedTime < 1f)
        {
            dmgDisplayText.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / 1f));
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        Destroy(dmgDisplayText.gameObject);

    }
}
