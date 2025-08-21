using API_PensamientoAlternativo.DTOs;
using MediatR;
using PensamientoAlternativo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Querys
{
    public class GetBreakYourLimitsViewQuery : IRequest<BreakYourLimitsViewContentDto>
    {
        public string View { get; }

        public GetBreakYourLimitsViewQuery(string view)
        {
            View = view;
        }
    }
}
