using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZombieSurvivor;

namespace ZombieSurvivorKata
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void StartGameWithNoSurvivors()
        {
            var game = new Game();
            Assert.AreEqual(0, game.Survivors.Count());
        }

        [TestMethod]
        public void StartLevelBlue()
        {
            var game = new Game();

            Assert.AreEqual(Level.Blue, game.Level);
        }

        [TestMethod]
        public void RecordGameStarted()
        {
            var game = new Game();

            Assert.AreEqual(1, game.GameHistory.Count());
            Assert.IsTrue(game.GameHistory[0].EventDetail.StartsWith("New game started"));
        }

        [TestMethod]
        public void AddNewSurvivorAtStart()
        {
            var game = new Game();
            var John = new Survivor("John");
            var successfullyAddedSurvivor = game.AddSurvivorToGame(John);

            Assert.IsTrue(successfullyAddedSurvivor);
            Assert.AreEqual(1, game.Survivors.Count());
        }

        [TestMethod]
        public void SurvivorsCantHaveTheSameName()
        {
            var game = new Game();
            var John = new Survivor("John");
            game.AddSurvivorToGame(John);

            var newJohn = new Survivor("John");
            var successfullyAddedSurvivor = game.AddSurvivorToGame(newJohn);

            Assert.IsFalse(successfullyAddedSurvivor);
            Assert.AreEqual(1, game.Survivors.Count());
        }

        [TestMethod]
        public void RecordAddingSurvivorToGame()
        {
            var game = new Game();
            var John = new Survivor("John");
            game.AddSurvivorToGame(John);
            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith($"Survivor {John.Name} was added to the game"));
        }

        [TestMethod]
        public void AllSurvivorsDead_CheckEndOfGameFlag()
        {
            var game = new Game();
            var John = new Survivor("John");
            var Whaley = new Survivor("Whaley");
            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);

            John.ReceiveWound(2);
            Whaley.ReceiveWound(2);

            Assert.IsFalse(John.IsAlive);
            Assert.IsFalse(Whaley.IsAlive);
            Assert.AreEqual(2, game.Survivors.Count());
            Assert.IsTrue(game.IsEndOfGame);
        }

        [TestMethod]
        public void RecordEndOfGame()
        {
            var game = new Game();
            var John = new Survivor("John");
            var Whaley = new Survivor("Whaley");
            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);

            John.ReceiveWound(2);
            Whaley.ReceiveWound(2);

            var continueGame = game.GameRound();
            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsFalse(continueGame);
            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith("The game has ended, all Survivors died"));
        }

        [TestMethod]
        public void RecordEquipmentPickedUpBySurvivor()
        {
            var game = new Game();
            var survivor = new Survivor("John");
            game.AddSurvivorToGame(survivor);
            var equipment = EquipmentFactory.GetEquipment(EquipmentType.BaseballBat);

            survivor.PickUpItem(equipment);
            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith($"{survivor.Name} picks up a piece of equipment ({equipment.Name})"));
        }

        [TestMethod]
        public void RecordSurvivorLevelUp()
        {
            var game = new Game();
            var John = new Survivor("John", 6);
            var Whaley = new Survivor("Whaley", 10);
            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);
            John.KilledZombie();
            var newLevel = John.Level;

            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith($"{John.Name} has leveled up  and is now {newLevel}"));
        }

        [TestMethod]
        public void RecordGameLevelUp()
        {
            var game = new Game();
            var John = new Survivor("John", 6);
            var Whaley = new Survivor("Whaley", 4);
            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);

            var gameLevelBefore = game.Level;
            John.KilledZombie();
            var newGameLevel = game.Level;

            game.RecordAnyGameLevelChange(gameLevelBefore);
            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith($"The game level is now {newGameLevel}!"));
        }

        [TestMethod]
        public void RecordSurvivorWoundedEvent()
        {
            var game = new Game();
            var survivor = new Survivor("John");
            game.AddSurvivorToGame(survivor); ;
            survivor.ReceiveWound(1);

            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith($"{survivor.Name} has been wounded!"));
        }

        [TestMethod]
        public void RecordSurvivorDroppedEquipmentEvent()
        {
            var game = new Game();
            var survivor = new Survivor("John");
            game.AddSurvivorToGame(survivor); ;

            var baseballBat = EquipmentFactory.GetEquipment(EquipmentType.BaseballBat);
            var katana = EquipmentFactory.GetEquipment(EquipmentType.Katana);
            var kar98 = EquipmentFactory.GetEquipment(EquipmentType.Kar98);
            var bottledWater = EquipmentFactory.GetEquipment(EquipmentType.BottledWater);
            var knife = EquipmentFactory.GetEquipment(EquipmentType.Knife);

            survivor.PickUpItem(baseballBat);
            survivor.PickUpItem(katana);
            survivor.PickUpItem(kar98);
            survivor.PickUpItem(bottledWater);
            survivor.PickUpItem(knife);

            var result = survivor.ReceiveWound(1);

            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith($"{survivor.Name} drops a piece of equipment"));
        }

        [TestMethod]
        public void RecordSurvivorKilleded()
        {
            var game = new Game();
            var survivor = new Survivor("John");
            game.AddSurvivorToGame(survivor); ;
            survivor.ReceiveWound(2);

            var lastEvent = game.GameHistory.LastOrDefault();

            Assert.IsNotNull(lastEvent);
            Assert.IsTrue(lastEvent.EventDetail.StartsWith($"{survivor.Name} has been killed!"));
        }

        [TestMethod]
        public void SurvivorBlueThenGameLevelBlue()
        {
            var game = new Game();
            var John = new Survivor("John", 3);
            var Whaley = new Survivor("Whaley", 6);

            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);

            Assert.AreEqual(Level.Blue, game.Level);
        }

        [TestMethod]
        public void SurvivorYellowThenGameLevelYellow()
        {
            var game = new Game();
            var John = new Survivor("John", 15);
            var Whaley = new Survivor("Whaley", 6);

            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);

            Assert.AreEqual(Level.Yellow, game.Level);
        }

        [TestMethod]
        public void SurvivorOrangeThenGameLevelOrange()
        {
            var game = new Game();
            var John = new Survivor("John", 20);
            var Whaley = new Survivor("Whaley", 6); ;
            var pumpkin = new Survivor("Pumpkin", 20);

            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);
            game.AddSurvivorToGame(pumpkin);

            Assert.AreEqual(Level.Orange, game.Level);
        }

        [TestMethod]
        public void SurvivorRedThenGameLevelRed()
        {
            var game = new Game();
            var John = new Survivor("John", 45);
            var Whaley = new Survivor("Whaley", 6);
            var pumpkin = new Survivor("Pumpkin", 20);

            game.AddSurvivorToGame(John);
            game.AddSurvivorToGame(Whaley);
            game.AddSurvivorToGame(pumpkin);

            Assert.AreEqual(Level.Red, game.Level);
        }
    }
}
