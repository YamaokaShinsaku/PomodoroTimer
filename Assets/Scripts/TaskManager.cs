using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TaskManager : MonoBehaviour
{
    public InputField taskInputField;
    public Button addTaskButton;
    public GameObject taskPrefab;
    public RectTransform taskPanel;          // タスクを表示するCanvas
    private int count;      // 生成数のカウント

    // 初期の生成位置 (左上)
    public Vector2 initialSpawnPosition = new Vector2(0, 250);
    private Vector2 currentSpawnPosition;
    private readonly float offsetY = 50f;  // Y方向へのオフセット量

    private List<GameObject> tasks = new List<GameObject>(); // タスクのリスト

    public static TaskManager instance;

    void Start()
    {
        instance = this;
        currentSpawnPosition = initialSpawnPosition;
        addTaskButton.onClick.AddListener(AddTask);  // ボタンのクリックリスナーを追加
    }

    void AddTask()
    {
        if(count >= 5)
        {
            return;
        }

        // 入力されたタスクのテキストを取得
        string taskText = taskInputField.text;
        if (!string.IsNullOrEmpty(taskText))
        {
            // TaskPanelの子として新しいタスクを生成
            GameObject newTask = Instantiate(taskPrefab, taskPanel.transform);
            newTask.GetComponentInChildren<Text>().text = taskText;

            // 生成されたタスクの位置を設定
            RectTransform taskRectTransform = newTask.GetComponent<RectTransform>();
            taskRectTransform.anchoredPosition = currentSpawnPosition;

            // タスクリストに追加
            tasks.Add(newTask);

            // 次の生成位置を更新
            UpdateSpawnPosition();

            // 入力フィールドをクリア
            taskInputField.text = string.Empty;
            count++;
        }
    }

    void UpdateSpawnPosition()
    {
        // Y座標を下に移動 (負の方向) して新しい生成位置を設定
        currentSpawnPosition.y -= offsetY;
    }

    public void DeleteTask(GameObject taskToDelete)
    {
        // タスクリストから該当タスクを削除
        int taskIndex = tasks.IndexOf(taskToDelete);
        tasks.RemoveAt(taskIndex);
        Destroy(taskToDelete);

        // 以降のタスクを上に詰める
        for (int i = taskIndex; i < tasks.Count; i++)
        {
            RectTransform taskRectTransform = tasks[i].GetComponent<RectTransform>();
            Vector2 targetPosition = taskRectTransform.anchoredPosition;
            targetPosition.y += offsetY;

            // DoTweenを使用して位置をアニメーションで移動
            taskRectTransform.DOAnchorPos(targetPosition, 0.5f);
        }

        // 更新された生成位置をリセット
        currentSpawnPosition.y += offsetY;
        count--;
    }
}
