using System;
using System.Collections;
using UnityEngine;

namespace Player.Triggers
{
    public class InputTrigger
    {
        public bool isInputTriggered { get; private set; }
        private Func<IEnumerator, Coroutine> startCoroutine;

        public InputTrigger(Func<IEnumerator, Coroutine> startCoroutineAction)
        {
            startCoroutine = startCoroutineAction;
        }

        public void TriggerInput()
        {
            isInputTriggered = true;
            startCoroutine.Invoke(ResetTriggerNextFrame());
        }

        private IEnumerator ResetTriggerNextFrame()
        {
            yield return new WaitForEndOfFrame();
            isInputTriggered = false;
        }
    }
}