using UnityEngine;

public class ComboTwoTypes : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
        if (itemSender == null || itemTarget == null)
        {
            Debug.LogWarning(this + " argument(s) is null!");
            return false;
        }

        // ������� + ������� ��� �� �����
        if (itemSender.Type != ItemType.Bag &&
            itemSender.Type != ItemType.Multi &&
            itemSender.Color.ColorName != ColorsName.Empty &&
            itemTarget.Color.ColorName != ColorsName.Empty &&
            itemSender.Type == itemTarget.Type)
        {
            // ���� ����� �����������
            if (itemSender.Color.ColorName != ColorsName.Multi &&
                itemSender.Color.ColorName == itemTarget.Color.ColorName)
            {
                // ������������� � ������������� ������
                itemTarget.SetView(ItemType.Multi);
                itemTarget.SetColor(ColorsName.Multi);
                //itemTarger._anim.Play("Item_TypeChange");
            }
            else // ���� ����� ������
            {
                // ������ ��� �� �����
                itemTarget.SetColor(ColorsName.Empty);
                //itemTarger._anim.Play("Item_TypeChange");
            }

            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
