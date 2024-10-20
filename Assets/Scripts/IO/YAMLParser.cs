/*
The following license applies to the YAMLDotNet source code.
https://github.com/aaubry/YamlDotNet

MIT License

Copyright (c) 2016 Süleyman Yasir KULA

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 

*/

using YamlDotNet.Serialization;
using AoJCabViewer.Cabinets;

namespace AoJCabViewer.IO {

    /// <summary>
    /// Contains functions related to parsing YAML data.
    /// </summary>
    public class YAMLParser {

        /// <summary>
        /// Parses description.yaml into a CabData object for use in the viewer.
        /// </summary>
        /// <param name="yamlContent">(string) The YAML data to be parsed.</param>
        /// <returns>A CabData object, returns null if parsing unsuccesful.</returns>
        public static Data LoadYamlData(string yamlContent) {

            try {
                
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(new HyphenatedNamingConvention()) // Custom convention for handling hyphens
                    .IgnoreUnmatchedProperties()
                    .Build();

                Data yamlData = deserializer.Deserialize<Data>(yamlContent);

                MCP.Log($"YAML data loaded successfully: {yamlData.Name}");
                return yamlData;

            } catch (System.Exception ex) {

                MCP.LogError($"Failed to load YAML content: {ex.Message}");
                return null;

            }
        }
    }

    /// <summary>
    /// Custom naming convention to handle hyphenated keys in YAML.
    /// </summary>
    public class HyphenatedNamingConvention : INamingConvention {

        public string Apply(string value) => value.Replace('_', '-');

    }

}