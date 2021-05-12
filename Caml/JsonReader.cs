using Newtonsoft.Json;
using System.IO;

namespace Caml
{
    public class JsonReader
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
            using (var jsonReader = new JsonTextReader(textReader))
            {
                return new JsonSerializer().Deserialize<T>(jsonReader);
            }
        }
    }
}