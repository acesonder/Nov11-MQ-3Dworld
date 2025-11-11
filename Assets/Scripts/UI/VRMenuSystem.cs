using UnityEngine;
using UnityEngine.UI;

namespace MQ3DVirtualHangout.UI
{
    /// <summary>
    /// VR-optimized menu system for navigating application features
    /// </summary>
    public class VRMenuSystem : MonoBehaviour
    {
        [Header("Menu Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject worldBrowserPanel;
        [SerializeField] private GameObject modelLibraryPanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("Menu Settings")]
        [SerializeField] private float menuDistance = 2f;
        [SerializeField] private bool followPlayer = true;

        private GameObject activePanel;
        private Transform playerCamera;

        private void Start()
        {
            playerCamera = Camera.main.transform;
            ShowMainMenu();
        }

        private void Update()
        {
            if (followPlayer && activePanel != null && playerCamera != null)
            {
                PositionMenuInFrontOfPlayer();
            }
        }

        /// <summary>
        /// Shows the main menu
        /// </summary>
        public void ShowMainMenu()
        {
            HideAllPanels();
            ShowPanel(mainMenuPanel);
        }

        /// <summary>
        /// Shows the world browser
        /// </summary>
        public void ShowWorldBrowser()
        {
            HideAllPanels();
            ShowPanel(worldBrowserPanel);
        }

        /// <summary>
        /// Shows the model library
        /// </summary>
        public void ShowModelLibrary()
        {
            HideAllPanels();
            ShowPanel(modelLibraryPanel);
        }

        /// <summary>
        /// Shows settings panel
        /// </summary>
        public void ShowSettings()
        {
            HideAllPanels();
            ShowPanel(settingsPanel);
        }

        /// <summary>
        /// Hides all menu panels
        /// </summary>
        public void HideAllPanels()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (worldBrowserPanel != null) worldBrowserPanel.SetActive(false);
            if (modelLibraryPanel != null) modelLibraryPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            
            activePanel = null;
        }

        private void ShowPanel(GameObject panel)
        {
            if (panel != null)
            {
                panel.SetActive(true);
                activePanel = panel;
                PositionMenuInFrontOfPlayer();
            }
        }

        private void PositionMenuInFrontOfPlayer()
        {
            if (activePanel != null && playerCamera != null)
            {
                Vector3 position = playerCamera.position + playerCamera.forward * menuDistance;
                position.y = playerCamera.position.y;
                
                activePanel.transform.position = position;
                activePanel.transform.rotation = Quaternion.LookRotation(activePanel.transform.position - playerCamera.position);
            }
        }

        /// <summary>
        /// Toggles menu visibility
        /// </summary>
        public void ToggleMenu()
        {
            if (activePanel != null && activePanel.activeSelf)
            {
                HideAllPanels();
            }
            else
            {
                ShowMainMenu();
            }
        }
    }
}
