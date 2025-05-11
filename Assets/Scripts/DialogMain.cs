using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DialogMain : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject Dialog;
    [SerializeField] private TextMeshProUGUI TextController;
    [SerializeField] private KeyCode Controller = KeyCode.E;

    [Header("Post-Processing")]
    [SerializeField] private Volume volume;
    private ColorAdjustments color;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private Vignette vignette;
    private LensDistortion lensDistortion;

    private string[] Words;
    private float wordSpeed = 0.02f;
    private int index = 0;

    private bool isTyping = false;
    private bool canTalk = false;
    private bool dialogStarted = false; 
    private Coroutine typingCoroutine;

    [SerializeField] private AudioSource TimeSound;

    private void Start()
    {
        Words = new string[]
        {
            "Bu kapý... sýradan bir geçit deðil. Burasý zamanýn akýþýna açýlan bir kapý.",
            "Geçmiþ... henüz yaþanmamýþ gelecekten daha güçlü olabilir.",
            "Þimdi, burada yaptýðým bir seçim, yüzyýllar sonrasýný deðiþtirebilir.",
            "Ya da geçmiþe dönüp, hatalarý düzeltebilirim… belki de daha kötüsünü yaparým.",
            "Her þey birbirine baðlý.",
            "Zamaný bükebilirim, ama bu bir oyun deðil... Her dönüþümün bir bedeli var.",
            "Hazýrsan, zamaný kendin için yeniden yaz. Ama unutma:\nBazý þeyler deðiþtiðinde, bir daha asla eskisi gibi olmayacak."
        };

        Dialog.SetActive(false);
        TextController.gameObject.SetActive(false);

        if (volume.profile != null)
        {
            volume.profile.TryGet(out color);
            volume.profile.TryGet(out chromaticAberration);
            volume.profile.TryGet(out bloom);
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out lensDistortion);


            if (color != null)
            {
                color.postExposure.value = 0f;
                color.saturation.value = 0f;
                color.contrast.value = 0f;
                color.colorFilter.value = Color.white; 
            }


            if (chromaticAberration != null)
                chromaticAberration.intensity.value = 0f;

            if (bloom != null)
                bloom.intensity.value = 0f;

            if (vignette != null)
                vignette.intensity.value = 0f;

            if (lensDistortion != null)
                lensDistortion.intensity.value = 0f;
        }
    }

    private void Update()
    {
        if (canTalk && Input.GetKeyDown(Controller))
        {
            if (!Dialog.activeSelf)
            {
                TextController.gameObject.SetActive(false);
                Dialog.SetActive(true);
                StartDialog();
            }
            else if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                text.text = Words[index];
                isTyping = false;
            }
            else
            {
                NextSentence();
            }
        }
    }

    private void StartDialog()
    {
        if (!dialogStarted) 
        {
            dialogStarted = true; 
            index = 0;
            typingCoroutine = StartCoroutine(TypeText());
        }
    }

    private void NextSentence()
    {
        if (index < Words.Length - 1)
        {
            index++;
            typingCoroutine = StartCoroutine(TypeText());
        }
        else
        {
            Dialog.SetActive(false);
            text.gameObject.SetActive(false);
            StartCoroutine(TimeTransitionEffect());
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        text.text = "";
        foreach (char letter in Words[index])
        {
            text.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gate") && !dialogStarted) 
        {
            canTalk = true;
            TextController.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Gate"))
        {
            canTalk = false;
            TextController.gameObject.SetActive(false);
            Dialog.SetActive(false);
        }
    }

    IEnumerator TimeTransitionEffect()
    {
        float t = 0f;
        float duration = 2f;
       


        while (t < duration)
        {
            TimeSound.Play();
            t += Time.deltaTime;
            float lerp = t / duration;

          
            if (color != null)
            {
                color.postExposure.value = Mathf.Lerp(0f, -1f, lerp);
                color.saturation.value = Mathf.Lerp(0f, -80f, lerp);
                color.contrast.value = Mathf.Lerp(0f, 40f, lerp);
            }

            if (chromaticAberration != null)
                chromaticAberration.intensity.value = Mathf.Lerp(0f, 0.4f, lerp);

            if (bloom != null)
            {
                bloom.intensity.value = Mathf.Lerp(0f, 2f, lerp);
                bloom.threshold.value= Mathf.Lerp(0f, 1.1f, lerp);
            }

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(0f, 0.4f, lerp);

            if (lensDistortion != null)
                lensDistortion.intensity.value = Mathf.Lerp(0f, -1f, lerp);

            yield return null;
        }

       
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Back");
    }
}
