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
        if (itemSender.Type != TypeNames.Bag &&
            itemSender.Type != TypeNames.Multi &&
            itemSender.Color.Name != ColorNames.Empty &&
            itemTarget.Color.Name != ColorNames.Empty &&
            itemSender.Type == itemTarget.Type)
        {
            // ���� ����� �����������
            if (itemSender.Color.Name != ColorNames.Multi &&
                itemSender.Color.Name == itemTarget.Color.Name)
            {
                // ������������� � ������������� ������
                itemTarget.SetType(TypeNames.Multi);
                itemTarget.SetColor(ColorNames.Multi);
                //itemTarger._anim.Play("Item_TypeChange");
            }
            else // ���� ����� ������
            {
                // ������ ��� �� �����
                itemTarget.SetColor(ColorNames.Empty);
                //itemTarger._anim.Play("Item_TypeChange");
            }

            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
