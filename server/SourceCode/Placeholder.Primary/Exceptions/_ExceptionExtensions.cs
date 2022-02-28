using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Placeholder.Primary
{
    public static class _ExceptionExtensions
    {
        public static bool IsUniqueConstraintViolation(this Exception exception)
        {
            if (exception is DbUpdateException dbUpdateEx)
            {
                if (dbUpdateEx.InnerException != null && dbUpdateEx.InnerException.InnerException != null)
                {
                    if (dbUpdateEx.InnerException.InnerException is SqlException sqlException)
                    {
                        switch (sqlException.Number)
                        {
                            case 2627:  // Unique constraint error
                            case 547:   // Constraint check violation
                            case 2601:  // Duplicated key row error
                                        // Constraint violation exception
                                        // A custom exception of yours for concurrency issues
                                return true;
                            default:
                                break;
                        }
                    }
                }
            }

            return false;
        }
    }
}
