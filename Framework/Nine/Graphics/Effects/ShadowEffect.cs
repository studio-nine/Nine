#region Copyright 2009 - 2010 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Nine.Graphics.Effects
{
#if !WINDOWS_PHONE

    public partial class ShadowEffect : IEffectMatrices, IEffectFog, IEffectLights, IEffectMaterial, IEffectTexture
    {
        public bool FogEnabled
        {
            get { return fogMask > 0.5f; }
            set { fogMask = (value ? 1.0f : 0.0f); }
        }
        
        public DirectionalLight DirectionalLight0 { get; private set; }
        public DirectionalLight DirectionalLight1 { get; private set; }
        public DirectionalLight DirectionalLight2 { get; private set; }

        public bool LightingEnabled { get; set; }

        public void EnableDefaultLighting()
        {
            LightingEnabled = true;

            DirectionalLight0.Direction = Vector3.Normalize(-Vector3.One);
            DirectionalLight0.DiffuseColor = Color.Yellow.ToVector3();
            DirectionalLight0.SpecularColor = Color.White.ToVector3();
        }
        
        private void OnCreated() 
        {
            DirectionalLight0 = new DirectionalLight(_lightDirectionParameter, _lightDiffuseColorParameter, _lightSpecularColorParameter, null);
            DirectionalLight1 = new DirectionalLight(_lightDirectionParameter, _lightDiffuseColorParameter, _lightSpecularColorParameter, null);
            DirectionalLight2 = new DirectionalLight(_lightDirectionParameter, _lightDiffuseColorParameter, _lightSpecularColorParameter, null);
        }
        
        private void OnApplyChanges()
        {
            farClip = Math.Abs(LightProjection.M43 / (Math.Abs(LightProjection.M33) - 1));
            eyePosition = Matrix.Invert(View).Translation;
        }

        private void OnClone(ShadowEffect cloneSource) 
        {
            FogEnabled = cloneSource.FogEnabled;
            LightingEnabled = cloneSource.LightingEnabled;
        }

        bool IEffectTexture.TextureEnabled
        {
            get { return true; }
        }

        void IEffectTexture.SetTexture(string name, Texture texture) { }
    }

#endif
}
