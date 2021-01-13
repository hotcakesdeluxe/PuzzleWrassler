using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public interface IQueueable : IComparable
    {
        int Priority { get; }
        int QueueIndex { get; set; }
        int InsertionIndex { get; set; }
    }
}