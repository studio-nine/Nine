#region Using Statements
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Nine.Graphics.Effects
{
#if !WINDOWS_PHONE

    public partial class PointLightEffect : IEffectLights<IPointLight>, IEffectMatrices, IEffectTexture, IEffectSkinned
    {
        private Matrix view;
        private Matrix projection;

        public bool SkinningEnabled
        {
            get { return shaderIndex == 0; }
            set { shaderIndex = value ? 1 : 0; }
        }

        public Matrix View
        {
            get { return view; }
            set { view = value; dirtyFlag |= worldViewProjectionDirtyFlag; }
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; dirtyFlag |= worldViewProjectionDirtyFlag; }
        }

        public Texture2D Texture
        {
            get { return diffuseTexture; }
            set { diffuseTexture = value; diffuseTextureEnabled = (value != null); }
        }

        int MaxLights
        {
            get { return GraphicsDevice.GraphicsProfile == GraphicsProfile.Reach ? 1 : 4; }
        }

        public ReadOnlyCollection<IPointLight> Lights { get; private set; }

        private void OnCreated()
        {
            numLights = MaxLights;
            SpecularPower = 16;
            Lights = new ReadOnlyCollection<IPointLight>(lights);
            lights.ForEach(light =>
            {
                light.DiffuseColor = Vector3.One;
                light.Range = 10;
                light.Attenuation = MathHelper.E;
            });
        }

        private void OnClone(PointLightEffect cloneSource)
        {
            view = cloneSource.view;
            projection = cloneSource.projection;
        }

        private void OnApplyChanges()
        {
            if ((dirtyFlag & worldViewProjectionDirtyFlag) != 0 ||
                (dirtyFlag & WorldDirtyFlag) != 0)
            {
                Matrix wvp;
                Matrix.Multiply(ref _World, ref view, out wvp);
                Matrix.Multiply(ref wvp, ref projection, out wvp);
                worldViewProjection = wvp;

                Matrix viewInverse;
                Matrix.Invert(ref view, out viewInverse);
                eyePosition = viewInverse.Translation;
            }

            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i].DiffuseColor == Vector3.Zero &&
                    lights[i].SpecularColor == Vector3.Zero)
                {
                    numLights = i;
                    break;
                }
            }
        }

        void IEffectTexture.SetTexture(string name, Texture texture)
        {
            if (name == TextureNames.Diffuse)
                Texture = texture as Texture2D;
        }

        public void SetBoneTransforms(Matrix[] boneTransforms)
        {
            bones = boneTransforms;
        }

        partial class Class_lights : IPointLight { }
    }

#endif
}

