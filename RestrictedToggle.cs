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

using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace OttrOne.UdonToolbox
{
    /// <summary>
    /// This Behaviour can be used to restrict areas or Items to specific persons or / and world owners.
    /// Items selected as Target will be toggled. Means their Active-State changes from off->on and on->off.
    /// 
    /// One usage example is a collider blocking access to an area or showing props only to those people
    /// It's all local.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RestrictedToggle : UdonSharpBehaviour
    {
        [TextArea, SerializeField]
        private string[] Usernames;
        [SerializeField]
        private bool IncludeMaster;
        [SerializeField]
        private GameObject[] Targets;

        /// <summary>
        /// Filter username teextbox by linebreak and compare the names with the local user.
        /// If user is in textbox or: User is Instance master and master is included
        /// Toggle all targets to disable colliders or enable stuff
        /// </summary>
        void Start()
        {
            if (
                Array.IndexOf(Usernames, Networking.LocalPlayer.displayName) != -1
                || (Networking.LocalPlayer.isMaster && IncludeMaster)
                )
            {
                foreach (GameObject target in Targets)
                {
                    target.SetActive(!target.activeSelf);
                }
            }
        }
    }
}