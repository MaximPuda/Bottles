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
            itemSender.Color.Name != ColorNames.Empty &&
            itemTarget.Color.Name != ColorNames.Empty &&
            itemSender.Type == itemTarget.Type)
        {
            // Если цвета одиннаковые
            if (itemSender.Color.Name != ColorNames.Multi &&
                itemSender.Color.Name == itemTarget.Color.Name)
            {
                // Универсальная с универсальным цветом
                itemTarget.SetType(TypeNames.Multi);
                itemTarget.SetColor(ColorNames.Multi);
                //itemTarger._anim.Play("Item_TypeChange");
            }
            else // Если цвета разные
            {
                // Пустая той же формы
                itemTarget.SetColor(ColorNames.Empty);
                //itemTarger._anim.Play("Item_TypeChange");
            }

            itemSender.DestroyItem(false);

            return true;
        }

        return false;
    }
}
