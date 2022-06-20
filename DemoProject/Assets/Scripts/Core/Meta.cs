using Configs;
using Ui;
using UnityEngine;

namespace Core
{
    public static class Meta
    {
        private static GameConfig gameConfig;
        private static Game game;

        private static LoadingScreenUIController loadingScreenUIController;
        public static async void Init(GameTask gameTask)
        {
            var gameConfigLoader = new GameConfigLoader().Load();
            await gameConfigLoader;
            gameConfig = gameConfigLoader.Result;

            loadingScreenUIController = new LoadingScreenUIController(gameTask);
            loadingScreenUIController.Show(gameConfig.GameLoadingScreenShowTime);
            
            await gameTask.Delay(gameConfig.GameLoadingScreenShowTime);
            
            
        }

        
    }
}