using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Contas.Core.ViewModels;

namespace Contas.Core.Extensions
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            return value.GetType()
                            .GetMember(value.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static IEnumerable<DropdownViewModel> EnumToList<T>()
        {
            Type value = typeof(T);

            var values = Enum.GetValues(value);

            List<DropdownViewModel> enums = new List<DropdownViewModel>();

            foreach (int val in values)
            {
                var enumValue = (T)Enum.Parse(value, val.ToString()) as Enum;

                enums.Add(new DropdownViewModel(val, enumValue.GetDisplayName()));
            }

            return enums;
        }

        public static string GetDisplayName(this Enum value)
        {
            return value.GetAttribute<DisplayAttribute>().Name;
        }
    }
}
