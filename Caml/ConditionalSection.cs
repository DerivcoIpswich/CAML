using Caml.Expressions;
using Newtonsoft.Json;
using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Caml
{
    //todo: move IYamlConvertible logic out of object to custom serialiser/deserialiser
    [JsonConverter(typeof(ConditionalSectionJsonConverter))]
    public class ConditionalSection<T> : IYamlConvertible
    {
        private static readonly ExpressionFactory expressionFactory = new ExpressionFactory();

        public IExpression Expression { get; private set; }

        public T Value { get; private set; }

        public ConditionalSection()
            : this(default)
        {
        }

        public ConditionalSection(T value)
            : this(expressionFactory.BuildDefaultExpression(), value)
        {
        }

        public ConditionalSection(string expression, T value)
            : this(expressionFactory.BuildExpression(expression), value)
        {
        }

        public ConditionalSection(IExpression expression, T value)
        {
            Expression = expression;
            Value = value;
        }

        void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            parser.Consume<MappingStart>();
            Expression = expressionFactory.BuildExpression(parser.Consume<Scalar>().Value);
            Value = (T)nestedObjectDeserializer.Invoke(typeof(T));
            parser.Consume<MappingEnd>();
        }

        void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            emitter.Emit(new MappingStart());
            emitter.Emit(new Scalar(Expression.ToString()));
            nestedObjectSerializer.Invoke(Value, typeof(T));
            emitter.Emit(new MappingEnd());
        }

        public override string ToString() => Expression.ToString();
    }
}