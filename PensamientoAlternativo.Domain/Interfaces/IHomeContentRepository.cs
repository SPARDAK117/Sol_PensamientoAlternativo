using PensamientoAlternativo.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Domain.Interfaces
{
    public interface IHomeContentRepository
    {
        Task<List<Image>> GetBannersAsync(CancellationToken ct);
        Task<List<Service>> GetServicesAsync(CancellationToken ct);
        Task<List<Image>>GetImagesAsync(CancellationToken ct);
        Task<List<Video>> GetVideosAsync(CancellationToken ct);
        Task<List<Article>> GetArticlesAsync(CancellationToken ct);
        Task<List<Opinion>> GetOpinionsAsync(CancellationToken ct);
        Task<List<Faq>> GetFaqsAsync(CancellationToken ct);
    }

}
