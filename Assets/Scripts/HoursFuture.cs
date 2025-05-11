using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class HoursFuture : MonoBehaviour
{
    [Header("Post-Processing")]
    [SerializeField] private Volume volume;
    [SerializeField] private Volume VolumeEnv;
    private ColorAdjustments color;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private Vignette vignette;
    private LensDistortion lensDistortion;


    private void Start()
    {
        volume.enabled = false;
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


    public void Effect()
    {
        StartCoroutine(TimeTransitionEffect());
    }


    IEnumerator TimeTransitionEffect()
    {
        float t = 0f;
        float duration = 2f;
        VolumeEnv.enabled = false;
        volume.enabled = true;
     
        while (t < duration)
        {
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
                bloom.threshold.value = Mathf.Lerp(0f, 1.1f, lerp);
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
