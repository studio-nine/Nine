﻿#region Copyright 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Nine.Content.Pipeline.Processors;
#endregion

namespace Nine.Content.Pipeline.Graphics.ObjectModel
{
    partial class DrawableSurfaceContent
    {
        [ContentSerializer(Optional = true)]
        public virtual Vector3 Position
        {
            get { return position; }
            set { position = value; Transform = Matrix.CreateTranslation(value); }
        }
        Vector3 position;

        [ContentSerializer(Optional = true)]
        public virtual Vector2 TextureScale
        {
            get { return textureScale; }
            set { textureScale = value; UpdateTextureTransform(); }
        }
        Vector2 textureScale = Vector2.One;

        [ContentSerializer(Optional = true)]
        public virtual Vector2 TextureOffset
        {
            get { return textureOffset; }
            set { textureOffset = value; UpdateTextureTransform(); }
        }
        Vector2 textureOffset = Vector2.Zero;
        
        private void UpdateTextureTransform()
        {
            TextureTransform = Nine.Graphics.TextureTransform.CreateScale(textureScale.X, textureScale.Y) *
                               Nine.Graphics.TextureTransform.CreateTranslation(textureOffset.X, textureOffset.Y);
        }
    }
}
