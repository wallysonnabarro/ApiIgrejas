using AspNetCore.Reporting;

namespace Service.Interface
{
    public interface IRenderType
    {
        RenderType GetRenderType(string type);
    }
}
