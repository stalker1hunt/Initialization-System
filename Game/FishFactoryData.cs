using System;
using System.Collections.Generic;
using System.Linq;
using FishRoom.Controllers;
using FishRoom.Initialization.Interfaces;
using FishRoom.Models;

namespace FishRoom.Initialization.Game
{
    public class FishFactoryData : IFishFactoryData //TODO add interface
    {
        private readonly Dictionary<FishName, List<FishModel>> _fishStorageData =
            new Dictionary<FishName, List<FishModel>>();

        public event Action<FishModel> OnDestroyFish;
        public event Action<FishModel> OnUpdateFish;

        public Dictionary<FishName, List<FishModel>> GetFishListByType()
        {
            return _fishStorageData;
        }


        public List<FishModel> FishDates(FishName fishName)
        {
            return _fishStorageData[fishName];
        }

        public void AddFish(FishModel fishModel)
        {
            _fishStorageData.TryGetValue(fishModel.FishName, out var fishDates);

            if (fishDates != null)
            {
                fishDates.Add(fishModel);
            }
            else
            {
                var localFishDates = new List<FishModel>();
                localFishDates.Add(fishModel);
                _fishStorageData.Add(fishModel.FishName, localFishDates);
            }

            OnAddFish?.Invoke();
        }

        public List<FishModel> GetMergeFishByType(FishName fishName)
        {
            var fishLevel = new Dictionary<int, List<FishModel>>();
            var mergeFishes = new List<FishModel>();

            var fishes = FishDates(fishName);

            var notHungryFishes = fishes.Where(t => !t.IsHungry).ToList();

            foreach (var model in notHungryFishes)
            {
                fishLevel.TryGetValue(model.Lvl, out var fishDates);

                if (fishDates != null)
                {
                    fishDates.Add(model);
                }
                else
                {
                    var localFishDates = new List<FishModel>();
                    localFishDates.Add(model);
                    fishLevel.Add(model.Lvl, localFishDates);
                }
            }

            foreach (var fish in fishLevel) mergeFishes.AddRange(fish.Value);

            return mergeFishes;
        }

        public void UpdateController(FishModel model, FishModel destroyFish)
        {
            _fishStorageData[destroyFish.FishName].Remove(destroyFish);
            OnDestroyFish?.Invoke(destroyFish);
            OnUpdateFish?.Invoke(model);
        }

        public FishModel GetDestroyedFishModel(FishModel fishModelToMerge)
        {
            var destroyModel = GetMergeFishByType(fishModelToMerge.FishName);

            destroyModel = destroyModel.FindAll(x => x.Lvl == fishModelToMerge.Lvl);

            foreach (var model in destroyModel.Where(element => element != fishModelToMerge))
                return model;

            throw new Exception("Cant not find fish model for merge");
        }

        public event Action OnAddFish;

        public void Dispose()
        {
            _fishStorageData.Clear();
        }
    }
}