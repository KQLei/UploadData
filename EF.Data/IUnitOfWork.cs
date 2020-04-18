using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Data
{
    public interface IUnitOfWork
    {
        void Commit();

        void RollBack();
    }
}
