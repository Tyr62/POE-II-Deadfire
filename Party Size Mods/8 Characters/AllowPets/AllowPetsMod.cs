using Game;
using Game.GameData;
using Patchwork;

namespace PoE2Mods.AllowPets
{
    [ModifiesType]
    public class mod_Equipment : Equipment
    {
        [ModifiesMember("HasEquipmentSlot")]
        public bool HasEquipmentSlotNew(EquipmentSlot slot)
        {
            CharacterStats component = GetComponent<CharacterStats>();
            switch (slot)
            {
                case EquipmentSlot.Head:
                    return !(bool)component || component.CharacterRace != Race.Godlike;
                case EquipmentSlot.Pet:
                    return true;
                default:
                    return true;
            }
        }
    }
}
