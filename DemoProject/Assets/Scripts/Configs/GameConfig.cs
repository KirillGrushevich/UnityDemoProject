using System;
using System.Linq;
using System.Threading.Tasks;
using DataLoading;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config", order = 0)]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public float GameLoadingScreenShowTime { get; private set; }
    }

    public class GameConfigLoader : ConfigLoader
    {
        public Task<GameConfig> Load()
        {
            return LoadInternal<GameConfig>(nameof(GameConfig));
        }
    }
}