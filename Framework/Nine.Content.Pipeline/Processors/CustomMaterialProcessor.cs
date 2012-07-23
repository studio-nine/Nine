﻿#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Graphics;
using Nine.Content.Pipeline.Graphics;
using Nine.Content.Pipeline.Graphics.Materials;
using Nine.Graphics.Materials;
using Nine.Content.Pipeline.Xaml;
using System.Text;
using System.Security.Cryptography;

#endregion

namespace Nine.Content.Pipeline.Processors
{
    [DefaultContentProcessor]    
    public class CustomMaterialProcessor : ContentProcessor<CustomMaterial, CustomMaterial>
    {
        public override CustomMaterial Process(CustomMaterial input, ContentProcessorContext context)
        {
            if (!string.IsNullOrEmpty(input.Code))
            {
                if (input.Source != null)
                {
                    context.Logger.LogWarning(null, null, "Replacing custom material shaders");
                    input.Source = null;
                }
                
                var hashString = new StringBuilder();
                var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input.Code));
                for (int i = 0; i < hash.Length; i++)
                {
                    hashString.Append(hash[i].ToString("X2"));
                }

                var name = hashString.ToString().ToUpperInvariant();
                var assetName = Path.Combine(ContentProcessorContextExtensions.DefaultOutputDirectory, name);
                var sourceFile = Path.Combine(context.IntermediateDirectory, input.GetType().Name + "-" + name + ".fx");

                File.WriteAllText(sourceFile, input.Code);

                var source = context.BuildAsset<EffectContent, CustomEffectContent>(
                    new ExternalReference<EffectContent>(sourceFile), "CustomEffectProcessor", null, null, assetName);
                
                XamlSerializer.SerializationData[new PropertyInstance(input, "Source")] =
                    new ContentReference<CompiledEffectContent>(source.Filename);
                input.Code = null;
            }
            return input;
        }
    }
}