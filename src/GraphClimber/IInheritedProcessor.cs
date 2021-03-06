﻿namespace GraphClimber
{
    /// <summary>
    /// Defines an actor that can process read values that are 
    /// of type <typeparamref name="TField"/> or descendents
    /// of that type.
    /// </summary>
    /// <typeparam name="TField"></typeparam>
    public interface IReadProcessor<in TField>
    {
        void ProcessForRead(IReadValueDescriptor<TField> descriptor);
    }

    /// <summary>
    /// Defines an actor that can process values that are 
    /// of static type <typeparamref name="TField"/> or less 
    /// </summary>
    /// <typeparam name="TField"></typeparam>
    /// <remarks>
    /// Example: Set all fields that their static type is 
    /// assignable from IEnumerable{char} to string.Empty.
    /// This includes object and IEnumerable, IEnumerable{string}.
    /// </remarks>
    public interface IWriteProcessor<out TField>
    {
        void ProcessForWrite(IWriteValueDescriptor<TField> descriptor);
    }

    /// <summary>
    /// Defines an actor that can process values that are 
    /// of runtime type <typeparamref name="TRuntime"/> and static type <typeparamref name="TField"/>.
    /// </summary>
    /// <typeparam name="TField"></typeparam>
    /// <typeparam name="TRuntime"></typeparam>
    public interface IReadWriteProcessor<in TField, out TRuntime>
    {
        void ProcessForReadWrite(IReadWriteValueDescriptor<TField, TRuntime> descriptor);
    }

    /// <summary>
    /// Defines an actor that can process values that are 
    /// of both runtime and static type <typeparamref name="TField"/>.
    /// </summary>
    /// <typeparam name="TField"></typeparam>
    public interface IReadWriteProcessor<TField>
    {
        void ProcessForReadWrite(IReadWriteValueDescriptor<TField> descriptor);
    }
}