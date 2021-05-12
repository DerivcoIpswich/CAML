using System.IO;
using YamlDotNet.Serialization;

namespace Caml
{
    public class CamlReader
    {
        public T Deserialise<T>(string input)
        {
            using (var reader = new StringReader(input))
            {
                return Deserialise<T>(reader);
            }
        }

        public T Deserialise<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return Deserialise<T>(reader);
            }
        }

        public T Deserialise<T>(TextReader textReader)
        {
            return new DeserializerBuilder()
                .Build()
                .Deserialize<T>(textReader);
        }
    }
}