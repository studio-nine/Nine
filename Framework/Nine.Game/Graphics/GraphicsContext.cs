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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Nine.Graphics.ParticleEffects;
#endregion

namespace Nine.Graphics
{
    /// <summary>
    /// Defines a context that contains commonly used parameters for rendering.
    /// </summary>
    public class GraphicsContext
    {
        /// <summary>
        /// Gets the underlying graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Gets the graphics settings
        /// </summary>
        public GraphicsSettings Settings
        {
            get { return settings ?? (settings = new GraphicsSettings()); }
            protected set { settings = value; }
        }
        private GraphicsSettings settings;

        /// <summary>
        /// Gets the sprite batch to draw strings and 2D textures.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch ?? (spriteBatch = new SpriteBatch(GraphicsDevice)); }
        }
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Gets the model batch to draw models and custom geometries.
        /// </summary>
        public ModelBatch ModelBatch
        {
            get { return modelBatch ?? (modelBatch = new ModelBatch(GraphicsDevice)); }
        }
        private ModelBatch modelBatch;

        /// <summary>
        /// Gets the primitive batch to draw 3D dynamic primitives.
        /// </summary>
        public PrimitiveBatch PrimitiveBatch
        {
            get { return primitiveBatch ?? (primitiveBatch = new PrimitiveBatch(GraphicsDevice)); }
        }
        private PrimitiveBatch primitiveBatch;

        /// <summary>
        /// Gets the particle batch to draw 3D particle system effects.
        /// </summary>
        public ParticleBatch ParticleBatch
        {
            get { return particleBatch ?? (particleBatch = new ParticleBatch(GraphicsDevice)); }
        }
        private ParticleBatch particleBatch;

        /// <summary>
        /// Gets the elapsed time since last update.
        /// </summary>
        public TimeSpan ElapsedTime { get; internal set; }

        /// <summary>
        /// Gets the elapsed seconds since last update.
        /// </summary>
        public float ElapsedSeconds { get; internal set; }

        /// <summary>
        /// Gets the view matrix for this drawing operation.
        /// </summary>
        public Matrix View
        {
            get { return view; }
            set { view = value; frustumNeedsUpdate = true; }
        }

        /// <summary>
        /// Gets the projection matrix for this drawing operation.
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; frustumNeedsUpdate = true; }
        }

        /// <summary>
        /// Gets the view frustum for this drawing operation.
        /// </summary>
        public BoundingFrustum ViewFrustum
        {
            get
            {
                if (frustumNeedsUpdate)
                {
                    frustumNeedsUpdate = false;
                    frustum.Matrix = view * projection;
                }
                return frustum;
            }
        }

        private Matrix view;
        private Matrix projection;
        private BoundingFrustum frustum = new BoundingFrustum(Matrix.Identity);
        private bool frustumNeedsUpdate = true;

        /// <summary>
        /// Initializes a new instance of <c>GraphicsContext</c>.
        /// </summary>
        /// <param name="graphics"></param>
        protected internal GraphicsContext(GraphicsDevice graphics, GraphicsSettings settings)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");

            Settings = settings;
            GraphicsDevice = graphics;
        }
    }
}