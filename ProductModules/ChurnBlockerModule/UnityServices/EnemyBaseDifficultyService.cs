using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.UnityService
{
    public class EnemyBaseDifficultyService : MonoBehaviour
    {
        [Inject]
        private CharInfo charInfo;

        [Inject]
        private DifficultyInternalModel difficultyInternalModel;

        [Inject]
        private DifficultyHelper difficultyHelper;

        private bool isNewLevel = true;

        public async Task SetCharHealth (int health) 
        {
            if (isNewLevel)
            {
                charInfo.StartHealth = health;
                isNewLevel = false;
            }
            else
            {
                charInfo.FinishHealth =  health;
    
                if(difficultyInternalModel.ServerResultModel == 0)
                {
                    await difficultyHelper.CalculateDifficultyManually();
                    return;
                }
                await difficultyHelper.CalculateDifficulty();
            }
        }

        public async Task SetCharHealth(double health)
        {
            if (isNewLevel)
            {
                charInfo.StartHealth = health;
                isNewLevel = false;
            }
            else
            {
                charInfo.FinishHealth = health;

                if (difficultyInternalModel.ServerResultModel == 0)
                {
                    await difficultyHelper.CalculateDifficultyManually();
                    return;
                }
                await difficultyHelper.CalculateDifficulty();
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            isNewLevel = true;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

    }
}
