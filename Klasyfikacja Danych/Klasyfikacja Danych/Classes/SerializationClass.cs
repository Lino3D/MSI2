﻿using Klasyfikacja_Danych.Neural_Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Klasyfikacja_Danych.Classes
{
    public static class SerializationClass
    {
        


        public static void ConSerializer<T>(this T toSerialize, string filename)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));

            using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                using (XmlDictionaryWriter writer =
                    XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    dcs.WriteObject(writer, toSerialize);
                }
            }
        }
        public static T ConDeSerializer<T>(this T deSerializeType, string filename)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            T n;
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {

                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
                {
                    n = (T)dcs.ReadObject(reader);
                    reader.Close();
                }
                fs.Close();
            }
            return n;
        }

    }
}
