﻿#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Nine.Graphics.ParticleEffects
{
    /// <summary>
    /// Defines a basic controller that changes the color of the particle
    /// effect based on time.
    /// </summary>
    public class ColorController : ParticleController<Range<Color>>
    {
        /// <summary>
        /// Range of values controlling the particle end color and alpha. 
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Range<Color> EndColor { get; set; }

        public ColorController()
        {
            EndColor = Color.White;
        }

        public override Vector3 Border { get { return Vector3.Zero; } }

        protected override void OnReset(ref Particle particle, ref Range<Color> tag)
        {
            tag.Min = particle.Color;
            tag.Max = Color.Lerp(EndColor.Min, EndColor.Max, (float)Random.NextDouble());
        }

        protected override void OnUpdate(float elapsedTime, ref Particle particle, ref Range<Color> tag)
        {
            particle.Color = Color.Lerp(tag.Min, tag.Max, particle.Age);
        }
    }

    /// <summary>
    /// Defines a basic controller that fade the particle in and out.
    /// </summary>
    public class FadeController : ParticleController
    {
        public override Vector3 Border{ get { return Vector3.Zero; } }

        protected override void OnReset(ref Particle particle) { }

        protected override void OnUpdate(float elapsedTime, ref Particle particle)
        {
            particle.Alpha = particle.Age * (1 - particle.Age) * (1 - particle.Age) * 6.7f;
        }
    }

    /// <summary>
    /// Defines a basic controller that changes the size of the particle
    /// effect based on time.
    /// </summary>
    public class SizeController : ParticleController<float>
    {
        /// <summary>
        /// Range of values controlling the particle end size.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Range<float> EndSize { get; set; }

        public SizeController()
        {
            EndSize = 1;
        }

        public override Vector3 Border { get { return Vector3.Zero; } }

        protected override void OnReset(ref Particle particle, ref float tag)
        {
            tag = (MathHelper.Lerp(EndSize.Min, EndSize.Max, (float)Random.NextDouble()) - particle.Size) / particle.Duration;
        }

        protected override void OnUpdate(float elapsedTime, ref Particle particle, ref float tag)
        {
            particle.Size += tag * elapsedTime;
        }
    }

    /// <summary>
    /// Defines a basic controller that changes the rotation of the particle
    /// effect based on time.
    /// </summary>
    public class RotationController : ParticleController<float>
    {
        /// <summary>
        /// Range of values controlling the particle end rotation.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Range<float> EndRotation { get; set; }

        public override Vector3 Border { get { return Vector3.Zero; } }

        protected override void OnReset(ref Particle particle, ref float tag)
        {
            tag = (MathHelper.Lerp(EndRotation.Min, EndRotation.Max, (float)Random.NextDouble()) - particle.Rotation) / particle.Duration;
        }

        protected override void OnUpdate(float elapsedTime, ref Particle particle, ref float tag)
        {
            particle.Rotation += tag * elapsedTime;
        }
    }

    /// <summary>
    /// Defines a basic controller that controls the acceleration of the particle effect.
    /// </summary>
    public class SpeedController : ParticleController<float>
    {
        /// <summary>
        /// Range of values representing the particle end speed in proportion to its start speed.
        /// A value of 1 means no change, a value of 0 means complete stop.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Range<float> EndSpeed { get; set; }

        public override Vector3 Border 
        {
            get 
            {
                return Vector3.One * (ParticleEffect.Speed.Max + EndSpeed.Max) * 0.5f * ParticleEffect.Duration.Max; 
            }
        }

        protected override void OnReset(ref Particle particle, ref float tag)
        {
            tag = MathHelper.Lerp(EndSpeed.Min, EndSpeed.Max, (float)Random.NextDouble());
        }

        protected override void OnUpdate(float elapsedTime, ref Particle particle, ref float tag)
        {
            float timeLeft = particle.Duration - particle.ElapsedTime;
            particle.Velocity -= particle.Velocity * (1 - tag) * (elapsedTime / timeLeft);
        }
    }
    
    /// <summary>
    /// Defines a basic controller that applies a constant linear force on the particle effect.
    /// </summary>
    public class ForceController : ParticleController
    {
        /// <summary>
        /// Gets or sets the force amount.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Vector3 Force { get; set; }

        public ForceController()
        {
            Force = -Vector3.UnitZ;
        }

        public override Vector3 Border
        {
            get 
            {
                Vector3 border = Force;
                border.X = Math.Abs(border.X);
                border.Y = Math.Abs(border.Y);
                border.Z = Math.Abs(border.Z);
                return Vector3.One * ParticleEffect.Speed.Max * ParticleEffect.Duration.Max +
                       border * ParticleEffect.Duration.Max * ParticleEffect.Duration.Max * 0.5f; 
            }
        }

        protected override void OnReset(ref Particle particle) { }

        protected override void OnUpdate(float elapsedTime, ref Particle particle)
        {
            particle.Velocity += Force * elapsedTime;
        }
    }

    /// <summary>
    /// Defines a basic controller that applies a constant tangent force on the particle effect.
    /// </summary>
    public class TangentForceController : ParticleController
    {
        /// <summary>
        /// Gets or sets the force amount.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Force { get; set; }

        /// <summary>
        /// Gets or sets the up axis.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Vector3 Up { get; set; }

        /// <summary>
        /// Gets or sets the center position.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Vector3 Center { get; set; }

        public TangentForceController()
        {
            Up = Vector3.UnitZ;
        }

        public override Vector3 Border { get { return Vector3.Zero; } }

        protected override void OnReset(ref Particle particle) { }

        protected override void OnUpdate(float elapsedTime, ref Particle particle)
        {
            Vector3 normal = particle.Position - Center;
            Vector3 force = Vector3.Cross(Up, normal);
            Vector3.Normalize(ref force, out force);

            particle.Velocity += force * (Force * elapsedTime);
        }
    }

    /// <summary>
    /// Defines a basic controller that absorbs the particles to a given point.
    /// </summary>
    public class AbsorbController : ParticleController
    {
        /// <summary>
        /// Gets or sets the absorb position.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the absorb force.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Force { get; set; }

        public override Vector3 Border { get { return Vector3.Zero; } }

        protected override void OnReset(ref Particle particle) { }

        protected override void OnUpdate(float elapsedTime, ref Particle particle)
        {
            particle.Velocity += Vector3.Normalize(Position - particle.Position) * Force * elapsedTime;
        }
    }
}