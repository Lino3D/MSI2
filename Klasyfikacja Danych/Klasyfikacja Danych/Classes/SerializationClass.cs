using Klasyfikacja_Danych.Neural_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Klasyfikacja_Danych.Classes
{
    public static class SerializationClass
    {
        public static void SerializeObject<T>(this T toSerialize, String filename)
        {

            XmlSerializer ser = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(filename);
           ser.Serialize(writer, toSerialize);
           writer.Close();

            using (StringWriter textWriter = new StringWriter())
            {
            //    ser.Serialize(textWriter, network);
          //      return textWriter.ToString();
            }
      
        }


    }
}
