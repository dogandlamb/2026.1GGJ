using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneControll : MonoBehaviour
{
    public Image backgroundImage;
    public TextMeshProUGUI content;
    public Button choice1;
    public Button choice2;

    private string storyPath = Constrants.STORY_PATH;
    private List<ExcelReader.ExcelData> storyData;
    private int currentLine = 1; // 0行用作标记

    // Start is called before the first frame update
    void Start()
    {
        LoadStoryFromFile(storyPath);
        DisplayNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }

    void LoadStoryFromFile(string path)
    {
        storyData = ExcelReader.ReadExcel(path);
        if(storyData == null || storyData.Count == 0)
        {
            Debug.LogError("No data in file");
        }
    }

    void DisplayNextLine()
    {
        if(currentLine >= storyData.Count)
        {
            Debug.Log("End of story");
            return;
        }
        var data = storyData[currentLine];
        content.text = data.content;
        if(!string.IsNullOrEmpty(data.backgroundFileName) )
        {
            UpdateBackground(data.backgroundFileName);
        }
        currentLine++;
    }

    void UpdateBackground(string fileName)
    {
        string imagePath = Constrants.IMAGE_PATH + fileName;
        Sprite sprite = Resources.Load<Sprite>("Assets/Resources/Images/1.png.meta");
        if(sprite != null)
        {
            backgroundImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Fail to load image");
        }
    }
}
