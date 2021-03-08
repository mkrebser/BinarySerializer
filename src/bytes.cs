using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Serialize
{
    /// <summary>
    /// Writing data to byte arrays, little endian format
    /// </summary>
    public static class Bytes
    {
        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, ushort val)
        {
            BitsConverter.Write(arr, val);
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, int val)
        {
            BitsConverter.Write(arr, val);
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, bool val)
        {
            arr.Add(BitsConverter.Bool(val));
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, byte val)
        {
            arr.Add(val);
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, double val)
        {
            BitsConverter.Write(arr, val);
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, long val)
        {
            BitsConverter.Write(arr, val);
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, float val)
        {
            BitsConverter.Write(arr, val);
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, Vector3 val)
        {
            arr.Write(val.x);
            arr.Write(val.y);
            arr.Write(val.z);
        }

        /// <summary>
        /// Write a value to byte array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this List<byte> arr, Quaternion val)
        {
            arr.Write(val.x);
            arr.Write(val.y);
            arr.Write(val.z);
            arr.Write(val.w);
        }

        /// <summary>
        /// Write a value to byte array, return new index.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, ushort val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 2;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, int val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 4;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, bool val)
        {
            arr[index] = BitsConverter.Bool(val);
            return index + 1;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, byte val)
        {
            arr[index] = val;
            return index + 1;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, double val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 8;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, long val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 8;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, float val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 4;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, Vector3 val)
        {
            index = arr.Write(index, val.x);
            index = arr.Write(index, val.y);
            index = arr.Write(index, val.z);
            return index;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this List<byte> arr, int index, Quaternion val)
        {
            index = arr.Write(index, val.x);
            index = arr.Write(index, val.y);
            index = arr.Write(index, val.z);
            index = arr.Write(index, val.w);
            return index;
        }




        /// <summary>
        /// Write a value to byte array, return new index.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, ushort val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 2;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, int val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 4;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, bool val)
        {
            arr[index] = BitsConverter.Bool(val);
            return index + 1;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, byte val)
        {
            arr[index] = val;
            return index + 1;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, double val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 8;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, long val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 8;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, float val)
        {
            BitsConverter.Write(arr, val, index);
            return index + 4;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, Vector3 val)
        {
            index = arr.Write(index, val.x);
            index = arr.Write(index, val.y);
            index = arr.Write(index, val.z);
            return index;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, Vector3Int val)
        {
            index = arr.Write(index, val.x);
            index = arr.Write(index, val.y);
            index = arr.Write(index, val.z);
            return index;
        }

        /// <summary>
        /// Write a value to byte array, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Write(this byte[] arr, int index, Quaternion val)
        {
            index = arr.Write(index, val.x);
            index = arr.Write(index, val.y);
            index = arr.Write(index, val.z);
            index = arr.Write(index, val.w);
            return index;
        }




        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out ushort val)
        {
            val = BitsConverter.ReadUInt16(arr, index);
            return index + 2;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out int val)
        {
            val = BitsConverter.ReadInt32(arr, index);
            return index + 4;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out bool val)
        {
            val = arr[index] != 0;
            return index + 1;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out byte val)
        {
            val = arr[index];
            return index + 1;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out double val)
        {
            val = BitsConverter.ReadDouble(arr, index);
            return index + 8;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out long val)
        {
            val = BitsConverter.ReadLong(arr, index);
            return index + 8;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out float val)
        {
            val = BitsConverter.ReadSingle(arr, index);
            return index + 4;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out Vector3 val)
        {
            var x = BitsConverter.ReadSingle(arr, index);
            var y = BitsConverter.ReadSingle(arr, index + 4);
            var z = BitsConverter.ReadSingle(arr, index + 8);
            val = new Vector3(x, y, z);
            return index + 12;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out Vector3Int val)
        {
            var x = BitsConverter.ReadInt32(arr, index);
            var y = BitsConverter.ReadInt32(arr, index + 4);
            var z = BitsConverter.ReadInt32(arr, index + 8);
            val = new Vector3Int(x, y, z);
            return index + 12;
        }

        /// <summary>
        /// Read a value from byte array, output value, return new index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Read(this byte[] arr, int index, out Quaternion val)
        {
            var x = BitsConverter.ReadSingle(arr, index);
            var y = BitsConverter.ReadSingle(arr, index + 4);
            var z = BitsConverter.ReadSingle(arr, index + 8);
            var w = BitsConverter.ReadSingle(arr, index + 12);
            val = new Quaternion(x, y, z, w);
            return index + 16;
        }

        /// <summary>
        /// Write a collection of primitive to bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="collection"></param>
        public static void WriteCollection<T>(this List<byte> bytes, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                bytes.Write((int)0);
                bytes.Write((int)0);
                return;
            }

            //write type
            var length_index = bytes.Count;
            //temp write length
            bytes.Write((int)0);

            //get index of collection length
            var collection_length_index = bytes.Count;
            //write collection length
            int collection_length = 0;
            bytes.Write(collection_length);

            //record position of byte count
            var start = bytes.Count;

            if (typeof(T) == typeof(int))
            {
                var col = (IEnumerable<int>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                var col = (IEnumerable<double>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                var col = (IEnumerable<bool>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                var col = (IEnumerable<float>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else if (typeof(T) == typeof(ushort))
            {
                var col = (IEnumerable<ushort>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else if (typeof(T) == typeof(Vector3))
            {
                var col = (IEnumerable<Vector3>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                var col = (IEnumerable<Quaternion>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else if (typeof(T) == typeof(byte))
            {
                var col = (IEnumerable<byte>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else
            {
                throw new System.Exception("Error, unsupported type. " +
                    "Supported types are int,bool,byte,float,double,Quaternion(d),Vector3(d)");
            }

            //get length
            var len = bytes.Count - start;
            //write length
            bytes.Write(length_index, len);
            //write collection length
            bytes.Write(collection_length_index, collection_length);
        }

        public static int WriteCollection<T>(this byte[] bytes, int index, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                index = bytes.Write(index, 0);
                return index;
            }

            //write type
            var length_index = index;
            //temp write length
            index = bytes.Write(index, (int)0);

            //get index of collection length
            var collection_length_index = index;
            //write collection length
            int collection_length = 0;
            index = bytes.Write(index, collection_length);

            //record position
            var start = index;

            if (typeof(T) == typeof(int))
            {
                var col = (IEnumerable<int>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(double))
            {
                var col = (IEnumerable<double>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(bool))
            {
                var col = (IEnumerable<bool>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(float))
            {
                var col = (IEnumerable<float>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(ushort))
            {
                var col = (IEnumerable<ushort>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                var col = (IEnumerable<Vector3>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                var col = (IEnumerable<Quaternion>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(byte))
            {
                var col = (IEnumerable<byte>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else if (typeof(T) == typeof(string))
            {
                var col = (IEnumerable<string>)collection;
                foreach (var t in col)
                    index = bytes.Write(index, t);
            }
            else
            {
                throw new System.Exception("Error, unsupported type. " +
                    "Supported types are int,bool,byte,float,double,Quaternion(d),Vector3(d)");
            }

            //get length
            var len = index - start;
            //write length
            bytes.Write(length_index, len);

            //write collection length
            bytes.Write(collection_length_index, collection_length);

            //return index
            return index;
        }

        public static int Write(this byte[] bytes, int index, string s)
        {
            if (s == null)
                return bytes.Write((int)index, 0);
            if (s == "")
                return bytes.Write((int)index, -1);
            index = bytes.Write((int)index, StringByteLength(s));
            System.Text.Encoding.Unicode.GetBytes(s, 0, s.Length, bytes, index);
            return index + StringByteLength(s);
        }

        public static int StringByteLength(string s)
        {
            return s == null ? 0 : s.Length * 2;
        }

        public static int Read(this byte[] bytes, int index, out string s)
        {
            int count;
            index = bytes.Read(index, out count);
            if (count == 0) //length of zero is null string
            {
                s = null;
                return index;
            }
            if (count <= 0) // length of less than zero is empty string
            {
                s = "";
                return index;
            }
            s = System.Text.Encoding.Unicode.GetString(bytes, index, count);
            return index + count;
        }

        /// <summary>
        /// Padding added to strings that isn't apart of the data (number of bytes)
        /// This is just an integer length value
        /// </summary>
        public static int string_padding = 4;

        /// <summary>
        /// read a collection of primitives and return index of the last byte + 1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static int ReadCollection<T>(this byte[] bytes, int index, out IEnumerable<T> collection)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            if (typeof(T) == typeof(int))
            {
                collection = (IEnumerable<T>)ReadIntCollection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(double))
            {
                collection = (IEnumerable<T>)ReadDoubleCollection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(bool))
            {
                collection = (IEnumerable<T>)ReadBoolCollection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(float))
            {
                collection = (IEnumerable<T>)ReadFloatCollection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(ushort))
            {
                collection = (IEnumerable<T>)ReadUShortCollection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                collection = (IEnumerable<T>)ReadVector3Collection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                collection = (IEnumerable<T>)ReadQuaternionCollection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(byte))
            {
                collection = (IEnumerable<T>)ReadByteCollection(bytes, index, col_length);
            }
            else if (typeof(T) == typeof(string))
            {
                collection = (IEnumerable<T>)ReadStringCollection(bytes, index, col_length);
            }
            else
            {
                throw new System.Exception("Error, unsupported type. " +
                    "Supported types are int,bool,byte,float,double,Quaternion(d),Vector3(d)");
            }

            return index + col_length;
        }

        static IEnumerable<int> ReadIntCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                int dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        /// <summary>
        /// read a collection of primitive to bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="collection"></param>
        static IEnumerable<byte> ReadByteCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                byte dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        static IEnumerable<float> ReadFloatCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                float dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        static IEnumerable<double> ReadDoubleCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                double dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        static IEnumerable<ushort> ReadUShortCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                ushort dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        static IEnumerable<bool> ReadBoolCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                bool dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        static IEnumerable<Vector3> ReadVector3Collection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                Vector3 dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        static IEnumerable<Quaternion> ReadQuaternionCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                Quaternion dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }
        static IEnumerable<string> ReadStringCollection(this byte[] data, int index, int length)
        {
            var stop = index + length;
            while (index < stop)
            {
                string dat;
                index = data.Read(index, out dat);
                yield return dat;
            }
        }

        public static int ReadIntArray(this byte[] bytes, int index, out int[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readIntArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static int[] readIntArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new int[num_items];
            int n = 0;
            while (index < stop)
            {
                int dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadByteArray(this byte[] bytes, int index, out byte[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readByteArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static byte[] readByteArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new byte[num_items];
            int n = 0;
            while (index < stop)
            {
                byte dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadFloatArray(this byte[] bytes, int index, out float[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readFloatArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static float[] readFloatArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new float[num_items];
            int n = 0;
            while (index < stop)
            {
                float dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadDoubleArray(this byte[] bytes, int index, out double[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readDoubleArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static double[] readDoubleArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new double[num_items];
            int n = 0;
            while (index < stop)
            {
                double dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadUShortArray(this byte[] bytes, int index, out ushort[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readUShortArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static ushort[] readUShortArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new ushort[num_items];
            int n = 0;
            while (index < stop)
            {
                ushort dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadBoolArray(this byte[] bytes, int index, out bool[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readBoolArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static bool[] readBoolArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new bool[num_items];
            int n = 0;
            while (index < stop)
            {
                bool dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadVector3Array(this byte[] bytes, int index, out Vector3[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readVector3Array(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static Vector3[] readVector3Array(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new Vector3[num_items];
            int n = 0;
            while (index < stop)
            {
                Vector3 dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadQuaternionArray(this byte[] bytes, int index, out Quaternion[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readQuaternionArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static Quaternion[] readQuaternionArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new Quaternion[num_items];
            int n = 0;
            while (index < stop)
            {
                Quaternion dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        public static int ReadStringArray(this byte[] bytes, int index, out string[] array)
        {
            int col_length;
            index = bytes.Read(index, out col_length);
            int num_items;
            index = bytes.Read(index, out num_items);
            array = readStringArray(bytes, index, col_length, num_items);
            return index + col_length;
        }
        static string[] readStringArray(this byte[] data, int index, int length, int num_items)
        {
            var stop = index + length;
            var array = new string[num_items];
            int n = 0;
            while (index < stop)
            {
                string dat;
                index = data.Read(index, out dat);
                array[n] = dat;
                n++;
            }
            return array;
        }

        /// <summary>
        /// This Class is used for Reading/Writing to byte arrays. It will write/write all data to a byte array
        /// in little endian format regardless of host architecture. 
        /// </summary>
        public static class BitsConverter
        {
            public static bool IsLittleEndian { get; private set; }
            static BitsConverter()
            {
                var i = new Int32();
                i.val = 1;
                IsLittleEndian = i._Byte0 != 0 ? true : false;
            }

            //System.Runtime.InteropServices namespace
            [StructLayout(LayoutKind.Explicit)]
            public struct Single
            {
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public float val;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public byte _Byte0;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(1)]
                public byte _Byte1;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(2)]
                public byte _Byte2;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(3)]
                public byte _Byte3;

                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte0 { get { return IsLittleEndian ? _Byte0 : _Byte3; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte1 { get { return IsLittleEndian ? _Byte1 : _Byte2; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte2 { get { return IsLittleEndian ? _Byte2 : _Byte1; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte3 { get { return IsLittleEndian ? _Byte3 : _Byte0; } }
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct Int32
            {
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public int val;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public byte _Byte0;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(1)]
                public byte _Byte1;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(2)]
                public byte _Byte2;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(3)]
                public byte _Byte3;

                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte0 { get { return IsLittleEndian ? _Byte0 : _Byte3; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte1 { get { return IsLittleEndian ? _Byte1 : _Byte2; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte2 { get { return IsLittleEndian ? _Byte2 : _Byte1; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte3 { get { return IsLittleEndian ? _Byte3 : _Byte0; } }
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct UInt16
            {
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public ushort val;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public byte _Byte0;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(1)]
                public byte _Byte1;

                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte0 { get { return IsLittleEndian ? _Byte0 : _Byte1; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte1 { get { return IsLittleEndian ? _Byte1 : _Byte0; } }
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct Double
            {
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public double val;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public byte _Byte0;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(1)]
                public byte _Byte1;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(2)]
                public byte _Byte2;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(3)]
                public byte _Byte3;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(4)]
                public byte _Byte4;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(5)]
                public byte _Byte5;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(6)]
                public byte _Byte6;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(7)]
                public byte _Byte7;

                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte0 { get { return IsLittleEndian ? _Byte0 : _Byte7; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte1 { get { return IsLittleEndian ? _Byte1 : _Byte6; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte2 { get { return IsLittleEndian ? _Byte2 : _Byte5; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte3 { get { return IsLittleEndian ? _Byte3 : _Byte4; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte4 { get { return IsLittleEndian ? _Byte4 : _Byte3; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte5 { get { return IsLittleEndian ? _Byte5 : _Byte2; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte6 { get { return IsLittleEndian ? _Byte6 : _Byte1; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte7 { get { return IsLittleEndian ? _Byte7 : _Byte0; } }
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct Long
            {
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public long val;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(0)]
                public byte _Byte0;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(1)]
                public byte _Byte1;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(2)]
                public byte _Byte2;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(3)]
                public byte _Byte3;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(4)]
                public byte _Byte4;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(5)]
                public byte _Byte5;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(6)]
                public byte _Byte6;
                /// <summary>
                /// raw byte
                /// </summary>
                [FieldOffset(7)]
                public byte _Byte7;

                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte0 { get { return IsLittleEndian ? _Byte0 : _Byte7; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte1 { get { return IsLittleEndian ? _Byte1 : _Byte6; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte2 { get { return IsLittleEndian ? _Byte2 : _Byte5; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte3 { get { return IsLittleEndian ? _Byte3 : _Byte4; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte4 { get { return IsLittleEndian ? _Byte4 : _Byte3; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte5 { get { return IsLittleEndian ? _Byte5 : _Byte2; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte6 { get { return IsLittleEndian ? _Byte6 : _Byte1; } }
                /// <summary>
                /// Guarenteed little endian format
                /// </summary>
                public byte Byte7 { get { return IsLittleEndian ? _Byte7 : _Byte0; } }
            }

            public static void Write(byte[] arr, float val, int index)
            {
                Single s = new Single();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                }
                else
                {
                    arr[index] = s._Byte3;
                    arr[index + 1] = s._Byte2;
                    arr[index + 2] = s._Byte1;
                    arr[index + 3] = s._Byte0;
                }
            }

            public static void Write(byte[] arr, int val, int index)
            {
                Int32 s = new Int32();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                }
                else
                {
                    arr[index] = s._Byte3;
                    arr[index + 1] = s._Byte2;
                    arr[index + 2] = s._Byte1;
                    arr[index + 3] = s._Byte0;
                }
            }

            public static void Write(byte[] arr, ushort val, int index)
            {
                UInt16 s = new UInt16();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                }
                else
                {
                    arr[index] = s._Byte1;
                    arr[index + 1] = s._Byte0;
                }
            }

            public static void Write(byte[] arr, double val, int index)
            {
                Double s = new Double();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                    arr[index + 4] = s._Byte4;
                    arr[index + 5] = s._Byte5;
                    arr[index + 6] = s._Byte6;
                    arr[index + 7] = s._Byte7;
                }
                else
                {
                    arr[index] = s._Byte7;
                    arr[index + 1] = s._Byte6;
                    arr[index + 2] = s._Byte5;
                    arr[index + 3] = s._Byte4;
                    arr[index + 4] = s._Byte3;
                    arr[index + 5] = s._Byte2;
                    arr[index + 6] = s._Byte1;
                    arr[index + 7] = s._Byte0;
                }
            }

            public static void Write(byte[] arr, long val, int index)
            {
                Long s = new Long();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                    arr[index + 4] = s._Byte4;
                    arr[index + 5] = s._Byte5;
                    arr[index + 6] = s._Byte6;
                    arr[index + 7] = s._Byte7;
                }
                else
                {
                    arr[index] = s._Byte7;
                    arr[index + 1] = s._Byte6;
                    arr[index + 2] = s._Byte5;
                    arr[index + 3] = s._Byte4;
                    arr[index + 4] = s._Byte3;
                    arr[index + 5] = s._Byte2;
                    arr[index + 6] = s._Byte1;
                    arr[index + 7] = s._Byte0;
                }
            }

            public static float ReadSingle(byte[] arr, int index)
            {
                Single s = new Single();
                s._Byte0 = arr[index];
                s._Byte1 = arr[index + 1];
                s._Byte2 = arr[index + 2];
                s._Byte3 = arr[index + 3];
                return s.val;
            }

            public static int ReadInt32(byte[] arr, int index)
            {
                Int32 s = new Int32();
                s._Byte0 = arr[index];
                s._Byte1 = arr[index + 1];
                s._Byte2 = arr[index + 2];
                s._Byte3 = arr[index + 3];
                return s.val;
            }

            public static ushort ReadUInt16(byte[] arr, int index)
            {
                UInt16 s = new UInt16();
                s._Byte0 = arr[index];
                s._Byte1 = arr[index + 1];
                return s.val;
            }

            public static double ReadDouble(byte[] arr, int index)
            {
                Double s = new Double();
                s._Byte0 = arr[index];
                s._Byte1 = arr[index + 1];
                s._Byte2 = arr[index + 2];
                s._Byte3 = arr[index + 3];
                s._Byte4 = arr[index + 4];
                s._Byte5 = arr[index + 5];
                s._Byte6 = arr[index + 6];
                s._Byte7 = arr[index + 7];
                return s.val;
            }

            public static long ReadLong(byte[] arr, int index)
            {
                Long s = new Long();
                s._Byte0 = arr[index];
                s._Byte1 = arr[index + 1];
                s._Byte2 = arr[index + 2];
                s._Byte3 = arr[index + 3];
                s._Byte4 = arr[index + 4];
                s._Byte5 = arr[index + 5];
                s._Byte6 = arr[index + 6];
                s._Byte7 = arr[index + 7];
                return s.val;
            }

            public static void Write(List<byte> arr, float val, int index)
            {
                Single s = new Single();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                }
                else
                {
                    arr[index] = s._Byte3;
                    arr[index + 1] = s._Byte2;
                    arr[index + 2] = s._Byte1;
                    arr[index + 3] = s._Byte0;
                }
            }

            public static void Write(List<byte> arr, int val, int index)
            {
                Int32 s = new Int32();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                }
                else
                {
                    arr[index] = s._Byte3;
                    arr[index + 1] = s._Byte2;
                    arr[index + 2] = s._Byte1;
                    arr[index + 3] = s._Byte0;
                }
            }

            public static void Write(List<byte> arr, ushort val, int index)
            {
                UInt16 s = new UInt16();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                }
                else
                {
                    arr[index] = s._Byte1;
                    arr[index + 1] = s._Byte0;
                }
            }

            public static void Write(List<byte> arr, double val, int index)
            {
                Double s = new Double();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                    arr[index + 4] = s._Byte4;
                    arr[index + 5] = s._Byte5;
                    arr[index + 6] = s._Byte6;
                    arr[index + 7] = s._Byte7;
                }
                else
                {
                    arr[index] = s._Byte7;
                    arr[index + 1] = s._Byte6;
                    arr[index + 2] = s._Byte5;
                    arr[index + 3] = s._Byte4;
                    arr[index + 4] = s._Byte3;
                    arr[index + 5] = s._Byte2;
                    arr[index + 6] = s._Byte1;
                    arr[index + 7] = s._Byte0;
                }
            }

            public static void Write(List<byte> arr, long val, int index)
            {
                Long s = new Long();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr[index] = s._Byte0;
                    arr[index + 1] = s._Byte1;
                    arr[index + 2] = s._Byte2;
                    arr[index + 3] = s._Byte3;
                    arr[index + 4] = s._Byte4;
                    arr[index + 5] = s._Byte5;
                    arr[index + 6] = s._Byte6;
                    arr[index + 7] = s._Byte7;
                }
                else
                {
                    arr[index] = s._Byte7;
                    arr[index + 1] = s._Byte6;
                    arr[index + 2] = s._Byte5;
                    arr[index + 3] = s._Byte4;
                    arr[index + 4] = s._Byte3;
                    arr[index + 5] = s._Byte2;
                    arr[index + 6] = s._Byte1;
                    arr[index + 7] = s._Byte0;
                }
            }

            public static void Write(List<byte> arr, float val)
            {
                Single s = new Single();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                }
                else
                {
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(List<byte> arr, int val)
            {
                Int32 s = new Int32();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                }
                else
                {
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(List<byte> arr, ushort val)
            {
                UInt16 s = new UInt16();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                }
                else
                {
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(List<byte> arr, double val)
            {
                Double s = new Double();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte7);
                }
                else
                {
                    arr.Add(s._Byte7);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(List<byte> arr, long val)
            {
                Long s = new Long();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte7);
                }
                else
                {
                    arr.Add(s._Byte7);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(SimpleList<byte> arr, float val, int index)
            {
                Single s = new Single();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Array[index] = s._Byte0;
                    arr.Array[index + 1] = s._Byte1;
                    arr.Array[index + 2] = s._Byte2;
                    arr.Array[index + 3] = s._Byte3;
                }
                else
                {
                    arr.Array[index] = s._Byte3;
                    arr.Array[index + 1] = s._Byte2;
                    arr.Array[index + 2] = s._Byte1;
                    arr.Array[index + 3] = s._Byte0;
                }
            }

            public static void Write(SimpleList<byte> arr, int val, int index)
            {
                Int32 s = new Int32();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Array[index] = s._Byte0;
                    arr.Array[index + 1] = s._Byte1;
                    arr.Array[index + 2] = s._Byte2;
                    arr.Array[index + 3] = s._Byte3;
                }
                else
                {
                    arr.Array[index] = s._Byte3;
                    arr.Array[index + 1] = s._Byte2;
                    arr.Array[index + 2] = s._Byte1;
                    arr.Array[index + 3] = s._Byte0;
                }
            }

            public static void Write(SimpleList<byte> arr, ushort val, int index)
            {
                UInt16 s = new UInt16();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Array[index] = s._Byte0;
                    arr.Array[index + 1] = s._Byte1;
                }
                else
                {
                    arr.Array[index] = s._Byte1;
                    arr.Array[index + 1] = s._Byte0;
                }
            }

            public static void Write(SimpleList<byte> arr, double val, int index)
            {
                Double s = new Double();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Array[index] = s._Byte0;
                    arr.Array[index + 1] = s._Byte1;
                    arr.Array[index + 2] = s._Byte2;
                    arr.Array[index + 3] = s._Byte3;
                    arr.Array[index + 4] = s._Byte4;
                    arr.Array[index + 5] = s._Byte5;
                    arr.Array[index + 6] = s._Byte6;
                    arr.Array[index + 7] = s._Byte7;
                }
                else
                {
                    arr.Array[index] = s._Byte7;
                    arr.Array[index + 1] = s._Byte6;
                    arr.Array[index + 2] = s._Byte5;
                    arr.Array[index + 3] = s._Byte4;
                    arr.Array[index + 4] = s._Byte3;
                    arr.Array[index + 5] = s._Byte2;
                    arr.Array[index + 6] = s._Byte1;
                    arr.Array[index + 7] = s._Byte0;
                }
            }

            public static void Write(SimpleList<byte> arr, long val, int index)
            {
                Long s = new Long();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Array[index] = s._Byte0;
                    arr.Array[index + 1] = s._Byte1;
                    arr.Array[index + 2] = s._Byte2;
                    arr.Array[index + 3] = s._Byte3;
                    arr.Array[index + 4] = s._Byte4;
                    arr.Array[index + 5] = s._Byte5;
                    arr.Array[index + 6] = s._Byte6;
                    arr.Array[index + 7] = s._Byte7;
                }
                else
                {
                    arr.Array[index] = s._Byte7;
                    arr.Array[index + 1] = s._Byte6;
                    arr.Array[index + 2] = s._Byte5;
                    arr.Array[index + 3] = s._Byte4;
                    arr.Array[index + 4] = s._Byte3;
                    arr.Array[index + 5] = s._Byte2;
                    arr.Array[index + 6] = s._Byte1;
                    arr.Array[index + 7] = s._Byte0;
                }
            }

            public static void Write(SimpleList<byte> arr, float val)
            {
                Single s = new Single();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                }
                else
                {
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(SimpleList<byte> arr, int val)
            {
                Int32 s = new Int32();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                }
                else
                {
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(SimpleList<byte> arr, ushort val)
            {
                UInt16 s = new UInt16();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                }
                else
                {
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(SimpleList<byte> arr, double val)
            {
                Double s = new Double();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte7);
                }
                else
                {
                    arr.Add(s._Byte7);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static void Write(SimpleList<byte> arr, long val)
            {
                Long s = new Long();
                s.val = val;
                if (IsLittleEndian)
                {
                    arr.Add(s._Byte0);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte7);
                }
                else
                {
                    arr.Add(s._Byte7);
                    arr.Add(s._Byte6);
                    arr.Add(s._Byte5);
                    arr.Add(s._Byte4);
                    arr.Add(s._Byte3);
                    arr.Add(s._Byte2);
                    arr.Add(s._Byte1);
                    arr.Add(s._Byte0);
                }
            }

            public static byte Bool(bool val)
            {
                return val ? (byte)1 : (byte)0;
            }
        }
    }
}