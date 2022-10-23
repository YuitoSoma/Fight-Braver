using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;

    int currentChipIndex;

    public Transform character;
    public GameObject[] stageChips;
    public int startChipIndex;
    public int preInstantiate;
    public List<GameObject> generatedStageList = new List<GameObject>();

    void Start()
    {
        currentChipIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
    }

    void Update()
    {
        // キャラクターのいちから現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        // 次のステージチップに入ったらステージ更新を行う
        if (charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    // 指定のIndexまでのステージチップを生成して、管理化に置く
    void UpdateStage(int toChipIndex) {
        if (toChipIndex <= currentChipIndex)
            return;

        // 指定のステージチップまでを生成
        for (int i = currentChipIndex + 1; i <= toChipIndex;i++)
        {
            GameObject stageObject = GenerateStage(i);

            // 生成したステージを管理リストに追加
            generatedStageList.Add(stageObject);
        }

        // ステージ保持上眼内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2)
            DestroyOldestStage();

        currentChipIndex = toChipIndex;
    }

    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, -0.5f, chipIndex * StageChipSize),
            Quaternion.identity
        );

        return stageObject;
    }

    // 一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
