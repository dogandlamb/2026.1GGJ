using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControll : MonoBehaviour
{
    public TextMeshProUGUI name;
    public Image backgroundImage;
    public string storyFile;
    public TextMeshProUGUI content;
    public GameObject choicePanel;
    public Button choice1;
    public Button choice2;
    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI tireText;
    public TextMeshProUGUI winterText;

    private string storyPath = Constrants.STORY_PATH;
    private string defaultStoryFileName = Constrants.DEFAULT_STORY_FILE_NAME;
    private string excelExtension = Constrants.EXCEL_EXTENTION;
    private int[,] sceneId =
    {
        {1, 6, 11},
        {2, 7, 12},
        {3, 8, 13},
        {4, 9, 14},
        {5, 10, 15}
    };
    private static Dictionary<string, Action> functionMap;


    private List<ExcelReader.ExcelData> storyData;
    private List<ExcelReader.SceneData> sceneData;
    private int currentLine = 1; // 0行用作标记
    private Action choiceAction;
    int x = 2, y = 0; // 记录位置，默认位于左中间医院
    bool isEnd = false;
    int money = 1000, hunger = 100, tire = 100, leftDays = 10;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAndLoadStory(defaultStoryFileName);
        functionMap = new Dictionary<string, Action>
        {
            ["dajia"] = dajia,
            ["xishiningren"] = xishiningren,
            ["paiduichi"] = paiduichi,
            ["kanyanchu"] = kanyanchu,
            ["maiyingyuanbang"] = maiyingyuanbang,
            ["fanlajitong"] = fanlajitong,
            ["chufa"] = chufa,
            ["bangzhu"] = bangzhu,
            ["xiuxi"] = xiuxi,
            ["qitao"] = qitao,
            ["gongyuanxiuxi"] = gongyuanxiuxi,
            ["goumailingshi"] = goumailingshi,
            ["chezhanchufa"] = chezhanchufa,
            ["luoshanji"] = luoshanji,
            ["ruzhu"] = ruzhu,
            ["gouzhifangchan"] = gouzhifangchan,
            ["getVictory"] = getVictory
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(leftDays <= 0 || money < 0 || hunger < 0 || tire < 0)
        {
            SceneManager.LoadScene("ReliveScene");
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
        if(isEnd == true)
        {
            if(Input.GetKeyDown(KeyCode.W) && x > 0) 
            {
                x--;
                move();
                LoadStoryFromFile(sceneId[x,y]);
                // 向上移动一格
            }
            if (Input.GetKeyDown(KeyCode.S) && x < 4)
            {
                x++;
                move();
                LoadStoryFromFile(sceneId[x, y]);
                // 向下移动一格
            }
            if(Input.GetKeyDown(KeyCode.A) && y > 0)
            {
                y--;
                move();
                LoadStoryFromFile(sceneId[x, y]);
                // 向左走一格
            }
            if(Input.GetKeyDown(KeyCode.D) && y < 2)
            {
                y++;
                move();
                LoadStoryFromFile(sceneId[x, y]);
                // 向右走一格
            }
        }
    }

    void InitializeAndLoadStory(string fileName)
    {
        Initialize();
        LoadSceneData();
    }

    void Initialize()
    {
        currentLine = 1;
        choicePanel.gameObject.SetActive(false); // 默认不显示选项
        setMoney();
    }

    void LoadStoryFromFile(int id)
    {
        string fileName = sceneData[id].storyFileName;
        name.text = sceneData[id].name;
        UpdateBackground(sceneData[id].backgroundFileName);
        isEnd = false; // 新场景没有结束
        currentLine = 1; // 重置位置

        var path = storyPath + fileName + excelExtension;
        storyData = ExcelReader.ReadExcel(path);
        if(storyData == null || storyData.Count == 0)
        {
            Debug.LogError("No data in file");
            return;
        }
        DisplayNextLine();
    }

    void LoadSceneData()
    {
        sceneData = ExcelReader.ReadSceneData();
        if (sceneData == null || sceneData.Count == 0)
        {
            Debug.LogError("No data in file");
        }
        else
        {
            LoadStoryFromFile(0); // 默认加载第一个，即未知
        }
    }

    void DisplayNextLine()
    {
        var data = storyData[currentLine];
        if(data.type == Constrants.END_OF_STORY)
        {
            content.text = "请移动到下一个位置";
            isEnd = true; // 标记已结束，可以移动
            return;
        }
        else if(data.type == "choice")
        {
            ShowChoices();
            return;
        }
        content.text = data.content;
        if(!string.IsNullOrEmpty(data.backgroundFileName) )
        {
            UpdateBackground(data.backgroundFileName);
        }
        currentLine++;
    }

    void ShowChoices()
    {
        var data = storyData[currentLine];
        choice1.onClick.RemoveAllListeners();
        choice2.onClick.RemoveAllListeners();
        choicePanel.SetActive(true);
        choice1Text.text = data.choice1content;
        choice2Text.text = data.choice2content;
        choice1.onClick.AddListener(() =>
        {
            currentLine = data.choice1FileName;
            ExecuteCommand(data.choice1Action);
            choicePanel.SetActive(false);
            DisplayNextLine();
        });
        choice2.onClick.AddListener(() =>
        {
            currentLine = data.choice2FileName;
            ExecuteCommand(data.choice2Action);
            choicePanel.SetActive(false);
            DisplayNextLine();
        });
        // 切换到对应行数
        
    }

    public void ExecuteCommand(string command)
    {
        if (functionMap.ContainsKey(command))
        {
            functionMap[command]();  // 这里要加括号，因为是调用
        }
        else
        {
            Debug.LogWarning($"未知命令: {command}");
        }
    }

    void UpdateBackground(string fileName)
    {
        string imagePath = Constrants.IMAGE_PATH + fileName;
        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if(sprite != null)
        {
            backgroundImage.sprite = sprite;
            Debug.Log("Successfully Loading");
        }
        else
        {
            Debug.LogError("Fail to load image");
        }
    }

    void setMoney()
    {
        moneyText.text = money.ToString();
        hungerText.text = hunger.ToString();
        tireText.text = tire.ToString();
        winterText.text = leftDays.ToString();
    }

    private void move()
    {
        money -= 10;
        hunger -= 5;
        tire -= 5;
        setMoney();
    }

    private void dajia()
    {
        tire -= 20;
        hunger -= 20;
        setMoney();
    }

    private void xishiningren()
    {
        money -= 50;
        tire -= 10;
        setMoney();
    }

    private void paiduichi()
    {
        hunger += 60;
        tire -= 20;
        setMoney();
    }

    private void kanyanchu()
    {
        money -= 50;
        tire += 70;
        setMoney();
    }

    private void maiyingyuanbang()
    {
        money -= 60;
        tire += 100;
    }
    private void fanlajitong()
    {
        System.Random r = new System.Random();
        int x = r.Next(0, 100);
        if(x <= 30)
        {
            content.text = "你什么都没有找到";
        }
        else if(x <= 60)
        {
            content.text = "你获得了一个鸡腿";
            hunger += 60;
        }
        else
        {
            content.text = "你被人揍了一顿";
            money -= 25;
            hunger -= 10;
            tire -= 10;
        }
        setMoney();
    }

    private void chufa()
    {
        money -= 10;
        LoadStoryFromFile(14);
        setMoney() ;
    }

    private void bangzhu()
    {
        hunger -= 10;
        tire -= 10;
        setMoney();
    }

    private void xiuxi()
    {
        hunger -= 10;
        tire += 70;
        leftDays--;
        setMoney();
    }

    private void qitao()
    {
        money += 100;
        hunger -= 20;
        tire -= 20;
        setMoney();
    }

    private void gongyuanxiuxi()
    {
        hunger -= 10;
        tire += 80;
        leftDays--;
        setMoney();
    }

    private void goumailingshi()
    {
        money -= 50;
        hunger += 70;
        setMoney();
    }

    private void chezhanchufa()
    {
        money -= 10;
        LoadStoryFromFile(7);
        setMoney();
    }

    private void luoshanji()
    {
        LoadStoryFromFile(16);
    }

    private void ruzhu()
    {
        hunger -= 30;
        tire += 60;
        leftDays--;
        setMoney();
    }

    private void gouzhifangchan()
    {
        if(money >= 2000)
        {
            money -= 2000;
            setMoney();
        }
    }

    private void getVictory()
    {
        SceneManager.LoadScene("VictoryScene");
    }
}
