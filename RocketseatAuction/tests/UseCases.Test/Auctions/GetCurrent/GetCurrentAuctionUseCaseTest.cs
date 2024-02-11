﻿using Bogus;
using FluentAssertions;
using Moq;
using RocketseatAuction.API.Contracts;
using RocketseatAuction.API.Entities;
using RocketseatAuction.API.Enums;
using RocketseatAuction.API.UseCases.Auctions.GetCurrent;
using Xunit;

namespace UseCases.Test.Auction.GetCurrent
{
    public class GetCurrentAuctionUseCaseTest
    {
        [Fact]
        public void Success() 
        {
            //ARRANGE
            var entity = new Faker<RocketseatAuction.API.Entities.Auction>()
                .RuleFor(auction => auction.Id, f => f.Random.Number(1,10))
                .RuleFor(auction => auction.Name, f => f.Lorem.Word())
                .RuleFor(auction => auction.Starts, f => f.Date.Past())
                .RuleFor(auction => auction.Ends, f => f.Date.Future())
                .RuleFor(auction => auction.Items, (f, auction) => new List<Item> 
                { 
                    new Item 
                    { 
                        Id = f.Random.Number(1,10),
                        Name = f.Commerce.Product(),
                        Brand = f.Commerce.Department(),
                        BasePrice = f.Random.Decimal(50, 1000),
                        Condition = f.PickRandom<Condition>(),
                        AuctionId = auction.Id
                    }
                }).Generate();

            var mock = new Mock<IAuctionRepository>();
            mock.Setup(i => i.GetCurrent()).Returns(entity);

            var useCase = new GetCurrentAuctionUseCase(mock.Object);

            //ACT
            var auction = useCase.Execute();

            //ASSERT
            auction.Should().NotBeNull();
            auction!.Id.Should().Be(entity.Id);
        }
    }
}