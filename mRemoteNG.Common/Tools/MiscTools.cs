﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using mRemoteNG.Messages;
using mRemoteNG.Resources.Language;
using System.Runtime.Versioning;
using mRemoteNG.App;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class MiscTools
    {
        public static string LeadingZero(string Number)
        {
            if (Convert.ToInt32(Number) < 10)
            {
                return "0" + Number;
            }

            return Number;
        }

        public static bool GetBooleanValue(object dataObject)
        {
            Type type = dataObject.GetType();

            if (type == typeof(bool))
            {
                return (bool)dataObject;
            }
            if (type == typeof(string))
            {
                return (string)dataObject == "1";
            }
            if (type == typeof(sbyte))
            {
                return (sbyte)dataObject == 1;
            }

            RuntimeCommon.MessageCollector.AddMessage(MessageClass.ErrorMsg, $"Conversion of object to boolean failed because the type, {type}, is not handled.");
            return false;
        }

        public static string PrepareValueForDB(string Text)
        {
            return Text.Replace("\'", "\'\'");
        }

        public static string GetExceptionMessageRecursive(Exception ex)
        {
            return GetExceptionMessageRecursive(ex, Environment.NewLine);
        }

        private static string GetExceptionMessageRecursive(Exception ex, string separator)
        {
            var message = ex.Message;
            if (ex.InnerException == null) return message;
            var innerMessage = GetExceptionMessageRecursive(ex.InnerException, separator);
            message = String.Join(separator, message, innerMessage);
            return message;
        }


        public class EnumTypeConverter : EnumConverter
        {
            private readonly Type _enumType;

            public EnumTypeConverter(Type type) : base(type)
            {
                _enumType = type;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
            {
                return destType == typeof(string);
            }

            public override object ConvertTo(ITypeDescriptorContext context,
                                             CultureInfo culture,
                                             object value,
                                             Type destType)
            {
                if (value == null) return null;
                var fi = _enumType.GetField(Enum.GetName(_enumType, value));
                var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                return dna != null ? dna.Description : value.ToString();
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
            {
                return srcType == typeof(string);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                foreach (var fi in _enumType.GetFields())
                {
                    var dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                    if (dna != null && (string)value == dna.Description)
                    {
                        return Enum.Parse(_enumType, fi.Name);
                    }
                }

                return value != null ? Enum.Parse(_enumType, (string)value) : null;
            }
        }

        public class YesNoTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (!(value is string)) return base.ConvertFrom(context, culture, value);
                if (string.Equals(value.ToString(), Language.Yes, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                if (string.Equals(value.ToString(), Language.No, StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }

                throw new Exception("Values must be \"Yes\" or \"No\"");
            }

            public override object ConvertTo(ITypeDescriptorContext context,
                                             CultureInfo culture,
                                             object value,
                                             Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return Convert.ToBoolean(value) ? Language.Yes : Language.No;
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                bool[] bools = {true, false};

                var svc = new StandardValuesCollection(bools);

                return svc;
            }
        }
    }
}