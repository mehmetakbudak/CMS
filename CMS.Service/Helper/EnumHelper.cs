using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace CMS.Service.Helper
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = string.Empty;
            Type type = e.GetType();
            Array values = System.Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val));
                    var descriptionAttribute = memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() as DescriptionAttribute;

                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                }
            }
            return description;
        }
    }
}
