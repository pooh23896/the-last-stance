using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PixelArtTowerDefenseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsText; // Pixel font voor credits
    [SerializeField] private Image baseHealthBar; // Pixel art gezondheidsbalk
    [SerializeField] private Button buyTowerButton; // Pixel art knop
    [SerializeField] private Button upgradeTowerButton; // Pixel art knop
    [SerializeField] private Button orbitalCannonButton; // Pixel art knop
    [SerializeField] private TextMeshProUGUI cannonCooldownText; // Pixel font voor cooldown
    [SerializeField] private Image towerPreview; // Pixel art tower preview sprite

    [SerializeField] private Sprite buttonNormalSprite; // Normale knop sprite
    [SerializeField] private Sprite buttonHoverSprite; // Hover/active knop sprite

    private int credits = 100; // Startcredits
    private float baseHealth = 100f; // Startgezondheid (0-100)
    private float cannonCooldown = 0f; // Cooldown-timer
    private const float CANNON_MAX_COOLDOWN = 8f; // 8 seconden cooldown
    private bool isPlacingTower = false; // Of de speler een tower plaatst

    void Start()
    {
        /* // Start van commentaar
        // Alleen initialiseren als referenties bestaan
        if (buyTowerButton != null) buyTowerButton.onClick.AddListener(OnBuyTowerClicked);
        if (upgradeTowerButton != null) upgradeTowerButton.onClick.AddListener(OnUpgradeTowerClicked);
        if (orbitalCannonButton != null) orbitalCannonButton.onClick.AddListener(OnOrbitalCannonClicked);

        // Voeg hover-effect toe als knoppen bestaan
        if (buyTowerButton != null) AddButtonHoverEffect(buyTowerButton);
        if (upgradeTowerButton != null) AddButtonHoverEffect(upgradeTowerButton);
        if (orbitalCannonButton != null) AddButtonHoverEffect(orbitalCannonButton);

        // Initialiseer UI (met null-checks)
        UpdateCreditsText();
        UpdateBaseHealthBar();
        UpdateCannonCooldownText();
        if (towerPreview != null) towerPreview.gameObject.SetActive(false); // Verberg preview
        */ // Einde van commentaar
    }

    void Update()
    {
        // Update alleen als nodig
        if (cannonCooldown > 0)
        {
            cannonCooldown -= Time.deltaTime;
            UpdateCannonCooldownText();
            if (orbitalCannonButton != null) orbitalCannonButton.interactable = false;
        }
        else
        {
            if (orbitalCannonButton != null) orbitalCannonButton.interactable = true;
        }

        // Towerplaatsing alleen als preview bestaat
        if (isPlacingTower && towerPreview != null)
        {
            UpdateTowerPreview();
        }
    }

    private void OnBuyTowerClicked()
    {
        int towerCost = 40; // Kosten van een tower
        if (credits >= towerCost)
        {
            credits -= towerCost;
            UpdateCreditsText();
            isPlacingTower = true;
            if (towerPreview != null) towerPreview.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Niet genoeg credits!");
        }
    }

    private void OnUpgradeTowerClicked()
    {
        int upgradeCost = 25; // Kosten van upgrade
        if (credits >= upgradeCost)
        {
            credits -= upgradeCost;
            UpdateCreditsText();
            Debug.Log("Tower geüpgraded!");
        }
        else
        {
            Debug.Log("Niet genoeg credits voor upgrade!");
        }
    }

    private void OnOrbitalCannonClicked()
    {
        if (cannonCooldown <= 0)
        {
            cannonCooldown = CANNON_MAX_COOLDOWN;
            Debug.Log("Orbital Cannon geactiveerd!");
        }
    }

    private void UpdateTowerPreview()
    {
        if (towerPreview == null) return;

        // Volg muispositie (pixel-perfect)
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Rond af naar dichtstbijzijnde pixel
        mousePos.x = Mathf.Round(mousePos.x);
        mousePos.y = Mathf.Round(mousePos.y);
        towerPreview.transform.position = mousePos;

        // Plaats tower bij klik
        if (Input.GetMouseButtonDown(0))
        {
            isPlacingTower = false;
            towerPreview.gameObject.SetActive(false);
            Debug.Log("Tower geplaatst op: " + mousePos);
        }
    }

    private void UpdateCreditsText()
    {
        if (creditsText != null) creditsText.text = $"{credits}";
    }

    private void UpdateBaseHealthBar()
    {
        if (baseHealthBar != null) baseHealthBar.fillAmount = baseHealth / 100f;
    }

    private void UpdateCannonCooldownText()
    {
        if (cannonCooldownText != null)
        {
            cannonCooldownText.text = cannonCooldown > 0 ? $"{Mathf.Ceil(cannonCooldown)}" : "";
        }
    }

    private void AddButtonHoverEffect(Button button)
    {
        if (button == null || buttonNormalSprite == null || buttonHoverSprite == null) return;

        // Voeg EventTrigger toe voor hover
        //EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        //Image buttonImage = button.GetComponent<Image>();

        // Pointer Enter
        //EventTrigger.Entry pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        //pointerEnter.callback.AddListener((data) => { if (buttonImage != null) buttonImage.sprite = buttonHoverSprite; });
        //trigger.triggers.Add(pointerEnter);

        // Pointer Exit
        //EventTrigger.Entry pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        //pointerExit.callback.AddListener((data) => { if (buttonImage != null) buttonImage.sprite = buttonNormalSprite; });
        //trigger.triggers.Add(pointerExit);
    }

    public void TakeBaseDamage(float damage)
    {
        baseHealth -= damage;
        if (baseHealth < 0) baseHealth = 0;
        UpdateBaseHealthBar();
        if (baseHealth <= 0)
        {
            Debug.Log("Game Over!");
        }
    }
}