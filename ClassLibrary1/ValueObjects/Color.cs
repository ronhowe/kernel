using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Exceptions;

namespace ClassLibrary1.Domain.ValueObjects
{
    public class Color : ValueObject
    {
        static Color()
        {
        }

        private Color()
        {
            Code = Color.Black;
        }

        private Color(string code)
        {
            Code = code;
        }

        public static Color From(string code)
        {
            var color = new Color { Code = code };

            if (!SupportedColors.Contains(color))
            {
                throw new UnsupportedColorException(code);
            }

            return color;
        }

        public static Color White => new("#FFFFFF");

        public static Color Red => new("#FF5733");

        public static Color Orange => new("#FFC300");

        public static Color Yellow => new("#FFFF66");

        public static Color Green => new("#CCFF99 ");

        public static Color Blue => new("#6666FF");

        public static Color Purple => new("#9966CC");

        public static Color Grey => new("#999999");

        public static Color Black => new("#000000");

        public string Code { get; private set; }

        public static implicit operator string(Color color)
        {
            return color.ToString();
        }

        public static explicit operator Color(string code)
        {
            return From(code);
        }

        public override string ToString()
        {
            return Code;
        }

        protected static IEnumerable<Color> SupportedColors
        {
            get
            {
                yield return White;
                yield return Red;
                yield return Orange;
                yield return Yellow;
                yield return Green;
                yield return Blue;
                yield return Purple;
                yield return Grey;
                yield return Black;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
