using Caml.Expressions;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Caml
{
    public class ConditionalSectionJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ConditionalSection<>).IsAssignableFrom(objectType) && objectType.GenericTypeArguments.Any();
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var typeArgument = objectType.GenericTypeArguments[0];
            string condition = null;
            object value = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (reader.Path.Equals("Expression"))
                    {
                        condition = reader.ReadAsString();
                    }
                    else if (reader.Path.Equals("Value"))
                    {
                        reader.Read();
                        value = serializer.Deserialize(reader, typeArgument);
                    }
                }
            }

            var expression = new ExpressionFactory().BuildExpression(condition);
            var type = typeof(ConditionalSection<>).MakeGenericType(typeArgument);
            return Activator.CreateInstance(type, expression, value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
            {
                return;
            }

            var type = typeof(ConditionalSection<>).MakeGenericType(value.GetType().GenericTypeArguments[0]);
            var expression = (IExpression)type.GetProperty(nameof(ConditionalSection<object>.Expression)).GetValue(value);
            var val = type.GetProperty(nameof(ConditionalSection<object>.Value)).GetValue(value);

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(ConditionalSection<object>.Expression));
            writer.WriteValue(expression?.ToString());
            writer.WritePropertyName(nameof(ConditionalSection<object>.Value));
            serializer.Serialize(writer, val);
            writer.WriteEndObject();
        }
    }
}