using System.Threading.Tasks;
using Core;
using DataLoading;
using UnityEngine;

namespace Ui
{
    public class LoadingScreenUIController
    {
        private class LoadingScreenAssetDataLoader : AssetLoader
        {
            public Task<LoadingScreenUiView> Load()
            {
                return LoadInternal<LoadingScreenUiView>(nameof(LoadingScreenUiView));
            }

            public void Unload()
            {
                UnloadInternal();
            }
        }

        private readonly GameTask gameTask;
        
        private LoadingScreenUiModel config;
        private Task<LoadingScreenUiModel> configLoader;

        public LoadingScreenUIController(GameTask gameTask)
        {
            this.gameTask = gameTask;
            LoadConfig();
        }

        public async void Show(float showingTime)
        {
            if (configLoader is { IsCompleted: false })
            {
                await gameTask.AwaitWhile(configLoader);
            }
            
            var dataLoader = new LoadingScreenAssetDataLoader();
            var viewLoaderTask = new LoadingScreenAssetDataLoader().Load();
            await viewLoaderTask;

            var view = viewLoaderTask.Result;
            view.Show(config.FadeInTime, config.SliderTravelTime, config.SliderIntervalTime);

            await gameTask.Delay(showingTime, HideView);

            void HideView()
            {
                view.Hide(config.FadeOutTime, ReleaseView);
            }
            
            void ReleaseView()
            {
                Object.Destroy(view.gameObject);
                dataLoader.Unload();
            }
        }

        private async void LoadConfig()
        {
            configLoader = new LoadingScreenUiModelLoader().Load();
            await configLoader;

            config = configLoader.Result;
        }
    }
}
