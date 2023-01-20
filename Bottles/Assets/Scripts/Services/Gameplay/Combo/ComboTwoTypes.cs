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

        // Буталка + бутылка той же формы
        if (itemSender.Type != TypeNames.Bag &&
            itemSender.Type != TypeNames.Multi &&
            itemSender.Color.Name != ColorsName.Empty &&
            itemTarget.Color.Name != ColorsName.Empty &&
            itemSender.Type == itemTarget.Type)
        {
            // Если цвета одиннаковые
            if (itemSender.Color.Name != ColorsName.Multi &&
                itemSender.Color.Name == itemTarget.Color.Name)
            {
                // Универсальная с универсальным цветом
                itemTarget.SetView(TypeNames.Multi);
                itemTarget.SetColor(ColorsName.Multi);
                //itemTarger._anim.Play("Item_TypeChange");
            }
            else // Если цвета разные
            {
                // Пустая той же формы
                itemTarget.SetColor(ColorsName.Empty);
                //itemTarger._anim.Play("Item_TypeChange");
            }

            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
