using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Core.Helper
{
    public static class EnumHelper
    {
        public static List<SelectEnumOption> GetEnumOptions<T>() where T : struct
        {
            List<SelectEnumOption> enumOptions = new List<SelectEnumOption>();

            foreach (var item in Enum.GetValues(typeof(T)))
            {
                enumOptions.Add(new SelectEnumOption
                {
                    Text = item.ToString(),
                    Value = (int)item
                });
            }

            return enumOptions;
        }
    }

    public class SelectEnumOption
    {
        public int Value { get; set; }

        public string Text { get; set; }
    }
}
