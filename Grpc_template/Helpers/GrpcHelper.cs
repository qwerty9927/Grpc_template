using System.Collections;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc_template.Common;
using Grpc_template.Protos;
using Grpc.Core;
using Mapster;

namespace Grpc_template.Helpers;

public static class GrpcHelper
{
    private static TDestination ObjectTypeConverting<TSource, TDestination, TObjectDestination>(
        TSource source)
        where TDestination : new()
    {
        TDestination target = new TDestination();
        PropertyInfo[] properties = source.GetType().GetProperties();

        for (var i = 0; i < properties.Length; i++)
        {
            var targetProp = target.GetType().GetProperty(properties[i].Name);
            var sourceValue = properties[i].GetValue(source);

            if (targetProp == null || sourceValue == null) continue;

            if (targetProp.PropertyType == typeof(Any))
            {
                targetProp.SetValue(target,
                    Any.Pack(ConvertBuildInType(sourceValue) ?? (IMessage)sourceValue.Adapt<TObjectDestination>()!));
                continue;
            }

            targetProp.SetValue(target, sourceValue);
        }

        return target;
    }

    private static TDestination IterableTypeConverting<TSource, TDestination, TIterableDestination>(
        TSource source)
        where TDestination : new()
    {
        TDestination target = new TDestination();
        PropertyInfo[] properties = source.GetType().GetProperties();

        for (var i = 0; i < properties.Length; i++)
        {
            var targetProp = target.GetType().GetProperty(properties[i].Name);
            var sourceValue = properties[i].GetValue(source);

            if (targetProp == null || sourceValue == null) continue;

            if (targetProp.PropertyType == typeof(RepeatedField<Any>) && sourceValue is IList)
            {
                var messages = (RepeatedField<Any>)targetProp.GetValue(target)!;
                var paramSourceData = (IList)sourceValue;
                foreach (var item in paramSourceData)
                {
                    var packedMessage =
                        Any.Pack((IMessage)item.Adapt<TIterableDestination>()!);
                    messages.Add(packedMessage);
                }

                continue;
            }

            targetProp.SetValue(target, sourceValue);
        }

        return target;
    }

    private static TDestination PagingTypeConverting<TSource, TDestination, TIterableDestination>(
        TSource source)
        where TDestination : new()
    {
        TDestination target = new TDestination();
        PropertyInfo[] properties = source.GetType().GetProperties();

        for (var i = 0; i < properties.Length; i++)
        {
            var targetProp = target.GetType().GetProperty(properties[i].Name);
            var sourceValue = properties[i].GetValue(source);

            if (targetProp == null || sourceValue == null) continue;

            if (targetProp.PropertyType == typeof(PagingGrpcResponse) &&
                sourceValue is PagingResponse)
            {
                PagingGrpcResponse paging =
                    IterableTypeConverting<PagingResponse, PagingGrpcResponse,
                        TIterableDestination>((PagingResponse)sourceValue);

                targetProp.SetValue(target, paging);

                continue;
            }

            targetProp.SetValue(target, sourceValue);
        }

        return target;
    }

    public static TDestination ConvertingStrategy<TSource, TDestination, TNestedDestination>(TSource source)
        where TSource : BaseResponse where TDestination : IMessage, new()
    {
        return typeof(TDestination) switch
        {
            { } t when t == typeof(GrpcResponse) =>
                ObjectTypeConverting<TSource, TDestination, TNestedDestination>(source),
            { } t when t == typeof(GrpcIterableResponse) =>
                IterableTypeConverting<TSource, TDestination, TNestedDestination>(source),
            { } t when t == typeof(GrpcPagingResponse) =>
                PagingTypeConverting<TSource, TDestination, TNestedDestination>(source),
            _ => throw new BaseException("Converting type not supported", (int)StatusCode.Internal)
        };
    }

    private static IMessage? ConvertBuildInType<TSource>(TSource source)
    {
        return source switch
        {
            bool boolValue => new BoolValue { Value = boolValue },
            int intValue => new Int32Value { Value = intValue },
            long longValue => new Int64Value { Value = longValue },
            float floatValue => new FloatValue { Value = floatValue },
            double doubleValue => new DoubleValue { Value = doubleValue },
            string stringValue => new StringValue { Value = stringValue },
            byte[] byteArray => new BytesValue { Value = ByteString.CopyFrom(byteArray) },
            _ => null
        };
    }
}