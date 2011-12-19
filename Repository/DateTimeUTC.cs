using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PivotalTracker.FluentAPI.Repository
{
    /// <summary>
    /// DateTimeUTC represent an UTC DateTime in Pivotal XML Format.
    /// This class helps in the XML Deserialization and is used in DTO Class inside Repositories.
    /// </summary>
    public class DateTimeUTC : IXmlSerializable
    {
        public DateTimeUTC()
        {
            DateTime = System.DateTime.Now;
        }

        public DateTimeUTC(DateTime? source)
        {
            DateTime = source;
        }

        public DateTime? DateTime { get; set; }
        public void ReadXml(XmlReader reader)
        {
            string lValue = reader.ReadElementContentAsString();
            if (String.IsNullOrWhiteSpace(lValue))
                DateTime = null;
            else
                DateTime = System.DateTime.Parse(lValue.Replace("UTC", ""), null, DateTimeStyles.AssumeUniversal);
        }

        public void WriteXml(XmlWriter writer)
        {
            if (DateTime != null)
                writer.WriteString(DateTime.Value.ToString("z").Replace("Z", " UTC"));
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public static implicit operator DateTime?(DateTimeUTC source)
        {
            if (source == null)
                return null;

            return source.DateTime;
        }

        public static implicit operator DateTimeUTC(DateTime? source)
        {
            if (source == null)
                return null;

            return new DateTimeUTC(source);
        }

        public static implicit operator DateTime(DateTimeUTC source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.DateTime.HasValue)
                throw new InvalidOperationException("Source has no value");

            return source.DateTime.Value;
        }

        public static implicit operator DateTimeUTC(DateTime source)
        {
            return new DateTimeUTC(source);
        }
    }
}
