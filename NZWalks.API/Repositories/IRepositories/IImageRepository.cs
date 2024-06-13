using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.IRepositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image imageDomainModel);
    }
}