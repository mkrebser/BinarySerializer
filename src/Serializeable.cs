using System.Collections;
using System.Collections.Generic;

using System.IO;

namespace Serialize
{
    /// <summary>
    /// An Efficient Serialization interface
    /// </summary>
    public interface ISerializeable
    {
        /// <summary>
        /// Write data to a buffer
        /// </summary>
        /// <param name="data"></param>
        void Write(DataBuffer data);
        /// <summary>
        /// Read data from a buffer and populate object fields
        /// </summary>
        /// <param name="data"></param>
        void Read(DataBuffer data);
    }

    public static class ISerializeableExtensions
    {
        /// <summary>
        /// Returns a deep copy of an object (creates a lot of garabage)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="i"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T i) where T : ISerializeable, new()
        {
            //write to a databuffer
            var w = new DataBuffer(256);
            i.Write(w);
            //read from a databuffer
            var r = w.Copy();
            var new_t = new T();
            new_t.Read(r);
            //return
            return new_t;
        }
    }

    /// <summary>
    /// wrapper class for byte array
    /// </summary>
    public class DataBuffer
    {
        /// <summary>
        /// Max Buffer Size in bytes. This value is only enforced when performing DiskIO
        /// </summary>
        public static readonly int MaxBufferSize = 1000000000;

        /// <summary>
        /// internal index that represents the end of the buffer. (It's exposed because some operations need it to be)
        /// </summary>
        public int Count = 0;
        /// <summary>
        /// data buffer (It's exposed because some operations need it to be)
        /// </summary>
        public byte[] data;
        public DataBuffer(int count) { data = new byte[count]; }
        public DataBuffer() { data = new byte[0]; }

        /// <summary>
        /// pool of property maps
        /// </summary>
        public Pool<Dictionary<string, int>> property_map_pool;

        /// <summary>
        /// shorthand for double array size and copy old items into buffer
        /// </summary>
        /// <param name="newsize"> if 0, then array will double in size, if non zero, then array will resize to this size</param>
        public void Resize(int newsize = 0)
        {
            if (newsize == 0)
            {
                System.Array.Resize(ref data, data.Length == 0 ? 2 : data.Length * 2);
            }
            else
                System.Array.Resize(ref data, newsize);
        }

        /// <summary>
        /// add item to back of buffer
        /// </summary>
        public void Add(byte b)
        {
            if (Count >= data.Length)
                Resize();
            data[Count] = b;
            Count++;
        }

        public DataBuffer Copy()
        {
            var d = new DataBuffer(data.Length);
            data.CopyTo(d.data, 0);
            return d;
        }

        /// <summary>
        /// Write the entire data buffer to the desired path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        public void Save(string path, FileMode mode = FileMode.Create)
        {
            mkdirs(path);
            if (Count > MaxBufferSize)
                throw new System.Exception("Error, Data Buffer length exceeds " +
                    "maximum allowed size of: " + MaxBufferSize);
            using (FileStream f = new FileStream(path, mode, FileAccess.Write))
            {
                //Write length of data
                var bytes = new Bytes.BitsConverter.Int32();
                bytes.val = Count;
                f.WriteByte(bytes.Byte0);
                f.WriteByte(bytes.Byte1);
                f.WriteByte(bytes.Byte2);
                f.WriteByte(bytes.Byte3);
                f.Write(data, 0, Count);
            }
        }

        /// <summary>
        /// make directories. The input path should be to a file not a directory.
        /// </summary>
        /// <param name="path"></param>
        public static void mkdirs(string path)
        {
            var dpath = "";
            if (!Directory.Exists(dpath = Path.GetDirectoryName(path)))
            {
                Stack<string> append = new Stack<string>();
                while (!Directory.Exists(dpath))
                {
                    append.Push(Path.GetFileName(dpath));
                    dpath = Path.GetDirectoryName(dpath);
                }

                while (append.Count > 0)
                {
                    Directory.CreateDirectory(dpath = Path.Combine(dpath, append.Pop()));
                }
            }
        }

