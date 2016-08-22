using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace ProjectToYou.Code
{
    public class JsonParser
    {
        public class DynamicJsonConverter : System.Web.Script.Serialization.JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, System.Web.Script.Serialization.JavaScriptSerializer serializer)
            {
                if (dictionary == null)
                    throw new ArgumentNullException("dictionary");

                if (type == typeof(object))
                {
                    return new DynamicJsonObject(dictionary);
                }

                return null;
            }


            public override IEnumerable<Type> SupportedTypes
            {
                get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(object) })); }
            }
            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                if (obj is IDictionary<string, object>)
                {
                    return obj as IDictionary<string, object>;
                }
                else if (obj is DynamicJsonObject)
                {
                    return (obj as DynamicJsonObject).Dictionary;
                }
                return null;
            }
        }

        static JavaScriptSerializer jss = null;
        static JavaScriptSerializer dymicJss = null;
        static JavaScriptSerializer DymicSerializer
        {
            get
            {
                if (dymicJss == null)
                {

                    dymicJss = new JavaScriptSerializer();
                    dymicJss.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });
                }
                return dymicJss;
            }
        }
        static JavaScriptSerializer Serializer
        {
            get
            {
                if (jss == null)
                {
                    jss = new JavaScriptSerializer();
                }
                return jss;
            }
        }
        public class DynamicJsonObject : DynamicObject
        {
            public IDictionary<string, object> Dictionary { get; set; }

            public DynamicJsonObject(IDictionary<string, object> dictionary)
            {
                this.Dictionary = dictionary;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                if (!this.Dictionary.ContainsKey(binder.Name))
                {
                    result = null;

                    return true;
                }

                result = this.Dictionary[binder.Name];

                if (result is IDictionary<string, object>)
                {
                    result = new DynamicJsonObject(result as IDictionary<string, object>);
                }
                else if (result is ArrayList && (result as ArrayList) is IDictionary<string, object>)
                {
                    result = new List<DynamicJsonObject>((result as ArrayList).ToArray().Select(x => new DynamicJsonObject(x as IDictionary<string, object>)));
                }
                else if (result is ArrayList)
                {
                    result = new List<object>((result as ArrayList).ToArray());
                }

                return this.Dictionary.ContainsKey(binder.Name);
            }

            public override string ToString()
            {
                var sb = new StringBuilder("{");
                ToString(sb);
                return sb.ToString();
            }
            private void ToString(StringBuilder sb)
            {
                var firstInDictionary = true;
                foreach (var pair in Dictionary)
                {
                    if (!firstInDictionary)
                        sb.Append(",");
                    firstInDictionary = false;
                    var value = pair.Value;
                    var name = pair.Key;
                    if (value is string)
                    {
                        sb.AppendFormat("{0}:\"{1}\"", name, value);
                    }
                    else if (value is IDictionary<string, object>)
                    {
                        new DynamicJsonObject((IDictionary<string, object>)value).ToString(sb);
                    }
                    else if (value is ArrayList)
                    {
                        sb.Append(name + ":[");
                        var firstInArray = true;
                        foreach (var arrayValue in (ArrayList)value)
                        {
                            if (!firstInArray)
                                sb.Append(",");
                            firstInArray = false;
                            if (arrayValue is IDictionary<string, object>)
                                new DynamicJsonObject((IDictionary<string, object>)arrayValue).ToString(sb);
                            else if (arrayValue is string)
                                sb.AppendFormat("\"{0}\"", arrayValue);
                            else
                                sb.AppendFormat("{0}", arrayValue);

                        }
                        sb.Append("]");
                    }
                    else
                    {
                        sb.AppendFormat("{0}:{1}", name, value);
                    }
                }
                sb.Append("}");
            }
        }
        public static dynamic Dynamic(string json)
        {
            dynamic dy = DymicSerializer.Deserialize(json, typeof(object)) as dynamic;
            return dy;
        }
        public static T Deserialize<T>(string json)
        {
            T dy = Serializer.Deserialize<T>(json);
            return dy;
        }

        public static string Serialize(object obj)
        {
            string dy = Serializer.Serialize(obj);
            return dy;
        }
    }
}
