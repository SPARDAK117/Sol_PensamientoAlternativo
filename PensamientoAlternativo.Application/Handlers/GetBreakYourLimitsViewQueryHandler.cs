using MediatR;
using PensamientoAlternativo.Application.DTOs;
using PensamientoAlternativo.Application.DTOs.BlogDto;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Application.Querys;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers
{
    public class GetBreakYourLimitsViewQueryHandler : IRequestHandler<GetBreakYourLimitsViewQuery, BreakYourLimitsViewContentDto>
    {
        private readonly IHomeContentRepository _repo;


        public GetBreakYourLimitsViewQueryHandler(IHomeContentRepository repo)
        {
            _repo = repo;
        }

        public async Task<BreakYourLimitsViewContentDto> Handle(GetBreakYourLimitsViewQuery request, CancellationToken ct)
        {
            //var categories = await _repo.GetCategoriesAsync(ct);
            //var topArticles = await _repo.GetTopArticlesAsync(ct);
            //var (latestArticles, totalCount) = await _repo.GetLatestArticlesAsync(1, 8, ct);

            //var totalPages = (int)Math.Ceiling((double)totalCount / 8);
            return new BreakYourLimitsViewContentDto
            {
                View = "RompeTusLimites",
                Sections = new List<SectionBreakYourLimitsDto>
            {
                    new()
                    {
                        IdSection = 2,
                        Section = "Rompe Tus Limites Carrusel Fotos",
                        Content = await _repo.GetImagesBreakYourLimitsAsync(ct)
                    },
                    new()
                    {
                        IdSection = 3,
                        Section = "Rompe Tus Limites Videos",
                        Content = await _repo.GetVideosBreakYourLimitsAsync(ct)
                    },

            }
            
            };
        }
    }
}
