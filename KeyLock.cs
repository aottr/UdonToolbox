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

namespace OttrOne.UdonToolbox
{
    /// <summary>
    /// Open a lock by positioning a key inside / close to it.
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class KeyLock : UdonSharpBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        private GameObject Lock;
        [SerializeField, Tooltip("Ignore transform information and only react to trigger collider in Lock.")]
        private bool ColliderLock;
        [SerializeField, Range(0, 4f)]
        private float Range = 0.1f;

        [Header("Unlocks")]
        [SerializeField]
        private GameObject[] LockedObjects;
        [SerializeField, Tooltip("Expects bool variable 'open' to be false by default.")]
        private Animator Anim;

        // optimization of the Update loop
        private bool unlocked = false;

        private Vector3 keyOriginPos;
        private Quaternion keyOriginRot;

        private void Start()
        {
            keyOriginPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            keyOriginRot = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }

        /// <summary>
        /// Check the distance between the key and the lock in each frame.
        /// </summary>
        public void Update()
        {
            if (unlocked) return;
            if (ColliderLock) return;

            if (Vector3.Distance(transform.position, Lock.transform.position) <= Range)
            {
                this._Open();
            }
        }

        /// <summary>
        /// Open the lock and activate the objects / play animation
        /// </summary>
        public void _Open()
        {
            foreach (GameObject obj in LockedObjects)
            {
                obj.SetActive(true);
            }
            if (Anim != null)
            {
                Anim.SetBool("open", true);
            }
            unlocked = true;
        }

        /// <summary>
        /// Close the lock and deactivate the objects / play animation
        /// </summary>
        public void _Close()
        {
            foreach (GameObject obj in LockedObjects)
            {
                obj.SetActive(false);
            }
            if (Anim != null)
            {
                Anim.SetBool("open", false);
            }
            unlocked = false;
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (!ColliderLock) return;
            var LockCollider = Lock.GetComponent(typeof(Collider));
            if (LockCollider == otherCollider) this._Open();
        }

        /// <summary>
        /// Reset key location
        /// </summary>
        public void _ResetKey()
        {
            transform.SetPositionAndRotation(keyOriginPos, keyOriginRot);
        }
    }
}