using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace DataSpider.Common
{
    public class EnumDescription : Attribute
    {
        public string Text
        {
            get;
            private set;
        }

        public EnumDescription(string text)
        {
            Text = text;
        }

        public static string GetText(Enum en)
        {
            Type type = en.GetType();
            FieldInfo fieldInfo = type.GetField(en.ToString());
            if (fieldInfo != null)
            {
                object[] attrs = fieldInfo.GetCustomAttributes(typeof(EnumDescription), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((EnumDescription)attrs[0]).Text;
                }
            }

            return en.ToString();
        }

        public static object Parse(Type type, string descText)
        {
            FieldInfo[] fieldInfos = type.GetFields();
            if (fieldInfos == null)
            {
                return null;
            }

            for (int i = 0; i < fieldInfos.Length; ++i)
            {
                object[] attrs = fieldInfos[i].GetCustomAttributes(typeof(EnumDescription), false);
                if (attrs != null && attrs.Length > 0 && ((EnumDescription)attrs[0]).Text == descText)
                {
                    return Enum.Parse(type, fieldInfos[i].Name);
                }
            }

            return null;
        }
    }
}
