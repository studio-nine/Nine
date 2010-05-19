#region Copyright 2009 (c) Nightin Games
//=============================================================================
//
//  Copyright 2009 (c) Nightin Games. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nine.Navigation
{
    public interface IMovable
    {
        Vector3 Position { get; set; }

        Vector3 Forward { get; }

        float Speed { get; }

        float MaxSpeed { get; }

        float BoundingRadius { get; }
    }    
}