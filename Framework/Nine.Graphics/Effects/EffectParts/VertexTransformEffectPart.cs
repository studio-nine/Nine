﻿#region Copyright 2009 - 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nine.Graphics.Effects.EffectParts
{
#if !WINDOWS_PHONE

    internal class VertexTransformEffectPart : LinkedEffectPart, IEffectMatrices
    {
        private uint dirtyMask = 0;
        
        private Matrix world;
        private EffectParameter worldParameter;
        private const uint worldDirtyMask = 1 << 0;

        private Matrix view;
        private Matrix projection;
        private EffectParameter worldViewProjectionParameter;
        private const uint worldViewProjectionDirtyMask = 1 << 1;

        [ContentSerializerIgnore]
        public Matrix World
        {
            get { return world; }
            set { world = value; dirtyMask |= worldDirtyMask; dirtyMask |= worldViewProjectionDirtyMask; }
        }

        [ContentSerializerIgnore]
        public Matrix View
        {
            get { return view; }
            set { view = value; dirtyMask |= worldViewProjectionDirtyMask; }
        }

        [ContentSerializerIgnore]
        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; dirtyMask |= worldViewProjectionDirtyMask; }
        }

        protected internal override void OnApply()
        {
            if ((dirtyMask & worldDirtyMask) != 0)
            {
                if (worldParameter == null)
                    worldParameter = GetParameter("World");
                worldParameter.SetValue(world);
                dirtyMask &= ~worldDirtyMask;
            }

            if ((dirtyMask & worldViewProjectionDirtyMask) != 0)
            {
                if (worldViewProjectionParameter == null)
                    worldViewProjectionParameter = GetParameter("WorldViewProjection");

                Matrix wvp;
                Matrix.Multiply(ref world, ref view, out wvp);
                Matrix.Multiply(ref wvp, ref projection, out wvp);

                worldViewProjectionParameter.SetValue(wvp);
                dirtyMask &= ~worldViewProjectionDirtyMask;
            }
        }

        protected internal override LinkedEffectPart Clone()
        {
            return new VertexTransformEffectPart();
        }
    }

#endif
}