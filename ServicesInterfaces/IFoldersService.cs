using Domains.Entities.DTOs;
using Domains.Entities.FolderDbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesInterfaces
{
    public interface IFoldersService
    {
        Task<Folders> GetFolder(string folderNodeId);
        Task<List<Files>> GetFolderFiles(string folderNodeId);
        Task<List<Folders>> GetFolderSubfolders(string folderNodeId);
        Task<AddNewFolderResponse> AddNewFolder(string parentNodeId, string folderName);
        Task<AddNewFileResponse> AddNewFile(string parentNodeId, string fileName);
        Task<DeleteFolderResponse> DeleteFolder(string NodeId);
        Task<DeleteFileResponse> DeleteFile(int id);
        Task<List<Files>> FindByName(string NodeId);
        Task<List<Files>> FindByNameWithinFolder(string parentNode, string serachString);
    }
}
