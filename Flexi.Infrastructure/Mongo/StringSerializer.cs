using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Flexi.Infrastructure.Mongo;

public abstract class StringSerializer<T> : IBsonSerializer<T?>
{
    protected abstract T? FromString(string value);

    protected abstract string? ToString(T? value);

    object? IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args)!;
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T? value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();
            return;
        }

        context.Writer.WriteString(ToString(value));
    }

    public T? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;

        var bsonType = bsonReader.GetCurrentBsonType();
        if (bsonType == BsonType.Null)
        {
            bsonReader.ReadNull();
            return default;
        }

        return FromString(context.Reader.ReadString());
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        switch (value)
        {
            case T typedValue:
                context.Writer.WriteString(ToString(typedValue));
                break;

            case null:
                context.Writer.WriteNull();
                break;

            default:
                throw new NotSupportedException($"Could not serialize value '{value}'. Not a valid {nameof(T)}");
        }
    }

    public Type ValueType => typeof(T);
}