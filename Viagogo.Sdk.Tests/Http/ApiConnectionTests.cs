﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Viagogo.Sdk.Http;

namespace Viagogo.Sdk.Tests.Http
{
    [TestFixture]
    public class ApiConnectionTests
    {
        private static  ApiConnection CreateConnection(IConnection conn = null)
        {
            return new ApiConnection(
                conn ?? new Mock<IConnection>(MockBehavior.Loose).Object);
        }

        private static IApiResponse<T> CreateResponse<T>(T body = null)
            where T : class, new()
        {
            return new ApiResponse<T> {BodyAsObject = body ?? new T()};
        }

        private class Foo
        {
        }

        [Test]
        public void Connection_ShouldReturnTheConnectionPassedToTheCtor()
        {
            var expectedConnection = new Mock<IConnection>(MockBehavior.Loose).Object;
            var apiConnection = CreateConnection(conn: expectedConnection);

            var actualConnection = apiConnection.Connection;

            Assert.AreSame(expectedConnection, actualConnection);
        }

        [Test]
        public async void PostAsync_ShouldPassTheGivenUriToTheConnection()
        {
            var expectedUri = new Uri("https://api.vgg.io");
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    expectedUri,
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(expectedUri, null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassPostHttpMethodToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    HttpMethod.Post,
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(new Uri("https://vgg.com/endpoint"), null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldPassTheGivenDataToTheConnection()
        {
            var expectedBody = new {foo = "bar"};
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    expectedBody,
                                    It.IsAny<string>()))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(new Uri("https://vgg.com/endpoint"), expectedBody);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldHalJsonContentTypeToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    "application/hal+json"))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(new Uri("https://vgg.com/endpoint"), null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldHalJsonAcceptHeaderToTheConnection()
        {
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    "application/hal+json"))
                    .Returns(Task.FromResult(CreateResponse<Foo>()))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            await apiConnection.PostAsync<Foo>(new Uri("https://vgg.com/endpoint"), null);

            mockConn.Verify();
        }

        [Test]
        public async void PostAsync_ShouldReturnTheBodyOfTheResponseReturnedByTheConnection()
        {
            var expectedResult = new Foo();
            var mockConn = new Mock<IConnection>(MockBehavior.Loose);
            mockConn.Setup(c => c.SendRequestAsync<Foo>(
                                    It.IsAny<Uri>(),
                                    It.IsAny<HttpMethod>(),
                                    It.IsAny<string>(),
                                    It.IsAny<object>(),
                                    It.IsAny<string>()))
                                    .Returns(Task.FromResult(CreateResponse(body: expectedResult)))
                    .Verifiable();
            var apiConnection = CreateConnection(conn: mockConn.Object);

            var actualResult = await apiConnection.PostAsync<Foo>(new Uri("https://vgg.com/endpoint"), null);

            Assert.AreSame(expectedResult, actualResult);
        }
    }
}
