using Domain.Interfaces;
using Domains.Entities.DTOs;
using Domains.Entities.FolderDbModels;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class FoldersService : IFoldersService
    {
        private readonly ILogger _logger;
        private readonly IFoldersRepository _folderRepository;

        public FoldersService(
            ILogger<FoldersService> logger,
            IFoldersRepository foldersRepository)
        {
            _logger = logger;
            _folderRepository = foldersRepository;
        }

        public async Task<Folders> GetFolder(string folderNodeId)
        {
            _logger.LogInformation("FoldersService GetFolder invoked");

            return await _folderRepository.GetFolder(folderNodeId);
        }

        public async Task<List<Files>> GetFolderFiles(string folderNodeId)
        {
            _logger.LogInformation("FoldersService GetFolderAndItsFiles invoked");

            var parentFolder = await _folderRepository.GetFolder(folderNodeId);

            if (parentFolder == null)
            {
                return null;
            }

            return await _folderRepository.GetFolderFiles(parentFolder.Id);
        }

        public async Task<List<Folders>> GetFolderSubfolders(string folderNodeId)
        {
            _logger.LogInformation("FoldersService GetFolderAndItsChildren invoked");

            return await _folderRepository.GetFolderSubfolders(folderNodeId);
        }

        public async Task<AddNewFolderResponse> AddNewFolder(string parentNodeId, string folderName)
        {
            _logger.LogInformation("FoldersService AddNewFolder invoked");

            var response = new AddNewFolderResponse();
            using (IDbContextTransaction transaction = await _folderRepository.BeginTransaction())
            {
                try
                {
                    var parentFolder = await _folderRepository.GetFolder(parentNodeId);

                    if (parentFolder == null)
                    {
                        transaction.Dispose();

                        response.ActionSuccessful = false;
                        response.ErrorMessage = $"Can not find folder NodeId {parentNodeId}";
                        return response;
                    }

                    var node = parentFolder.ChildrenFoldersCount + 1;
                    var parentNode = parentFolder.NodeId;
                    string childrenNode = parentNode + "." + node.ToString();

                    var newFolder = new Folders()
                    {
                        Name = folderName,
                        ParentNode = parentNode,
                        NodeId = childrenNode,
                        ChildrenFoldersCount = 0
                    };

                    parentFolder.ChildrenFoldersCount++;

                    await _folderRepository.AddFolder(newFolder);
                    await _folderRepository.SaveChangesAsync();

                    await transaction.CommitAsync();

                    response.ActionSuccessful = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    _logger.LogError(ex, "Error at transaction, method AddNewFolder");

                    response.ActionSuccessful = false;
                    response.ErrorMessage = ex.Message;
                }
                finally
                {
                    if (_folderRepository.GetCurrentTransaction() == transaction)
                    {
                        transaction.Dispose();
                    }
                }

                return response;
            }
        }

        public async Task<AddNewFileResponse> AddNewFile(string parentNodeId, string fileName)
        {
            _logger.LogInformation("FoldersService AddNewFolder invoked");

            var response = new AddNewFileResponse();
            using (IDbContextTransaction transaction = await _folderRepository.BeginTransaction())
            {
                try
                {
                    var parentFolder = await _folderRepository.GetFolder(parentNodeId);

                    if (parentFolder == null)
                    {
                        transaction.Dispose();

                        response.ActionSuccessful = false;
                        response.ErrorMessage = $"Can not find folder NodeId {parentNodeId}";
                        return response;
                    }

                    await _folderRepository.AddFile(new Files() { Name = fileName, Folders = parentFolder });

                    await _folderRepository.SaveChangesAsync();

                    await transaction.CommitAsync();

                    response.ActionSuccessful = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError(ex, "Error at method AddNewFile");

                    response.ActionSuccessful = false;
                    response.ErrorMessage = ex.Message;
                }
                finally
                {
                    if (_folderRepository.GetCurrentTransaction() == transaction)
                    {
                        transaction.Dispose();
                    }
                }
            }

            return response;
        }

        public async Task<DeleteFolderResponse> DeleteFolder(string NodeId)
        {
            _logger.LogInformation("FoldersService DeleteFolder invoked");

            if (NodeId == "0")
            {
                return new DeleteFolderResponse()
                {
                    ActionSuccessful = false,
                    ErrorMessage = "Can not delete root folder"
                };
            }
            var response = new DeleteFolderResponse();
            using (IDbContextTransaction transaction = await _folderRepository.BeginTransaction())
            {
                try
                {
                    var selectedFolder = await _folderRepository.GetFolder(NodeId);

                    if (selectedFolder == null)
                    {
                        transaction.Dispose();

                        response.ActionSuccessful = false;
                        response.ErrorMessage = $"Can not find folder NodeId {NodeId}";
                        return response;
                    }

                    var nodeIdWithDot = selectedFolder.NodeId + ".";

                    List<Folders> nodesToRemove = await _folderRepository.GetFolderNodes(selectedFolder.NodeId);
                    List<Files> filesToRemove = new List<Files>();

                    foreach(var file in nodesToRemove)
                    {
                        var files = await _folderRepository.GetFolderFiles(file.Id);
                        filesToRemove.AddRange(files);
                    }

                    nodesToRemove.Add(selectedFolder);
                    _folderRepository.RemoveMultipleFolders(nodesToRemove);
                    _folderRepository.RemoveMultipleFiles(filesToRemove);

                    await _folderRepository.SaveChangesAsync();
                    await transaction.CommitAsync();

                    response.ActionSuccessful = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    _logger.LogError(ex, "Error at transaction, method DeleteFolder for {NodeId}", NodeId);

                    response.ActionSuccessful = false;
                    response.ErrorMessage = ex.Message;
                }
                finally
                {
                    if (_folderRepository.GetCurrentTransaction() == transaction)
                    {
                        transaction.Dispose();
                    }
                }
                return response;
            }
        }

        public async Task<DeleteFileResponse> DeleteFile(int id)
        {
            _logger.LogInformation("FoldersService DeleteFile invoked");

            var file = await _folderRepository.GetFile(id);

            if (file == null)
            {
                return new DeleteFileResponse()
                {
                    ActionSuccessful = false,
                    ErrorMessage = $"Can not find file with if {id}"
                };
            }

            _folderRepository.RemoveFile(file);

            await _folderRepository.SaveChangesAsync();

            return new DeleteFileResponse()
            {
                ActionSuccessful = true
            };
        }

        public async Task<List<Files>> FindByName(string serachString)
        {
            _logger.LogInformation("FoldersService FindByName invoked");

            return await _folderRepository.FindByName(serachString);
        }

        public async Task<List<Files>> FindByNameWithinFolder(string parentNode, string serachString)
        {
            _logger.LogInformation("FoldersService FindByNameWithinFolder invoked");
            var folder = await _folderRepository.GetFolder(parentNode);

            if(folder == null)
            {
                return null;
            }
            else
            {
                return await _folderRepository.FindByNameAndFolder(serachString, folder.Id);
            }
        }
    }
}
