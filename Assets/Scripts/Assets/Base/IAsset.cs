using System;
using UnityEngine;

namespace Game.Assets
{
    public interface IAsset
    {
        GameObject Asset { get; }
        
        event Action<IAsset> OnReleased;
    }
}