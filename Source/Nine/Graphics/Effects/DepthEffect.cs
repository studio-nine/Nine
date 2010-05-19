#region Copyright 2009 - 2010 (c) Nightin Games
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Nightin Games. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Nine.Graphics.Effects
{
    public partial class DepthEffect : IEffectMatrices, IEffectSkinned
    {
        public bool SkinningEnabled
        {
            get { return Parameters["ShaderIndex"].GetValueInt32() == 0; }
            set { Parameters["ShaderIndex"].SetValue(value ? 1 : 0); }
        }

        public DepthEffect(GraphicsDevice graphics) : base(GetSharedEffect(graphics))
        {
            InitializeComponent();
        }

        public Matrix[] GetBoneTransforms(int count)
        {
            return _bones.GetValueMatrixArray(count);
        }
        
        public void SetBoneTransforms(Matrix[] boneTransforms)
        {
            bones = boneTransforms;
        }

        protected override void OnApply()
        {
            farClip = Math.Abs(Projection.M43 / (Math.Abs(Projection.M33) - 1));

            base.OnApply();
        }
    }
}
