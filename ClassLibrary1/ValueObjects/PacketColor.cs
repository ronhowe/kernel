using ClassLibrary1.Domain.Common;
using ClassLibrary1.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace ClassLibrary1.Domain.ValueObjects
{
    public class PacketColor : ValueObject
    {
        public PacketColor()
        {
            Code = PacketColor.Black;
        }

        [JsonConstructor]
        public PacketColor(string code)
        {
            Code = code;
        }

        public static PacketColor From(string code)
        {
            var color = new PacketColor { Code = code };

            if (!SupportedColors.Contains(color))
            {
                throw new UnsupportedColorException(code);
            }

            return color;
        }

        public static PacketColor White => new("#FFFFFF");

        public static PacketColor Red => new("#FF5733");

        public static PacketColor Orange => new("#FFC300");

        public static PacketColor Yellow => new("#FFFF66");

        public static PacketColor Green => new("#CCFF99 ");

        public static PacketColor Blue => new("#6666FF");

        public static PacketColor Purple => new("#9966CC");

        public static PacketColor Grey => new("#999999");

        public static PacketColor Black => new("#000000");

        public string Code { get; set; }

        public static implicit operator string(PacketColor color)
        {
            return color.ToString();
        }

        public static explicit operator PacketColor(string code)
        {
            return From(code);
        }

        public override string ToString()
        {
            return Code;
        }

        protected static IEnumerable<PacketColor> SupportedColors
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
