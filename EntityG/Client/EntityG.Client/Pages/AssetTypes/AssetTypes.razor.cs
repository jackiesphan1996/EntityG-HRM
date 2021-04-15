using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;
using EntityG.Contracts.Requests.AssetTypes;
using EntityG.Contracts.Responses.AssetTypes;

namespace EntityG.Client.Pages.AssetTypes
{
    public partial class AssetTypes
    {
        private List<AssetTypeDto> AssetTypeResponses = new List<AssetTypeDto>();
        private AssetTypeDto AssetType = new AssetTypeDto();

        private int Page { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private bool IsLoading { get; set; }
        private bool AssetTypeDialogVisible { get; set; }
        private Form<AssetTypeDto> AssetTypeDialogForm { get; set; }

        private string AssetTypeDialogTitle { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAllAssetTypes();
        }

        private void HandleCreate()
        {
            AssetType = new AssetTypeDto();
            AssetTypeDialogTitle = "Create Asset Type";
            AssetTypeDialogVisible = true;
        }

        private async Task GetAllAssetTypes()
        {
            IsLoading = true;
            var response = await _assetTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                AssetTypeResponses = response.Data;
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
        private async Task HandleAssetTypeDialogOk()
        {
            if (this.AssetTypeDialogForm.Validate())
            {
                await SaveAsync(AssetType);
            }

        }

        private void HandleAssetTypeDialogCancel()
        {
            AssetTypeDialogVisible = false;
        }
        private async Task SaveAsync(AssetTypeDto assetType)
        {
            if (assetType.Id == 0)
            {
                await CreateAsync(assetType);
            }
            else
            {
                await UpdateAsync(assetType);
            }
        }

        private void Edit(int id)
        {
            var existedAssetType = AssetTypeResponses.FirstOrDefault(x => x.Id.Equals(id));
            AssetType = new AssetTypeDto()
            {
                Id = id,
                Name = existedAssetType.Name,
                Description = existedAssetType.Description
            };

            AssetTypeDialogTitle = "Update Asset Type";
            AssetTypeDialogVisible = true;
        }
        private async Task CreateAsync(AssetTypeDto assetType)
        {
            var request = new CreateAssetTypeDto {Name = assetType.Name, Description = assetType.Description};

            var response = await _assetTypeManager.CreateAsync(request);

            if (response.Succeeded)
            {
                await GetAllAssetTypes();
                AssetTypeDialogVisible = false;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task UpdateAsync(AssetTypeDto assetType)
        {
            var request = new UpdateAssetTypeDto
            {
                Id = assetType.Id, 
                Name = assetType.Name,
                Description = assetType.Description
            };

            var response = await _assetTypeManager.UpdateAsync(request);

            if (response.Succeeded)
            {
                await GetAllAssetTypes();
                AssetTypeDialogVisible = false;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }
        private async Task ShowDeleteConfirm(AssetTypeDto assetType)
        {
            var content = $"Are you sure to delete Asset Type  '{assetType.Name}' ?";
            var title = "Delete confirmation";
            var confirmResult = await _confirmService.Show(content, title, ConfirmButtons.YesNo);
            if (confirmResult == ConfirmResult.Yes)
            {
                await Delete(assetType);
            }
        }

        private async Task Delete(AssetTypeDto assetType)
        {
            var response = await _assetTypeManager.DeleteAsync(assetType.Id);
            if (response.Succeeded)
            {
                await GetAllAssetTypes();
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