        /// <summary>
        /// Load the entire data buffer from the desired path. This will overwrite the current 'this.data'
        /// and reset the Count to 0
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="pad_buffer"> padding to add at the end of the internal buffer </param>
        public void Load(string path, FileMode mode = FileMode.Open, int pad_buffer = 0)
        {
            if (pad_buffer < 0)
                throw new System.Exception("Error, buffer padding cannot be less tahn 0");
            using (FileStream f = new FileStream(path, mode, FileAccess.Read))
            {
                //get length of data
                byte b1 = (byte)f.ReadByte(), b2 = (byte)f.ReadByte(),
                    b3 = (byte)f.ReadByte(), b4 = (byte)f.ReadByte();
                var length = (b4 << 24) | (b3 << 16) | (b2 << 8) | b1;

                if (length > MaxBufferSize)
                    throw new System.Exception("Error, Data Buffer length exceeds " +
                        "maximum allowed size of: " + MaxBufferSize);
                if (data == null || data.Length < (length + pad_buffer))
                    data = new byte[length + pad_buffer];
                f.Read(data, 0, length);
                Count = 0;
            }
        }

        /// <summary>
        /// shorthand for setting count = 0. This resets the buffer
        /// </summary>
        public void Reset()
        {
            Count = 0;
        }

        /// <summary>
        /// Test function to make sure types read and write the same amount of bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Verify<T>(T data) where T : ISerializeable
        {
            DataBuffer d = new DataBuffer(100);
            data.Write(d);
            var c = d.Count;

            d.Reset();

            data.Read(d);
            var c1 = d.Count;

            if (c1 != c)
                throw new System.Exception("Incorrect Counts. Invalid read/write will courrput the data buffer");

            return true;
        }
    }

    internal static class InternalObjects
    {
        public struct PropertyMapHeader : ISerializeable
        {
            public int index_of_map { get; private set; }
            public PropertyMapHeader(int index)
            {
                index_of_map = index;
            }

            public void Read(DataBuffer d)
            {
                index_of_map = d.ReadInt();
            }

            public void Write(DataBuffer d)
            {
                d.Write(index_of_map);
            }
        }

        public struct Property : ISerializeable
        {
            public string name { get; private set; }
            public int index { get; private set; }

            public Property(string name, int index) { this.name = name; this.index = index; }

            public void Read(DataBuffer d)
            {
                name = d.ReadString();
                index = d.ReadInt();
            }

            public void Write(DataBuffer d)
            {
                d.Write(name);
                d.Write(index);
            }
        }
    }

    /// <summary>
    /// Store objects as 'properties', objects using properties should not also use the Generic data buffer Read and Write functions.
    /// This property object should only be created in a using(var p = new PropertyReader(DataBuffer){//read all properties} statement
    /// </summary>
    public struct PropertyReader : System.IDisposable
    {
        /// <summary>
        /// index immediatly after the property reader header object ends
        /// </summary>
        public int StartIndex;
        /// <summary>
        /// index right after where the property map ends
        /// </summary>
        public int EndIndex;
        DataBuffer d;

        Dictionary<string, int> property_map;

        /// <summary>
        /// Create a property reader object by reading in data from the buffer
        /// </summary>
        /// <param name="d"></param>
        public PropertyReader(DataBuffer d)
        {
            var start = d.Count;

            try
            {
                this.d = d;

                if (d.property_map_pool == null)
                    d.property_map_pool = new Pool<Dictionary<string, int>>();
                this.property_map = d.property_map_pool.Get();

                //try to read properties
                InternalObjects.PropertyMapHeader header = default(InternalObjects.PropertyMapHeader);
                header.Read(d);
                this.StartIndex = d.Count; //set the start

                //set the read index to the location of the map
                d.Count = header.index_of_map;

                //construct the property map
                foreach (var property in d.ReadICollection<InternalObjects.Property>())
                    property_map.Add(property.name, property.index);

                //set end index
                this.EndIndex = d.Count;

                //reset index
                d.Count = this.StartIndex;
            }
            catch (System.Exception e)
            {
                d.Count = start;
                this.EndIndex = -1;
                this.StartIndex = -1;
                this.property_map = null;
                this.d = null;
#if DEBUG
                Debug.Log("Threw exception " + e + " While Creating property reader.");   
#endif
            }
        }

