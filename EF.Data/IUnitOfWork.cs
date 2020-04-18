using EF.Core;

namespace EF.Data
{
    public interface IUnitOfWork
    {
        void Commit();

        void RollBack();

        Repository<T> Repository<T>() where T : BaseEntity;
    }
}
