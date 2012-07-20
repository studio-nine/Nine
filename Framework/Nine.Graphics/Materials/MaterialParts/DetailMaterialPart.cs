﻿namespace Nine.Graphics.Materials.MaterialParts
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Nine.Graphics.Drawing;

    [ContentSerializable]
    public class DetailMaterialPart : MaterialPart
    {
        private EffectParameter textureParameter;
        private EffectParameter detailTextureScaleParameter;

        public Texture2D DetailTexture { get; set; }

        public Vector2 DetailTextureScale
        {
            get { return detailTextureScale.HasValue ? detailTextureScale.Value : Vector2.One; }
            set { detailTextureScale = (value == Vector2.One ? (Vector2?)null : value); }
        }
        private Vector2? detailTextureScale;

        protected internal override void OnBind()
        {
            if ((textureParameter = GetParameter("Texture")) == null)
                MaterialGroup.MaterialParts.Remove(this);
            if ((detailTextureScaleParameter = GetParameter("DetailTextureScale")) == null)
                MaterialGroup.MaterialParts.Remove(this);
        }

        protected internal override void BeginApplyLocalParameters(DrawingContext context, MaterialGroup material)
        {
            if (detailTextureScale.HasValue)
                detailTextureScaleParameter.SetValue(detailTextureScale.Value);
            textureParameter.SetValue(DetailTexture);
        }

        protected internal override void EndApplyLocalParameters()
        {
            if (detailTextureScale.HasValue)
                detailTextureScaleParameter.SetValue(Vector2.One);
        }

        protected internal override MaterialPart Clone()
        {
            return new DetailMaterialPart()
            {
                DetailTexture = this.DetailTexture,
                DetailTextureScale = this.DetailTextureScale,
            };
        }

        public override void SetTexture(TextureUsage usage, Texture texture)
        {
            if (usage == TextureUsage.Detail)
                DetailTexture = texture as Texture2D;
        }

        protected internal override string GetShaderCode(MaterialUsage usage)
        {
            return usage == MaterialUsage.Default ? GetShaderCode("DetailTexture") : null;
        }
    }
}
