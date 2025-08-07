using API_PensamientoAlternativo.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Querys
{
    public class GetViewContentQuery : IRequest<ViewContentDto>
    {
        public string View { get; }

        public GetViewContentQuery(string view)
        {
            View = view;
        }
    }
}
