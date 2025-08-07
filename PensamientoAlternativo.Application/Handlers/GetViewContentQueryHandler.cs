using API_PensamientoAlternativo.DTOs;
using MediatR;
using PensamientoAlternativo.Application.DTOs;
using PensamientoAlternativo.Application.Querys;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers
{
    public class GetViewContentQueryHandler : IRequestHandler<GetViewContentQuery, ViewContentDto>
    {
        private readonly IHomeContentRepository _repo;

        public GetViewContentQueryHandler(IHomeContentRepository repo)
        {
            _repo = repo;
        }

        public async Task<ViewContentDto> Handle(GetViewContentQuery request, CancellationToken ct)
        {
            if (!string.Equals(request.View, "Home", StringComparison.OrdinalIgnoreCase))
                throw new NotImplementedException("Vista no soportada aún");

            return new ViewContentDto
            {
                View = request.View,
                Sections = new List<SectionContentDto>
                {
                    new()
                    {
                        IdSection = 1,
                        Section = "Home Banner principal",
                        Content = await _repo.GetBannersAsync(ct)
                    },
                    new()
                    {
                        IdSection = 2,
                        Section = "Home Services",
                        Content = await _repo.GetServicesAsync(ct)
                    },
                    new()
                    {
                        IdSection = 3,
                        Section = "Home Videos",
                        Content = await _repo.GetVideosAsync(ct)
                    },
                    new()
                    {
                        IdSection = 4,
                        Section = "Home Recent Articles",
                        Content = await _repo.GetArticlesAsync(ct)
                    },
                    new()
                    {
                        IdSection = 5,
                        Section = "Home Customer Opinions",
                        Content = await _repo.GetOpinionsAsync(ct)
                    },
                    new()
                    {
                        IdSection = 6,
                        Section = "Home Top Question Answer",
                        Content = await _repo.GetFaqsAsync(ct)
                    }
                }
            };

        }
    }
}
