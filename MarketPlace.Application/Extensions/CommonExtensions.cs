using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Extensions
{
    public static class CommonExtensions
    {
        public static string GetEnumName(this System.Enum myEnum)
        {
            var getEnum = myEnum.GetType().GetMember(myEnum.ToString()).FirstOrDefault();
            if (getEnum != null)
            {
                return getEnum.GetCustomAttribute<DisplayAttribute>()?.GetName();
            }
            return "";
        }
    }
}
