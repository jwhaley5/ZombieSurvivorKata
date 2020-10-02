using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ZombieSurvivor;


namespace UnitTests
{
    [TestClass]
    public class SurvivorTests
    {
        [TestMethod]
        public void StartWithName()
        {
            var survivor = new Survivor("John");

            Assert.IsNotNull(survivor.Name);
        }

        [TestMethod]
        public void StartWithNoWounds()
        {
            var survivor = new Survivor("John");

            Assert.AreEqual(0, survivor.Wounds);
        }

        [TestMethod]
        public void StartAlive()
        {
            var survivor = new Survivor("John");

            Assert.IsTrue(survivor.IsAlive);
        }

        [TestMethod]
        public void DieIfTwoWoundsReceived()
        {
            var survivor = new Survivor("John");
            survivor.ReceiveWound(1);
            survivor.ReceiveWound(1);

            Assert.AreEqual(2, survivor.Wounds);
            Assert.IsFalse(survivor.IsAlive);
        }

        [TestMethod]
        public void AdditionalWoundsIgnored()
        {
            var survivor = new Survivor("John");
            survivor.ReceiveWound(1);
            survivor.ReceiveWound(1);
            survivor.ReceiveWound(1);

            Assert.AreEqual(2, survivor.Wounds);
            Assert.IsFalse(survivor.IsAlive);
        }

        [TestMethod]
        public void StartLevelBlue()
        {
            var survivor = new Survivor("John");

            Assert.AreEqual(Level.Blue, survivor.Level);
        }

        [TestMethod]
        public void StartWithNoExperience()
        {
            var survivor = new Survivor("John");

            Assert.AreEqual(0, survivor.Experience);
        }

        [TestMethod]
        public void StartWithThreeActions()
        {
            var survivor = new Survivor("John");

            Assert.AreEqual(3, survivor.ActionsRemaining);
        }

        [TestMethod]
        public void StartWithNoEquipment()
        {
            var survivor = new Survivor("John");

            Assert.AreEqual(0, survivor.Inventory.Count);
        }

        [TestMethod]
        public void StartWithMaximumCarryingCapacity()
        {
            var survivor = new Survivor("John");

            Assert.AreEqual(5, survivor.CarryingCapacity);
        }

        [TestMethod]
        public void ReduceCarryingCapacityAfterReceivingWound()
        {
            var survivor = new Survivor("John");
            survivor.ReceiveWound(1);

            Assert.AreEqual(4, survivor.CarryingCapacity);
        }

        [TestMethod]
        public void AddOneEquipmentToInventory()
        {
            var survivor = new Survivor("John");
            var knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);

            var isPickedUp = survivor.PickUpItem(knife);

            Assert.IsTrue(survivor.Inventory.Contains(knife));
            Assert.IsTrue(isPickedUp);
        }

        [TestMethod]
        public void OneItemPickedUpGoesInHand()
        {
            var survivor = new Survivor("John");
            var knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);

            var isPickedUp = survivor.PickUpItem(knife);

