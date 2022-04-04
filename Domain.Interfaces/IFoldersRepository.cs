using Domains.Entities.DTOs;
using Domains.Entities.FolderDbModels;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFoldersRepository
    {
        Task<IDbContextTransaction> BeginTransaction();
        Task<int> SaveChangesAsync();
        IDbContextTransaction GetCurrentTransaction();
        Task<Folders> GetFolder(string FolderNodeId);
        Task<Files> GetFile(long id);
        Task<List<Files>> GetFolderFiles(long FolderNodeId);
        Task<List<Folders>> GetFolderSubfolders(string FolderNodeId);
        Task<Folders> AddFolder(Folders newFolder);
        Task<Files> AddFile(Files newFile);
        Task<List<Folders>> GetFolderNodes(string nodeId);
        void RemoveMultipleFolders(List<Folders> folders);
        void RemoveMultipleFiles(List<Files> files);
        void RemoveFile(Files file);
        Task<List<Files>> FindByName(string serachString);
        Task<List<Files>> FindByNameAndFolder(string serachString, long folderId);
    }
}
