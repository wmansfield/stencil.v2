using Android.Graphics;
using Android.Text.Style;

namespace Stencil.Forms.Droid.Controls
{
    public class CustomTypefaceSpan : MetricAffectingSpan
    {
        public CustomTypefaceSpan(Typeface typeface, float textSize)
        {
            _typeface = typeface;
            _textSize = textSize;
        }

        private Typeface _typeface;
        private float _textSize;

        public override void UpdateDrawState(Android.Text.TextPaint drawState)
        {
            this.Apply(drawState);
        }
        public override void UpdateMeasureState(Android.Text.TextPaint drawState)
        {
            this.Apply(drawState);
        }

        private void Apply(Paint paint)
        {
            Typeface oldTypeface = paint.Typeface;
            TypefaceStyle oldStyle = oldTypeface != null ? oldTypeface.Style : 0;
            TypefaceStyle fakeStyle = oldStyle & ~_typeface.Style;

            if ((fakeStyle & TypefaceStyle.Bold) != 0)
            {
                paint.FakeBoldText = true;
            }

            if ((fakeStyle & TypefaceStyle.Italic) != 0)
            {
                paint.TextSkewX = -0.25f;
            }

            paint.SetTypeface(_typeface);
            if (_textSize != 0)
            {
                paint.TextSize = _textSize;
            }
        }
    }
}