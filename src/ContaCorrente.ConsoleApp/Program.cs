namespace ContaCorrente.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Clientes
            Cliente cliente1 = new Cliente("João", "Silva", "123.456.789-00");
            Cliente cliente2 = new Cliente("Maria", "Pereira", "987.654.321-00");
            Cliente cliente3 = new Cliente("Carlos", "Santos", "111.222.333-44");

            // Contas
            ContaCorrente conta1 = new ContaCorrente(1, cliente1, true, 1000);
            ContaCorrente conta2 = new ContaCorrente(2, cliente2, false, 0);
            ContaCorrente conta3 = new ContaCorrente(3, cliente3, true, 1500);

            // Depósitos
            conta1.Depositar(500);
            conta2.Depositar(300);
            conta3.Depositar(1000);

            // Saques
            conta1.Sacar(200);
            conta2.Sacar(400);
            conta3.Sacar(3000);

            // Transferência entre contas
            conta1.Transferir(conta2, 100);

            // Visualização de saldo e extrato
            ExibirInformacoesConta(conta1);
            ExibirInformacoesConta(conta2);
            ExibirInformacoesConta(conta3);

            Console.WriteLine("Pressione qualquer tecla para sair");
            Console.ReadKey();
        }

        static void ExibirInformacoesConta(ContaCorrente conta)
        {
            // Saldo da conta
            decimal saldo = conta.VerSaldo();

            // Lista com as movimentações
            List<Movimentacao> extrato = conta.VerExtrato();

            Console.WriteLine($"Conta {conta.Numero}: {conta.Dono.Nome} {conta.Dono.Sobrenome}, CPF: {conta.Dono.CPF}, Tipo: {(conta.IsEspecial ? "Conta Especial" : "Conta Não Especial")}, Limite: {conta.Limite}, Saldo: {saldo}");
            Console.WriteLine("Extrato:");

            // Foreach que percorre cada movimentação na lista de movimentações da conta
            foreach (var mov in extrato)
            {
                Console.WriteLine($"Valor: {mov.Valor}, Tipo: {(mov.IsCredito ? "Crédito" : "Débito")}, Operação: {mov.TipoOperacao}, Situação: {mov.Situacao}");
                if (!string.IsNullOrEmpty(mov.MensagemErro))
                {
                    Console.WriteLine(mov.MensagemErro);
                }
            }
            Console.WriteLine();
        }
    }

    public class Movimentacao
    {
        public decimal Valor { get; set; }
        public bool IsCredito { get; set; }
        public string TipoOperacao { get; set; }
        public string Situacao { get; set; }
        public string MensagemErro { get; set; }
    }

    public class Cliente
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string CPF { get; set; }

        public Cliente(string nome, string sobrenome, string cpf)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            CPF = cpf;
        }
    }

    public class ContaCorrente
    {
        public int Numero { get; set; }
        public decimal Saldo { get; private set; }
        public bool IsEspecial { get; set; }
        public decimal Limite { get; set; }
        public List<Movimentacao> Movimentacoes { get; set; }
        public Cliente Dono { get; set; }

        public ContaCorrente(int numero, Cliente dono, bool isEspecial, decimal limite)
        {
            Numero = numero;
            Dono = dono;
            IsEspecial = isEspecial;
            Limite = limite;
            Movimentacoes = new List<Movimentacao>();
        }

        public void Depositar(decimal valor)
        {
            Saldo += valor;
            Movimentacoes.Add(new Movimentacao { Valor = valor, IsCredito = true, TipoOperacao = "Depósito", Situacao = "Autorizada" });
        }

        public void Sacar(decimal valor)
        {
            if (valor <= Saldo + Limite)
            {
                Saldo -= valor;
                Movimentacoes.Add(new Movimentacao { Valor = valor, IsCredito = false, TipoOperacao = "Saque", Situacao = "Autorizada" });
            }
            else
            {
                string erro = $"Erro: Saque de {valor} excede o limite da conta {Numero}.";
                Movimentacoes.Add(new Movimentacao { Valor = valor, IsCredito = false, TipoOperacao = "Saque", Situacao = "Cancelada", MensagemErro = erro });
            }
        }

        public void Transferir(ContaCorrente contaDestino, decimal valor)
        {
            // Saca o valor da conta
            Sacar(valor);

            // Verifica a ultima movimentação da lista com as "Movimentações", caso a situação seja diferente de cancelada, realiza a transferência.
            if (Movimentacoes.Last().Situacao != "Cancelada")
            {
                contaDestino.Depositar(valor);
                Movimentacoes.Last().TipoOperacao = "Transferência";
                contaDestino.Movimentacoes.Last().TipoOperacao = "Transferência";
            }
        }

        public decimal VerSaldo()
        {
            return Saldo;
        }

        public List<Movimentacao> VerExtrato()
        {
            return Movimentacoes;
        }
    }
}