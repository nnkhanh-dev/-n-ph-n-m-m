using QLThuCung.Areas.Admin.ViewModels;

namespace QLThuCung.Areas.Admin.Services
{
    public interface ISanphamService
    {
        Task<IEnumerable<SanPhamResponse>> List();
        Task<SanPhamResponse> Details(string id);
        Task<bool> Create(SanPhamResquest model);
        Task<bool> Edit(string id, SanPhamResquest model, List<string> anhmoi);
        Task<bool> Delete(string id);
    }
}
