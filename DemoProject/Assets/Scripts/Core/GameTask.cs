using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class GameTask
    {
        public GameTask()
        {
            CancellationTokenSource = new CancellationTokenSource();
            
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif 
        }

        public GameTask(CancellationTokenSource cancellationToken)
        {
            CancellationTokenSource = cancellationToken;
            
#if UNITY_EDITOR            
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        public CancellationTokenSource CancellationTokenSource { get; }

        public void Cancel()
        {
            if (CancellationTokenSource.IsCancellationRequested)
            {
                return;
            }
                    
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
        }
        
        public async Task Delay(float timer, Action callback = null, Action cancellationCallback = null)
        {
            await DelayWhile(timer, () => true, callback, cancellationCallback);
        }

        /// <summary>
        /// Delays execution of the task while timer is not completed and check function returns true.
        /// </summary>
        /// <param name="timer">delay timer</param>
        /// <param name="check">check function</param>
        /// <param name="callback">call when completed</param>
        /// <param name="cancellationCallback">call when canceled</param>
        public async Task DelayWhile(float timer, Func<bool> check, Action callback = null, Action cancellationCallback = null)
        {
            var finishTime = Time.time + timer;
            
            if (!CheckToken)
            {
                return;
            }
            
            try
            {
                while (Time.time < finishTime)
                {
                    await Task.Yield();
                    if (check.Invoke())
                    {
                        continue;
                    }
                    
                    break;
                }

                callback?.Invoke();
            }
            catch (OperationCanceledException exception)
            {
                CatchException(exception, cancellationCallback);
            }
        }
        
        /// <summary>
        /// Delays execution of the task while check function returns true.
        /// </summary>
        /// <param name="check">check function</param>
        /// <param name="callback">call when completed</param>
        /// <param name="cancellationCallback">call when canceled</param>
        public async Task AwaitWhile(Func<bool> check, Action callback = null, Action cancellationCallback = null)
        {
            if (!CheckToken)
            {
                return;
            }
            
            try
            {
                while (!check.Invoke())
                {
                    await Task.Yield();
                }

                callback?.Invoke();
            }
            catch (OperationCanceledException exception)
            {
                CatchException(exception, cancellationCallback);
            }
        }
        
        /// <summary>
        /// Delays execution of the task while check task executes
        /// </summary>
        /// <param name="task">check function</param>
        /// <param name="callback">call when completed</param>
        /// <param name="cancellationCallback">call when canceled</param>
        public async Task AwaitWhile(Task task, Action callback = null, Action cancellationCallback = null)
        {
            if (!CheckToken)
            {
                return;
            }
            
            try
            {
                while (!task.IsCompleted)
                {
                    if (task.IsCanceled)
                    {
                        cancellationCallback?.Invoke();
                        return;
                    }
                    await Task.Yield();
                }

                callback?.Invoke();
            }
            catch (OperationCanceledException exception)
            {
                CatchException(exception, cancellationCallback);
            }
        }

        private void CatchException(OperationCanceledException exception, Action cancellationCallback)
        {
            if (exception.CancellationToken != CancellationTokenSource.Token)
            {
                return;
            }
            
            cancellationCallback?.Invoke();
        }

        private bool CheckToken => !CancellationTokenSource.IsCancellationRequested;
        
        

#if UNITY_EDITOR

        private void OnPlayModeStateChanged(PlayModeStateChange playModeState)
        {
            switch (playModeState)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    Cancel();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(playModeState), playModeState, null);
            }
        }
#endif
    }
}