        public void Dispose()
        {
            if (ReferenceEquals(d, null))
                return;

            //set count to the end of the map
            d.Count = EndIndex;
            //free map
            property_map.Clear();
            d.property_map_pool.Add(property_map);
            //set buffer ref to null
            d = null;
        }

        /// <summary>
        /// get length of collection
        /// </summary>
        /// <param name="property_name"></param>
        /// <returns></returns>
        public int CollectionCount(string property_name)
        {
            int index;
            if (property_map.TryGetValue(property_name, out index))
            {
                return d.PeekCollectionLength(index);
            }
            return 0;
        }

        public T ReadProperty<T>(System.Func<T> read_function, string property_name, T default_value = default(T))
        {
            if (ReferenceEquals(d, null))
            {
#if DEBUG
                Debug.Log("Did not read property " + (ReferenceEquals(null, property_name) ? "NULL" : property_name));
#endif
                return default_value;
            }

            T value = default_value;
            int index;
            if (property_map.TryGetValue(property_name, out index))
            {
                d.Count = index;
                value = read_function();
            }
#if UNITY_EDITOR
            else
                Debug.Log("Failed to find property " + (property_name == null ? "NULL" : property_name));
#endif
            return value;
        }

        public void ReadProperty<T>(System.Action<DataBuffer> read_function, string property_name) where T : class
        {
            if (ReferenceEquals(d, null))
            {
#if DEBUG
                Debug.Log("Did not read property " + (ReferenceEquals(null, property_name) ? "NULL" : property_name));
#endif
                return;
            }

            int index;
            if (property_map.TryGetValue(property_name, out index))
            {
                d.Count = index;
                read_function(d);
            }
#if UNITY_EDITOR
            else
                Debug.Log("Failed to find property " + (property_name == null ? "NULL" : property_name));
#endif
        }

        public void ReadProperty<T>(ref T item, string property_name) where T : ISerializeable
        {
            if (ReferenceEquals(d, null))
            {
#if DEBUG
                Debug.Log("Did not read property " + (ReferenceEquals(null, property_name) ? "NULL" : property_name));
#endif
                return;
            }

            int index;
            if (property_map.TryGetValue(property_name, out index))
            {
                d.Count = index;
                item.Read(d);
            }
#if UNITY_EDITOR
            else
                Debug.Log("Failed to find property " + (property_name == null ? "NULL" : property_name));
#endif
        }
    }

    /// <summary>
    /// static class holding non null template objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Empty<T>
    {
        /// <summary>
        /// an empty ienumerable
        /// </summary>
        public static IEnumerable<T> Collection { get { yield break; } }
        public static readonly T[] Array = new T[0];
    }

    /// <summary>
    /// Store objects as 'properties', objects using properties should not also use the Generic data buffer Read and Write functions
    /// This property object should only be created in a using(var p = new PropertyReader(DataBuffer){ //write all properties} statement
    /// </summary>
    public struct PropertyWriter : System.IDisposable
    {
        /// <summary>
        /// index of property write header
        /// </summary>
        public int StartIndex;
        DataBuffer d;

        Dictionary<string, int> property_map;

        IEnumerable<InternalObjects.Property> properties()
        {
            foreach (var pair in property_map)
            {
                yield return new InternalObjects.Property(pair.Key, pair.Value);
            }
        }

        public PropertyWriter(DataBuffer d)
        {
            this.d = d;

            //make property map
            if (d.property_map_pool == null)
                d.property_map_pool = new Pool<Dictionary<string, int>>();
            this.property_map = d.property_map_pool.Get();

            //assign start index
            this.StartIndex = d.Count;

            //reserve space for header
            InternalObjects.PropertyMapHeader header = default(InternalObjects.PropertyMapHeader);
            header.Write(d);
        }

