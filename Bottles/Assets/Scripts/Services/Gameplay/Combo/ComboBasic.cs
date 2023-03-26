public class ComboBasic : Combo
{
    public override bool TryDoCombo(ItemController itemSender, ItemController itemTarget)
    {
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
                itemTarget.PlayTypeChangeAnim();
            }
            else // ���� ����� ������
            {
                // ������ ��� �� �����
                itemTarget.SetColor(ColorNames.Empty);
                itemTarget.PlayTypeChangeAnim();
            }

            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
