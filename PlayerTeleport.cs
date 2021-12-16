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

namespace OttrOne.UdonToolbox
{
    /// <summary>
    /// Simple script to teleport the local player to a given position (and rotation)
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlayerTeleport : UdonSharpBehaviour
    {
        /// <summary>
        /// Position and rotation data the player will be teleported to
        /// </summary>
        [SerializeField]
        private Transform[] SpawnPoints;

        /// <summary>
        /// teleport when leaving a collider
        /// </summary>
        [SerializeField, Tooltip("Teleport when leaving a collider.")]
        private bool ReactToTrigger = false;

        /// <summary>
        /// Trigger teleportation by interacting with the gameobject
        /// </summary>
        public override void Interact()
        {
            this.TeleportToSpawn();
        }

        /// <summary>
        /// Trigger teleportation by receiving the local event string
        /// </summary>
        public void _Teleport()
        {
            this.TeleportToSpawn();
        }

        /// <summary>
        /// Trigger teleportation by leaving a trigger collider
        /// </summary>
        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (player.isLocal && ReactToTrigger) this.TeleportToSpawn();
        }

        /// <summary>
        /// Teleport the player
        /// </summary>
        private void TeleportToSpawn()
        {
            int rnd = Random.Range(0, SpawnPoints.Length - 1);
            Networking.LocalPlayer.TeleportTo(SpawnPoints[rnd].position, SpawnPoints[rnd].rotation);
        }
    }
}