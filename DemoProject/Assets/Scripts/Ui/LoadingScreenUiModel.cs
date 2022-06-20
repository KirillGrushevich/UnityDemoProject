using System.Threading.Tasks;
using DataLoading;
using UnityEngine;

namespace Ui
{
    [CreateAssetMenu(fileName = "LoadingScreenUiModel", menuName = "Configs/UI/LoadingScreenUiModel", order = 0)]
    public class LoadingScreenUiModel : ScriptableObject
    {
        [field: SerializeField] public float FadeInTime { get; private set; }
        [field: SerializeField] public float FadeOutTime { get; private set; }
        [field: SerializeField] public float SliderTravelTime { get; private set; }
        [field: SerializeField] public float SliderIntervalTime { get; private set; }
    }

    public class LoadingScreenUiModelLoader : ConfigLoader
    {
        public Task<LoadingScreenUiModel> Load()
        {
            return LoadInternal<LoadingScreenUiModel>(nameof(LoadingScreenUiModel));
        }
    }
}