﻿using Microsoft.Azure.Cosmos;
using Moq;
using System.Collections.Generic;
using System.Threading;

namespace Bookstore.API.UnitTests.TestHelpers
{
    public static class CosmosExtensions
    {
        public static Mock<ItemResponse<T>> SetupCreateItemAsync<T>(this Mock<Container> containerMock)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock
                .Setup(x => x.CreateItemAsync(
                    It.IsAny<T>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .Callback((T item, PartitionKey? partitionKey, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Setup(x => x.Resource).Returns(null))
                .ReturnsAsync((T item, PartitionKey? partitionKey, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Object);

            return itemResponseMock;
        }

        public static (Mock<FeedResponse<T>> feedResponseMock, Mock<FeedIterator<T>> feedIterator) SetupItemQueryIteratorMock<T>(this Mock<Container> containerMock, IEnumerable<T> itemsToReturn)
        {
            var feedRepsonseMock = new Mock<FeedResponse<T>>();
            feedRepsonseMock.Setup(x => x.Resource).Returns(itemsToReturn);
            var iteratorMock = new Mock<FeedIterator<T>>();
            iteratorMock.SetupSequence(x => x.HasMoreResults).Returns(true).Returns(false);
            iteratorMock.Setup(x => x.ReadNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(feedRepsonseMock.Object);
            containerMock.Setup(x => x.GetItemQueryIterator<T>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()))
                .Returns(iteratorMock.Object);

            return (feedRepsonseMock, iteratorMock);
        }

        public static Mock<ItemResponse<T>> SetupDeleteItemAsync<T>(this Mock<Container> containerMock)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock.Setup(x => x.DeleteItemAsync<T>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
                .Callback((string id, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Setup(x => x.Resource).Returns(null))
                .ReturnsAsync((string id, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Object);


            return itemResponseMock;
        }

        public static Mock<ItemResponse<T>> SetupReplaceItemAsync<T>(this Mock<Container> containerMock)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock.Setup(x => x.ReplaceItemAsync(
                It.IsAny<T>(),
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
                .Callback((T item, string id, PartitionKey? pk, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Setup(x => x.Resource).Returns(null))
                .ReturnsAsync((T item, string id, PartitionKey? pk, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Object);

            return itemResponseMock;
        }

        public static Mock<ItemResponse<T>> SetupReadItemAsync<T>(this Mock<Container> containerMock, T itemToReturn)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock.Setup(x => x.ReadItemAsync<T>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
                .Callback((string id, PartitionKey? pk, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Setup(x => x.Resource).Returns(itemToReturn))
                .ReturnsAsync((string id, PartitionKey? pk, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Object);

            return itemResponseMock;
        }

        public static Mock<ItemResponse<T>> SetupReadItemAsyncNotFound<T>(this Mock<Container> containerMock)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock.Setup(x => x.ReadItemAsync<T>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
                .Callback((string id, PartitionKey? pk, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Setup(x => x.StatusCode).Returns(System.Net.HttpStatusCode.NotFound))
                .ReturnsAsync((string id, PartitionKey? pk, ItemRequestOptions opts, CancellationToken ct) => itemResponseMock.Object);

            return itemResponseMock;
        }
    }
}
