using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private List<ExcelReader.ExcelData> storyData;
    private List<ExcelReader.SceneData> sceneData;
    private int currentLine = 1; // 0行用作标记
    int x = 2, y = 0; // 记录位置，默认位于左中间医院
    bool isEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAndLoadStory(defaultStoryFileName);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
        if(isEnd == true)
        {
            if(Input.GetKeyDown(KeyCode.W) && x > 0) 
            {
                x--;
                LoadStoryFromFile(sceneId[x,y]);
                // 向上移动一格
            }
            if (Input.GetKeyDown(KeyCode.S) && x < 4)
            {
                x++;
                LoadStoryFromFile(sceneId[x, y]);
                // 向下移动一格
            }
            if(Input.GetKeyDown(KeyCode.A) && y > 0)
            {
                y--;
                LoadStoryFromFile(sceneId[x, y]);
                // 向左走一格
            }
            if(Input.GetKeyDown(KeyCode.D) && y < 2)
            {
                y++;
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
            choicePanel.SetActive(false);
            DisplayNextLine();
        });
        choice2.onClick.AddListener(() =>
        {
            currentLine = data.choice2FileName;
            choicePanel.SetActive(false);
            DisplayNextLine();
        });
        // 切换到对应行数
        
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
}
