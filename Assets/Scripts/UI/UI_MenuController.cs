using UnityEngine;
using UnityEngine.UI;

public class UI_MenuController : MonoBehaviour
{
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject skillTreePanel;
    [SerializeField] private GameObject craftPanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private GameObject characterBtn;
    [SerializeField] private GameObject skillTreeBtn;
    [SerializeField] private GameObject craftBtn;
    [SerializeField] private GameObject optionsBtn;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject panelsContainer;
    [SerializeField] private GameObject buttonsContainer;

    [SerializeField] private Color colorBtnActive;

    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_MaterialTooltip materialTooltip;
    public UI_SkillTooltip skillTooltip;

    public UI_CraftingWindow craftingWindow;

    private void Start()
    {
        CloseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchPanel(characterPanel);
            SwitchActiveButton(characterBtn);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchPanel(craftPanel);
            SwitchActiveButton(craftBtn);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchPanel(skillTreePanel);
            SwitchActiveButton(skillTreeBtn);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchPanel(optionsPanel);
            SwitchActiveButton(optionsBtn);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    public void SwitchPanel(GameObject panel)
    {
        AudioManager.Instance.PlaySFX(7);

        menu.gameObject.SetActive(true);

        for (int i = 0; i < panelsContainer.transform.childCount; i++)
        {
            panelsContainer.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void SwitchActiveButton(GameObject btn)
    {
        for (int i = 0; i < buttonsContainer.transform.childCount; ++i)
        {
            buttonsContainer.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }

        btn.GetComponent<Image>().color = colorBtnActive;
    }

    public void CloseMenu()
    {
        if (menu.activeSelf)
        {
            AudioManager.Instance.PlaySFX(7);
            menu.gameObject.SetActive(false);
        }
    }
}
