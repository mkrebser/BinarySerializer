# Serialize

Simple binary serialization interface.  
Contains many functions for writing various types to and from a data buffer class.  

To implement serialization for an object, it must inherit the ISerialize interface. Note* Data must be read and written in the same order.

```  
public struct TransformStruct : ISerializeable
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
	
    public void Write(DataBuffer data)
    {
        data.Write(Position);
        data.Write(Rotation);
        data.Write(Scale);
    }

    public void Read(DataBuffer data)
    {
        Position = data.ReadVector3();
        Rotation = data.ReadQuaternion();
        Scale = data.ReadVector3();
    }
}  
```  

Additionally there is another more flexible serializer that serializes objects into binary dictionaries using the PropertyReader & PropertyWriter. This way versioning of objects can be easily applied.  

```
public class class1
{
	Vector3 position;
	Quaternion rotation;
	Vector3 scale;
	float radius;
	List<TransformStruct> all_instance_transforms;
	int attr1;
	double attr2;
	List<double> collection1;
	
	public void Read(DataBuffer d)
	{
		using (var r = new PropertyReader(d))
		{
			TransformStruct transform = default(TransformStruct); 
			r.ReadProperty(ref transform, "transform");
			
			this.position = transform.Position;
			this.rotation = transform.Rotation;
			this.scale = transform.Scale;

			this.radius = r.ReadProperty(d.ReadFloat, "radius");
			this.all_instance_transforms = r.ReadProperty(d.ReadICollection<TransformStruct>, "all_instance_transforms").ToList();

			this.attr1 = r.ReadProperty(d.ReadInt, "attr1_key");
			this.attr2 = r.ReadProperty(d.ReadDouble, "attr2_key");

			this.collection1 = new List<double>(r.ReadProperty(d.ReadCollection<double>, "collection1_key", Empty<double>.Collection));
		}
	}

	public void Write(DataBuffer d)
	{
		using (var w = new PropertyWriter(d))
		{
			w.WriteProperty(new TransformStruct(this.position, this.rotation, this.scale).Write, "transform");
			w.WriteProperty(d.Write, "radius", this.radius);
			w.WriteProperty(d.WriteICollection, "all_instance_transforms", all_instance_transforms);
			
			w.WriteProperty(d.Write, "attr1_key", this.attr1);
			w.WriteProperty(d.Write, "attr2_key", this.attr2);
			
			w.WriteProperty(d.WriteCollection, "collection1_key", this.collection1);
		}
	}
}
```


