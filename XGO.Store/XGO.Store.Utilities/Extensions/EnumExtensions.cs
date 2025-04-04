using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XGO.Store.Utilities.Extensions
{
    public static class EnumExtensions
    {
        public static string DescriptionAttr<T>(this T source) where T : Enum
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            FieldInfo fi = source.GetType()
                                 .GetField(source.ToString());

            if (fi == null) return string.Empty;

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
