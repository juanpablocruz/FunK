using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FunK.Tests
{
    using static F;
    public class OperationTests
    {
        static Func<int, int> Add3 = i => i + 3;
        static Func<int, Func<int,int>> Times = n => i => i * n;
        static Func<int, int> Times10 = i => Times(10)(i);
        static Func<int, int> Sub5 = i => i - 5;
        static Func<int, Task<int>> Add5 = i => Async(i + 5);
        static Func<string, string> Capitalize = str => str.ToUpper();

        static Func<string, Func<int, List<string>>> CreateList = value => i => 
            Identity(new List<string>(i))
            .Map( l => {
                l.AddRange(Enumerable.Repeat(value, i));
                return l;
                })()
            ;

        static Func<int, Task<List<string>>> FetchNames = i => Async(CreateList("value")(i));
        static Func<int, int> ThrowErr = i => throw new InvalidCastException();
        static Func<int, Task<int>> WaitAndReturn = i => Task.FromResult(i);

        static List<string> GetExpectedListValue(int n, string text)
            => Identity(1).Map(Times(n)).Map(CreateList(text))();

        static Task<Result<int>> GetFirstResult(int i)
            => Begin(i).Then(Add3).Then(Add5).Finally(WaitAndReturn);

        [Fact]
        public async void Can_Compose_And_Flat_Value()
        {
            var value = await Begin(2)
                .Then(Add3)
                .Then(Add5)
                .Then(WaitAndReturn)
                .Finally(FetchNames);

            var expected = new List<string>(10);
            expected.AddRange(Enumerable.Repeat("value", 10));

            Assert.Equal(expected, value.GetOrThrow());
        }


        [Fact]
        public async void Can_Compose_With_Throwing_Functions_And_Throw_On_Flattening()
        {
            var value = await Begin(2)
                .Then(Add3)
                .Then(Add5)
                .Finally(ThrowErr);

            Assert.Throws<InvalidCastException>(() => value.GetOrThrow());
        }

        Result<int> ShouldReturnInt()
        {
            var a = true;
            return a ? Result(2) : Result<int>(new ArgumentNullException());
        }


        [Fact]
        public void test()
        {
            var num = ShouldReturnInt();

            var res2 = num.Map(x => x.ToString());

            var inner = num.Match(
                Error: ex => 0,
                Success: t => t
            );

            var innerValue = num.GetOrThrow<ArgumentNullException>();
        }

        [Fact]
        public async void Can_Compose_Operations()
        {
            var firstResult = await GetFirstResult(2);
            var composedResult = await firstResult.Apply(Add5).Finally();
            Assertions.ResultEquals(15, composedResult);
        }

        [Fact]
        public async void Can_Compose_Async_Operations()
        {
            var composedResult = await GetFirstResult(2).Apply(Add5).Finally();
            Assertions.ResultEquals(15, composedResult);
        }

        [Fact]
        public async void Can_Compose_With_Single_Branching()
        {
            var Add5If15 = Begin(1).Then(Add5);
            Predicate<int> Is15 = x => x == 15;

            var composedResult = await Begin(2)
                .Then(Add3)
                .Then(Add5)
                .Apply(Add5If15)
                .Then(WaitAndReturn)
                .Then(Add5)
                .IfThen(Is15, Add5If15)
                .Finally();

            Assertions.ResultEquals(20, composedResult);
        }

        [Fact]
        public async void Can_Compose_With_Complex_Branching()
        {
            var Add5If15 = Begin(1).Then(Add5).Then(Sub5).Then(Add5);
            var Sub5IfNot15 = Begin(1).Then(Sub5);
            Predicate<int> IsNot15 = x => x != 15;

            var composedResult = await GetFirstResult(2)
                .Apply(Add5)
                .IfThenElse(IsNot15, Add5If15, Sub5IfNot15)
                .Finally();

            Assertions.ResultEquals(10, composedResult);
        }



        [Fact]
        public async void Can_Compose_With_Enumerable_And_Map_Each_Element()
        {
            var value = await Begin(2)
                .Then(Add3)
                .Then(Add5)
                .Then(Times10)
                .Then(Times10)
                .Then(WaitAndReturn)
                .Then(FetchNames)
                .ThenForEach(Capitalize)
                .Finally();

            Assertions.ResultEquals(GetExpectedListValue(10 * 10 * 10, "VALUE"), value);
        }

        

        [Fact]
        public async void Can_Compose_With_Result()
        {
            Task<Result<int>> ConvertToResult(int i) => Begin(i).Finally();
            var value = await Begin(2)
                    .Then(Add3)
                    .Then(ConvertToResult)
                    .Finally();

            Assertions.ResultEquals(5, value);
        }

        [Fact]
        public async void Can_Compose_With_Enumerable_And_Map_Each_Element_On_Finally()
        {
            var value = await Begin(2)
                .Then(Add3)
                .Then(Add5)
                .Then(Times10)
                .Then(Times10)
                .Then(WaitAndReturn)
                .Then(FetchNames)
                .FinallyForEach(Capitalize);

            Assertions.ResultEquals(GetExpectedListValue(10 * 10 * 10, "VALUE"), value);
        }


        [Fact]
        public async void Can_Begin_With_Tuples_And_Compose_Deconstructed()
        {
            (int, string) ValidateIsTuple((int, string) data) => data;
            string Concat(int val, string name) => $"{name} {val}";
            string ToUpper(string str) => str.ToUpper();
            var intval = 2;
            var name = "number";

            var value = await Begin((intval, name))
                   .Then(ValidateIsTuple)
                   .Then(Concat)
                   .Then(ToUpper)
                   .Finally();
                ;

            Assertions.ResultEquals("NUMBER 2", value);
        }

        [Fact]
        public async void Can_Compose_With_Single_Branching_With_Func()
        {
            int Add5If10(int val) => val + 5;
            Predicate<int> Is10 = x => x == 10;

            var composedResult = await Begin(2)
                .Then(Add3)
                .Then(Add5)
                .Then(WaitAndReturn)
                .Then(Add5)
                .IfThen(Is10, Add5If10)
                .Finally();

            Assertions.ResultEquals(15, composedResult);
        }

        [Fact]
        public async void Can_Compose_With_Complex_Branching_With_Funcs()
        {
            int Add5If15(int val) => val + 5;
            int Sub5IfNot15(int val) => val - 5;
            Predicate<int> IsNot15 = x => x != 15;

            var composedResult = await GetFirstResult(2)
                .Apply(Add5)
                .IfThenElse(IsNot15, Add5If15, Sub5IfNot15)
                .Finally();

            Assertions.ResultEquals(10, composedResult);
        }

        [Fact]
        public async void Can_Compose_With_Complex_Branching_With_Funcs_Different_Return_Type()
        {
            string Add5If15(int val) => $"{val + 5}";
            string Sub5IfNot15(int val) => $"{val - 5}";
            Predicate<int> IsNot15 = x => x != 15;

            var composedResult = await GetFirstResult(2)
                .Apply(Add5)
                .IfThenElse(IsNot15, Add5If15, Sub5IfNot15)
                .Finally();

            Assertions.ResultEquals("10", composedResult);
        }


        [Fact]
        public async void Can_Begin_With_Tuples_And_Compose_Passing_Tuple()
        {
            (int, string) ValidateIsTuple(int val, string data) => (val,data);
            string Concat((int, string) data) => $"{data.Item2} {data.Item1}";
            string ToUpper(string str) => str.ToUpper();
            var intval = 2;
            var name = "number";

            var value = await Begin((intval, name))
                   .Then(ValidateIsTuple)
                   .Then(Concat)
                   .Then(ToUpper)
                   .Finally();
            ;

            Assertions.ResultEquals("NUMBER 2", value);
        }

        [Fact]
        public async void Can_Begin_With_Tuples_And_Compose_Passing_Tuple_And_Result()
        {
            Task<Result<(int, string)>> ValidateIsTuple(int val, string data) => Begin((val, data)).Finally();
            string Concat((int, string) data) => $"{data.Item2} {data.Item1}";
            string ToUpper(string str) => str.ToUpper();
            var intval = 2;
            var name = "number";

            var value = await Begin((intval, name))
                   .Then(ValidateIsTuple)
                   .Then(Concat)
                   .Then(ToUpper)
                   .Finally();
            ;

            Assertions.ResultEquals("NUMBER 2", value);
        }

        [Fact]
        public async void Can_Compose_With_Complex_Branching_With_Funcs_Different_Return_Type_Async_Result()
        {
            Task<Result<string>> Add5If15(int val) => Begin(val).Finally(v => $"{v + 5}");
            Task<Result<string>> Sub5IfNot15(int val) => Begin(val).Finally(v => $"{v - 5}");
            Predicate<int> IsNot15 = x => x != 15;

            var composedResult = await GetFirstResult(2)
                .Apply(Add5)
                .IfThenElse(IsNot15, Add5If15, Sub5IfNot15)
                .Finally();

            Assertions.ResultEquals("10", composedResult);
        }


        [Fact]
        public async void Can_Compose_With_Func_Returning_Task()
        {
            async Task FuncReturningTask(int i)
            {
                await Task.FromResult(i);
            }


            var value = await Begin(1)
                .Then(FuncReturningTask)
                .Finally(Add3);

            Assertions.ResultEquals(4, value);
        }

        [Fact(Skip = "Benchmark")]
        public async void Parallel_Benchmark_Huge_List()
        {
            
            var data = Range(1, 1000000000);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var valueStraight = await Begin(data)
                .Then(list => list.Map(e => e * 5))
                .Finally();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            var watchParallel = System.Diagnostics.Stopwatch.StartNew();
            var valueForEach = await Begin(data)
                .ThenForEach(e => e * 5)
                .Finally();
            watchParallel.Stop();
            var elapsedParallelMs = watchParallel.ElapsedMilliseconds;

            Assert.True(elapsedMs > elapsedParallelMs);
        }


        [Fact]
        public async void TestParallelMap()
        {
            var data = Range(1, 1000000000);
            Func<int, int> multiply = (a) => a * 2;
            Func<int, int> add2 = (a) => a + 2;
            Func<int, string> toString = (a) => a.ToString();
            var sw = new System.Diagnostics.Stopwatch();
            var fn = toString.Compose(multiply).Compose(add2);
            sw.Start();
            sw.Stop();

            sw.Restart();
            var bestResAsync = (await Begin(data)
                .ThenForEachAsync(fn)
                .Finally()).GetOrThrow();
            sw.Stop();
            var elapsedBestAsync = sw.ElapsedTicks;

            sw.Restart();
            var res = (await Begin(data)
                .ThenForEach(multiply)
                .ThenForEach(add2)
                .ThenForEach(toString)
                .Finally()).GetOrThrow();
            sw.Stop();
            var elapsedSync = sw.ElapsedTicks;

            sw.Restart();
            var resAsync = (await Begin(data)
                .ThenForEachAsync(multiply)
                .ThenForEachAsync(add2)
                .ThenForEachAsync(toString)
                .Finally()).GetOrThrow();
            sw.Stop();
            var elapsedAsync = sw.ElapsedTicks;

            Assert.True(elapsedAsync < elapsedSync);
        }


        [Fact(Skip = "Benchmark")]
        public async void Parallel_Benchmark_Heavy_Task()
        {

            var data = Range(1, 1000000);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var valueStraight = await Begin(data)
                .Then(list => list.Map(e =>
                {
                    Thread.Sleep(2);
                    return e * 5;
                }))
                .Finally();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            var watchParallel = System.Diagnostics.Stopwatch.StartNew();
            var valueForEach = await Begin(data)
                .ThenForEach(e =>
                {
                    Thread.Sleep(2);
                    return e * 5;
                })
                .Finally();
            watchParallel.Stop();
            var elapsedParallelMs = watchParallel.ElapsedMilliseconds;

            Assert.True(elapsedMs > elapsedParallelMs);
        }
    }
}

