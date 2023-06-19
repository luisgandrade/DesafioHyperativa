using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioHyperativa.CamadaDados
{
    public class UnidadeDeTrabalho : IUnidadeDeTrabalho
    {
        private readonly CartoesContext _cartoesContext;

        public UnidadeDeTrabalho(CartoesContext cartoesContext)
        {
            _cartoesContext = cartoesContext;
        }

        public async Task CommitarTransacao()
        {
            if(_cartoesContext.ChangeTracker.HasChanges())
                await _cartoesContext.SaveChangesAsync();
        }
    }
}
