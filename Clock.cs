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
using TMPro;
using System;

namespace OttrOne.UdonToolbox
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Clock : UdonSharpBehaviour
    {
        [SerializeField]
        private bool ShowSeconds = false;
        [SerializeField]
        private bool Show24hFormat = false;
        [SerializeField]
        private bool PlayerLocalTime = true;
        [SerializeField]
        private int UTCOffset = 0;
        [SerializeField]
        private TextMeshProUGUI TextMeshPro;
        public override void PostLateUpdate()
        {
            DateTime dt;
            if (PlayerLocalTime)
                dt = DateTime.Now;
            else
                dt = DateTime.UtcNow.AddHours(UTCOffset);

            string time = "";
            if (Show24hFormat)
            {
                time = $"{LeadingZero(dt.Hour)}";
            }
            else
            {
                if (dt.Hour % 12 == 0) time = "12";
                else time = $"{LeadingZero(dt.Hour % 12)}";
            }

            time += $":{ LeadingZero(dt.Minute)}";

            if (ShowSeconds) time += $":{LeadingZero(DateTime.Now.Second)}";

            if (!Show24hFormat)
            {
                if (dt.Hour <= 11) time += " AM";
                else time += " PM";
            }

            TextMeshPro.text = time;
        }
        private string LeadingZero(int number)
        {
            return number.ToString().PadLeft(2, '0');
        }
        void Start()
        {
            TextMeshPro.text = "00:00:00";
        }
    }
}