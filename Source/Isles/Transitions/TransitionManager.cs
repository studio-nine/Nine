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


namespace Isles.Transitions
{
    public sealed class TransitionManager
    {
        public void Start<T>(ITransition<T> transition, object target, string field, bool overrideExistingTransitions)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}