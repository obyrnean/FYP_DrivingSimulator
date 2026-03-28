using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject languagePanel;
    public GameObject rulesPanel;
    public GameObject locationPanel;
    public GameObject vehiclePanel;
    public GameObject introductionPanel;
    public GameObject feedbackPanel;
    public GameObject playPanel;
    public GameObject inspectionPanel;

    // Call this from buttons
    public void ShowPanel(GameObject panelToShow)
    {
        // Hide all panels first
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        languagePanel.SetActive(false);
        rulesPanel.SetActive(false);
        locationPanel.SetActive(false);
        vehiclePanel.SetActive(false);
        introductionPanel.SetActive(false);
        feedbackPanel.SetActive(false);
        playPanel.SetActive(false);
        inspectionPanel.SetActive(false);

        // Show the selected panel
        panelToShow.SetActive(true);
    }
}

