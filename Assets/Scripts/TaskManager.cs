using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public InputField taskInputField;
    public Button addTaskButton;
    public GameObject taskPrefab;
    public Canvas taskCanvas;          // タスクを表示するCanvas


    // 初期の生成位置 (左上)
    public Vector2 currentSpawnPosition = new Vector2(-115, 500);
    private readonly float offsetY = 50f;  // Y方向へのオフセット量

    void Start()
    {
        addTaskButton.onClick.AddListener(AddTask);  // ボタンのクリックリスナーを追加
    }

    void AddTask()
    {
        // 入力されたタスクのテキストを取得
        string taskText = taskInputField.text;
        if (!string.IsNullOrEmpty(taskText))
        {
            // TaskCanvasの子として新しいタスクを生成
            GameObject newTask = Instantiate(taskPrefab, taskCanvas.transform);
            newTask.GetComponentInChildren<Text>().text = taskText;

            // 生成されたタスクの位置を設定
            RectTransform taskRectTransform = newTask.GetComponent<RectTransform>();
            taskRectTransform.anchoredPosition = currentSpawnPosition;

            // 次の生成位置を更新
            UpdateSpawnPosition();

            // 入力フィールドをクリア
            taskInputField.text = string.Empty;
        }
    }

    void UpdateSpawnPosition()
    {
        // Y座標を下に移動 (負の方向) して新しい生成位置を設定
        currentSpawnPosition.y -= offsetY;
    }
}
