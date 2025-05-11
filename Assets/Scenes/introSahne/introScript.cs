using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class introScript : MonoBehaviour
{
    public Image logo;
    public TextMeshProUGUI introMetin;
    public float beklemeSuresi = 3.5f;
    void Start()
    {
        StartCoroutine(introAnim());
    }
    IEnumerator introAnim()
    {
        yield return soldurmaVeBuyutme(logo, 0, 1, beklemeSuresi, 7);
        yield return soldurmaVeBuyutme(introMetin, 0, 1, beklemeSuresi, 3);
        yield return soldurmaVeBuyutme(introMetin, 1, 0, beklemeSuresi, 3);
        yield return soldurmaVeBuyutme(logo, 1, 0, beklemeSuresi, 4);
        SceneManager.LoadScene(1);
    }
    IEnumerator soldurmaVeBuyutme(Graphic element, float baslangicAlfa, float bitisAlfa, float gerceklesmeSuresi, float hedefBuyukluk)
    {
        float zaman = 0f;
        Transform obj = element.gameObject.transform;
        Color color = element.color;
        Vector3 originalBuyukluk = obj.localScale;
        while (zaman < gerceklesmeSuresi)
        {
            zaman += Time.deltaTime;
            obj.localScale = Vector3.Lerp(originalBuyukluk, Vector3.one * hedefBuyukluk, zaman / gerceklesmeSuresi);
            color.a = Mathf.Lerp(baslangicAlfa, bitisAlfa, zaman / gerceklesmeSuresi);
            element.color = color;
            yield return null;
        }
        color.a = bitisAlfa;
        element.color = color;
    }
}