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
    public sealed class LinearTransition<T> : Transition<T>
    {
        public override float Evaluate(float position)
        {
            return position;
        }
    }

    public sealed class ExponentialTransition<T> : Transition<T>
    {
        public float Power { get; set; }


        public ExponentialTransition()
        {
            Power = MathHelper.E;
        }

        public override float Evaluate(float position)
        {
            return (float)((Math.Pow(Power, position) - 1) / (Power - 1));
        }
    }

    public sealed class SinTransition<T> : Transition<T>
    {
        public override float Evaluate(float position)
        {
            return (float)Math.Sin((position * 2 - 1) * MathHelper.PiOver2) * 0.5f + 0.5f;
        }
    }

    public sealed class CurveTransition<T> : Transition<T>
    {
        private float minPosition;
        private float maxPosition;
        private float minValue;
        private float maxValue;

        private Curve curve;

        public Curve Curve
        {
            get { return curve; }
            set
            {
                curve = value;

                minPosition = float.MaxValue;
                maxPosition = float.MinValue;
                minValue = float.MaxValue;
                maxValue = float.MinValue;

                foreach (CurveKey key in curve.Keys)
                {
                    if (key.Position < minPosition)
                        minPosition = key.Position;
                    if (key.Position > maxPosition)
                        maxPosition = key.Position;
                    if (key.Value < minValue)
                        minValue = key.Value;
                    if (key.Value > maxValue)
                        maxValue = key.Value;
                }
            }
        }

        public override float Evaluate(float position)
        {
            return Curve != null ? 
                (Curve.Evaluate(minPosition + position * (maxPosition - minPosition)) - minValue) / (maxValue - minValue) : 0;
        }
    }
}