using CMS.Storage.Dtos.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace CMS.Business.Helper
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = string.Empty;
            Type type = e.GetType();
            Array values = Enum.GetValues(type);

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

        public static List<LookupDto> GetEnumLookup(Type type)
        {
            var list = new List<LookupDto>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    var model = new LookupDto()
                    {
                        Id = (int)field.GetRawConstantValue(),
                        Name = fd.Description
                    };
                    list.Add(model);
                }
            }
            return list;
        }
    }
}
