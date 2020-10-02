using System;
using System.Collections.Generic;
using System.Text;

namespace ZombieSurvivor
{
    public class EquipmentFactory
    {
        public static Equipment GetEquipment(EquipmentType type)
        {
            switch (type)
            {
                case EquipmentType.BaseballBat:
                    return new Equipment("Baseball bat");
                case EquipmentType.Katana:
                    return new Equipment("Katana");
                case EquipmentType.Kar98:
                    return new Equipment("Kar 98");
                case EquipmentType.Molotov:
                    return new Equipment("Molotov");
                case EquipmentType.BottledWater:
                    return new Equipment("Bottled water");
                case EquipmentType.Pencil:
                    return new Equipment("Pencil");
                case EquipmentType.Sunglasses:
                    return new Equipment("Sunglasses");
                case EquipmentType.Knife:
                    return new Equipment("Knife");
                default:
                    return null;
            }
        }
    }
}
