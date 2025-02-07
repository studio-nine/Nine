#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace Nine.Graphics.UI.Internal
{
    using Microsoft.Xna.Framework;

    internal static class Vector2Extensions
    {
        public static Vector2 Deflate(this Vector2 size, Thickness thickness)
        {
            return new Vector2(
                (size.X - (thickness.Left + thickness.Right)).EnsurePositive(),
                (size.Y - (thickness.Top + thickness.Bottom)).EnsurePositive());
        }

        public static Vector2 Inflate(this Vector2 size, Thickness thickness)
        {
            return new Vector2(
                (size.X + (thickness.Left + thickness.Right)).EnsurePositive(),
                (size.Y + (thickness.Top + thickness.Bottom)).EnsurePositive());
        }

        public static bool IsCloseTo(this Vector2 value1, Vector2 value2)
        {
            return !value1.IsDifferentFrom(value2);
        }

        public static bool IsDifferentFrom(this Vector2 value1, Vector2 value2)
        {
            return value1.X.IsDifferentFrom(value2.X) || value1.Y.IsDifferentFrom(value2.Y);
        }
    }
}
