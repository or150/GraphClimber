using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace GraphClimber.Tests
{
    public class GenericArgumentBinderTests
    {

        [Test]
        [TestCaseSource("TestData")]
        public void Should_Return_Method_Info_When_Can(MethodInfo methodInfo, Type[] realTypes, MethodInfo expectedMethodInfo)
        {
            MethodInfo actualMethodInfo;

            Assert.That(new GenericArgumentBinder().TryBind(methodInfo, realTypes, out actualMethodInfo), Is.EqualTo(expectedMethodInfo != null));
            Assert.That(expectedMethodInfo, Is.EqualTo(actualMethodInfo));

        }

        public static IEnumerable<object[]> TestData
        {
            get
            {
                foreach (var methodInfo in typeof (ITestData).GetMethods())
                {
                    foreach (var data in methodInfo.GetCustomAttributes<TestDataAttribute>())
                    {
                        yield return new object[]
                        {
                            methodInfo,
                            new[] {data.RealType},
                            data.ShouldReturnMethod ? methodInfo.MakeGenericMethod(data.GenericParameterTypes) : null
                        };
                    }
                }
            }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        private class TestDataAttribute : Attribute
        {
            private readonly Type _realType;
            private readonly bool _shouldReturnMethod;
            private readonly Type[] _genericParameterTypes;

            public TestDataAttribute(Type realType, bool shouldReturnMethod, params Type[] genericParameterTypes)
            {
                _realType = realType;
                _shouldReturnMethod = shouldReturnMethod;
                _genericParameterTypes = genericParameterTypes;
            }

            public Type RealType
            {
                get { return _realType; }
            }

            public bool ShouldReturnMethod
            {
                get { return _shouldReturnMethod; }
            }

            public Type[] GenericParameterTypes
            {
                get { return _genericParameterTypes; }
            }
        }

        private interface ITestData
        {

            [TestData(typeof(object), true, typeof(object))]
            void Simplest<T>(T a);

            [TestData(typeof(List<List<int>>), true, typeof(List<int>), typeof(int))]
            void HardTest<TEnumerable, TKaki>(IEnumerable<TEnumerable> a)
                where TEnumerable : IEnumerable<TKaki>;


            [TestData(typeof(int[][]), false)]
            [TestData(typeof(int[,][]), true, typeof(int))]
            void ArrayTest<TArray>(TArray[,][] array);

            [TestData(typeof (int[][]), true, typeof (int[]))]
            void OtherArrayTest<TEnumerable>(TEnumerable[] array)
                where TEnumerable : IEnumerable<int>;


            [TestData(typeof(int[]), true, typeof(object))]
            [TestData(typeof(Stopwatch), true, typeof(Stopwatch))]
            void NewTest<T>(T a)
                where T : new();

            [TestData(typeof(TestClass), true, typeof(TestClass))]
            [TestData(typeof(OtherTestClass), true, typeof(OtherTestClass))]
            [TestData(typeof(OtherStuff), false)]
            void TestAllInterfaceImplementatinos<T>(T obj)
                where T : IEquatable<int>;

        }

        class OtherTestClass : IEquatable<DateTime>, IEquatable<int>
        {
            public bool Equals(int other)
            {
                throw new NotSupportedException();
            }

            public bool Equals(DateTime other)
            {
                throw new NotSupportedException();
            }
        }

        class TestClass : IEquatable<int>, IEquatable<DateTime>
        {
            public bool Equals(int other)
            {
                throw new NotSupportedException();
            }

            public bool Equals(DateTime other)
            {
                throw new NotSupportedException();
            }
        }

        class OtherStuff : IEquatable<DateTime>
        {
            public bool Equals(DateTime other)
            {
                throw new NotImplementedException();
            }
        }

    }
}