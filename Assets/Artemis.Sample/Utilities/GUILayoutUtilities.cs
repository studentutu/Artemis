using System;
using UnityEngine;

namespace Artemis.Sample
{
    public static class GUILayoutUtilities
    {
        public static void Button(string label, Action action, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(label, options))
            {
                action.Invoke();
            }
        }
    }
}