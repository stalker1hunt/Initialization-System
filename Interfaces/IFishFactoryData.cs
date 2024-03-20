using System;
using System.Collections.Generic;
using FishRoom.Controllers;
using FishRoom.Models;

namespace FishRoom.Initialization.Interfaces
{
    public interface IFishFactoryData: IDisposable
    {
        event Action<FishModel> OnDestroyFish;
        event Action<FishModel> OnUpdateFish;
        Dictionary<FishName, List<FishModel>> GetFishListByType();
        List<FishModel> FishDates(FishName fishName);
        void AddFish(FishModel fishModel);
        List<FishModel> GetMergeFishByType(FishName fishName);
        void UpdateController(FishModel model, FishModel destroyFish);
        FishModel GetDestroyedFishModel(FishModel fishModelToMerge);
    }
}