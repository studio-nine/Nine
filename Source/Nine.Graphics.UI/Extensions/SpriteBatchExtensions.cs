﻿namespace Nine.Graphics
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Nine.Graphics.UI.Media;

    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, Rectangle rect, Color c)
        {
            var Texture = Nine.Graphics.GraphicsResources<BlankTexture>.GetInstance(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(Texture.Texture, rect, c);
        }

        public static void Draw(this SpriteBatch spriteBatch, BoundingRectangle rect, Brush c)
        {
            // I would say this is a temporarily way of drawing
            if (c is SolidColorBrush)
            {
                var Texture = Nine.Graphics.GraphicsResources<BlankTexture>.GetInstance(spriteBatch.GraphicsDevice);
                // Absolute Rendering
                spriteBatch.Draw(Texture.Texture,
                    new Vector2(rect.X, rect.Y), null, (c as SolidColorBrush).Color, 0, 
                    Vector2.Zero, new Vector2(rect.Width, rect.Height), SpriteEffects.None, 0);
                //spriteBatch.Draw(Texture.Texture, rect, (c as SolidColorBrush).Color);
            }
            else if (c is ImageBrush)
            {
                var ImageBrush = c as ImageBrush;
                var Rect = ImageBrush.Calculate(ImageBrush.Source ,rect);
                spriteBatch.Draw(ImageBrush.Source, Rect, null, Color.White, 0, Vector2.Zero, ImageBrush.Effects, 0);
            }
        }
    }
}