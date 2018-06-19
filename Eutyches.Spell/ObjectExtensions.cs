//=============================================================================
// Copyright © 2018 Joseph S. Maza <joseph.maza@gmail.com>. All Rights Reserved.
//=============================================================================

using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Eutyches.Spell
{
    public static class ObjectExtensions
    {
        #region Methods

        /// <summary>
        /// Clones the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T.</returns>
        /// <exception cref="ArgumentException">The type must be serializable. - source</exception>
        public static T Clone<T>(T source) where T : class, new()
        {
            if(!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if(source == null)
            {
                return new T();
            }

            using(var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, source);
                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Compares the memory streams.
        /// </summary>
        /// <param name="ms1">The MS1.</param>
        /// <param name="ms2">The MS2.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CompareMemoryStreams(MemoryStream ms1, MemoryStream ms2)
        {
            if(ms1 is null | ms2 is null)
                return false;

            if(ms1.Length != ms2.Length)
                return false;

            ms1.Position = 0;
            ms2.Position = 0;

            var ms1seq = ms1.ToArray();
            var ms2seq = ms2.ToArray();

            return ms1seq.SequenceEqual(ms2seq);
        }

        /// <summary>
        /// To the memory stream.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>MemoryStream.</returns>
        public static MemoryStream ToMemoryStream(object obj)
        {
            if(obj is null)
                return null;

            using(var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return ms;
            }
        }

        #endregion Methods
    }
}