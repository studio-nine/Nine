﻿#region Copyright 2009 - 2010 (c) Nightin Games
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Nightin Games. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
#endregion

namespace Isles.Pipeline
{
    /// <summary>
    /// Build-time type used to hold the output data from the SpriteSheetProcessor.
    /// This is saved into XNB format by the SpriteSheetWriter helper class, then
    /// at runtime, the SpriteSheetReader loads the data into a SpriteSheet object.
    /// </summary>
    public class ImageListContent
    {
        // Single texture contains many separate sprite images.
        public List<Texture2DContent> Textures { get; set; }

        // Remember where in the texture each sprite has been placed.
        public List<Rectangle> SpriteRectangles { get; set; }

        // Index to each of the texture.
        public List<int> SpriteTextures { get; set; }

        // Store the original sprite filenames, so we can look up sprites by name.
        public Dictionary<string, int> SpriteNames { get; set; }


        public ImageListContent()
        {
            Textures = new List<Texture2DContent>();
            SpriteRectangles = new List<Rectangle>();
            SpriteTextures = new List<int>();
            SpriteNames = new Dictionary<string, int>();
        }
    }


    /// <summary>
    /// Content pipeline support class for saving sprite sheet data into XNB format.
    /// </summary>
    [ContentTypeWriter]
    public class ImageListWriter : ContentTypeWriter<ImageListContent>
    {
        /// <summary>
        /// Saves sprite sheet data into an XNB file.
        /// </summary>
        protected override void Write(ContentWriter output, ImageListContent value)
        {
            output.WriteObject(value.Textures.ToArray());
            output.WriteObject(value.SpriteRectangles.ToArray());
            output.WriteObject(value.SpriteTextures.ToArray());
            output.WriteObject(value.SpriteNames);
        }


        /// <summary>
        /// Tells the content pipeline what worker type
        /// will be used to load the sprite sheet data.
        /// </summary>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(Isles.Graphics.ImageListReader).AssemblyQualifiedName;
        }


        /// <summary>
        /// Tells the content pipeline what CLR type the sprite sheet
        /// data will be loaded into at runtime.
        /// </summary>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Isles.Graphics.ImageList).AssemblyQualifiedName;
        }
    }
}
