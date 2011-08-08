#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Nine.Graphics;

#endregion

namespace Nine.Graphics.ObjectModel
{
    public partial class AmbientLight : Light<IAmbientLight>
    {
        public GraphicsDevice GraphicsDevice { get; private set; }

        public override BoundingBox BoundingBox
        {
            get { return BoundingBoxExtensions.Max; }
        }

        public AmbientLight(GraphicsDevice graphics)
        {
            GraphicsDevice = graphics;
            AmbientLightColor = Vector3.One * 0.2f;
        }

        protected internal override IEnumerable<Drawable> FindAffectedDrawables(ISceneManager<Drawable> allDrawables,
                                                                                IEnumerable<Drawable> drawablesInViewFrustum)
        {
            return drawablesInViewFrustum;
        }

        protected override void Enable(IAmbientLight light)
        {
            light.AmbientLightColor = AmbientLightColor;
        }

        protected override void Disable(IAmbientLight light)
        {
            light.AmbientLightColor = Vector3.Zero;
        }

        [ContentSerializer(Optional = true)]
        public Vector3 AmbientLightColor
        {
            get { return ambientLightColor; }
            set { ambientLightColor = value; }
        }
        private Vector3 ambientLightColor;
    }
}