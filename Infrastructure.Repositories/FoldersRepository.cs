using Domain.Interfaces;
using Domains.Entities.DTOs;
using Domains.Entities.FolderDbModels;
using Infrastructure.FolderDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FoldersRepository : IFoldersRepository
    {
        private readonly ILogger _logger;
        private readonly FolderDbContext _context;
        public FoldersRepository(
            ILogger<FoldersRepository> logger,
            FolderDbContext context
            )
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return _context.Database.CurrentTransaction;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Folders> GetFolder(string FolderNodeId)
        {
            return await _context.Folders.Where(folder => folder.NodeId == FolderNodeId).FirstOrDefaultAsync();
        }

        public async Task<List<Files>> GetFolderFiles(long FolderNodeId)
        {
            return await _context.Files.Where(file => file.Folders.Id == FolderNodeId).ToListAsync();
        }

        public async Task<Files> GetFile(long id)
        {
            return await _context.Files.Where(file => file.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Folders>> GetFolderSubfolders(string FolderNodeId)
        {
            return await _context.Folders.Where(folder => folder.ParentNode == FolderNodeId).ToListAsync();
        }

        public async Task<Folders> AddFolder(Folders newFolder)
        {
            var response = await _context.Folders.AddAsync(newFolder);

            return response.Entity;
        }

        public async Task<Files> AddFile(Files newFile)
        {
            var response = await _context.Files.AddAsync(newFile);

            return response.Entity;
        }

        public async Task<List<Folders>> GetFolderNodes(string nodeId)
        {
            return await _context.Folders.Where(folder => folder.NodeId.StartsWith(nodeId)).Include(folder => folder.Files).ToListAsync();
        }

        public void RemoveMultipleFolders(List<Folders> folders)
        {
            _context.Folders.RemoveRange(folders);
        }

        public void RemoveMultipleFiles(List<Files> files)
        {
            _context.Files.RemoveRange(files);
        }

        public void RemoveFile(Files file)
        {
            _context.Files.Remove(file);
        }

        public async Task<List<Files>> FindByName(string serachString)
        {
            return await _context.Files.Where(file => file.Name.StartsWith(serachString))  
                                                                     .OrderBy(file => file.Name)
                                                                     .Take(10)
                                                                     .ToListAsync();
        }

        public async Task<List<Files>> FindByNameAndFolder(string serachString,long folderId)
        {
            return await _context.Files.Where(file => file.FoldersId == folderId)
                                       .Where(file => file.Name.StartsWith(serachString))
                                       .OrderBy(file => file.Name)
                                       .Take(10)
                                       .ToListAsync();
        }
    }
}
