﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

// ReSharper disable PossibleMultipleEnumeration

namespace PeanutButter.Utils.Tests
{
    [TestFixture]
    public class TestAsyncExtensionsForIEnumerables
    {
        [TestFixture]
        public class ForEachAsync
        {
            [Test]
            public async Task WithIndex_ShouldBeAbleToDoAsync()
            {
                //--------------- Arrange -------------------
                var collection = GetRandomCollection<int>(200);
                var collector = new List<int>();
                var indexes = new List<int>();

                //--------------- Assume ----------------

                //--------------- Act ----------------------
                await collection.ForEachAsync(async (x, y) => await Task.Run(() =>
                {
                    collector.Add(x);
                    indexes.Add(y);
                }));

                //--------------- Assert -----------------------
                Expect(collector).To.Be.Equal.To(collection);
                Expect(indexes).To.Contain.Exactly(collection.Count()).Items();
                Expect(indexes).To.Contain.All().Matched.By((x, y) => x == y);
            }

            [Test]
            public async Task ForEach_ShouldBeAbleToDoAsync()
            {
                //--------------- Arrange -------------------
                var collection = GetRandomCollection<int>(200);
                var collector = new List<int>();

                //--------------- Assume ----------------

                //--------------- Act ----------------------
                await collection.ForEachAsync(async (i) => await Task.Run(() => collector.Add(i)));

                //--------------- Assert -----------------------
                Expect(collector).To.Be.Equal.To(collection);
            }
        }

        [TestFixture]
        public class ToArrayAsync
        {
            [Test]
            public async Task ShouldProvideAwaitableToArray()
            {
                // Arrange
                var expected = GetRandomCollection<string>(3);
                var src = Task.FromResult(
                    expected
                );
                // Pre-Assert
                // Act
                var result = await src.ToArrayAsync();
                // Assert
                Expect(result).To.Equal(expected);
            }
        }

        [TestFixture]
        public class AggregateAsync
        {
            private async Task<bool> IsEven(int i)
            {
                return await Task.FromResult(i % 2 == 0);
            }

            [Test]
            public async Task ShouldBeAbleToAggregateAsync()
            {
                // Arrange
                var src = GetRandomCollection<int>(10, 20);
                var expected = src.Where(i => i % 2 == 0).ToArray();
                // Pre-Assert
                // Act
                var result = await src.AggregateAsync(
                    new List<int>(),
                    async (acc, cur) =>
                    {
                        if (await (IsEven(cur)))
                            acc.Add(cur);
                        return acc;
                    }
                );
                // Assert
                Expect(result).To.Equal(expected);
            }
        }
    }
}