            Assert.IsTrue(survivor.Inventory.Contains(knife));
            Assert.IsTrue(knife.InHand);
        }

        [TestMethod]
        public void TwoItemsPickedUpGoesInHand()
        {
            var survivor = new Survivor("John");
            var Knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);
            var katana = EquipmentFactory.GetEquipment(EquipmentType.Katana);

            survivor.PickUpItem(Knife);
            survivor.PickUpItem(katana);

            Assert.IsTrue(survivor.Inventory.Contains(Knife));
            Assert.IsTrue(survivor.Inventory.Contains(katana));
            Assert.IsTrue(Knife.InHand);
            Assert.IsTrue(katana.InHand);
        }

        [TestMethod]
        public void ThirdItemGoesToReserve()
        {
            var survivor = new Survivor("John");
            var Knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);
            var katana = EquipmentFactory.GetEquipment(EquipmentType.Katana);
            var bottledWater = EquipmentFactory.GetEquipment(EquipmentType.BottledWater);

            survivor.PickUpItem(Knife);
            survivor.PickUpItem(katana);
            survivor.PickUpItem(bottledWater);

            Assert.IsTrue(survivor.Inventory.Contains(Knife));
            Assert.IsTrue(survivor.Inventory.Contains(katana));
            Assert.IsTrue(survivor.Inventory.Contains(bottledWater));
            Assert.IsTrue(Knife.InHand);
            Assert.IsTrue(katana.InHand);
            Assert.IsFalse(bottledWater.InHand);
        }

        [TestMethod]
        public void EquipmentNotAddedWhenFullEquipmentCapacity()
        {
            var survivor = new Survivor("John");
            var baseballBat = EquipmentFactory.GetEquipment(EquipmentType.BaseballBat);
            var katana = EquipmentFactory.GetEquipment(EquipmentType.Katana);
            var sunglasses = EquipmentFactory.GetEquipment(EquipmentType.Sunglasses);
            var bottledWater = EquipmentFactory.GetEquipment(EquipmentType.BottledWater);
            var kar98 = EquipmentFactory.GetEquipment(EquipmentType.Kar98);
            var knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);

            survivor.PickUpItem(katana);
            survivor.PickUpItem(sunglasses);
            survivor.PickUpItem(bottledWater);
            survivor.PickUpItem(baseballBat);
            survivor.PickUpItem(kar98);

            var isPickedUp = survivor.PickUpItem(knife);

            Assert.IsFalse(survivor.Inventory.Contains(knife));
            Assert.IsFalse(isPickedUp);
        }

        [TestMethod]
        public void DropItemWhenWoundedAndFullEquipmentCapacity()
        {
            var survivor = new Survivor("John");

            var baseballBat = EquipmentFactory.GetEquipment(EquipmentType.BaseballBat);
            var katana = EquipmentFactory.GetEquipment(EquipmentType.Katana);
            var sunglasses = EquipmentFactory.GetEquipment(EquipmentType.Sunglasses);
            var bottledWater = EquipmentFactory.GetEquipment(EquipmentType.BottledWater);
            var knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);

            survivor.PickUpItem(baseballBat);
            survivor.PickUpItem(katana);
            survivor.PickUpItem(sunglasses);
            survivor.PickUpItem(bottledWater);
            survivor.PickUpItem(knife);

            var result = survivor.ReceiveWound(1);

            Assert.IsTrue(result.isEquipmentDropped);
            Assert.IsNotNull(result.droppedEquipment);
            Assert.AreEqual(4, survivor.CarryingCapacity);
            Assert.IsFalse(result.droppedEquipment.InHand);
        }

        [TestMethod]
        public void NotDropItemWhenWoundedAndSufficientEquipmentCapacity()
        {
            var survivor = new Survivor("John");

            var baseballBat = EquipmentFactory.GetEquipment(EquipmentType.BaseballBat);
            var katana = EquipmentFactory.GetEquipment(EquipmentType.Katana);
            var knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);

            survivor.PickUpItem(baseballBat);
            survivor.PickUpItem(katana);
            survivor.PickUpItem(knife);

            var result = survivor.ReceiveWound(1);

            Assert.IsFalse(result.isEquipmentDropped);
        }

        [TestMethod]
        public void GainOneExperienceWhenZombieKilled()
        {
            var survivor = new Survivor("John");
            survivor.KilledZombie();

            Assert.AreEqual(1, survivor.Experience);
        }

        [TestMethod]
        public void TurnLevelBlueWhenSixExperience()
        {
            var survivor = new Survivor("John", 6);

            Assert.AreEqual(Level.Blue, survivor.Level);
        }

        [TestMethod]
        public void TurnLevelYellowWhenSevenExperience()
        {
            var survivor = new Survivor("John", 7);

            Assert.AreEqual(Level.Yellow, survivor.Level);
        }

        [TestMethod]
        public void TurnLevelOrangeWhenNineteenExperience()
        {
            var survivor = new Survivor("John", 19);

            Assert.AreEqual(Level.Orange, survivor.Level);
        }

        [TestMethod]
        public void TurnLevelRedWhenFortyThreeExperience()
        {
            var survivor = new Survivor("John", 43);

            Assert.AreEqual(Level.Red, survivor.Level);
        }
    }
}
