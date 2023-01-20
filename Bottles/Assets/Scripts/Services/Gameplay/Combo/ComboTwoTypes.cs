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
            itemSender.Color.Name != ColorsName.Empty &&
            itemTarget.Color.Name != ColorsName.Empty &&
            itemSender.Type == itemTarget.Type)
        {
            // ���� ����� �����������
            if (itemSender.Color.Name != ColorsName.Multi &&
                itemSender.Color.Name == itemTarget.Color.Name)
            {
                // ������������� � ������������� ������
                itemTarget.SetView(TypeNames.Multi);
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
