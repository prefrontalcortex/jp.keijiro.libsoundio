using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

static class SpanNativeArrayExtensions
{
    public static NativeArray<T>
        GetNativeArray<T>(this ReadOnlySpan<T> span) where T : unmanaged
    {
        unsafe
        {
            fixed (void* ptr = &span.GetPinnableReference())
            {
                var array = NativeArrayUnsafeUtility.
                    ConvertExistingDataToNativeArray<T>
                        (ptr, span.Length, Allocator.None);

                var handle = AtomicSafetyHandle.GetTempUnsafePtrSliceHandle();
                NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref array, handle);

                return array;
            }
        }
    }

    public static ReadOnlySpan<T>
        GetReadOnlySpan<T>(this NativeArray<T> array) where T : unmanaged
    {
        unsafe {
            var ptr = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(array);
            return new Span<T>(ptr, array.Length);
        }
    }
}
