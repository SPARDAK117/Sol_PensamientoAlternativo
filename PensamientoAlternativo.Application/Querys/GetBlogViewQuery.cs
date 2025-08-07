using MediatR;
using PensamientoAlternativo.Application.DTOs.BlogDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Querys
{
    public class GetBlogViewQuery : IRequest<BlogViewDto> { }

}
