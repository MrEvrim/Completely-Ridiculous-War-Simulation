using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Prefab dizileri
    public GameObject[] bluePrefabs; // Mavi takım için prefab'lar
    public GameObject[] redPrefabs;  // Kırmızı takım için prefab'lar

    // Butonlar
    public Button bluePrefab1Button;
    public Button bluePrefab2Button;
    public Button bluePrefab3Button;
    public Button redPrefab1Button;
    public Button redPrefab2Button;
    public Button redPrefab3Button;
    public Button startBattleButton;

    // UI TextMeshPro
    public TextMeshProUGUI redCoinText;
    public TextMeshProUGUI blueCoinText;

    private GameObject selectedPrefab;
    private bool isBattleStarted = false;

    public int redCoin = 500, blueCoin = 500;

    void Start()
    {
        // Butonlara işlevler ekleyin
        bluePrefab1Button.onClick.AddListener(() => SelectPrefab(bluePrefabs[0]));
        bluePrefab2Button.onClick.AddListener(() => SelectPrefab(bluePrefabs[1]));
        bluePrefab3Button.onClick.AddListener(() => SelectPrefab(bluePrefabs[2]));
        redPrefab1Button.onClick.AddListener(() => SelectPrefab(redPrefabs[0]));
        redPrefab2Button.onClick.AddListener(() => SelectPrefab(redPrefabs[1]));
        redPrefab3Button.onClick.AddListener(() => SelectPrefab(redPrefabs[2]));
        startBattleButton.onClick.AddListener(StartBattle);

        // Varsayılan olarak mavi prefab'ı seçili
        selectedPrefab = bluePrefabs[0];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && !isBattleStarted) // Sol tıklama
        {
            if (selectedPrefab != null)
            {
                if ((selectedPrefab == bluePrefabs[0] && blueCoin >= 50))
                {
                    PlacePrefab();
                    blueCoin -= 50;
                }
                else if ((selectedPrefab == bluePrefabs[1] && blueCoin >= 100))
                {
                    PlacePrefab();
                    blueCoin -= 100;
                }
                else if ((selectedPrefab == bluePrefabs[2] && blueCoin >= 75))
                {
                    PlacePrefab();
                    blueCoin -= 75;
                }
                else if ((selectedPrefab == redPrefabs[0]  && blueCoin >= 50))
                {
                    PlacePrefab();
                    redCoin -= 50;
                }
                else if ((selectedPrefab == redPrefabs[1] && blueCoin >= 100))
                {
                    PlacePrefab();
                    redCoin -= 100;
                }
                else if ((selectedPrefab == redPrefabs[2] && blueCoin >= 75))
                {
                    PlacePrefab();
                    redCoin -= 75;
                }

            }
        }

        redCoinText.text = $"Red Coins: {redCoin}";
        blueCoinText.text = $"Blue Coins: {blueCoin}";
    }

    void StartBattle()
    {
        isBattleStarted = true;
        Debug.Log("Savaş başladı!");
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.SetBattleMode(true);
        }
        foreach (UnitRanger ranger in FindObjectsOfType<UnitRanger>())
        {
            ranger.SetBattleMode(true);
        }

    }

    void PlacePrefab()
    {
        Debug.Log("PlacePrefab çağrıldı!"); // Test amacıyla ekleyin

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 15f; // Kamera ile uzaklık
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (selectedPrefab != null)
        {
            Instantiate(selectedPrefab, new Vector3(worldPosition.x, worldPosition.y, worldPosition.z), Quaternion.identity);
            Debug.Log("Prefab instantiate edildi!");
        }
        else
        {
            Debug.Log("Seçilen prefab null!");
        }
    }

    void SelectPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
        Debug.Log($"{prefab.name} prefab seçildi");
    }
}
