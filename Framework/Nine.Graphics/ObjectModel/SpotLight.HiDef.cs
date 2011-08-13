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
using Nine.Graphics;

#if !WINDOWS_PHONE
using Nine.Graphics.Effects;
using Nine.Graphics.Effects.Deferred;
#endif
#endregion

namespace Nine.Graphics.ObjectModel
{
    public partial class SpotLight : IDeferredLight
    {
        SpotLightEffect multiPasseffect;
        public override Effect Effect
        {
            get { return multiPasseffect ?? (multiPasseffect = GraphicsResources<SpotLightEffect>.GetInstance(GraphicsDevice)); }
        }

        DeferredSpotLight deferredLight;
        DeferredSpotLight GetDeferredLight()
        {
            return deferredLight ?? (deferredLight = GraphicsResources<DeferredSpotLight>.GetInstance(GraphicsDevice));
        }

        bool IDeferredLight.Contains(Vector3 point)
        {
            return ((IDeferredLight)GetDeferredLight()).Contains(point);
        }

        Effect IDeferredLight.Effect
        {
            get
            {
                GetDeferredLight();
                deferredLight.Position = Position;
                deferredLight.Direction = Direction;
                deferredLight.Range = Range;
                deferredLight.Attenuation = Attenuation;
                deferredLight.SpecularColor = SpecularColor;
                deferredLight.DiffuseColor = DiffuseColor;
                deferredLight.Falloff = Falloff;
                deferredLight.InnerAngle = InnerAngle;
                deferredLight.OuterAngle = OuterAngle;
                return ((IDeferredLight)GetDeferredLight()).Effect;
            }
        }

        IndexBuffer IDeferredLight.IndexBuffer
        {
            get { return ((IDeferredLight)GetDeferredLight()).IndexBuffer; }
        }

        VertexBuffer IDeferredLight.VertexBuffer
        {
            get { return ((IDeferredLight)GetDeferredLight()).VertexBuffer; }
        }
    }
}