namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    using Opsive.UltimateCharacterController.Inventory;

    /// <summary>
    /// Draws a popup with the list of available abilities.
    /// </summary>
    public class AbilityDrawerAttribute : ObjectDrawerAttribute
    {
        public AbilityDrawerAttribute() { }
    }

    /// <summary>
    /// Draws a popup with the list of available ItemSet abilities.
    /// </summary>
    public class ItemSetAbilityDrawerAttribute : ObjectDrawerAttribute
    {
        public ItemSetAbilityDrawerAttribute() { }
    }

    /// <summary>
    /// Draws a popup with the list of available effects.
    /// </summary>
    public class EffectDrawerAttribute : ObjectDrawerAttribute
    {
        public EffectDrawerAttribute() { }
    }

    /// <summary>
    /// Draws a popup with the list of available ItemSet categories.
    /// </summary>
    public class ItemSetCategoryDrawerAttribute : ObjectDrawerAttribute
    {
        public ItemSetCategoryDrawerAttribute() { }
    }

    /// <summary>
    /// Ised by the ItemSetCategoryDrawer.
    /// </summary>
    public interface IItemSetManagerTask
    {
        ItemSetManagerBase ItemSetManager { get; }
    }
}