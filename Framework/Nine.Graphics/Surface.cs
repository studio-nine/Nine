﻿namespace Nine.Graphics
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Markup;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Nine.Graphics.Drawing;
    using Nine.Graphics.Materials;

    /// <summary>
    /// A triangle mesh constructed from heightmap to represent game surface. 
    /// The up axis of the surface is Vector.UnitY.
    /// </summary>
    [ContentSerializable]
    [ContentProperty("Material")]
    public class Surface : Transformable, Nine.IContainer, ISceneObject, IDrawableObject, ISurface, IPickable, INotifyCollectionChanged<object>, IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the underlying GraphicsDevice.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Gets the patches that made up this surface.
        /// </summary>
        [ContentSerializerIgnore]
        public SurfacePatchCollection Patches
        {
            get { EnsureHeightmapUpToDate(); return patches; }
        }
        private SurfacePatchCollection patches;

        /// <summary>
        /// Gets the number of segments of each patch.
        /// </summary>
        public int PatchSegmentCount
        {
            get { return patchSegmentCount; }
            set { patchSegmentCount = value; heightmapNeedsUpdate = true; }
        }
        private int patchSegmentCount = 32;
        
        /// <summary>
        /// Gets the count of patches along the x axis.
        /// </summary>
        public int PatchCountX
        {
            get { EnsureHeightmapUpToDate(); return patchCountX; }
        }
        internal int patchCountX;

        /// <summary>
        /// Gets the count of patches along the z axis.
        /// </summary>
        public int PatchCountZ
        {
            get { EnsureHeightmapUpToDate(); return patchCountZ; }
        }
        internal int patchCountZ;

        /// <summary>
        /// Gets the number of the smallest square block in X axis, or heightmap texture U axis.
        /// </summary>
        public int SegmentCountX
        {
            get { EnsureHeightmapUpToDate(); return segmentCountX; }
        }
        private int segmentCountX;

        /// <summary>
        /// Gets the number of the smallest square block in Z axis, or heightmap texture V axis.
        /// </summary>
        public int SegmentCountZ
        {
            get { EnsureHeightmapUpToDate(); return segmentCountZ; }
        }
        private int segmentCountZ;

        /// <summary>
        /// Gets the size of the surface geometry in 3 axis.
        /// </summary>
        public Vector3 Size
        {
            get { return heightmap.Size; } 
        }

        /// <summary>
        /// Gets the step of the surface heightmap.
        /// </summary>
        public float Step
        {
            get { return heightmap.Step; }
        }

        /// <summary>
        /// Gets or sets the transform matrix for vertex uv coordinates.
        /// </summary>
        public Matrix TextureTransform 
        {
            get { return textureTransform; }
            set 
            {
                textureTransform = value;
                if (heightmap != null)
                    Invalidate();
                else
                    heightmapNeedsUpdate = true;
            }        
        }
        Matrix textureTransform = Matrix.Identity;
        
        /// <summary>
        /// Gets the current vertex type used by this surface.
        /// </summary>
        [ContentSerializerIgnore]
        public Type VertexType
        {
            get { return vertexType; }
            set
            {
                if (vertexType == null)
                    vertexType = typeof(VertexPositionNormalTexture);
                if (vertexType != value)
                {
                    vertexType = value;
                    heightmapNeedsUpdate = true;
                }
            }
        }
        private Type vertexType = typeof(VertexPositionNormalTexture);

        [ContentSerializer(ElementName = "VertexType")]
        internal string VertexTypeSerializer
        {
            get { return vertexType.AssemblyQualifiedName; }
            set 
            {
                var type = Type.GetType(value) ?? 
                           Type.GetType(string.Format("Nine.Graphics.{0}, Nine.Graphics", value)) ?? 
                           Type.GetType(string.Format("Microsoft.Xna.Framework.Graphics.{0}, Microsoft.Xna.Framework.Graphics", value));
                if (type == null)
                    throw new InvalidOperationException("Unknown vertex type " + value);
                VertexType = type;
            }
        }

        /// <summary>
        /// Gets the underlying heightmap that contains height, normal, tangent data.
        /// </summary>
        [ContentSerializer]
        public Heightmap Heightmap
        {
            get { return heightmap; }
            set
            {
                if (heightmap != value)
                {
                    if (heightmap != null)
                        heightmap.Invalidate -= Heightmap_Invalidate;
                    heightmap = value;
                    if (heightmap != null)
                        heightmap.Invalidate += Heightmap_Invalidate;
                    UpdateHeightmap();
                }
            }
        }
        internal Heightmap heightmap;
        private bool heightmapNeedsUpdate = true;

        /// <summary>
        /// Gets the max level of detail of this surface.
        /// </summary>
        public int MaxLevelOfDetail 
        {
            get { return Geometry.MaxLevelOfDetail; } 
        }

        /// <summary>
        /// Gets or sets the distance at which the surface starts to switch to a lower resolution geometry.
        /// </summary>
        public float LevelOfDetailStart { get; set; }

        /// <summary>
        /// Gets or sets the distance at which the surface has switched to the lowest resolution geometry.
        /// </summary>
        public float LevelOfDetailEnd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether level of detail is enabled.
        /// </summary>
        public bool LevelOfDetailEnabled
        {
            get { return Geometry.LevelOfDetailEnabled; }
            set { if (value) { Geometry.EnableLevelOfDetail(); } }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Surface"/> is visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the material of this drawable surface.
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// Gets a collection containning all the materials that are sorted based on level of detail.
        /// </summary>
        public MaterialLevelOfDetail MaterialLevels
        {
            get { return materialLevels; }
            set { materialLevels = value ?? new MaterialLevelOfDetail(); }
        }
        private MaterialLevelOfDetail materialLevels = new MaterialLevelOfDetail();

        /// <summary>
        /// Gets or sets a value indicating whether this object casts shadow.
        /// </summary>
        public bool CastShadow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this object receives shadow.
        /// </summary>
        public bool ReceiveShadow { get; set; }
        #endregion
        
        #region ISpatialQueryable
        /// <summary>
        /// Gets the local bottom left position of the surface.
        /// </summary>
        public Vector3 Position
        {
            get { return Transform.Translation; }
        }

        /// <summary>
        /// Gets the absolute bottom left position of the surface.
        /// </summary>
        public Vector3 AbsolutePosition
        {
            get { return AbsoluteTransform.Translation; }
        }

        /// <summary>
        /// Gets the local center position of the surface.
        /// </summary>
        public Vector3 Center
        {
            get { return Position + Size * 0.5f; }
        }

        /// <summary>
        /// Gets the absolute center position of the surface.
        /// </summary>
        public Vector3 AbsoluteCenter
        {
            get { return AbsolutePosition + Size * 0.5f; }
        }

        /// <summary>
        /// Called when local or absolute transform changed.
        /// </summary>
        protected override void OnTransformChanged()
        {
            if (patches != null)
                for (int i = 0; i < patches.Count; ++i)
                    patches[i].UpdatePosition();
        }

        /// <summary>
        /// Gets the axis aligned bounding box of this surface.
        /// </summary>
        public BoundingBox BoundingBox
        {
            get
            {
                EnsureHeightmapUpToDate();
                BoundingBox box;
                Vector3 absolutePosition = AbsolutePosition;
                box.Min = boundingBox.Min + absolutePosition;
                box.Max = boundingBox.Max + absolutePosition;
                return box;
            }
        }
        private BoundingBox boundingBox;
        #endregion

        #region Members
        /// <summary>
        /// Gets the underlying geometry.
        /// </summary>
        internal SurfaceGeometry Geometry
        {
            get { return geometry ?? (geometry = SurfaceGeometry.GetInstance(GraphicsDevice, PatchSegmentCount)); }
        }
        private SurfaceGeometry geometry;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes a new instance of the <see cref="Surface"/> class.
        /// </summary>
        public Surface(GraphicsDevice graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");

            GraphicsDevice = graphics;
            LevelOfDetailStart = 100;
            LevelOfDetailEnd = 1000;
            Visible = true;
            ReceiveShadow = true;
            Material = null;
        }

        /// <summary>
        /// Creates a new instance of Surface.
        /// The default vertex format is VertexPositionColorNormalTexture.
        /// </summary>
        /// <param name="graphics">Graphics device.</param>
        /// <param name="step">Size of the smallest square block that made up the surface.</param>
        /// <param name="segmentCountX">Number of the smallest square block in X axis, or heightmap texture U axis.</param>
        /// <param name="segmentCountY">Number of the smallest square block in Y axis, or heightmap texture V axis.</param>
        /// <param name="patchSegmentCount">Number of the smallest square block that made up the surface patch.</param>
        public Surface(GraphicsDevice graphics, float step, int segmentCountX, int segmentCountY, int patchSegmentCount)
            : this(graphics, new Heightmap(step, segmentCountX, segmentCountY), patchSegmentCount)
        { }
        
        /// <summary>
        /// Creates a new instance of Surface.
        /// The default vertex format is VertexPositionColorNormalTexture.
        /// </summary>
        /// <param name="graphics">Graphics device.</param>
        /// <param name="heightmap">The heightmap geometry to create from.</param>
        public Surface(GraphicsDevice graphics, Heightmap heightmap)
            : this(graphics, heightmap, 32)
        { }

        /// <summary>
        /// Creates a new instance of Surface.
        /// The default vertex format is VertexPositionColorNormalTexture.
        /// </summary>
        /// <param name="graphics">Graphics device.</param>
        /// <param name="heightmap">The heightmap geometry to create from.</param>
        /// <param name="patchSegmentCount">Number of the smallest square block that made up the surface patch.</param>
        public Surface(GraphicsDevice graphics, Heightmap heightmap, int patchSegmentCount)
            : this(graphics, heightmap, patchSegmentCount, null)
        { }

        /// <summary>
        /// Creates a new instance of Surface.
        /// The default vertex format is VertexPositionColorNormalTexture.
        /// </summary>
        /// <param name="graphics">Graphics device.</param>
        /// <param name="heightmap">The heightmap geometry to create from.</param>
        /// <param name="patchSegmentCount">Number of the smallest square block that made up the surface patch.</param>
        public Surface(GraphicsDevice graphics, Heightmap heightmap, int patchSegmentCount, Type vertexType)
            : this(graphics)
        {
            if (heightmap == null)
                throw new ArgumentNullException("heightmap");

            PatchSegmentCount = patchSegmentCount;
            VertexType = VertexType;

            if (heightmap != null)
                heightmap.Invalidate += Heightmap_Invalidate;
            this.heightmap = heightmap;
            this.heightmapNeedsUpdate = true;
        }

        internal void EnsureHeightmapUpToDate()
        {
            if (heightmapNeedsUpdate)
            {
                heightmapNeedsUpdate = false;
                UpdateHeightmap();
            }
        }

        private void UpdateHeightmap()
        {
            var removedPatches = patches;

            if (heightmap == null)
            {
                boundingBox = new BoundingBox();
                patchCountX = patchCountZ = 0;
                segmentCountX = segmentCountZ = 0;
                patches = null;

                if (removed != null && removedPatches != null)
                    foreach (var patch in removedPatches)
                        removed(patch);
                return;
            }

            if (patchSegmentCount < 2 || patchSegmentCount % 2 != 0 ||
                heightmap.Width % patchSegmentCount != 0 ||
                heightmap.Height % patchSegmentCount != 0)
            {
                throw new ArgumentOutOfRangeException(
                    "patchSegmentCount must be a even number, " +
                    "segmentCountX/segmentCountY must be a multiple of patchSegmentCount.");
            }

            boundingBox = heightmap.BoundingBox;

            // Create patches
            var newPatchCountX = heightmap.Width / patchSegmentCount;
            var newPatchCountZ = heightmap.Height / patchSegmentCount;

            // Invalid patches when patch count has changed
            if (newPatchCountX != patchCountX || newPatchCountZ != patchCountZ)
            {
                patches = null;
                if (removed != null && removedPatches != null)
                    foreach (var patch in removedPatches)
                        removed(patch);
            }

            patchCountX = newPatchCountX;
            patchCountZ = newPatchCountZ;

            // Store these values in case they change
            segmentCountX = heightmap.Width;
            segmentCountZ = heightmap.Height;

            // Convert vertex type
            if (vertexType == null || vertexType == typeof(VertexPositionNormalTexture))
                ConvertVertexType<VertexPositionNormalTexture>(PopulateVertex);
            else if (vertexType == typeof(VertexPositionColor))
                ConvertVertexType<VertexPositionColor>(PopulateVertex);
            else if (vertexType == typeof(VertexPositionTexture))
                ConvertVertexType<VertexPositionTexture>(PopulateVertex);
            else if (vertexType == typeof(VertexPositionNormal))
                ConvertVertexType<VertexPositionNormal>(PopulateVertex);
            else if (vertexType == typeof(VertexPositionNormalDualTexture))
                ConvertVertexType<VertexPositionNormalDualTexture>(PopulateVertex);
            else if (vertexType == typeof(VertexPositionNormalTangentBinormalTexture))
                ConvertVertexType<VertexPositionNormalTangentBinormalTexture>(PopulateVertex);
            else if (vertexType == typeof(VertexPositionColorNormalTexture))
                ConvertVertexType<VertexPositionColorNormalTexture>(PopulateVertex);
            else
                throw new NotSupportedException("Vertex type not supported. Try using Surface.ConvertVertexType<T> instead.");

            heightmapNeedsUpdate = false;
        }

        /// <summary>
        /// Converts and fills the surface vertex buffer to another vertex full.
        /// The default vertex format is VertexPositionColorNormalTexture.
        /// This method must be called immediately after the surface is created.
        /// </summary>
        public void ConvertVertexType<T>(SurfaceVertexConverter<T> fillVertex) where T : struct, IVertexType
        {
            if (fillVertex == null)
                throw new ArgumentNullException("fillVertex");

            if (vertexType != typeof(T) || patches == null)
            {
                vertexType = typeof(T);
                if (patches != null)
                {
                    foreach (SurfacePatch patch in patches)
                    {
                        if (removed != null)
                            removed(patch);
                        patch.Dispose();
                    }
                }

                var i = 0;
                var patchArray = new SurfacePatch[patchCountX * patchCountZ];

                for (int z = 0; z < patchCountZ; z++)
                    for (int x = 0; x < patchCountX; ++x)
                        patchArray[i++] = new SurfacePatch<T>(this, x, z);
                patches = new SurfacePatchCollection(this, patchArray);

                if (added != null)
                    foreach (SurfacePatch patch in patches)
                        added(patch);
            }

            foreach (SurfacePatch<T> patch in patches)
                patch.FillVertex = fillVertex;

            Invalidate();
        }

        private void Heightmap_Invalidate(object sender, EventArgs args)
        {
            Invalidate();
        }

        /// <summary>
        /// TODO: Passes dirty rectangle during heightmap invalidation.
        /// </summary>
        private void Invalidate()
        {
            if (heightmap != null)
            {
                boundingBox = heightmap.BoundingBox;
                if (patches != null)
                    foreach (SurfacePatch patch in patches)
                        patch.Invalidate();
            }
        }

        /// <summary>
        /// Populates a single vertex using default settings.
        /// </summary>
        internal void PopulateVertex(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref VertexPositionNormalTexture vertex)
        {
            Vector2 uv = new Vector2();

            int xSurface = (x + (xPatch * patchSegmentCount));
            int zSurface = (z + (zPatch * patchSegmentCount));

            // Texture will map to the whole surface by default.
            uv.X = 1.0f * xSurface / segmentCountX;
            uv.Y = 1.0f * zSurface / segmentCountZ;

            vertex.Position = heightmap.GetPosition(xSurface, zSurface);
            vertex.Normal = heightmap.GetNormal(xSurface, zSurface);
            vertex.TextureCoordinate = Nine.Graphics.TextureTransform.Transform(textureTransform, uv);
        }

        private void PopulateVertex(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref VertexPositionColor vertex)
        {
            vertex.Position = input.Position;
            vertex.Color = Color.White;
        }

        private void PopulateVertex(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref VertexPositionNormal vertex)
        {
            vertex.Position = input.Position;
            vertex.Normal = input.Normal;
        }

        private void PopulateVertex(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref VertexPositionTexture vertex)
        {
            vertex.Position = input.Position;
            vertex.TextureCoordinate = input.TextureCoordinate;
        }

        private void PopulateVertex(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref VertexPositionNormalDualTexture vertex)
        {
            vertex.Position = input.Position;
            vertex.Normal = input.Normal;
            vertex.TextureCoordinate = input.TextureCoordinate;
            vertex.TextureCoordinate1 = input.TextureCoordinate;
        }

        private void PopulateVertex(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref VertexPositionNormalTangentBinormalTexture vertex)
        {
            int xSurface = (x + (xPatch * patchSegmentCount));
            int ySurface = (z + (zPatch * patchSegmentCount));

            vertex.Position = input.Position;
            vertex.TextureCoordinate = input.TextureCoordinate;
            vertex.Normal = input.Normal;
            vertex.Tangent = Heightmap.GetTangent(xSurface, ySurface);

            Vector3.Cross(ref vertex.Normal, ref vertex.Tangent, out vertex.Binormal);
        }

        private void PopulateVertex(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref VertexPositionColorNormalTexture vertex)
        {
            vertex.Position = input.Position;
            vertex.Normal = input.Normal;
            vertex.Color = Color.White;
            vertex.TextureCoordinate = input.TextureCoordinate;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Manually updates the level of detail of each surface patch.
        /// </summary>
        /// <param name="eyePosition">The eye position.</param>
        /// <remarks>
        /// If you are draw the surface using scene, level of details are automatically updated.

        /// </remarks>
        public void UpdateLevelOfDetail(Vector3 eyePosition)
        {
            EnsureHeightmapUpToDate();

            if (!LevelOfDetailEnabled || heightmap == null)
                return;

            float start = Math.Min(LevelOfDetailStart, LevelOfDetailEnd);
            float end = Math.Max(LevelOfDetailStart, LevelOfDetailEnd);

            // Ensure the lod difference between adjacent patches does not exceeds 1.
            float distanceBetweenPatches = heightmap.Step * patchSegmentCount;
            float lodDistance = Math.Max(end - start, distanceBetweenPatches * MaxLevelOfDetail * 1.414f);

            float patchDistance;

            for (int i = 0; i < patches.Count; ++i)
            {
                Vector3.Distance(ref patches[i].center, ref eyePosition, out patchDistance);
                
                int patchLod = (int)(MaxLevelOfDetail * (patchDistance - start) / lodDistance);
                if (patchLod < 0)
                    patchLod = 0;
                else if (patchLod > MaxLevelOfDetail)
                    patchLod = MaxLevelOfDetail;
                
                patches[i].DetailLevel = patchLod;
            }

            for (int i = 0; i < patches.Count; ++i)
            {
                patches[i].UpdateLevelOfDetail();
            }
        }
        #endregion

        #region ISurface Members
        /// <summary>
        /// Gets the height.
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            return GetHeight(position.X, position.Z);
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public float GetHeight(float x, float z)
        {
            if (heightmap == null)
                return 0;
            return AbsolutePosition.Y + heightmap.GetHeight(x - AbsolutePosition.X, z - AbsolutePosition.Z);
        }

        /// <summary>
        /// Gets the normal.
        /// </summary>
        public Vector3 GetNormal(Vector3 position)
        {
            return GetNormal(position.X, position.Z);
        }

        /// <summary>
        /// Gets the normal.
        /// </summary>
        public Vector3 GetNormal(float x, float z)
        {
            if (heightmap == null)
                return Vector3.Zero;
            return heightmap.GetNormal(x - AbsolutePosition.X, z - AbsolutePosition.Z);
        }

        /// <summary>
        /// Gets the height and normal of the surface at a given location.
        /// </summary>
        /// <returns>False if the location is outside the boundary of the surface.</returns>
        public bool TryGetHeightAndNormal(Vector3 position, out float height, out Vector3 normal)
        {
            return TryGetHeightAndNormal(position.X, position.Z, out height, out normal);
        }

        /// <summary>
        /// Gets the height and normal of the surface at a given location.
        /// </summary>
        /// <returns>False if the location is outside the boundary of the surface.</returns>
        public bool TryGetHeightAndNormal(float x, float z, out float height, out Vector3 normal)
        {
            float baseHeight;
            if (heightmap != null && heightmap.TryGetHeightAndNormal(x - AbsolutePosition.X, z - AbsolutePosition.Z, out baseHeight, out normal))
            {
                height = baseHeight + AbsolutePosition.Y;
                return true;
            }
            normal = Vector3.Zero;
            height = float.MinValue;
            return false;
        }


        /// <summary>
        /// Gets the height of the surface at a given location.
        /// </summary>
        /// <returns>False if the location is outside the boundary of the surface.</returns>
        public bool TryGetHeight(float x, float z, out float height)
        {
            Vector3 normal;
            float baseHeight;
            if (heightmap != null && heightmap.TryGetHeightAndNormal(x - AbsolutePosition.X, z - AbsolutePosition.Z, true, false, out baseHeight, out normal))
            {
                height = baseHeight + AbsolutePosition.Y;
                return true;
            }
            height = float.MinValue;
            return false;
        }
        #endregion

        #region IPickable Members
        /// <summary>
        /// Determines whether the specified point is above this surface.
        /// </summary>
        public bool IsAbove(Vector3 point)
        {
            return GetHeight(point.X, point.Z) <= point.Y;
        }

        /// <summary>
        /// Points under the heightmap and are within the boundary are picked.
        /// </summary>
        bool IPickable.Contains(Vector3 point)
        {
            return BoundingBox.Contains(point) == ContainmentType.Contains;
        }
        
        /// <summary>
        /// Checks whether a ray intersects the surface mesh.
        /// </summary>
        public float? Intersects(Ray ray)
        {
            float height;
            Vector3 start = ray.Position;
            Vector3 pickStep = ray.Direction * heightmap.Step * 0.5f;

            float? intersection = ray.Intersects(BoundingBox);
            if (intersection.HasValue)
            {
                if (ray.Direction.X == 0 && ray.Direction.Z == 0)
                {
                    if (TryGetHeight(ray.Position.X, ray.Position.Z, out height))
                    {
                        float distance = ray.Position.Y - height;
                        if (distance * ray.Direction.Y < 0)
                            return distance;
                    }
                    return null;
                }

                bool? isAboveLastTime = null;
                start += ray.Direction * intersection.Value;
                while (true)
                {
                    if (!TryGetHeight(start.X, start.Z, out height))
                        return null;

                    bool isAbove = (height <= start.Y);
                    if (isAboveLastTime == null)
                    {
                        if (!isAbove)
                            break;
                        isAboveLastTime = isAbove;
                    }
                    else if (isAboveLastTime.Value && !isAbove)
                    {
                        float distance;
                        Vector3.Multiply(ref pickStep, -0.5f, out pickStep);
                        Vector3.Subtract(ref start, ref pickStep, out start);
                        Vector3.Distance(ref ray.Position, ref start, out distance);
                        return distance;
                    }
                    Vector3.Add(ref start, ref pickStep, out start);
                }
            }
            return null;
        }
        #endregion

        #region IDrawable
        Material IDrawableObject.Material { get { return null; } }

        /// <summary>
        /// We only want to hook to the pre draw event to update level of detail.
        /// </summary>
        void IDrawableObject.OnAddedToView(DrawingContext context)
        {
            UpdateLevelOfDetail(context.CameraPosition);
        }

        void IDrawableObject.Draw(DrawingContext context, Material material) 
        {

        }

        float IDrawableObject.GetDistanceToCamera(Vector3 cameraPosition)
        {
            return 0; 
        }
        #endregion

        #region IContainer
        IList Nine.IContainer.Children
        {
            get { return patches; }
        }
        #endregion

        #region INotifyCollectionChanged
        private Action<object> added;
        private Action<object> removed;
        
        event Action<object> INotifyCollectionChanged<object>.Added
        {
            add { added += value; }
            remove { added -= value; }
        }

        event Action<object> INotifyCollectionChanged<object>.Removed
        {
            add { removed += value; }
            remove { removed -= value; }
        }
        #endregion

        #region ISceneObject
        void ISceneObject.OnAdded(DrawingContext context) 
        {
            EnsureHeightmapUpToDate();
        }
        void ISceneObject.OnRemoved(DrawingContext context) { }
        #endregion

        #region IDisposable
        /// <summary>
        /// Disposes any resources associated with this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (patches != null)
                    foreach (SurfacePatch patch in patches)
                        patch.Dispose();
            }
        }

        ~Surface()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// Fills a vertex data in a drawable surface.
    /// </summary>
    /// <typeparam name="T">The target vertex type.</typeparam>
    /// <param name="x">The x index of the vertex on the target patch, ranged from 0 to PatchSegmentCount inclusive.</param>
    /// <param name="z">The z index of the vertex on the target patch, ranged from 0 to PatchSegmentCount inclusive.</param>
    /// <param name="xPatch">The x index of the target patch.</param>
    /// <param name="zPatch">The z index of the target patch.</param>
    /// <param name="input">The input vertex contains the default position, normal and texture coordinates for the target vertex.</param>
    /// <param name="output">The output vertex to be set.</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void SurfaceVertexConverter<T>(int xPatch, int zPatch, int x, int z, ref VertexPositionNormalTexture input, ref T output);
}
