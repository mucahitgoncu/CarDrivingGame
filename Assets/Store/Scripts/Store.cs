using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.ComponentModel;
#if UNITY_EDITOR
using UnityEditor;
#endif

#region Store
public class Store : MonoBehaviour
{
    #region Variables
    //////////////////
    public GameObject player;
    [Header("Button")]
    Sprite coinsImage;
    [HideInInspector]
    public Image[] buttonBackground;
    [SerializeField]
    Color unlockColor = Color.white,
          lockColor = Color.grey;
    public Button[] buttons;
    [HideInInspector]
    public Text[] buttonsTexts;
    [HideInInspector]
    public Image[] buttonsImage;
    //////////////////
    [Header("Scenes References")]
    Text coinsText;
    Toggle randomToggle;
    [HideInInspector]
    public GameObject container;
    Canvas store;
    //Values
    int coins,
        index,
        isRandom = 1,
        startedButtonIndex = 0,
        buttonsPerPage = 10;
    [HideInInspector]
    public int[] prices;
    string have;
    public string[] data = { "Coins", "PlayerIndex", "isRandom", "Items" };
    //////////////////
    [Header("Items")]
    public GameObject[] items3D; // 3D items array

    bool isMove = false;
    Int32 currentIndex()
    {
        return index % 10;
    }
    int GetPageIndex(int x)
    {
        if (x < 11) return 0;
        while (x >= 10) { x /= 10; }
        int d = Convert.ToInt32(string.Format("{0}{1}", x, 0));
        return d;
    }
    [Header("ScriptSettings")]
    [HideInInspector]
    public bool hide = true;
    //Instance

    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Initialize Store
    private void Awake()
    {
        GetRefer();
        CheckSave();
        Initialize();
    }
    void GetRefer()
    {
        store = GetComponentInChildren<Canvas>();
        coinsText = GameObject.FindGameObjectWithTag("coinsText").GetComponent<Text>();
        randomToggle = GameObject.FindGameObjectWithTag("RandomPlayerToggle").GetComponent<Toggle>();
        container = GameObject.FindGameObjectWithTag("storeContainer");
        coinsImage = buttonsImage[0].sprite;
    }
    #region Coroutines
    public IEnumerator WaitPlayer()
    {
        yield return new WaitUntil(() => player != null);
        SettingPlayer();
        SetPlayer(index);
        RandomizePlayer();
    }
    #endregion
    void CheckSave()
    {
        index = PlayerPrefs.GetInt(data[1]);
        coins = PlayerPrefs.GetInt(data[0]);
        coinsText.text = coins.ToString();
        if (PlayerPrefs.HasKey(data[3]))
        {
            have = PlayerPrefs.GetString(data[3]);
        }
        else
        {
            have = items3D[0].name;
            PlayerPrefs.SetString(data[3], have);
        }
    }
    void Initialize()
    {
        SetToggle();
        StartCoroutine(WaitPlayer());
        SetLevelButtons(0);
    }
    public void OnOpenLevelMenu()
    {
        int x = GetPageIndex(index);
        if (x == currentIndex()) return;
        SetLevelButtons(x);
    }
    void SetLevelButtons(int current)
    {
        startedButtonIndex = current;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (startedButtonIndex + i >= items3D.Length) { buttons[i].gameObject.SetActive(false); }
            else if (!buttons[i].gameObject.activeSelf) { buttons[i].gameObject.SetActive(true); }
            int cIndex = i + startedButtonIndex;
            if (cIndex < items3D.Length)
            {
                if (!buttons[i].gameObject.activeSelf) buttons[i].gameObject.SetActive(true);
                if (have.Contains(items3D[cIndex].name))
                {
                    buttonBackground[i].color = unlockColor;
                    buttonsImage[i].sprite = coinsImage; // Use an appropriate icon for 3D items
                    buttonsTexts[i].text = null;
                }
                else
                {
                    buttonBackground[i].color = lockColor;
                    buttonsImage[i].sprite = coinsImage;
                    buttonsTexts[i].text = prices[cIndex].ToString();
                }
            }
            else { buttons[i].gameObject.SetActive(false); }
        }
    }
    public void SwipeButtons(int delta)
    {
        if (isMove) return;
        int n;
        if (delta < 0) n = -buttonsPerPage;
        else n = buttonsPerPage;
        n += startedButtonIndex;
        if (n >= items3D.Length) n = startedButtonIndex = 0;
        else if (n < 0) n = startedButtonIndex = GetPageIndex(items3D.Length - buttonsPerPage);
        SetLevelButtons(n);
    }
    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Buy/Set Items
    public void Buy(int i)
    {
        int x = i + startedButtonIndex;
        print(x);
        if (have.Contains(items3D[x].name))
        {
            Chance(x);
        }
        else
        {
            GameObject button = EventSystem.current.currentSelectedGameObject;
            int p = int.Parse(buttonsTexts[i].text);
            if (coins >= p)
            {
                coins -= p;
                coinsText.text = coins.ToString();
                buttonBackground[i].color = unlockColor;
                buttonsImage[i].sprite = coinsImage; // Use an appropriate icon for 3D items
                buttonsTexts[i].text = null;
                index = x;
                have += items3D[x].name + ",";
                SetPlayer(x);
                PlayerPrefs.SetInt(data[1], index);
                PlayerPrefs.SetInt(data[0], coins);
                PlayerPrefs.SetString(data[3], have);
            }
        }
    }
    public void Select()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        int i = int.Parse(button.name);
        Chance(i);
    }
    void Chance(int i)
    {
        SetPlayer(i);
        index = i;
        PlayerPrefs.SetInt(data[1], index);
        if (hide)
        {
            ShowCanvas(store);
        }
    }
    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Virtual
    public virtual void SetPlayer(int i)
    {
        // Implement the logic to set the player with the selected 3D item
    }
    public virtual void SettingPlayer()
    {
        // Implement the logic to set up the player
    }
    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region RandomToggle
    void SetToggle()
    {
        isRandom = PlayerPrefs.GetInt(data[2]);
        if (isRandom == 1)
        {
            randomToggle.isOn = true;
        }
        else
        {
            randomToggle.isOn = false;
        }
    }
    public void ToggleRandom()
    {
        bool Enable = randomToggle.isOn;
        if (Enable)
        {
            isRandom = 1;
        }
        else
        {
            isRandom = 0;
            PlayerPrefs.SetInt(data[1], index);
        }
        PlayerPrefs.SetInt(data[2], isRandom);
    }
    public void RandomizePlayer()
    {
        if (isRandom == 1)
        {
            List<int> AllItems = new List<int>();
            for (int i = 0; i < items3D.Length; i++)
            {
                if (have.Contains(items3D[i].name))
                {
                    AllItems.Add(i);
                }
            }
            index = AllItems[UnityEngine.Random.Range(0, AllItems.Count)];
            SetPlayer(index);
        }
    }
    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Coins
    public void SaveCoins(int c)
    {
        coins += c;
        coinsText.text = coins.ToString();
        PlayerPrefs.SetInt(data[0], coins);
    }
    #endregion
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Functions
    public void ShowCanvas(Canvas c)
    {
        c.enabled = !c.isActiveAndEnabled;
    }
    public void SetPlayer(GameObject p)
    {
        player = p;
    }
    #endregion
}
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////
#region Editor Settings
#if UNITY_EDITOR
[CustomEditor(typeof(Store))]
public class Store_Editor : Editor
{
    bool increasePrice = true;
    int after = 1,
        add = 100,
        price = 100;
    public override void OnInspectorGUI()
    {
        SetSettings();
    }
    public void SetSettings()
    {
        DrawDefaultInspector(); // for non-HideInInspector fields
        Store script = (Store)target;
        EditorGUILayout.LabelField("Script Settings", EditorStyles.boldLabel);
        price = EditorGUILayout.IntField("Default price:", price);
        script.hide = EditorGUILayout.Toggle("Hide Store after buy", script.hide);
        increasePrice = EditorGUILayout.Toggle("Increase price", increasePrice);
        if (increasePrice)
        {
            after = EditorGUILayout.IntSlider("Every X buttons:", after, 1, script.items3D.Length);
            add = EditorGUILayout.IntField("increase price in:", add);
        }
        else
        {
            after = 0;
            add = 0;
        }
        if (script.container == null)
        {
            script.container = GameObject.FindGameObjectWithTag("storeContainer");
        }
        int x = after;
        int p = price;
        if (script.prices.Length != script.items3D.Length)
        {
            script.prices = new int[script.items3D.Length];
            for (int i = 0; i < script.items3D.Length; i++)
            {
                if (i == x)
                {
                    x += after;
                    p += add;
                }
                script.prices[i] = p;
            }
        }
        int n = script.buttons.Length;
        if (script.buttonsTexts.Length != n || script.buttonsImage.Length != n || script.buttonBackground.Length != n || script.buttons.Length == 0)
        {
            script.buttons = script.container.GetComponentsInChildren<Button>();
            script.buttonsTexts = new Text[n];
            script.buttonsImage = new Image[n];
            script.buttonBackground = new Image[n];
            for (int i = 0; i < script.buttons.Length; i++)
            {
                script.buttonsImage[i] = script.buttons[i].transform.GetChild(0).GetComponent<Image>();
                script.buttonsTexts[i] = script.buttons[i].GetComponentInChildren<Text>();
                script.buttonBackground[i] = script.buttons[i].GetComponent<Image>();
            }
        }
    }
}
#endif
#endregion
