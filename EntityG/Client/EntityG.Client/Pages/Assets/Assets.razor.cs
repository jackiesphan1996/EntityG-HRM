using AntDesign;
using EntityG.Contracts.Requests.Assets;
using EntityG.Contracts.Responses.Shared;
using EntityG.Client.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityG.Contracts.Responses.Assets;
using EntityG.Contracts.Responses.AssetTypes;

namespace EntityG.Client.Pages.Assets
{
    public partial class Assets
    {
        private IEnumerable<AssetDto> AssetResponses = new List<AssetDto>();
        private List<AssetTypeDto> AssetTypes = new List<AssetTypeDto>();
        private List<LookupDto> Employees = new List<LookupDto>();
        private AssetRequest Asset = new AssetRequest();

        private int Page { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private bool IsLoading { get; set; }
        private bool AssetDialogVisible { get; set; }
        private Form<AssetRequest> AssetDialogForm { get; set; }

        private string AssetDialogTitle { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var allTasks = new List<Task>
            {
                GetAllAssets(),
                GetAllAssetTypes(),
                GetAlLEmployees()
            };

            await Task.WhenAll(allTasks);
        }

        private async Task GetAllAssetTypes()
        {
            var response = await _assetTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                AssetTypes = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task GetAlLEmployees()
        {
            var response = await _employeeManager.GetAllAsync();
            if (response.Succeeded)
            {
                Employees = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private void HandleCreate()
        {
            Asset = new AssetRequest();
            Asset.PurchaseDate = DateTime.Now;
            AssetDialogTitle = "Create Asset";
            AssetDialogVisible = true;
        }

        private async Task GetAllAssets()
        {
            IsLoading = true;
            var response = await _assetManager.GetAllAsync();
            if (response.Succeeded)
            {
                AssetResponses = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }

            IsLoading = false;
        }
        private async Task HandleAssetDialogOk()
        {
            if (this.AssetDialogForm.Validate())
            {
                await SaveAsync(Asset);
            }

        }

        private void HandleAssetDialogCancel()
        {
            AssetDialogVisible = false;
        }
        private async Task SaveAsync(AssetRequest asset)
        {
            if (asset.Id == 0)
            {
                await CreateAsync(asset);
            }
            else
            {
                await UpdateAsync(asset);
            }
        }

        private void Edit(int id)
        {
            var existedAsset = AssetResponses.FirstOrDefault(x => x.Id.Equals(id));

            Asset = new AssetRequest
            {
                Id = id,
                AssetName = existedAsset.AssetName,
                Description = existedAsset.Description,
                PurchasePrice = existedAsset.PurchasePrice,
                PurchaseDate = existedAsset.PurchaseDate,
                AssetTypeId = existedAsset.AssetTypeId,
                IsActive = existedAsset.IsActive,
                UsedById = existedAsset.UsedById
            };

            AssetDialogTitle = "Update Asset";
            AssetDialogVisible = true;
        }
        private async Task CreateAsync(AssetRequest asset)
        {
            var request = new CreateAssetDto
            {
                AssetName = asset.AssetName,
                AssetTypeId = asset.AssetTypeId,
                PurchaseDate = asset.PurchaseDate,
                PurchasePrice = asset.PurchasePrice,
                IsActive = asset.IsActive,
                UsedById = asset.UsedById,
                Description = asset.Description
            };

            var response = await _assetManager.CreateAsync(request);

            if (response.Succeeded)
            {
                await GetAllAssets();
                AssetDialogVisible = false;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task UpdateAsync(AssetRequest asset)
        {
            var request = new UpdateAssetDto
            {
                Id = asset.Id,
                AssetName = asset.AssetName,
                AssetTypeId = asset.AssetTypeId,
                UsedById = asset.UsedById,
                PurchaseDate = asset.PurchaseDate,
                PurchasePrice = asset.PurchasePrice,
                IsActive = asset.IsActive,
                Description = asset.Description
            };

            var response = await _assetManager.UpdateAsync(request);

            if (response.Succeeded)
            {
                await GetAllAssets();
                AssetDialogVisible = false;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }
        private async Task ShowDeleteConfirm(AssetDto asset)
        {
            var content = $"Are you sure to delete Asset Type  '{asset.AssetName}' ?";
            var title = "Delete confirmation";
            var confirmResult = await _confirmService.Show(content, title, ConfirmButtons.YesNo);
            if (confirmResult == ConfirmResult.Yes)
            {
                await Delete(asset);
            }
        }

        private async Task Delete(AssetDto asset)
        {
            var response = await _assetManager.DeleteAsync(Asset.Id);
            if (response.Succeeded)
            {
                await GetAllAssets();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }
    }
}