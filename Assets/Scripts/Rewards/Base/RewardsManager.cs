using System;
using Game.Inventory;
using Game.Items.Fields;
using Game.Rewards.Fields;
using Zenject;

namespace Game.Rewards
{
    public class RewardsManager
    {
        private InventoryManager _inventoryManager;

        public event Action<IRewardField> OnReward;

        [Inject]
        private void Install(InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }

        public void Reward(IRewardField rewardField)
        {
            if (rewardField is RewardsField rewardsData)
            {
                rewardsData.Value?.ForEach(Reward);
            }
            else if (rewardField is ItemField itemField)
            {
                _inventoryManager.Add(itemField.Value, 1);
                OnReward?.Invoke(rewardField);
            }
        }
    }
}