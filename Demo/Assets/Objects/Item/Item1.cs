using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item1", menuName = "Item/Item1")]

public class Item1 : Item
{
    PlayerController playerController;

    private void OnEnable()
    {
        this.itemName = "BerserkFillUp";
        this.itemDescription = "Fill up your Berserk Bar immediately";
        this.itemType = ItemType.Active;
        this.cd = 15f;
        this.cost = 100;
        this.code = "daikalop12a";
        this.isEnable = true;
        this.historyDescription = "Núi Bà Đen là ngọn núi lửa đã tắt nằm ở trung tâm tỉnh Tây Ninh, Việt Nam. Với độ cao 986 m, đây là ngọn núi cao nhất miền Nam Việt Nam hiện nay, được mệnh danh \"Đệ nhất thiên sơn\". Trung tâm của quần thể tâm linh là điện Bà thờ Linh Sơn Thánh Mẫu, còn gọi là Bà Đen – vị mẫu thần có ơn phù hộ, độ trì, tương truyền có khả năng cảnh báo trước điềm dữ, tai ương.";
        InitializeLocalization("Item1", "Item1_Description", "Item1_HistoryDescription");
        LoadItemState();
        
    }

    public override void Activate()
    {
        if (isEnable==false) return;
        playerController= FindAnyObjectByType<PlayerController>();
        
        BerserkGauge bg = playerController.GetComponent<BerserkGauge>();
        bg.IncreaseProgress(bg.maxValue);
    }
}