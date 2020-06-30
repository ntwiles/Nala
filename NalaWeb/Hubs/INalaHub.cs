using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NalaWeb.Hubs
{
    public interface INalaHub
    {
        Task OutputChange(Guid questionId, int score);
    }
}
