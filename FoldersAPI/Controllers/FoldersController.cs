using Domain.Interfaces;
using Domains.Entities.DTOs;
using Domains.Entities.FolderDbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoldersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IFoldersService _foldersService;

        public FoldersController(
            ILogger<FoldersController> logger,
            IFoldersService foldersService)
        {
            _logger = logger;
            _foldersService = foldersService;
        }

        [HttpGet("get-folder")]
        public async Task<ActionResult<Folders>> GetFolder(string folderNodeId)
        {
            _logger.LogInformation("GetFolder invoked");

            var response = await _foldersService.GetFolder(folderNodeId);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders");
                return this.BadRequest("No folders available");
            }
            else
            {
                return response;
            }
        }

        [HttpGet("get-folder-files")]
        public async Task<ActionResult<List<Files>>> GetFolderFiles(string folderNodeId)
        {
            _logger.LogInformation("GetFolderAndItsFiles invoked");

            var response = await _foldersService.GetFolderFiles(folderNodeId);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else
            {
                return response;
            }
        }

        [HttpGet("get-folder-subfolders")]
        public async Task<ActionResult<List<Folders>>> GetFoldersSubfolders(string folderNodeId)
        {
            _logger.LogInformation("GetFoldersSubfoldersAndFiles invoked");

            var response = await _foldersService.GetFolderSubfolders(folderNodeId);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else
            {
                return response;
            }
        }

        [HttpPost("add-new-file")]
        public async Task<ActionResult<List<Folders>>> AddNewFile([FromBody] AddNewFileRequest request)
        {
            _logger.LogInformation("AddNewFile called with parameters {@request}", request);

            var response = await _foldersService.AddNewFile(request.ParentNodeId, request.FileName);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else if (!response.ActionSuccessful)
            {
                //Remove if in production, exception message passed
                return this.BadRequest(response.ErrorMessage);
            }
            else
            {
                return this.Ok();
            }
        }

        [HttpPost("add-new-folder")]
        public async Task<ActionResult<List<Folders>>> AddNewFolder([FromBody] AddNewFolderRequest request)
        {
            _logger.LogInformation("AddNewFolder called with parameters {@request}", request);

            var response = await _foldersService.AddNewFolder(request.ParentNodeId, request.FolderName);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else if (!response.ActionSuccessful)
            {
                //Remove if in production, exception message passed
                return this.BadRequest(response.ErrorMessage);
            }
            else
            {
                return this.Ok();
            }
        }

        [HttpPost("delete-folder")]
        public async Task<ActionResult<List<Folders>>> DeleteFolder([FromBody] DeleteFolderRequest request)
        {
            _logger.LogInformation("DeleteFolder called with parameters {@request}", request);

            var response = await _foldersService.DeleteFolder(request.NodeId);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else if (!response.ActionSuccessful)
            {
                //Remove if in production, exception message passed
                return this.BadRequest(response.ErrorMessage);
            }
            else
            {
                return this.Ok();
            }
        }

        [HttpDelete("delete-file")]
        public async Task<ActionResult<List<Folders>>> DeleteFile(int id)
        {
            _logger.LogInformation("DeleteFile called with parameters {@id}", id);

            var response = await _foldersService.DeleteFile(id);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else if (!response.ActionSuccessful)
            {
                //Remove if in production, exception message passed
                return this.BadRequest(response.ErrorMessage);
            }
            else
            {
                return this.Ok();
            }
        }

        [HttpGet("find-files-by-name")]
        public async Task<ActionResult<List<Files>>> FindFilesByName(string serachString)
        {
            _logger.LogInformation("FindByName called with parameters {serachString}", serachString);

            var response = await _foldersService.FindByName(serachString);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else
            {
                return this.Ok(response);
            }
        }

        [HttpGet("find-files-by-name-within-the-folder")]
        public async Task<ActionResult<List<Files>>> FindFilesByNameWithinTheFolder(string parentNode, string serachString)
        {
            _logger.LogInformation("FindByNameWithinTheFolder called with parameters {serachString}", serachString);

            var response = await _foldersService.FindByNameWithinFolder(parentNode, serachString);

            if (response == null)
            {
                _logger.LogInformation("Could not find any folders nor files");
                return this.BadRequest("No folders available");
            }
            else
            {
                return this.Ok(response);
            }
        }
    }
}
