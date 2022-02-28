using System;
using Placeholder.Data.Sql;
using Placeholder.Data.Sql.Models;
using Placeholder.Domain;
using Placeholder.Primary.Business.Synchronization;

namespace Placeholder.Primary.Business.Direct
{
    public interface INestedOperation<TDbModel, TDomainModel>
        where TDomainModel : DomainModel
    {
        NestedInsertInfo<TDbModel, TDomainModel> PrepareNestedInsert(PlaceholderContext db, TDomainModel insertEntity);
        TDomainModel FinalizeNestedInsert(PlaceholderContext db, NestedInsertInfo<TDbModel, TDomainModel> insertInfo, Availability availability);

        NestedUpdateInfo<TDbModel, TDomainModel> PrepareNestedUpdate(PlaceholderContext db, TDomainModel updateModel);
        TDomainModel FinalizeNestedUpdate(PlaceholderContext db, NestedUpdateInfo<TDbModel, TDomainModel> updateInfo, Availability availability);
    }
}