        public void WriteProperty<T>(System.Action<T> write_function, string property_name, T value)
        {
            int index = d.Count; //record index
            property_map.Add(property_name, index); //add property
            write_function(value); //write the value
        }

        public void WriteProperty(System.Action<DataBuffer> write_function, string property_name)
        {
            int index = d.Count;
            property_map.Add(property_name, index);
            write_function(d);
        }

        public void Dispose()
        {
            //record map begin index
            int map_index = d.Count;
            //write the map
            d.WriteICollection<InternalObjects.Property>(properties());
            //record map end index
            int end_index = d.Count;

            //reset the buffer to the header index
            d.Count = StartIndex;
            //create and write the header
            var header = new InternalObjects.PropertyMapHeader(map_index);
            header.Write(d);

            //reset the buffer index to the end
            d.Count = end_index;

            //free property map
            property_map.Clear();
            d.property_map_pool.Add(property_map);

            //set buffer ref to null
            d = null;
        }
    }

    public static class DataBufferExtenions
    {
        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, ushort val)
        {
            var bytes = new Bytes.BitsConverter.UInt16();
            bytes.val = val;
            if (Bytes.BitsConverter.IsLittleEndian)
            {
                arr.Add(bytes._Byte0);
                arr.Add(bytes._Byte1);
            }
            else
            {
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte0);
            }
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, int val)
        {
            var bytes = new Bytes.BitsConverter.Int32();
            bytes.val = val;
            if (Bytes.BitsConverter.IsLittleEndian)
            {
                arr.Add(bytes._Byte0);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte3);
            }
            else
            {
                arr.Add(bytes._Byte3);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte0);
            }
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, bool val)
        {
            arr.Add(Bytes.BitsConverter.Bool(val));
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, byte val)
        {
            arr.Add(val);
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, double val)
        {
            var bytes = new Bytes.BitsConverter.Double();
            bytes.val = val;
            if (Bytes.BitsConverter.IsLittleEndian)
            {
                arr.Add(bytes._Byte0);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte3);
                arr.Add(bytes._Byte4);
                arr.Add(bytes._Byte5);
                arr.Add(bytes._Byte6);
                arr.Add(bytes._Byte7);
            }
            else
            {
                arr.Add(bytes._Byte7);
                arr.Add(bytes._Byte6);
                arr.Add(bytes._Byte5);
                arr.Add(bytes._Byte4);
                arr.Add(bytes._Byte3);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte0);
            }
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, long val)
        {
            var bytes = new Bytes.BitsConverter.Long();
            bytes.val = val;
            if (Bytes.BitsConverter.IsLittleEndian)
            {
                arr.Add(bytes._Byte0);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte3);
                arr.Add(bytes._Byte4);
                arr.Add(bytes._Byte5);
                arr.Add(bytes._Byte6);
                arr.Add(bytes._Byte7);
            }
            else
            {
                arr.Add(bytes._Byte7);
                arr.Add(bytes._Byte6);
                arr.Add(bytes._Byte5);
                arr.Add(bytes._Byte4);
                arr.Add(bytes._Byte3);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte0);
            }
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, float val)
        {
            var bytes = new Bytes.BitsConverter.Single();
            bytes.val = val;
            if (Bytes.BitsConverter.IsLittleEndian)
            {
                arr.Add(bytes._Byte0);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte3);
            }
            else
            {
                arr.Add(bytes._Byte3);
                arr.Add(bytes._Byte2);
                arr.Add(bytes._Byte1);
                arr.Add(bytes._Byte0);
            }
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, Vector3 val)
        {
            arr.Write(val.x);
            arr.Write(val.y);
            arr.Write(val.z);
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, Vector3Int val)
        {
            arr.Write(val.x);
            arr.Write(val.y);
            arr.Write(val.z);
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer arr, Quaternion val)
        {
            arr.Write(val.x);
            arr.Write(val.y);
            arr.Write(val.z);
            arr.Write(val.w);
        }

        /// <summary>
        /// Write color32
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="c"></param>
        public static void Write(this DataBuffer arr, Color32 c)
        {
            arr.Write(c.ToInt());
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, ushort val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, int val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, bool val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, byte val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, double val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, long val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, float val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, Vector3 val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, Vector3Int val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, Quaternion val)
        {
            Bytes.Write(arr.data, index, val);
        }

        /// <summary>
        /// Write a value at index
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static void Write(this DataBuffer arr, int index, Color32 val)
        {
            Bytes.Write(arr.data, index, val.ToInt());
        }

        /// <summary>
        /// append value
        /// </summary>
        /// <param name="data"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer data, string s)
        {
            var strlen = s == null ? 0 : Bytes.StringByteLength(s); //uses UTF-16 (Unicode) encoding

            //increase buffer size if necessary
            if (data.Count + strlen + Bytes.string_padding >= data.data.Length) //make sure to add collection padding
                data.Resize((data.Count + strlen) * 2);

            data.Count = Bytes.Write(data.data, data.Count, s);
        }

        /// <summary>
        /// write at index
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="s"></param>
        public static void Write(this DataBuffer data, int index, string s)
        {
            Bytes.Write(data.data, index, s);
        }


        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static string ReadString(this DataBuffer data)
        {
            string s;
            data.Count = Bytes.Read(data.data, data.Count, out s);
            return s;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static int ReadInt(this DataBuffer data)
        {
            int val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static long ReadLong(this DataBuffer data)
        {
            long val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static ushort ReadUShort(this DataBuffer data)
        {
            ushort val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static double ReadDouble(this DataBuffer data)
        {
            double val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static bool ReadBool(this DataBuffer data)
        {
            bool val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static byte ReadByte(this DataBuffer data)
        {
            byte val;
            val = data.data[data.Count];
            data.Count++;
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static float ReadFloat(this DataBuffer data)
        {
            float val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static Quaternion ReadQuaternion(this DataBuffer data)
        {
            Quaternion val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static Vector3 ReadVector3(this DataBuffer data)
        {
            Vector3 val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }
        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static Vector3Int ReadVector3Int(this DataBuffer data)
        {
            Vector3Int val;
            data.Count = data.data.Read(data.Count, out val);
            return val;
        }

        /// <summary>
        /// read type and advance Count
        /// </summary>
        /// <param name="val"></param>
        public static Color32 ReadColor32(this DataBuffer data)
        {
            int val;
            data.Count = data.data.Read(data.Count, out val);
            return ColorExtensions.IntToColor32(val);
        }

        /// <summary>
        /// read collection and advance count. Ienumerables are lazy, so you should read the returned collection
        /// before continuing reading
        /// </summary>
        /// <param name="val"></param>
        public static IEnumerable<T> ReadCollection<T>(this DataBuffer data)
        {
            IEnumerable<T> val;
            data.Count = data.data.ReadCollection<T>(data.Count, out val);
            return val;
        }

        /// <summary>
        /// Write collection of Iserializable
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="Count"> If count is greater than zero, then only indices 0...Count will be written</param>
        public static void WriteICollection<T>(this DataBuffer data, IEnumerable<T> col, int Count) where T : ISerializeable
        {
            if (col == null || Count == 0)
            {
                data.Write((int)0);
                data.Write((int)0);
                return;
            }

            int length_pos = data.Count;
            data.Write((int)0);

            int collection_length_index = data.Count;
            int collection_length = 0;
            data.Write(collection_length);

            int start = data.Count;

            foreach (var c in col)
            {
                //stop at max count
                if (Count > 0 && collection_length >= Count)
                    break;

                var loop_start_count = data.Count;

                c.Write(data);

                if (data.Count == loop_start_count)
                    throw new System.Exception(typeof(T) + " ISerialize write method does not advance the data buffer count.");

                collection_length++;
            }
            data.Write(length_pos, data.Count - start);
            data.Write(collection_length_index, collection_length);
        }

        /// <summary>
        /// Write collection of Iserializable
        /// </summary>
        /// <param name="data"></param>
        /// <param name="col"></param>
        /// <param name="Count"> If count is greater than zero, then only indices 0...Count will be written</param>
        public static void WriteICollection<T>(this DataBuffer data, IEnumerable<T> col) where T : ISerializeable
        {
            WriteICollection<T>(data, col, -1);
        }

        /// <summary>
        /// read collection and make new objects
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadICollection<T>(this DataBuffer data)
            where T : ISerializeable, new()
        {
            var length = data.ReadInt();
            data.ReadInt(); //read number of items (we dont use it, so just consume)
            var stop = length + data.Count;
            while (data.Count < stop)
            {
                int loop_start_length = data.Count;

                var t = new T();
                t.Read(data);

                if (loop_start_length == data.Count)
                    throw new System.Exception(typeof(T) + " ISerialize Read Method does not advance the data buffer count.");

                yield return t;
            }
        }

        /// <summary>
        /// Read into array of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T[] ReadIArray<T>(this DataBuffer data) where T : ISerializeable, new()
        {
            var length = data.ReadInt();
            var num_items = data.ReadInt();
            var array = new T[num_items];
            var stop = length + data.Count;
            int n = 0;
            while (data.Count < stop)
            {
                int loop_start_length = data.Count;

                array[n] = new T();
                array[n].Read(data);

                if (loop_start_length == data.Count)
                    throw new System.Exception(typeof(T) + " ISerialize Read Method does not advance the data buffer count.");

                n++;
            }

            return array;
        }

        /// <summary>
        /// Write a collection of primitive to bytes. The Generic Ienumerbale method is slower than the list and array method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="collection"></param>
        public static void WriteCollection<T>(this DataBuffer bytes, IEnumerable<T> collection)
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

            //write collection length
            int collection_length_index = bytes.Count;
            int collection_length = 0;
            bytes.Write(collection_length);

            //record position
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
            else if (typeof(T) == typeof(string))
            {
                var col = (IEnumerable<string>)collection;
                foreach (var t in col)
                {
                    bytes.Write(t);
                    collection_length++;
                }
            }
            else
            {
                throw new System.Exception("Error, unsupported type. " + typeof(T) +
                    " Supported types are int,bool,byte,float,double,Quaternion(d),Vector3(d)");
            }

            //get length
            var len = bytes.Count - start;
            //write length
            bytes.Write(length_index, len);
            //write collection length
            bytes.Write(collection_length_index, collection_length);
        }

        public static int PeekCollectionLength(this DataBuffer data, int index)
        {
            int len;
            Bytes.Read(data.data, index + 4, out len);
            return len;
        }

        public static void WriteIArray<T>(this DataBuffer data, T[] collection) where T : ISerializeable
        {
            int Count = ReferenceEquals(collection, null) ? 0 : collection.Length;
            var col = collection;

            if (col == null || Count == 0)
            {
                data.Write((int)0);
                data.Write((int)0);
                return;
            }

            int length_pos = data.Count;
            data.Write((int)0);

            int collection_length_index = data.Count;
            int collection_length = 0;
            data.Write(collection_length);

            int start = data.Count;

            for (int i = 0; i < Count; i++)
            {
                //stop at max count
                if (Count > 0 && collection_length >= Count)
                    break;

                var loop_start_count = data.Count;

                col[i].Write(data);

                if (data.Count == loop_start_count)
                    throw new System.Exception(typeof(T) + " ISerialize write method does not advance the data buffer count.");

                collection_length++;
            }
            data.Write(length_pos, data.Count - start);
            data.Write(collection_length_index, collection_length);
        }

        public static int[] ReadIntArray(this DataBuffer data)
        {
            int[] arr;
            data.Count = data.data.ReadIntArray(data.Count, out arr);
            return arr;
        }
        public static bool[] ReadBoolArray(this DataBuffer data)
        {
            bool[] arr;
            data.Count = data.data.ReadBoolArray(data.Count, out arr);
            return arr;
        }
        public static float[] ReadFloatArray(this DataBuffer data)
        {
            float[] arr;
            data.Count = data.data.ReadFloatArray(data.Count, out arr);
            return arr;
        }
        public static double[] ReadDoubleArray(this DataBuffer data)
        {
            double[] arr;
            data.Count = data.data.ReadDoubleArray(data.Count, out arr);
            return arr;
        }
        public static ushort[] ReadUShortArray(this DataBuffer data)
        {
            ushort[] arr;
            data.Count = data.data.ReadUShortArray(data.Count, out arr);
            return arr;
        }
        public static byte[] ReadByteArray(this DataBuffer data)
        {
            byte[] arr;
            data.Count = data.data.ReadByteArray(data.Count, out arr);
            return arr;
        }
        public static Vector3[] ReadVector3Array(this DataBuffer data)
        {
            Vector3[] arr;
            data.Count = data.data.ReadVector3Array(data.Count, out arr);
            return arr;
        }
        public static Quaternion[] ReadQuaternionArray(this DataBuffer data)
        {
            Quaternion[] arr;
            data.Count = data.data.ReadQuaternionArray(data.Count, out arr);
            return arr;
        }
        public static string[] ReadStringArray(this DataBuffer data)
        {
            string[] arr;
            data.Count = data.data.ReadStringArray(data.Count, out arr);
            return arr;
        }

        /// <summary>
        /// Write a nullable type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void WriteRefType<T>(this DataBuffer d, T obj) where T : ISerializeable, new()
        {
            if (default(T) == null)
            {
                if (ReferenceEquals(obj, null))
                {
                    d.Write(false);
                }
                else
                {
                    d.Write(true);
                    obj.Write(d);
                }
            }
            else
                throw new System.Exception("Error, this function is only for reference types");
        }

        /// <summary>
        /// Read a nullable type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T ReadRefType<T>(this DataBuffer d) where T : ISerializeable, new()
        {
            if (default(T) == null)
            {
                var notnull = d.ReadBool();

                if (notnull)
                {
                    var newT = new T();
                    newT.Read(d);
                    return newT;
                }
                else
                    return default(T);
            }
            else
                throw new System.Exception("Error, this function is only for reference types");
        }

        public static T StringToEnum<T>(string s, T default_value = default(T)) where T : struct
        {
            T enum_val;
            if (System.Enum.TryParse(s, out enum_val))
            {
                return enum_val;
            }

            return default_value;
        }
    }

    public static class ColorExtensions
    {
        public static int ToInt(this Color c)
        {
            var c32 = new Color32((byte)(255 * c.r), (byte)(255 * c.g),
                (byte)(255 * c.b), (byte)(255 * c.a));
            return c32.ToInt();
        }

        public static int ToInt(this Color32 c)
        {
            return (c.r << 24) | (c.g << 16) | (c.b << 8) | c.a;
        }

        public static Color IntToColor(int i)
        {
            var c32 = IntToColor32(i);
            var c = new Color();
            c.r = c32.r * 0.00390625f;
            c.g = c32.g * 0.00390625f;
            c.b = c32.b * 0.00390625f;
            c.a = c32.a * 0.00390625f;
            return c;
        }

        public static Color32 IntToColor32(int i)
        {
            var c = new Color32();
            c.r = (byte)(i >> 24);
            c.g = (byte)(i >> 16);
            c.b = (byte)(i >> 8);
            c.a = (byte)(i);
            return c;
        }

        public static Color32 ToColor32(this Color color)
        {
            return (Color32)color;
        }
    }

#if UNITY_EDITOR
    namespace serialize_test
    {
        public class serialize_test
        {
            struct test_obj : ISerializeable
            {
                public string a;
                float b;
                bool c;

                public test_obj(string a, float b, bool c) { this.a = a; this.b = b; this.c = c; }

                public void Read(DataBuffer d)
                {
                    using (var reader = new PropertyReader(d))
                    {
                        a = reader.ReadProperty(d.ReadString, "str_prop");
                        b = reader.ReadProperty(d.ReadFloat, "float_prop");
                        c = reader.ReadProperty(d.ReadBool, "bool_prop");
                    }
                }

                public void Write(DataBuffer d)
                {
                    using (var writer = new PropertyWriter(d))
                    {
                        writer.WriteProperty(d.Write, "str_prop", a);
                        writer.WriteProperty(d.Write, "float_prop", b);
                        writer.WriteProperty(d.Write, "bool_prop", c);
                    }
                }
                public override string ToString()
                {
                    return a + ", " + b + ", " + c;
                }
            }

            struct test_obj2 : ISerializeable
            {
                public string a;
                float b;
                bool c;

                public test_obj2(string a, float b, bool c) { this.a = a; this.b = b; this.c = c; }

                public void Read(DataBuffer d)
                {
                    a = d.ReadString();
                    b = d.ReadFloat();
                    c = d.ReadBool();
                }

                public void Write(DataBuffer d)
                {
                    d.Write(a);
                    d.Write(b);
                    d.Write(c);
                }
                public override string ToString()
                {
                    return a + ", " + b + ", " + c;
                }
            }

            public void test() { serialize_test1(); serialize_test2(); }

            void serialize_test1()
            {
                DataBuffer d = new DataBuffer(1024);
                test_obj o = new test_obj("abc", 1.23f, true);

                using (var writer = new PropertyWriter(d))
                {
                    writer.WriteProperty<int>(d.Write, "int_prop", 1);
                    writer.WriteProperty<bool>(d.Write, "bool_prop", true);
                    writer.WriteProperty<string>(d.Write, "str_prop", "yoyoyoyoyoyo");
                    writer.WriteProperty(o.Write, "test_obj");
                    writer.WriteProperty<IEnumerable<test_obj>>(d.WriteICollection, "test_obj_collection", new List<test_obj>() { o, o, o, o, o });
                    writer.WriteProperty<string[]>(d.WriteCollection, "str_array", new string[] { "a", "b", "c" });
                }

                d.Count = 0;

                using (var reader = new PropertyReader(d))
                {
                    foreach (var c in reader.ReadProperty(d.ReadICollection<test_obj>, "test_obj_collection"))
                        Debug.Log(c);

                    Debug.Log(reader.ReadProperty(d.ReadInt, "int_prop"));
                    Debug.Log(reader.ReadProperty(d.ReadString, "str_prop"));
                    Debug.Log(reader.ReadProperty(d.ReadBool, "bool_prop"));

                    reader.ReadProperty(ref o, "test_obj");
                    Debug.Log(o);

                    foreach (var c in reader.ReadProperty(d.ReadStringArray, "str_array"))
                        Debug.Log(c);
                }
            }
            void serialize_test2()
            {
                DataBuffer d = new DataBuffer(1024);
                test_obj2 o = new test_obj2("abc", 1.23f, true);

                using (var writer = new PropertyWriter(d))
                {
                    writer.WriteProperty<int>(d.Write, "int_prop", 1);
                    writer.WriteProperty<bool>(d.Write, "bool_prop", true);
                    writer.WriteProperty<string>(d.Write, "str_prop", "yoyoyoyoyoyo");
                    writer.WriteProperty(o.Write, "test_obj");
                    writer.WriteProperty<IEnumerable<test_obj2>>(d.WriteICollection, "test_obj_collection", new List<test_obj2>() { o, o, o, o, o });
                    writer.WriteProperty<string[]>(d.WriteCollection, "str_array", new string[] { "a", "b", "c" });
                }

                d.Count = 0;

                using (var reader = new PropertyReader(d))
                {
                    foreach (var c in reader.ReadProperty(d.ReadICollection<test_obj2>, "test_obj_collection"))
                        Debug.Log(c);

                    Debug.Log(reader.ReadProperty(d.ReadInt, "int_prop"));
                    Debug.Log(reader.ReadProperty(d.ReadString, "str_prop"));
                    Debug.Log(reader.ReadProperty(d.ReadBool, "bool_prop"));

                    reader.ReadProperty(ref o, "test_obj");
                    Debug.Log(o);

                    foreach (var c in reader.ReadProperty(d.ReadStringArray, "str_array"))
                        Debug.Log(c);
                }
            }
        }


    }
#endif
}
