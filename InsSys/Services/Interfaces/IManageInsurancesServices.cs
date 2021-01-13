using InsSys.Models;
using InsSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsSys.Services.Interfaces
{
    public interface IManageInsurancesServices
    {

        InsDTO GetInsDTO(Insurance record, string ICName, string PackageName);
        Insurance GetRecord(int id);
    }
}