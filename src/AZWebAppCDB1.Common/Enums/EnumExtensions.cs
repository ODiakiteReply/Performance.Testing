using System;
using System.Reflection;
using System.ComponentModel;

namespace Common.Extensions.Enum
{
    public static class EnumExtensions
    {
        public static int ToInt(this System.Enum @enum)
        {
            return (int)(IConvertible)@enum;
        }
        public static long ToLong(this System.Enum @enum)
        {
            return (long)(IConvertible)@enum;
        }

        public static T ToEnum<T>(this string sString)
        {
            return ToEnumOr<T>(sString, () =>
                  throw new InvalidOperationException($"{sString} is not an underlying value of the enumerator {typeof(T)}"));
        }

        public static T ToEnumOr<T>(this string sString, Func<T> or)
        {
            T instance = (T)System.Enum.Parse(typeof(T), sString);
            if (!System.Enum.IsDefined(typeof(T), instance) && !instance.ToString().Contains(","))
                return or();
            return instance;
        }

        public static string GetEnumDescription(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
