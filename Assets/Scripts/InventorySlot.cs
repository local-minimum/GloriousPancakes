using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    Image Image;

    Vector3 origin;

    [SerializeField]
    float shakeTime = 0.5f;

    [SerializeField]
    float shakeMagnitude = 0.005f;

    private void Start()
    {
        origin = transform.localPosition;
        Image.gameObject.SetActive(false);
    }

    IEnumerator<WaitForSeconds> Shake()
    {
        float start = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - start < shakeTime)
        {
            transform.localPosition = origin + new Vector3(Random.Range(-shakeMagnitude, shakeMagnitude), Random.Range(-shakeMagnitude, shakeMagnitude));
            yield return new WaitForSeconds(0.02f);
        }
        transform.localPosition = origin;
    }

    public void Slot(Sprite sprite)
    {
        Image.sprite = sprite;
        Image.gameObject.SetActive(true);
        StartCoroutine(Shake());
    }
}
