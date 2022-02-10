using System;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Artemis.Utilities
{
    public static class PlayerLoopUtilities
    {
        public static void AddSubSystem<TParent, TInner>(Action action)
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref var parentSystemRef = ref FindSystem<TParent>(playerLoop);
            Array.Resize(ref parentSystemRef.subSystemList, parentSystemRef.subSystemList.Length + 1);
            var subSystem = new PlayerLoopSystem { type = typeof(TInner), updateDelegate = action.Invoke };
            parentSystemRef.subSystemList[parentSystemRef.subSystemList.Length - 1] = subSystem;
            PlayerLoop.SetPlayerLoop(playerLoop);
            Application.quitting += RemoveSubSystem<TParent, TInner>;
        }

        private static void RemoveSubSystem<TParent, TInner>()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            ref var parentSystemRef = ref FindSystem<TParent>(playerLoop);
            var subSystems = parentSystemRef.subSystemList.ToList();
            subSystems.RemoveAll(subSystem => subSystem.type == typeof(TInner));
            parentSystemRef.subSystemList = subSystems.ToArray();
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private static ref PlayerLoopSystem FindSystem<T>(PlayerLoopSystem parentSystem)
        {
            for (int i = 0; i < parentSystem.subSystemList.Length; i++)
            {
                if (parentSystem.subSystemList[i].type == typeof(T))
                {
                    return ref parentSystem.subSystemList[i];
                }
            }

            throw new Exception($"System of type '{typeof(T).Name}' not found inside system '{parentSystem.type.Name}'.");
        }
    }
}