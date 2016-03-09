using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace MDS.TC.Web.Infrastructure
{
    public class HtmlStyle
    {
        public static Dictionary<string, HtmlStyle> Registry = new Dictionary<string, HtmlStyle>();

        public HtmlStyle(object source, string tag, params string[] classes)
        {
            Source = source;
            Tags = new List<string>();
            Tags.Add(tag);
            Rules = new List<Func<Object, Object>>();
            Classes = new List<string>();
            foreach (var @class in classes)
            {
                Classes.Add(@class);
            }
        }

        public object Source { private get; set; }
        private List<string> Tags { get; set; }
        private List<string> Classes { get; set; }
        private List<Func<Object, Object>> Rules { get; set; }

        public HtmlStyle AddTag(string tag)
        {
            Tags.Add(tag);
            return this;
        }

        public HtmlStyle AddClass(string tag)
        {
            Classes.Add(tag);
            return this;
        }

        public HtmlStyle AddRule(Func<Object, Object> rule)
        {
            Rules.Add(rule);
            return this;
        }

        public HtmlStyle DeleteClasses(params string[] classes)
        {
            foreach (var @class in classes)
            {
                Classes.Remove(@class);
            }
            return this;
        }


        public MvcHtmlString Build(params string[] tags)
        {
            if (tags.Length > 0)
                Tags[0] = tags[0];

            var doc = new XmlDocument();
            var root = (XmlElement)doc.AppendChild(doc.CreateElement(Tags[0]));
            root.SetAttribute("class", string.Join(",", Classes.ToArray()));
            var lastElement = root;

            if (Tags.Count > 1)
            {
                foreach (var tag in Tags.GetRange(1, Tags.Count - 1))
                {
                    lastElement = (XmlElement)lastElement.AppendChild(doc.CreateElement(tag));
                }
            }

            foreach (var rule in Rules)
            {
                Source = rule.Invoke(Source);
            }

            lastElement.InnerText = Source.ToString();
            return new MvcHtmlString(root.OuterXml);
        }

        public HtmlStyle SaveAs(string name)
        {
            if (!Registry.ContainsKey(name))
            {
                Registry.Add(name, this);
            }
            else
            {
                Registry[name] = this;
            }
            return this;
        }

        public static HtmlStyle Get(string name)
        {
            if (!Registry.ContainsKey(name))
                throw new Exception("The style required not found");

            return Registry[name];
        }

    }




    public static class StyleHelper
    {
        private static HtmlStyle ObjToStyle(this object source)
        {
            return source.GetType() == typeof(HtmlStyle) ? (HtmlStyle)source : new HtmlStyle(source, "span");
        }

        public static HtmlStyle AsCurrency(this object source)
        {
            var style = new HtmlStyle(source, "span", "text-right");
            style.AddRule(s => string.Format("{0:C2}", s));
            return style;

        }

        public static HtmlStyle AtRight(this object source)
        {
            return DeleteClassesAlignmente(source).AddClass("text-right");
        }

        public static HtmlStyle AtCenter(this object source)
        {
            return DeleteClassesAlignmente(source).AddClass("text-center");
        }

        public static HtmlStyle AtLeft(this object source)
        {
            return DeleteClassesAlignmente(source).AddClass("text-left");
        }

        private static HtmlStyle DeleteClassesAlignmente(this object source)
        {
            return ObjToStyle(source).DeleteClasses("text-right", "text-center", "text-left");
        }

        public static HtmlStyle Bold(this object source)
        {
            return ObjToStyle(source).AddTag("strong");
        }

        public static HtmlStyle Small(this object source)
        {
            return ObjToStyle(source).AddTag("small");
        }

        public static HtmlStyle Italic(this object source)
        {
            return ObjToStyle(source).AddTag("em");
        }

        public static HtmlStyle Underlined(this object source)
        {
            return ObjToStyle(source).AddTag("u");
        }

        public static MvcHtmlString Apply(this object source, string name, params string[] tags)
        {
            var styleSaved = HtmlStyle.Get(name);
            styleSaved.Source = source;
            return styleSaved.Build(tags);
        }
    }
}