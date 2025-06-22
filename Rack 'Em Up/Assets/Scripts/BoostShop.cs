using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BoostShop : MonoBehaviour
{
    public PlayerBallHandler player;
    public GameObject shopPanel;

    public Button buyX2Button;
    public Button buyMissProtectButton;
    public Button buyX3Button;

    public static bool IsShopActive { get; private set; } = false;

    private bool x2Active = false;
    private bool x3Active = false;
    private bool missProtectActive = false;

    void Start()
    {
        shopPanel.SetActive(false);

        buyX2Button.onClick.AddListener(BuyX2Coins);
        buyMissProtectButton.onClick.AddListener(BuyMissProtection);
        buyX3Button.onClick.AddListener(BuyX3Coins);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (shopPanel.activeSelf)
                CloseShop();
            else
                OpenShop();
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsShopActive = true;
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsShopActive = false;
    }

    void BuyX2Coins()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing in BuyX2Coins!");
            return;
        }

        if (player.coins >= 250 && !x2Active && !x3Active)
        {
            player.coins -= 250;
            x2Active = true;
            player.SetCoinMultiplier(2);
            Debug.Log("2x Coins Activated!");
            StartCoroutine(EndX2After(30f));
            player.UpdateCoinUI();
        }
        else if (x2Active || x3Active)
            Debug.Log("A coin multiplier is already active!");
        else
            Debug.Log("Not enough coins for 2x!");
    }

    void BuyMissProtection()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing in BuyMissProtection!");
            return;
        }

        if (player.coins >= 100 && !missProtectActive)
        {
            player.coins -= 100;
            missProtectActive = true;
            player.SetMissProtection(true);
            Debug.Log("Miss Protection Activated!");
            StartCoroutine(EndMissProtectAfter(10f));
            player.UpdateCoinUI();
        }
        else if (missProtectActive)
            Debug.Log("Miss Protection already active!");
        else
            Debug.Log("Not enough coins for Miss Protection!");
    }

    void BuyX3Coins()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing in BuyX3Coins!");
            return;
        }

        if (player.coins >= 500 && !x2Active && !x3Active)
        {
            player.coins -= 500;
            x3Active = true;
            player.SetCoinMultiplier(3);
            Debug.Log("3x Coins Activated!");
            StartCoroutine(EndX3After(30f));
            player.UpdateCoinUI();
        }
        else if (x2Active || x3Active)
            Debug.Log("A coin multiplier is already active!");
        else
            Debug.Log("Not enough coins for 3x!");
    }

    IEnumerator EndX2After(float time)
    {
        yield return new WaitForSeconds(time);
        player.SetCoinMultiplier(1);
        x2Active = false;
        Debug.Log("2x Coins Ended.");
    }

    IEnumerator EndX3After(float time)
    {
        yield return new WaitForSeconds(time);
        player.SetCoinMultiplier(1);
        x3Active = false;
        Debug.Log("3x Coins Ended.");
    }

    IEnumerator EndMissProtectAfter(float time)
    {
        yield return new WaitForSeconds(time);
        player.SetMissProtection(false);
        missProtectActive = false;
        Debug.Log("Miss Protection Ended.");
    }

    public bool IsShopOpen()
    {
        return shopPanel != null && shopPanel.activeSelf;
    }
}
