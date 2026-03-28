using System.Collections;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("Scene Panels (Only One Active)")] // ADD SCENES HERE
    public GameObject scene1Panel;
    public GameObject scene2Panel;
    public GameObject scene3Panel;
    public GameObject scene4Panel;
    public GameObject scene5Panel;
    public GameObject scene6Panel;
    public GameObject scene7Panel;
    public GameObject scene8Panel;
    public GameObject scene9Panel;
    public GameObject scene10Panel;
    public GameObject scene11Panel;
    public GameObject scene12Panel;
    public GameObject scene13Panel;
    public GameObject scene13_2Panel;
    public GameObject scene14Panel;
    public GameObject scene15Panel;
    public GameObject scene16Panel;
    public GameObject scene17Panel;
    public GameObject scene18Panel;
    public GameObject scene18_2Panel;
    public GameObject scene19Panel;
    public GameObject scene19_2Panel;
    public GameObject scene20Panel;
    public GameObject scene21Panel;
    public GameObject scene21_2Panel;
    public GameObject scene22Panel;
    public GameObject scene22_2Panel;
    public GameObject scene23Panel;
    public GameObject scene24Panel;
    public GameObject scene25Panel;
    public GameObject scene26Panel;
    public GameObject scene27Panel;
    public GameObject scene28Panel;
    public GameObject scene29Panel;
    public GameObject scene30Panel;
    public GameObject scene30_2Panel;
    public GameObject scene31Panel;
    public GameObject scene32Panel;
    public GameObject scene33Panel;
    public GameObject scene33_2Panel;
    public GameObject scene33_3Panel;
    public GameObject scene34Panel;
    public GameObject scene35Panel;
    public GameObject scene36Panel;
    public GameObject scene37Panel;
    public GameObject scene38Panel;
    public GameObject scene39Panel;
    public GameObject scene40Panel;
    public GameObject scene41Panel;
    public GameObject scene42Panel;

    [Header("Overlay Panels (Can Sit On Top)")]
    public GameObject mapPanel;

    [Header("Scene Sounds (Optional)")]
    public AudioClip scene1Sound;
    public AudioClip scene13_2Sound;
    public AudioClip scene17Sound;
    public AudioClip scene18Sound;
    public AudioClip scene18_2Sound;
    public AudioClip scene19_2Sound;
    public AudioClip scene30_2Sound;
    public AudioClip scene33_2Sound;
    public AudioClip scene35Sound;
    public AudioClip scene39Sound;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private GameObject currentPanel;
    private bool isSwitching = false;

    private void Start()
    {
        // Detect which panel is active at start // ADD SCENES HERE
        if (scene1Panel.activeSelf) currentPanel = scene1Panel;
        else if (scene2Panel.activeSelf) currentPanel = scene2Panel;
        else if (scene3Panel.activeSelf) currentPanel = scene3Panel;
        else if (scene4Panel.activeSelf) currentPanel = scene4Panel;
        else if (scene5Panel.activeSelf) currentPanel = scene5Panel;
        else if (scene6Panel.activeSelf) currentPanel = scene6Panel;
        else if (scene7Panel.activeSelf) currentPanel = scene7Panel;
        else if (scene8Panel.activeSelf) currentPanel = scene8Panel;
        else if (scene9Panel.activeSelf) currentPanel = scene9Panel;
        else if (scene10Panel.activeSelf) currentPanel = scene10Panel;
        else if (scene11Panel.activeSelf) currentPanel = scene11Panel;
        else if (scene12Panel.activeSelf) currentPanel = scene12Panel;
        else if (scene13Panel.activeSelf) currentPanel = scene13Panel;
        else if (scene13_2Panel.activeSelf) currentPanel = scene13_2Panel;
        else if (scene14Panel.activeSelf) currentPanel = scene14Panel;
        else if (scene15Panel.activeSelf) currentPanel = scene15Panel;
        else if (scene16Panel.activeSelf) currentPanel = scene16Panel;
        else if (scene17Panel.activeSelf) currentPanel = scene17Panel;
        else if (scene18Panel.activeSelf) currentPanel = scene18Panel;
        else if (scene18_2Panel.activeSelf) currentPanel = scene18_2Panel;
        else if (scene19Panel.activeSelf) currentPanel = scene19Panel;
        else if (scene19_2Panel.activeSelf) currentPanel = scene19_2Panel;
        else if (scene20Panel.activeSelf) currentPanel = scene20Panel;
        else if (scene21Panel.activeSelf) currentPanel = scene21Panel;
        else if (scene21_2Panel.activeSelf) currentPanel = scene21_2Panel;
        else if (scene22Panel.activeSelf) currentPanel = scene22Panel;
        else if (scene22_2Panel.activeSelf) currentPanel = scene22_2Panel;
        else if (scene23Panel.activeSelf) currentPanel = scene23Panel;
        else if (scene24Panel.activeSelf) currentPanel = scene24Panel;
        else if (scene25Panel.activeSelf) currentPanel = scene25Panel;
        else if (scene26Panel.activeSelf) currentPanel = scene26Panel;
        else if (scene27Panel.activeSelf) currentPanel = scene27Panel;
        else if (scene28Panel.activeSelf) currentPanel = scene28Panel;
        else if (scene29Panel.activeSelf) currentPanel = scene29Panel;
        else if (scene30Panel.activeSelf) currentPanel = scene30Panel;
        else if (scene30_2Panel.activeSelf) currentPanel = scene30_2Panel;
        else if (scene31Panel.activeSelf) currentPanel = scene31Panel;
        else if (scene32Panel.activeSelf) currentPanel = scene32Panel;
        else if (scene33Panel.activeSelf) currentPanel = scene33Panel;
        else if (scene33_2Panel.activeSelf) currentPanel = scene33_2Panel;
        else if (scene33_3Panel.activeSelf) currentPanel = scene33_3Panel;
        else if (scene34Panel.activeSelf) currentPanel = scene34Panel;
        else if (scene35Panel.activeSelf) currentPanel = scene35Panel;
        else if (scene36Panel.activeSelf) currentPanel = scene36Panel;
        else if (scene37Panel.activeSelf) currentPanel = scene37Panel;
        else if (scene38Panel.activeSelf) currentPanel = scene38Panel;
        else if (scene39Panel.activeSelf) currentPanel = scene39Panel;
        else if (scene40Panel.activeSelf) currentPanel = scene40Panel;
        else if (scene41Panel.activeSelf) currentPanel = scene41Panel;
        else if (scene42Panel.activeSelf) currentPanel = scene42Panel;

        else currentPanel = scene1Panel; // fallback

        ShowOnly(currentPanel);

        if (mapPanel != null)
            mapPanel.SetActive(true);

        // Play sound for starting panel if applicable
        PlaySceneSound(currentPanel);
    }

    #region Scene Switching

    public void GoToScene2() => Switch(scene2Panel);
    public void GoToScene3() => Switch(scene3Panel);
    public void GoToScene4() => Switch(scene4Panel);
    public void GoToScene5() => Switch(scene5Panel);
    public void GoToScene6() => Switch(scene6Panel);
    public void GoToScene7() => Switch(scene7Panel);
    public void GoToScene8() => Switch(scene8Panel);
    public void GoToScene9() => Switch(scene9Panel);
    public void GoToScene10() => Switch(scene10Panel);
    public void GoToScene11() => Switch(scene11Panel);
    public void GoToScene12() => Switch(scene12Panel);
    public void GoToScene13() => Switch(scene13Panel);
    public void GoToScene13_2() => Switch(scene13_2Panel);
    public void GoToScene14() => Switch(scene14Panel);
    public void GoToScene15() => Switch(scene15Panel);
    public void GoToScene16() => Switch(scene16Panel);
    public void GoToScene17() => Switch(scene17Panel);
    public void GoToScene18() => Switch(scene18Panel);
    public void GoToScene18_2() => Switch(scene18_2Panel);
    public void GoToScene19() => Switch(scene19Panel);
    public void GoToScene19_2() => Switch(scene19_2Panel);
    public void GoToScene20() => Switch(scene20Panel);
    public void GoToScene21() => Switch(scene21Panel);
    public void GoToScene21_2() => Switch(scene21_2Panel);
    public void GoToScene22() => Switch(scene22Panel);
    public void GoToScene22_2() => Switch(scene22_2Panel);
    public void GoToScene23() => Switch(scene23Panel);
    public void GoToScene24() => Switch(scene24Panel);
    public void GoToScene25() => Switch(scene25Panel);
    public void GoToScene26() => Switch(scene26Panel);
    public void GoToScene27() => Switch(scene27Panel);
    public void GoToScene28() => Switch(scene28Panel);
    public void GoToScene29() => Switch(scene29Panel);
    public void GoToScene30() => Switch(scene30Panel);
    public void GoToScene30_2() => Switch(scene30_2Panel);
    public void GoToScene31() => Switch(scene31Panel);
    public void GoToScene32() => Switch(scene32Panel);
    public void GoToScene33() => Switch(scene33Panel);
    public void GoToScene33_2() => Switch(scene33_2Panel);
    public void GoToScene33_3() => Switch(scene33_3Panel);
    public void GoToScene34() => Switch(scene34Panel);
    public void GoToScene35() => Switch(scene35Panel);
    public void GoToScene36() => Switch(scene36Panel);
    public void GoToScene37() => Switch(scene37Panel);
    public void GoToScene38() => Switch(scene38Panel);
    public void GoToScene39() => Switch(scene39Panel);
    public void GoToScene40() => Switch(scene40Panel);
    public void GoToScene41() => Switch(scene41Panel);
    public void GoToScene42() => Switch(scene42Panel);

    void Switch(GameObject nextPanel)
    {
        if (!isSwitching && nextPanel != null)
            StartCoroutine(SwitchPanel(nextPanel));
    }

    IEnumerator SwitchPanel(GameObject nextPanel)
    {
        isSwitching = true;

        if (currentPanel != null)
            yield return StartCoroutine(FadeOut(currentPanel));

        currentPanel.SetActive(false);

        nextPanel.SetActive(true);

        // Play sound for this panel
        PlaySceneSound(nextPanel);

        yield return StartCoroutine(FadeIn(nextPanel));

        currentPanel = nextPanel;
        isSwitching = false;
    }

    #endregion

    void PlaySceneSound(GameObject panel)
    {
        if (SoundManager.Instance == null || !SoundManager.Instance.soundsEnabled) return;

        if (panel == scene1Panel)
            SoundManager.Instance.PlaySound(scene1Sound);
        else if (panel == scene13_2Panel)
            SoundManager.Instance.PlaySound(scene13_2Sound);
        else if (panel == scene17Panel)
            SoundManager.Instance.PlaySound(scene17Sound);
        else if (panel == scene18Panel)
            SoundManager.Instance.PlaySound(scene18Sound);
        else if (panel == scene18_2Panel)
            SoundManager.Instance.PlaySound(scene18_2Sound);
        else if (panel == scene19_2Panel)
            SoundManager.Instance.PlaySound(scene19_2Sound);
        else if (panel == scene30_2Panel)
            SoundManager.Instance.PlaySound(scene30_2Sound);
        else if (panel == scene33_2Panel)
            SoundManager.Instance.PlaySound(scene33_2Sound);
        else if (panel == scene35Panel)
            SoundManager.Instance.PlaySound(scene35Sound);
        else if (panel == scene39Panel)
            SoundManager.Instance.PlaySound(scene39Sound);
    }

    void ShowOnly(GameObject panelToShow)
    {
        scene1Panel.SetActive(false);
        scene2Panel.SetActive(false);
        scene3Panel.SetActive(false);
        scene4Panel.SetActive(false);
        scene5Panel.SetActive(false);
        scene6Panel.SetActive(false);
        scene7Panel.SetActive(false);
        scene8Panel.SetActive(false);
        scene9Panel.SetActive(false);
        scene10Panel.SetActive(false);
        scene11Panel.SetActive(false);
        scene12Panel.SetActive(false);
        scene13Panel.SetActive(false);
        scene13_2Panel.SetActive(false);
        scene14Panel.SetActive(false);
        scene15Panel.SetActive(false);
        scene16Panel.SetActive(false);
        scene17Panel.SetActive(false);
        scene18Panel.SetActive(false);
        scene18_2Panel.SetActive(false);
        scene19Panel.SetActive(false);
        scene19_2Panel.SetActive(false);
        scene20Panel.SetActive(false);
        scene21Panel.SetActive(false);
        scene21_2Panel.SetActive(false);
        scene22Panel.SetActive(false);
        scene22_2Panel.SetActive(false);
        scene23Panel.SetActive(false);
        scene24Panel.SetActive(false);
        scene25Panel.SetActive(false);
        scene26Panel.SetActive(false);
        scene27Panel.SetActive(false);
        scene28Panel.SetActive(false);
        scene29Panel.SetActive(false);
        scene30Panel.SetActive(false);
        scene30_2Panel.SetActive(false);
        scene31Panel.SetActive(false);
        scene32Panel.SetActive(false);
        scene33Panel.SetActive(false);
        scene33_2Panel.SetActive(false);
        scene33_3Panel.SetActive(false);
        scene34Panel.SetActive(false);
        scene35Panel.SetActive(false);
        scene36Panel.SetActive(false);
        scene37Panel.SetActive(false);
        scene38Panel.SetActive(false);
        scene39Panel.SetActive(false);
        scene40Panel.SetActive(false);
        scene41Panel.SetActive(false);
        scene42Panel.SetActive(false);

        panelToShow.SetActive(true);
    }

    #region Map Overlay

    public void ShowMap()
    {
        if (mapPanel != null)
            mapPanel.SetActive(true);
    }

    public void HideMap()
    {
        if (mapPanel != null)
            mapPanel.SetActive(false);
    }

    #endregion

    #region Fade Helpers

    IEnumerator FadeOut(GameObject panel)
    {
        CanvasGroup group = panel.GetComponent<CanvasGroup>();
        if (group == null)
            group = panel.AddComponent<CanvasGroup>();

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        group.alpha = 0f;
    }

    IEnumerator FadeIn(GameObject panel)
    {
        CanvasGroup group = panel.GetComponent<CanvasGroup>();
        if (group == null)
            group = panel.AddComponent<CanvasGroup>();

        group.alpha = 0f;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            group.alpha = t / fadeDuration;
            yield return null;
        }

        group.alpha = 1f;
    }

    #endregion
}