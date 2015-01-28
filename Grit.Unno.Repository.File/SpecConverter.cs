using Grit.Unno.Specs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.File
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer,
                                       object value,
                                       JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public class SpecConverter : JsonCreationConverter<Specification>
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();
        private static string[] types = Enum.GetNames(typeof(SpecificationType));
        protected override Specification Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            string name = (string)jObject.Property("Type");
            if (types.Any(n => n == name))
            {
                switch (name)
                {
                    case "Boolean":
                        return new BooleanSpecification();
                    case "Composite":
                        return new CompositeSpecification();
                    case "DateTime":
                        return new DateTimeSpecification();
                    case "Decimal":
                        return new DecimalSpecification();
                    case "Integer":
                        return new IntegerSpecification();
                    case "Required":
                        return new RequiredSpecification();
                    case "String":
                        return new StringSpecification();
                    case "Lookup":
                        return new LookupSpecification();
                }    
            }
            
            throw new ApplicationException(string.Format("Invalid specification type: {0}.", name));
        }

        /*
        /// <summary>
        /// If you don't care performance
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        protected override Specification Create(Type objectType, Newtonsoft.Json.Linq.JObject jObject)
        {
            string name = (string)jObject.Property("Type");
            if (!types.Any(n => n == name))
            {
                throw new ApplicationException(string.Format("Invalid specification type: {0}.", name));
            }
            Type type = assembly.GetType("Grit.Unno.Specs." + name + "Specification");
            return (Specification)Activator.CreateInstance(type);
        }
        */
    }
}
