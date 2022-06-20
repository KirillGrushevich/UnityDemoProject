using UnityEngine;

namespace Core
{
    public static class GameInitializer
    {
        private static GameTask mainGameTask;
        
        
        [RuntimeInitializeOnLoadMethod]
        //Game initialization
        public static void InitGame()
        {
            mainGameTask = new GameTask();
            
            Meta.Init(mainGameTask);
        }
    }
}
