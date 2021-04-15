using EntityG.BusinessLogic.Mappers;
using EntityG.Contracts.Requests.Assets;
using EntityG.Contracts.Responses.Assets;
using EntityG.EntityFramework.Entities;
using NUnit.Framework;
using System;

namespace EntityG.BusinessLogic.Test.Mappers
{
    [TestFixture]
    public class AssetMapperTest
    {
        [Test]
        public void Mapper_Maps_To_Entity_Correctly()
        {
            // Arrange
            var request = new CreateAssetDto
            {
                AssetName = "AssetName",
                AssetTypeId = 1,
                Description = "Description",
                IsActive = true,
                PurchaseDate = new DateTime(2021, 3, 12),
                PurchasePrice = 1000,
                UsedById = 99
            };

            // Act
            Asset asset = AssetMapper.ToEntity(request);

            // Assert
            Assert.IsNotNull(asset);
            Assert.AreEqual(request.AssetName, asset.AssetName);
            Assert.AreEqual(request.AssetTypeId, asset.AssetTypeId);
            Assert.AreEqual(request.Description, asset.Description);
            Assert.AreEqual(request.PurchaseDate, asset.PurchaseDate);
            Assert.AreEqual(request.PurchasePrice, asset.PurchasePrice);
            Assert.AreEqual(request.IsActive, asset.IsActive);
            Assert.AreEqual(request.UsedById, asset.UsedById);
        }

        [Test]
        public void Mapper_Maps_To_Response_Correctly()
        {
            // Arrange
            var asset = new Asset
            {
                Id = 1,
                AssetName = "AssetName",
                AssetTypeId = 1,
                Description = "Description",
                IsActive = true,
                PurchaseDate = new DateTime(2021, 3, 12),
                PurchasePrice = 1000,
                UsedById = 99,
                AssetType = new AssetType
                {
                    Id = 1,
                    Name = "AssetTypeName"
                },
                UsedBy = new Employee
                {
                    Id = 99,
                    EmployeeIdNumber = "hoang.phan"
                }
            };

            // Act
            AssetDto response = AssetMapper.Map(asset);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Id, asset.Id);
            Assert.AreEqual(response.AssetName, asset.AssetName);
            Assert.AreEqual(response.AssetTypeId, asset.AssetTypeId);
            Assert.AreEqual(response.AssetType, asset.AssetType.Name);
            Assert.AreEqual(response.Description, asset.Description);
            Assert.AreEqual(response.PurchaseDate, asset.PurchaseDate);
            Assert.AreEqual(response.PurchasePrice, asset.PurchasePrice);
            Assert.AreEqual(response.IsActive, asset.IsActive);
            Assert.AreEqual(response.UsedById, asset.UsedById);
            Assert.AreEqual(response.UsedBy, asset.UsedBy.EmployeeIdNumber);
        }
    }
}
