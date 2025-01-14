﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Sofee.Core.Api.Models.Languages;
using Xunit;

namespace Sofee.Core.Api.Tests.Unit.Services.Foundations
{
    public partial class LanguageServiceTests
    {
        [Fact]
        public async Task ShouldAddLanguageAsync()
        {
            //given 
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Language randomLanguage = CreateRandomLanguage(randomDateTime);
            Language inputLanguage = randomLanguage;
            Language storageLanguage = inputLanguage;
            Language expectedLanguage = storageLanguage.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertLanguageAsync(inputLanguage))
                    .ReturnsAsync(storageLanguage);

            //when
            Language actualLanguage =
                await this.languageService.AddLanguageAsync(inputLanguage);

            //then
            actualLanguage.Should().BeEquivalentTo(expectedLanguage);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLanguageAsync(inputLanguage),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
