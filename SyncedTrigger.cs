/*
 * Copyright 2021 AlexOttr <alex@ottr.one>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, 
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Enums;

namespace OttrOne.UdonToolbox
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SyncedTrigger : UdonSharpBehaviour
    {
        public GameObject[] Targets;

        [UdonSynced]
        public bool State;

        /// <summary>
        /// React to interaction events on the parent gameobject
        /// </summary>
        public override void Interact()
        {
            this._Toggle();
        }

        private void OwnershipTransferAndSync()
        {
            if (!Networking.LocalPlayer.IsOwner(gameObject)) Networking.SetOwner(Networking.LocalPlayer, gameObject);
            RequestSerialization();
        }

        private void SetActiveStates()
        {
            foreach (GameObject target in Targets)
            {
                target.SetActive(State);
            }
        }

        /// <summary>
        /// Exposed event to toggle all known gameobjects
        /// </summary>
        public void _Toggle()
        {
            State = !State;
            OwnershipTransferAndSync();
            SetActiveStates();
        }

        /// <summary>
        /// Exposed event to turn all known gameobjects on
        /// </summary>
        public void _TurnOn()
        {
            State = true;
            OwnershipTransferAndSync();
            SetActiveStates();
        }

        /// <summary>
        /// Exposed event to turn all known gameobjects off
        /// </summary>
        public void _TurnOff()
        {
            State = false;
            OwnershipTransferAndSync();
            SetActiveStates();
        }

        #region Synchronization for late players
        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (Networking.LocalPlayer.IsOwner(gameObject))
            {
                SendCustomEventDelayedSeconds("_SyncLatePlayer", 2.5F, EventTiming.Update);
            }
        }

        public void _SyncLatePlayer()
        {
            RequestSerialization();
        }
        #endregion

        public override void OnDeserialization()
        {
            SetActiveStates();
        }
    }
}
