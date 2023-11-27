using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Image parryImage;
    [SerializeField] private Image flaskImage;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;

    private float parryCooldown;
    private float flaskCooldown;
    private float dashCooldown;
    private float crystalCooldown;
    private float swordCooldown;
    private float blackholeCooldown;

    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private float currencyAmount;
    [SerializeField] private float increaseRate = 100;

    private Player player;
    private SkillManager skills;

    private void Start()
    {
        player = PlayerManager.Instance.Player;
        skills = SkillManager.Instance;

        if (player.Stats != null)
            player.Stats.OnHealthChange += Stats_OnHealthChange;

        parryCooldown = skills.ParrySkillController.GetCooldown();
        dashCooldown = skills.DashSkillController.GetCooldown();
        crystalCooldown = skills.CrystalSkillController.GetCooldown();
        swordCooldown = skills.SwordSkillController.GetCooldown();
        blackholeCooldown = skills.BlackholeSkillController.GetCooldown();

        UpdateHealthUI();

        currencyAmount = PlayerManager.Instance.GetCurrency();
        SetCurrencyText();
    }

    private void Update()
    {
        CheckCooldown(parryImage, parryCooldown);
        CheckCooldown(dashImage, dashCooldown);
        CheckCooldown(crystalImage, crystalCooldown);
        CheckCooldown(swordImage, swordCooldown);
        CheckCooldown(blackholeImage, blackholeCooldown);
        CheckCooldown(flaskImage, flaskCooldown);

        CheckCurrency();
    }

    private void CheckCooldown(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
            image.fillAmount -= 1 / cooldown * Time.deltaTime;

    }


    private void SetCooldown(Image image)
    {
        image.fillAmount = 1;
    }
    private void CheckCurrency()
    {
        if (currencyAmount < PlayerManager.Instance.GetCurrency())
            currencyAmount += Time.deltaTime * increaseRate;
        else
            currencyAmount = PlayerManager.Instance.GetCurrency();

        SetCurrencyText();
    }

    public void SetCurrencyText()
    {
        if (currencyAmount > 0)
            currencyText.text = ((int)currencyAmount).ToString("#,#", CultureInfo.InvariantCulture);
        else
            currencyText.text = "0";
    }

    public void SetParryCooldown() => SetCooldown(parryImage);
    public void SetDashCooldown() => SetCooldown(dashImage);
    public void SetCrystalCooldown() => SetCooldown(crystalImage);
    public void SetSwordCooldown() => SetCooldown(swordImage);
    public void SetBlackholeCooldown() => SetCooldown(blackholeImage);

    public void SetFlaskCooldown(float cooldown)
    {
        flaskCooldown = cooldown;
        SetCooldown(flaskImage);
    }
    

    private void Stats_OnHealthChange(object sender, System.EventArgs e)
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthSlider.maxValue = player.Stats.GetMaxHealthValue();
        healthSlider.value = player.Stats.currentHealth;
    }
}
