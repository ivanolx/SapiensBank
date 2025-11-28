using System.Text.Json.Serialization;

public class Conta
{
    public int Numero { get; set; }
    public string Cliente { get; set; }
    public string Cpf { get; set; }
    public string Senha { get; set; }
    public decimal Saldo { get; set; }
    public decimal Limite { get; set; }

    [JsonIgnore]
    public decimal SaldoDisponível => Saldo + Limite;

    public Conta(int numero, string cliente, string cpf, string senha, decimal limite = 0)
    {
        Numero = numero;
        Cliente = cliente;
        Cpf = cpf;
        Senha = senha;
        Limite = limite;
    }

    public void Depositar(decimal valor)
    {
        if (valor > 0)
        {
            this.Saldo += valor;
        }
    }

    public bool Sacar(decimal valor)
    {
        if (valor > 0 && this.SaldoDisponível >= valor)
        {
            this.Saldo -= valor;
            return true;
        }
        return false;
    }

    public void AumentarLimite(decimal valor)
    {
        if (valor > 0)
        {
            this.Limite += valor;
        }
    }

    public void DiminuirLimite(decimal valor)
    {
        if (valor > 0)
        {
            decimal novoLimite = this.Limite - valor;
            
            // Regra básica: não deixar o limite ficar negativo
            if (novoLimite >= 0)
            {
                this.Limite = novoLimite;
            }
            else if (this.Limite > 0 && novoLimite < 0)
            {
                this.Limite = 0; 
            }
        }
    }
}