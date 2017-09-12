using BLL;
using IDAL;
using ILibValueService;
using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibleValueService
{
    public partial class PTitleService : BaseService<PTitle>, IPTitleService
    {
        private IPTitleDAL iPTitleDal = DALContainer.Container.Resolve<IPTitleDAL>();
        public override void SetDal()
        {
            Dal = iPTitleDal;
        }
    }
}
