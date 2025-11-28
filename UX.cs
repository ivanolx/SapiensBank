using static System.Console;
using System.Linq; 

public class UX
{
    private readonly Banco _banco;
    private readonly string _titulo;

    public UX(string titulo, Banco banco)
    {
        _titulo = titulo;
        _banco = banco;
    }

    public void Executar()
    {
        CriarTitulo(_titulo);
        WriteLine(" [1] Criar Conta");
        WriteLine(" [2] Listar Contas");
        WriteLine(" [3] Efetuar Saque");
        WriteLine(" [4] Efetuar Depósito");
        WriteLine(" [5] Aumentar Limite");
        WriteLine(" [6] Diminuir Limite");
        ForegroundColor = ConsoleColor.Red;
        WriteLine("\n [9] Sair");
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        Write(" Digite a opção desejada: ");
        var opcao = ReadLine() ?? "";
        ForegroundColor = ConsoleColor.White;
        switch (opcao)
        {
            case "1": CriarConta(); break;
            case "2": MenuListarContas(); break;
            case "3": EfetuarSaque(); break;
            case "4": EfetuarDeposito(); break;
            case "5": AumentarLimite(); break;
            case "6": DiminuirLimite(); break;
        }
        if (opcao != "9")
        {
            Executar();
        }
        _banco.SaveContas();
    }

    private Conta? ObterConta()
    {
        CriarTitulo(_titulo + " - Buscar Conta");
        Write(" Digite o número da conta: ");
        
        if (!int.TryParse(ReadLine(), out int numeroConta))
        {
            CriarRodape("Número de conta inválido.");
            return null;
        }
        
        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numeroConta); 

        if (conta == null)
        {
            CriarRodape("Conta não encontrada.");
        }
        return conta;
    }

    private void EfetuarSaque()
    {
        Conta? conta = ObterConta();
        if (conta == null) return;

        Write(" Digite o valor para saque: R$");
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Valor de saque inválido.");
            return;
        }

        if (conta.Sacar(valor)) 
        {
            CriarRodape($"Saque de {valor:C} realizado com sucesso. Saldo Disponível: {conta.SaldoDisponível:C}.");
        }
        else
        {
            CriarRodape("Falha no Saque: Saldo ou Limite insuficiente.");
        }
    }

    private void EfetuarDeposito()
    {
        Conta? conta = ObterConta();
        if (conta == null) return;
        
        Write(" Digite o valor para depósito: R$");
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Valor de depósito inválido.");
            return;
        }

        conta.Depositar(valor); 
        CriarRodape($"Depósito de {valor:C} realizado com sucesso. Novo Saldo: {conta.Saldo:C}.");
    }

    private void AumentarLimite()
    {
        Conta? conta = ObterConta();
        if (conta == null) return;

        Write(" Digite o valor para aumentar o limite: R$");
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Valor de aumento inválido.");
            return;
        }
        
        conta.AumentarLimite(valor);
        CriarRodape($"Limite aumentado em {valor:C}. Novo Limite: {conta.Limite:C}.");
    }

    private void DiminuirLimite()
    {
        Conta? conta = ObterConta();
        if (conta == null) return;

        Write(" Digite o valor para diminuir o limite: R$");
        if (!decimal.TryParse(ReadLine(), out decimal valor) || valor <= 0)
        {
            CriarRodape("Valor de redução inválido.");
            return;
        }

        conta.DiminuirLimite(valor);
        CriarRodape($"Limite diminuído em {valor:C}. Novo Limite: {conta.Limite:C}.");
    }

    private void CriarConta()
    {
        CriarTitulo(_titulo + " - Criar Conta");
        Write(" Numero:  ");
        var numero = Convert.ToInt32(ReadLine());
        Write(" Cliente: ");
        var cliente = ReadLine() ?? "";
        Write(" CPF:     ");
        var cpf = ReadLine() ?? "";
        Write(" Senha:   ");
        var senha = ReadLine() ?? "";
        Write(" Limite:  ");
        var limite = Convert.ToDecimal(ReadLine());

        var conta = new Conta(numero, cliente, cpf, senha, limite);
        _banco.Contas.Add(conta);

        CriarRodape("Conta criada com sucesso!");
    }

    private void MenuListarContas()
    {
        CriarTitulo(_titulo + " - Listar Contas");
        foreach (var conta in _banco.Contas)
        {
            WriteLine($" Conta: {conta.Numero} - {conta.Cliente}");
            WriteLine($" Saldo: {conta.Saldo:C} | Limite: {conta.Limite:C}");
            WriteLine($" Saldo Disponível: {conta.SaldoDisponível:C}\n");
        }
        CriarRodape();
    }

    private void CriarLinha()
    {
        WriteLine("-------------------------------------------------");
    }

    private void CriarTitulo(string titulo)
    {
        Clear();
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(" " + titulo);
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
    }

    private void CriarRodape(string? mensagem = null)
    {
        CriarLinha();
        ForegroundColor = ConsoleColor.Green;
        if (mensagem != null)
            WriteLine(" " + mensagem);
        Write(" ENTER para continuar");
        ForegroundColor = ConsoleColor.White;
        ReadLine();
    }
}