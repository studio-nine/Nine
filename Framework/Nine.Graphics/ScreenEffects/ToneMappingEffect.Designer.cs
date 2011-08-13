// -----------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EffectCustomTool v1.5.1.0.
//     Runtime Version: v4.0.30319
//
//     EffectCustomTool is a part of Engine Nine. (http://nine.codeplex.com)
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// -----------------------------------------------------------------------------

namespace Nine.Graphics.ScreenEffects
{
#if !WINDOWS_PHONE

    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    [System.CodeDom.Compiler.GeneratedCode("Nine.Tools.EffectCustomTool", "1.5.1.0")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    partial class ToneMappingEffect : Effect
    {
        public ToneMappingEffect(GraphicsDevice graphics) 
            : base(graphics, graphics.GraphicsProfile == GraphicsProfile.Reach ? ReachEffectCode :
                                                                                 HiDefEffectCode)
        {
            CacheEffectParameters(Parameters);

            OnCreated();
        }

        /// <summary>
        /// Creates a new ToneMappingEffect by cloning parameter settings from an existing instance.
        /// </summary>
        protected ToneMappingEffect(ToneMappingEffect cloneSource) : base(cloneSource)
        {
            CacheEffectParameters(cloneSource.Parameters);

            OnCreated();

            CloneFrom(cloneSource);

            OnClone(cloneSource);
        }

        /// <summary>
        /// Creates a clone of the current ToneMappingEffect instance.
        /// </summary>
        public override Effect Clone()
        {
            return new ToneMappingEffect(this);
        }

        protected override void OnApply()
        {
            OnApplyChanges();

            ApplyChanges();

            base.OnApply();
        }

        private void CacheEffectParameters(EffectParameterCollection cloneSource)
        {
            this._ExposureParameter = cloneSource["Exposure"];
            this._MaxLuminanceParameter = cloneSource["MaxLuminance"];
            this._LuminanceTextureParameter = cloneSource["LuminanceTexture"];
            this._BloomTextureParameter = cloneSource["BloomTexture"];

        }

        #region Dirty Flags

        uint dirtyFlag = 0;

        const uint ExposureDirtyFlag = 1 << 0;
        const uint MaxLuminanceDirtyFlag = 1 << 1;
        const uint LuminanceTextureDirtyFlag = 1 << 2;
        const uint BloomTextureDirtyFlag = 1 << 3;

        #endregion

        #region Properties

        private float _Exposure;
        private EffectParameter _ExposureParameter;

        public float Exposure
        {
            get { return _Exposure; }
            set { if (_Exposure != value) { _Exposure = value; dirtyFlag |= ExposureDirtyFlag; } }
        }

        private float _MaxLuminance;
        private EffectParameter _MaxLuminanceParameter;

        public float MaxLuminance
        {
            get { return _MaxLuminance; }
            set { if (_MaxLuminance != value) { _MaxLuminance = value; dirtyFlag |= MaxLuminanceDirtyFlag; } }
        }

        private Texture2D _LuminanceTexture;
        private EffectParameter _LuminanceTextureParameter;

        public Texture2D LuminanceTexture
        {
            get { return _LuminanceTexture; }
            set { if (_LuminanceTexture != value) { _LuminanceTexture = value; dirtyFlag |= LuminanceTextureDirtyFlag; } }
        }

        private Texture2D _BloomTexture;
        private EffectParameter _BloomTextureParameter;

        public Texture2D BloomTexture
        {
            get { return _BloomTexture; }
            set { if (_BloomTexture != value) { _BloomTexture = value; dirtyFlag |= BloomTextureDirtyFlag; } }
        }


        #endregion

        #region Apply
        private void ApplyChanges()
        {
            if ((this.dirtyFlag & ExposureDirtyFlag) != 0)
            {
                this._ExposureParameter.SetValue(_Exposure);
                this.dirtyFlag &= ~ExposureDirtyFlag;
            }
            if ((this.dirtyFlag & MaxLuminanceDirtyFlag) != 0)
            {
                this._MaxLuminanceParameter.SetValue(_MaxLuminance);
                this.dirtyFlag &= ~MaxLuminanceDirtyFlag;
            }
            if ((this.dirtyFlag & LuminanceTextureDirtyFlag) != 0)
            {
                this._LuminanceTextureParameter.SetValue(_LuminanceTexture);
                this.dirtyFlag &= ~LuminanceTextureDirtyFlag;
            }
            if ((this.dirtyFlag & BloomTextureDirtyFlag) != 0)
            {
                this._BloomTextureParameter.SetValue(_BloomTexture);
                this.dirtyFlag &= ~BloomTextureDirtyFlag;
            }

        }
        #endregion

        #region Clone
        private void CloneFrom(ToneMappingEffect cloneSource)
        {
            this._Exposure = cloneSource._Exposure;
            this._MaxLuminance = cloneSource._MaxLuminance;
            this._LuminanceTexture = cloneSource._LuminanceTexture;
            this._BloomTexture = cloneSource._BloomTexture;

        }
        #endregion

        #region Structures

        #endregion

        #region ByteCode
        internal static byte[] ReachEffectCode = null;
        internal static byte[] HiDefEffectCode = null;

        static ToneMappingEffect()
        {
#if XBOX360
            ReachEffectCode = HiDefEffectCode = new byte[] 
            {
0xBC, 0xF0, 0x0B, 0xCF, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x06, 0xFE, 0xFF, 0x09, 0x01, 0x00, 0x00, 0x04, 0x0C, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x45, 0x78, 0x70, 0x6F, 0x73, 0x75, 0x72, 0x65, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x4D, 0x61, 0x78, 0x4C, 0x75, 0x6D, 0x69, 0x6E, 
0x61, 0x6E, 0x63, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x42, 0x61, 0x73, 0x69, 0x63, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0xAC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x11, 0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 0x65, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x02, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 
0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 
0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0xDC, 0x00, 0x00, 0x00, 0xD8, 0x00, 0x00, 0x00, 0x74, 
0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0xF4, 0x00, 0x00, 0x00, 0xF0, 0x00, 0x00, 0x00, 0x73, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x01, 0x14, 
0x00, 0x00, 0x01, 0x10, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x01, 0x34, 0x00, 0x00, 0x01, 0x30, 0x00, 0x00, 0x00, 0x6F, 
0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x01, 0x54, 0x00, 0x00, 0x01, 0x50, 0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x01, 0x74, 
0x00, 0x00, 0x01, 0x70, 0x00, 0x00, 0x00, 0x71, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x01, 0x94, 0x00, 0x00, 0x01, 0x90, 0x00, 0x00, 0x00, 0x11, 
0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 0x65, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 
0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x02, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x0D, 
0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04, 
0x00, 0x00, 0x03, 0xC8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x04, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x10, 
0x00, 0x00, 0x02, 0x80, 0x00, 0x00, 0x02, 0x7C, 0x00, 0x00, 0x00, 0x74, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x02, 0x98, 0x00, 0x00, 0x02, 0x94, 
0x00, 0x00, 0x00, 0x73, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x02, 0xB8, 0x00, 0x00, 0x02, 0xB4, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x10, 
0x00, 0x00, 0x02, 0xD8, 0x00, 0x00, 0x02, 0xD4, 0x00, 0x00, 0x00, 0x6F, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x02, 0xF8, 0x00, 0x00, 0x02, 0xF4, 
0x00, 0x00, 0x00, 0x70, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x03, 0x18, 0x00, 0x00, 0x03, 0x14, 0x00, 0x00, 0x00, 0x71, 0x00, 0x00, 0x00, 0x10, 
0x00, 0x00, 0x03, 0x38, 0x00, 0x00, 0x03, 0x34, 0x00, 0x00, 0x00, 0x0D, 0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x70, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x54, 0x6F, 0x6E, 0x65, 0x4D, 0x61, 0x70, 0x70, 
0x69, 0x6E, 0x67, 0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x04, 
0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x34, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x94, 
0x00, 0x00, 0x00, 0xA8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC4, 0x00, 0x00, 0x01, 0xB0, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x3C, 0x00, 0x00, 0x02, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x68, 
0x00, 0x00, 0x03, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xFC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 
0x00, 0x00, 0x03, 0xF4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x5D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xE0, 
0x00, 0x00, 0x03, 0xDC, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x02, 0x74, 0x10, 0x2A, 0x11, 0x00, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0xF4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 
0x00, 0x00, 0x01, 0x34, 0x00, 0x00, 0x01, 0x5C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x0C, 0x00, 0x00, 0x00, 0x1C, 
0x00, 0x00, 0x00, 0xFD, 0xFF, 0xFF, 0x03, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF6, 
0x00, 0x00, 0x00, 0x80, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA0, 
0x00, 0x03, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xAD, 0x00, 0x02, 0x00, 0x00, 
0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB8, 0x00, 0x00, 0x00, 0xC8, 0x00, 0x00, 0x00, 0xD8, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x90, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE9, 0x00, 0x02, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB8, 
0x00, 0x00, 0x00, 0xC8, 0x42, 0x61, 0x73, 0x69, 0x63, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0xAB, 0xAB, 0xAB, 0x00, 0x04, 0x00, 0x0C, 
0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 
0x00, 0x45, 0x78, 0x70, 0x6F, 0x73, 0x75, 0x72, 0x65, 0x00, 0xAB, 0xAB, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4C, 0x75, 0x6D, 0x69, 
0x6E, 0x61, 0x6E, 0x63, 0x65, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0x4D, 0x61, 0x78, 0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 
0x65, 0x00, 0x70, 0x73, 0x5F, 0x33, 0x5F, 0x30, 0x00, 0x32, 0x2E, 0x30, 0x2E, 0x31, 0x31, 0x36, 0x32, 0x36, 0x2E, 0x30, 0x00, 0xAB, 0xAB, 0xAB, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x01, 0xFC, 0x00, 0x10, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0xB4, 
0x10, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x21, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 
0x00, 0x00, 0x30, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3D, 0xE9, 0x78, 0xD5, 0x3E, 0x99, 0x16, 0x87, 0x3F, 0x16, 0x45, 0xA2, 
0x3F, 0x00, 0x00, 0x00, 0x3F, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x40, 0x02, 
0x00, 0x00, 0x12, 0x00, 0xC4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0x06, 0x20, 0x0C, 0x12, 0x00, 0x22, 0x00, 0x00, 0x00, 0x14, 0x84, 0x00, 0x00, 
0x00, 0x6C, 0x6C, 0x1B, 0x01, 0x01, 0x01, 0xFE, 0xFC, 0x18, 0x00, 0x01, 0x1F, 0x1F, 0xF1, 0xFF, 0x00, 0x00, 0x40, 0x00, 0x10, 0x08, 0x20, 0x01, 
0x1F, 0x1F, 0xF6, 0x88, 0x00, 0x00, 0x40, 0x00, 0x10, 0x28, 0x10, 0x01, 0x1F, 0x1F, 0xF6, 0x88, 0x00, 0x00, 0x40, 0x00, 0x4C, 0x2F, 0x00, 0x01, 
0x00, 0x00, 0x00, 0x1B, 0xE0, 0x02, 0x01, 0x00, 0xA8, 0x21, 0x00, 0x00, 0x00, 0xBE, 0xC0, 0x41, 0x90, 0x01, 0xFE, 0x00, 0x4C, 0x18, 0x00, 0x00, 
0x00, 0xB1, 0x6C, 0xC6, 0xE1, 0x00, 0x00, 0x00, 0xC8, 0x04, 0x00, 0x00, 0x00, 0x1B, 0x6C, 0x00, 0xE1, 0x00, 0x00, 0x00, 0xC8, 0x03, 0x00, 0x00, 
0x00, 0xC7, 0x6C, 0x00, 0xA0, 0x00, 0xFF, 0x00, 0x4C, 0x12, 0x00, 0x00, 0x00, 0x1B, 0xB1, 0x6C, 0xE1, 0x00, 0x00, 0x00, 0xC8, 0x01, 0x00, 0x00, 
0x00, 0xB1, 0x6C, 0x00, 0xE1, 0x00, 0x00, 0x00, 0xC8, 0x0F, 0x80, 0x00, 0x00, 0x6C, 0x00, 0x00, 0xE1, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x0D, 0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x00, 0x00, 0x00, 0x00, 
0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x11, 
0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 0x65, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x00, 0x00, 0x00, 0x00, 
            };
#else
            ReachEffectCode = new byte[] 
            {
0xCF, 0x0B, 0xF0, 0xBC, 0x10, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x09, 0xFF, 0xFE, 0x0C, 0x04, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x45, 0x78, 0x70, 0x6F, 0x73, 0x75, 0x72, 0x65, 
0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x4D, 0x61, 0x78, 0x4C, 0x75, 0x6D, 0x69, 0x6E, 
0x61, 0x6E, 0x63, 0x65, 0x00, 0x89, 0x00, 0x80, 0x0A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x42, 0x61, 0x73, 0x69, 0x63, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 
0x00, 0x01, 0x00, 0x80, 0x05, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0xAC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 0x65, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 
0x00, 0x7D, 0xB1, 0x02, 0x0A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x24, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x02, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 
0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 
0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x07, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0xDC, 0x00, 0x00, 0x00, 0xD8, 0x00, 0x00, 0x00, 0xAA, 0x00, 0x00, 0x00, 
0x00, 0x01, 0x00, 0x00, 0xF4, 0x00, 0x00, 0x00, 0xF0, 0x00, 0x00, 0x00, 0xA9, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x14, 0x01, 0x00, 0x00, 
0x10, 0x01, 0x00, 0x00, 0xAB, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x34, 0x01, 0x00, 0x00, 0x30, 0x01, 0x00, 0x00, 0xA5, 0x00, 0x00, 0x00, 
0x00, 0x01, 0x00, 0x00, 0x54, 0x01, 0x00, 0x00, 0x50, 0x01, 0x00, 0x00, 0xA6, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x74, 0x01, 0x00, 0x00, 
0x70, 0x01, 0x00, 0x00, 0xA7, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x94, 0x01, 0x00, 0x00, 0x90, 0x01, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 
0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 0x65, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0x2B, 0x06, 0x00, 0x05, 0x00, 0x00, 0x00, 
0x04, 0x00, 0x00, 0x00, 0x54, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 
0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
0xC8, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 
0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0xA4, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 
0x80, 0x02, 0x00, 0x00, 0x7C, 0x02, 0x00, 0x00, 0xAA, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x98, 0x02, 0x00, 0x00, 0x94, 0x02, 0x00, 0x00, 
0xA9, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0xB8, 0x02, 0x00, 0x00, 0xB4, 0x02, 0x00, 0x00, 0xAB, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 
0xD8, 0x02, 0x00, 0x00, 0xD4, 0x02, 0x00, 0x00, 0xA5, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0xF8, 0x02, 0x00, 0x00, 0xF4, 0x02, 0x00, 0x00, 
0xA6, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x18, 0x03, 0x00, 0x00, 0x14, 0x03, 0x00, 0x00, 0xA7, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 
0x38, 0x03, 0x00, 0x00, 0x34, 0x03, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 
0x00, 0x01, 0x00, 0x80, 0x05, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x70, 0x30, 0x00, 0x02, 0x0C, 0x00, 0x00, 0x00, 0x54, 0x6F, 0x6E, 0x65, 0x4D, 0x61, 0x70, 0x70, 
0x69, 0x6E, 0x67, 0x00, 0x07, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 
0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x34, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x94, 0x00, 0x00, 0x00, 
0xA8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC4, 0x00, 0x00, 0x00, 0xB0, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x3C, 0x02, 0x00, 0x00, 0x50, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x68, 0x02, 0x00, 0x00, 
0x54, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFC, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0xF4, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x93, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE0, 0x03, 0x00, 0x00, 
0xDC, 0x03, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0xF4, 0x03, 0x00, 0x00, 0x00, 0x02, 0xFF, 0xFF, 0xFE, 0xFF, 0x4E, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00, 
0x00, 0x02, 0xFF, 0xFF, 0x04, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0xFC, 0x00, 0x00, 0x00, 0x6C, 0x00, 0x00, 0x00, 
0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x7C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x8C, 0x00, 0x00, 0x00, 0x03, 0x00, 0x02, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x9C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xAC, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0xB8, 0x00, 0x00, 0x00, 0xC8, 0x00, 0x00, 0x00, 0xD8, 0x00, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0xEC, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x42, 0x61, 0x73, 0x69, 0x63, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0xAB, 0xAB, 0xAB, 0x04, 0x00, 0x0C, 0x00, 
0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 
0x00, 0xAB, 0xAB, 0xAB, 0x04, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x45, 0x78, 0x70, 0x6F, 
0x73, 0x75, 0x72, 0x65, 0x00, 0xAB, 0xAB, 0xAB, 0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 
0x65, 0x53, 0x61, 0x6D, 0x70, 0x6C, 0x65, 0x72, 0x00, 0xAB, 0xAB, 0xAB, 0x04, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x70, 0x73, 0x5F, 0x32, 0x5F, 0x30, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 
0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 
0x32, 0x36, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x32, 0x38, 0x34, 0x34, 0x00, 0xFE, 0xFF, 0x52, 0x00, 0x50, 0x52, 0x45, 0x53, 0x01, 0x02, 0x58, 0x46, 
0xFE, 0xFF, 0x26, 0x00, 0x43, 0x54, 0x41, 0x42, 0x1C, 0x00, 0x00, 0x00, 0x63, 0x00, 0x00, 0x00, 0x01, 0x02, 0x58, 0x46, 0x01, 0x00, 0x00, 0x00, 
0x1C, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x20, 0x60, 0x00, 0x00, 0x00, 0x30, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x40, 0x00, 0x00, 0x00, 0x50, 0x00, 0x00, 0x00, 0x4D, 0x61, 0x78, 0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 0x65, 0x00, 0xAB, 0xAB, 0xAB, 
0x00, 0x00, 0x03, 0x00, 0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x74, 0x78, 0x00, 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, 0x74, 0x20, 0x28, 0x52, 0x29, 
0x20, 0x48, 0x4C, 0x53, 0x4C, 0x20, 0x53, 0x68, 0x61, 0x64, 0x65, 0x72, 0x20, 0x43, 0x6F, 0x6D, 0x70, 0x69, 0x6C, 0x65, 0x72, 0x20, 0x39, 0x2E, 
0x32, 0x36, 0x2E, 0x39, 0x35, 0x32, 0x2E, 0x32, 0x38, 0x34, 0x34, 0x00, 0xFE, 0xFF, 0x0C, 0x00, 0x50, 0x52, 0x53, 0x49, 0x01, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x02, 0x00, 0x43, 0x4C, 0x49, 0x54, 
0x00, 0x00, 0x00, 0x00, 0xFE, 0xFF, 0x17, 0x00, 0x46, 0x58, 0x4C, 0x43, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x50, 0xA0, 0x02, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x30, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0xF0, 0xF0, 0xF0, 0xF0, 
0x0F, 0x0F, 0x0F, 0x0F, 0xFF, 0xFF, 0x00, 0x00, 0x51, 0x00, 0x00, 0x05, 0x02, 0x00, 0x0F, 0xA0, 0x00, 0x00, 0x00, 0x3F, 0xD5, 0x78, 0xE9, 0x3D, 
0xA2, 0x45, 0x16, 0x3F, 0x87, 0x16, 0x99, 0x3E, 0x51, 0x00, 0x00, 0x05, 0x03, 0x00, 0x0F, 0xA0, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1F, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x03, 0xB0, 0x1F, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x90, 0x00, 0x08, 0x0F, 0xA0, 0x1F, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x90, 0x01, 0x08, 0x0F, 0xA0, 0x1F, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x00, 0x90, 0x02, 0x08, 0x0F, 0xA0, 0x01, 0x00, 0x00, 0x02, 0x00, 0x00, 0x03, 0x80, 0x02, 0x00, 0x00, 0xA0, 0x42, 0x00, 0x00, 0x03, 
0x00, 0x00, 0x0F, 0x80, 0x00, 0x00, 0xE4, 0x80, 0x01, 0x08, 0xE4, 0xA0, 0x42, 0x00, 0x00, 0x03, 0x01, 0x00, 0x0F, 0x80, 0x00, 0x00, 0xE4, 0xB0, 
0x00, 0x08, 0xE4, 0xA0, 0x42, 0x00, 0x00, 0x03, 0x02, 0x00, 0x0F, 0x80, 0x00, 0x00, 0xE4, 0xB0, 0x02, 0x08, 0xE4, 0xA0, 0x06, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80, 0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0xA0, 
0x02, 0x00, 0x00, 0x03, 0x01, 0x00, 0x0F, 0x80, 0x01, 0x00, 0xE4, 0x80, 0x02, 0x00, 0xE4, 0x80, 0x08, 0x00, 0x00, 0x03, 0x00, 0x00, 0x02, 0x80, 
0x01, 0x00, 0xE4, 0x80, 0x02, 0x00, 0x1B, 0xA0, 0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x04, 0x80, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x55, 0x80, 
0x04, 0x00, 0x00, 0x04, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x55, 0x80, 0x03, 0x00, 0x00, 0xA0, 0x06, 0x00, 0x00, 0x02, 
0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80, 0x01, 0x00, 0x00, 0x02, 0x02, 0x00, 0x01, 0x80, 0x03, 0x00, 0x00, 0xA0, 0x04, 0x00, 0x00, 0x04, 
0x00, 0x00, 0x02, 0x80, 0x00, 0x00, 0xAA, 0x80, 0x01, 0x00, 0x00, 0xA0, 0x02, 0x00, 0x00, 0x80, 0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x02, 0x80, 
0x00, 0x00, 0xAA, 0x80, 0x00, 0x00, 0x55, 0x80, 0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x01, 0x80, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x55, 0x80, 
0x05, 0x00, 0x00, 0x03, 0x00, 0x00, 0x0F, 0x80, 0x01, 0x00, 0xE4, 0x80, 0x00, 0x00, 0x00, 0x80, 0x01, 0x00, 0x00, 0x02, 0x00, 0x08, 0x0F, 0x80, 
0x00, 0x00, 0xE4, 0x80, 0xFF, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
0x01, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x42, 0x6C, 0x6F, 0x6F, 0x6D, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x00, 0x01, 0x00, 0x80, 
0xFF, 0xFF, 0xFF, 0xFF, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 
0x4C, 0x75, 0x6D, 0x69, 0x6E, 0x61, 0x6E, 0x63, 0x65, 0x54, 0x65, 0x78, 0x74, 0x75, 0x72, 0x65, 0x00, 0x7D, 0xB1, 0x02, 
            };
            HiDefEffectCode = ReachEffectCode;
#endif
        }
        #endregion
    }

#endif
}