using PensamientoAlternativo.Application.DTOs;

namespace API_PensamientoAlternativo.DTOs
{
    public class ViewContentDto
    {
        public string View { get; set; } = string.Empty;
        public List<SectionContentDto > Sections { get; set; } = [];
    }
}
