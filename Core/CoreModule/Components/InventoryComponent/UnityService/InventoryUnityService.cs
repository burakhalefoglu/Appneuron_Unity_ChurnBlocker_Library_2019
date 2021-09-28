namespace AppneuronUnity.Core.CoreModule.Components.InventoryComponent.UnityService
{
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEngine;
    using Zenject;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataModel;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataManager;

    /// <summary>
    /// Defines the <see cref="InventoryUnityService" />.
    /// </summary>
    public class InventoryUnityService : MonoBehaviour
    {
        [Inject]
        private IInventoryUnityManager _inventoryUnityManager;

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            await _inventoryUnityManager.SendData(inventoryDataModel);
        }

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <param name="moderateMine">The moderateMine<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount, int moderateMine)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            inventoryDataModel.ModerateMine = moderateMine;
            await _inventoryUnityManager.SendData(inventoryDataModel);
        }

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <param name="moderateMine">The moderateMine<see cref="int"/>.</param>
        /// <param name="preciousMine">The preciousMine<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount, int moderateMine, int preciousMine)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            inventoryDataModel.ModerateMine = moderateMine;
            inventoryDataModel.PreciousMine = preciousMine;
            await _inventoryUnityManager.SendData(inventoryDataModel);
        }

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <param name="moderateMine">The moderateMine<see cref="int"/>.</param>
        /// <param name="preciousMine">The preciousMine<see cref="int"/>.</param>
        /// <param name="ıtems">The ıtems<see cref="Item[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount,
            int moderateMine,
            int preciousMine,
            Item[] ıtems)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            inventoryDataModel.ModerateMine = moderateMine;
            inventoryDataModel.PreciousMine = preciousMine;
            inventoryDataModel.Items = ıtems;

            await _inventoryUnityManager.SendData(inventoryDataModel);
        }

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <param name="moderateMine">The moderateMine<see cref="int"/>.</param>
        /// <param name="preciousMine">The preciousMine<see cref="int"/>.</param>
        /// <param name="ıtems">The ıtems<see cref="Item[]"/>.</param>
        /// <param name="skills">The skills<see cref="Skill[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount,
            int moderateMine,
            int preciousMine,
            Item[] ıtems,
            Skill[] skills)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            inventoryDataModel.ModerateMine = moderateMine;
            inventoryDataModel.PreciousMine = preciousMine;
            inventoryDataModel.Items = ıtems;
            inventoryDataModel.Skills = skills;

            await _inventoryUnityManager.SendData(inventoryDataModel);
        }

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <param name="moderateMine">The moderateMine<see cref="int"/>.</param>
        /// <param name="preciousMine">The preciousMine<see cref="int"/>.</param>
        /// <param name="ıtems">The ıtems<see cref="Item[]"/>.</param>
        /// <param name="skills">The skills<see cref="Skill[]"/>.</param>
        /// <param name="temporaryAbilities">The temporaryAbilities<see cref="TemporaryAbility[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount,
            int moderateMine,
            int preciousMine,
            Item[] ıtems,
            Skill[] skills,
            TemporaryAbility[] temporaryAbilities)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            inventoryDataModel.ModerateMine = moderateMine;
            inventoryDataModel.PreciousMine = preciousMine;
            inventoryDataModel.Items = ıtems;
            inventoryDataModel.Skills = skills;
            inventoryDataModel.TemporaryAbilities = temporaryAbilities;
            await _inventoryUnityManager.SendData(inventoryDataModel);
        }

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <param name="moderateMine">The moderateMine<see cref="int"/>.</param>
        /// <param name="preciousMine">The preciousMine<see cref="int"/>.</param>
        /// <param name="ıtems">The ıtems<see cref="Item[]"/>.</param>
        /// <param name="temporaryAbilities">The temporaryAbilities<see cref="TemporaryAbility[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount,
            int moderateMine,
            int preciousMine,
            Item[] ıtems,
            TemporaryAbility[] temporaryAbilities)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            inventoryDataModel.ModerateMine = moderateMine;
            inventoryDataModel.PreciousMine = preciousMine;
            inventoryDataModel.Items = ıtems;
            inventoryDataModel.TemporaryAbilities = temporaryAbilities;
            await _inventoryUnityManager.SendData(inventoryDataModel);
        }

        /// <summary>
        /// The SendInventoryData.
        /// </summary>
        /// <param name="minorMineCount">The minorMineCount<see cref="int"/>.</param>
        /// <param name="moderateMine">The moderateMine<see cref="int"/>.</param>
        /// <param name="preciousMine">The preciousMine<see cref="int"/>.</param>
        /// <param name="skills">The skills<see cref="Skill[]"/>.</param>
        /// <param name="temporaryAbilities">The temporaryAbilities<see cref="TemporaryAbility[]"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendInventoryData(int minorMineCount,
            int moderateMine,
            int preciousMine,
            Skill[] skills,
            TemporaryAbility[] temporaryAbilities)
        {
            var inventoryDataModel = new InventoryDataModel();
            inventoryDataModel.MinorMine = minorMineCount;
            inventoryDataModel.ModerateMine = moderateMine;
            inventoryDataModel.PreciousMine = preciousMine;
            inventoryDataModel.Skills = skills;
            inventoryDataModel.TemporaryAbilities = temporaryAbilities;
            await _inventoryUnityManager.SendData(inventoryDataModel);
        }
    }
}
