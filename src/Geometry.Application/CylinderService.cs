using Geometry.Domain.CubeModel;
using Geometry.Domain.CylinderInterface;
using Geometry.Domain.CylinderModel;

namespace Geometry.Application;

public class CylinderService
{
    private ICylinderRepository _cylinderRepository;

    public CylinderService(ICylinderRepository cylinderRepository)
    {
        _cylinderRepository = cylinderRepository;
    }

    public async Task<Guid> Insert(Cylinder cylinder)
    {
        return await _cylinderRepository.Insert(cylinder);
    }

    public async Task<Cylinder?> ReadById(Guid id)
    {
        return await _cylinderRepository.ReadById(id);
    }

    public async Task<bool> Update(Guid id)
    {
        return await _cylinderRepository.Update(id);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _cylinderRepository.Delete(id);
    }
}