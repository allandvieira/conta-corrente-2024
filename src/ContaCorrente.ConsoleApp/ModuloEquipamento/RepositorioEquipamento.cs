using ContaCorrente.ConsoleApp.Compartilhado;
using GestaoEquipamentos.ConsoleApp.Compartilhado;

namespace ContaCorrente.ConsoleApp.ModuloEquipamento
{
    public class RepositorioEquipamento
    {
        private Equipamento[] equipamentos = new Equipamento[100];

        public void CadastrarEquipamento(Equipamento novoEquipamento)
        {
            novoEquipamento.Id = GeradorId.GerarIdEquipamento();

            RegistrarItem(novoEquipamento);
        }

        public bool EditarEquipamento(int id, Equipamento novoEquipamento)
        {
            novoEquipamento.Id = id;

            for (int i = 0; i < equipamentos.Length; i++)
            {
                if (equipamentos[i] == null)
                    continue;

                else if (equipamentos[i].Id == id)
                {
                    equipamentos[i] = novoEquipamento;

                    return true;
                }
            }

            return false;
        }

        public bool ExcluirEquipamento(int id)
        {
            for (int i = 0; i < equipamentos.Length; i++)
            {
                if (equipamentos[i] == null)
                    continue;

                else if (equipamentos[i].Id == id)
                {
                    equipamentos[i] = null;
                    return true;
                }
            }

            return false;
        }

        public Equipamento[] SelecionarEquipamentos()
        {
            return equipamentos;
        }

        public bool ExisteEquipamento(int id)
        {
            for (int i = 0; i < equipamentos.Length; i++)
            {
                Equipamento e = equipamentos[i];

                if (e == null)
                    continue;

                else if (e.Id == id)
                    return true;
            }

            return false;
        }

        private void RegistrarItem(Equipamento equipamento)
        {
            for (int i = 0; i < equipamentos.Length; i++)
            {
                if (equipamentos[i] != null)
                    continue;

                else
                {
                    equipamentos[i] = equipamento;
                    break;
                }
            }
        }
    }
}